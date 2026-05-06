using UnityEngine;
using System.Collections.Generic;

public class PressurePlate : MonoBehaviour
{
    [Header("Seuil de masse (Science)")]
    [SerializeField] private float massThreshold = 2f;

    [Header("Action")]
    [SerializeField] private DoorController targetDoor;
    [SerializeField] private bool stayOpen = false;

    [Header("Couleurs")]
    [SerializeField] private Color inactiveColor = Color.red;
    [SerializeField] private Color activeColor = Color.green;
    [SerializeField] private Color partialColor = new Color(1f, 0.5f, 0f); // orange

    [Header("Debug")]
    [SerializeField] private bool showDebug = true;

    private Dictionary<Rigidbody, float> objectsOnPlate = new Dictionary<Rigidbody, float>();
    private float totalMass = 0f;
    private bool isActivated = false;
    private Renderer plateRenderer;

    private void Start()
    {
        plateRenderer = GetComponent<Renderer>();
        UpdateColor();
    }

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        if (rb == null) return;
        if (objectsOnPlate.ContainsKey(rb)) return;

        objectsOnPlate[rb] = rb.mass;
        RecalculateMass();
    }

    private void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        if (rb == null) return;
        if (!objectsOnPlate.ContainsKey(rb)) return;

        objectsOnPlate.Remove(rb);
        RecalculateMass();
    }

    private void RecalculateMass()
    {
        totalMass = 0f;
        foreach (float m in objectsOnPlate.Values)
            totalMass += m;

        if (showDebug)
            Debug.Log($"[PressurePlate] Masse : {totalMass:F2} / seuil : {massThreshold}");

        CheckActivation();
        UpdateColor();
    }

    private void CheckActivation()
    {
        bool shouldActivate = totalMass >= massThreshold;

        if (shouldActivate && !isActivated)
        {
            isActivated = true;
            targetDoor?.Open();
        }
        else if (!shouldActivate && isActivated && !stayOpen)
        {
            isActivated = false;
            targetDoor?.Close();
        }
    }

    private void UpdateColor()
    {
        if (plateRenderer == null) return;

        if (isActivated)
            plateRenderer.material.color = activeColor;         // vert  - seuil atteint
        else if (totalMass > 0f)
            plateRenderer.material.color = partialColor;        // orange - quelque chose dessus mais pas assez lourd
        else
            plateRenderer.material.color = inactiveColor;       // rouge - vide
    }

    public float GetTotalMass() => totalMass;
    public float GetThreshold() => massThreshold;
    public bool IsActivated() => isActivated;
    public float GetFillPercent() => Mathf.Clamp01(totalMass / massThreshold);
}