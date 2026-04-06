using UnityEngine;
using System.Collections.Generic;

public class PlayerTimeSlow : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private KeyCode slowKey = KeyCode.E;

    [Header("Slow Settings")]
    [SerializeField][Range(0.05f, 1f)] private float slowFactor = 0.3f;
    [SerializeField] private float maxDuration = 3f;
    [SerializeField] private float cooldown = 5f;
    [SerializeField] private bool holdToUse = true;

    [Header("Whitelist")]
    [SerializeField] private List<GameObject> slowTargets = new List<GameObject>();

    private float currentSlowTime;
    private float cooldownTimer;
    private bool isSlowing = false;

    // Cache des ISlowable trouves sur les targets
    private List<ISlowable> slowables = new List<ISlowable>();

    private void Awake()
    {
        currentSlowTime = maxDuration;
        BuildSlowableCache();
    }

    // Construit la liste des ISlowable depuis les GameObjects assignes
    private void BuildSlowableCache()
    {
        slowables.Clear();
        foreach (GameObject go in slowTargets)
        {
            if (go == null) continue;
            // Cherche ISlowable sur l'objet et ses enfants
            ISlowable[] found = go.GetComponentsInChildren<ISlowable>();
            slowables.AddRange(found);
        }
    }

    private void Update()
    {
        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;

        bool wantsSlow = holdToUse ? Input.GetKey(slowKey) : Input.GetKeyDown(slowKey);
        bool releasedSlow = holdToUse && Input.GetKeyUp(slowKey);

        if (holdToUse)
        {
            if (wantsSlow && CanUseSlow()) StartSlow();
            else if (releasedSlow || currentSlowTime <= 0f) StopSlow();
        }
        else
        {
            if (wantsSlow)
            {
                if (isSlowing) StopSlow();
                else if (CanUseSlow()) StartSlow();
            }
        }

        if (isSlowing)
        {
            currentSlowTime -= Time.deltaTime;
            if (currentSlowTime <= 0f)
            {
                currentSlowTime = 0f;
                StopSlow();
            }
        }
        else
        {
            if (cooldownTimer <= 0f && currentSlowTime < maxDuration)
                currentSlowTime = Mathf.Min(currentSlowTime + Time.deltaTime, maxDuration);
        }
    }

    private bool CanUseSlow() => cooldownTimer <= 0f && currentSlowTime > 0f;

    private void StartSlow()
    {
        if (isSlowing) return;
        isSlowing = true;
        foreach (ISlowable s in slowables) s.SlowDown(slowFactor);
    }

    private void StopSlow()
    {
        if (!isSlowing) return;
        isSlowing = false;
        foreach (ISlowable s in slowables) s.RestoreSpeed();
        if (currentSlowTime <= 0f) cooldownTimer = cooldown;
    }

    private void OnDisable()
    {
        StopSlow();
    }

    // Ajouter un objet a la whitelist dynamiquement (utile pour les rooms)
    public void RegisterTarget(GameObject go)
    {
        if (go == null || slowTargets.Contains(go)) return;
        slowTargets.Add(go);
        ISlowable[] found = go.GetComponentsInChildren<ISlowable>();
        slowables.AddRange(found);
    }

    // Getters pour UI
    public bool IsSlowing() => isSlowing;
    public float GetSlowPercent() => maxDuration <= 0f ? 0f : currentSlowTime / maxDuration;
    public float GetCooldownRemaining() => Mathf.Max(0f, cooldownTimer);
}