using UnityEngine;
using System.Collections.Generic;

public class PressurePlate : MonoBehaviour
{
    [Header("Seuil de masse (Science)")]
    [SerializeField] private float massThreshold = 2f;

    [Header("Action")]
    [SerializeField] private DoorController targetDoor;
    [SerializeField] private bool stayOpen = false; // reste ouverte meme si on part

    [Header("Debug")]
    [SerializeField] private bool showDebug = true;

    // Objets actuellement sur la plaque
    private Dictionary<Rigidbody, float> objectsOnPlate = new Dictionary<Rigidbody, float>();
    private float totalMass = 0f;
    private bool isActivated = false;

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
            Debug.Log($"[PressurePlate] Masse totale : {totalMass:F2} / seuil : {massThreshold}");

        CheckActivation();
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

    // Lecture publique pour UI ou autres scripts
    public float GetTotalMass() => totalMass;
    public float GetThreshold() => massThreshold;
    public bool IsActivated() => isActivated;
    public float GetFillPercent() => Mathf.Clamp01(totalMass / massThreshold);
}