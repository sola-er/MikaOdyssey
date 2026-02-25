using UnityEngine;
using System.Collections;

public class PlayerRescale : MonoBehaviour
{
    public enum SizeState { Small, Normal, Big }

    [Header("Refs")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private CapsuleCollider capsule;

    [Header("Scale Settings")]
    [SerializeField] private SizeState currentState = SizeState.Normal;
    [SerializeField] private float smallScale = 0.6f;
    [SerializeField] private float normalScale = 1.0f;
    [SerializeField] private float bigScale = 1.6f;

    [Header("Physics (Science)")]
    [SerializeField] private float baseMass = 1.0f;          
    [SerializeField] private float resizeDuration = 0.18f;   

    [Header("Anti-Clipping")]
    [SerializeField] private LayerMask solidMask;            
    [SerializeField] private float clearancePadding = 0.02f; 
    // Cached original collider dims (at Normal scale)
    private float baseHeight;
    private float baseRadius;
    private Vector3 baseCenter;

    private Coroutine resizeRoutine;

    private void Reset()
    {
        rb = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();
    }

    private void Awake()
    {
        if (!rb) rb = GetComponent<Rigidbody>();
        if (!capsule) capsule = GetComponent<CapsuleCollider>();

        baseHeight = capsule.height;
        baseRadius = capsule.radius;
        baseCenter = capsule.center;

        ApplyStateInstant(currentState);
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Alpha1)) TrySetState(SizeState.Small);
        if (Input.GetKeyDown(KeyCode.Alpha2)) TrySetState(SizeState.Normal);
        if (Input.GetKeyDown(KeyCode.Alpha3)) TrySetState(SizeState.Big);
    }

    public void TrySetState(SizeState target)
    {
        if (target == currentState) return;

        float targetScale = GetScale(target);

        
        if (targetScale > transform.localScale.x && !CanResizeTo(targetScale))
        {
            
            return;
        }

        if (resizeRoutine != null) StopCoroutine(resizeRoutine);
        resizeRoutine = StartCoroutine(ResizeSmooth(target));
    }

    private IEnumerator ResizeSmooth(SizeState target)
    {
        float startScale = transform.localScale.x;
        float endScale = GetScale(target);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / resizeDuration; // unscaled -> works even in slow time
            float s = Mathf.Lerp(startScale, endScale, Mathf.SmoothStep(0f, 1f, t));
            ApplyScaleAndPhysics(s);
            yield return null;
        }

        currentState = target;
        resizeRoutine = null;
    }

    private void ApplyStateInstant(SizeState state)
    {
        currentState = state;
        ApplyScaleAndPhysics(GetScale(state));
    }

    private void ApplyScaleAndPhysics(float scale)
    {
        // Visual scale
        transform.localScale = Vector3.one * scale;

        // Collider scale (keep consistent with character)
        capsule.height = baseHeight * scale;
        capsule.radius = baseRadius * scale;
        capsule.center = baseCenter * scale;

        // Mass scaling (science-like): m = m0 * s^3 (volume)
        rb.mass = baseMass * scale * scale * scale;
    }

    private float GetScale(SizeState state)
    {
        return state switch
        {
            SizeState.Small => smallScale,
            SizeState.Normal => normalScale,
            SizeState.Big => bigScale,
            _ => normalScale
        };
    }

    private bool CanResizeTo(float targetScale)
    {
        // We check if the resized capsule would overlap with solid colliders.
        // Use CapsuleCollider geometry in world space.
        float radius = (baseRadius * targetScale) - clearancePadding;
        float height = (baseHeight * targetScale) - clearancePadding;

        radius = Mathf.Max(0.01f, radius);
        height = Mathf.Max(radius * 2f + 0.01f, height);

        Vector3 centerWorld = transform.TransformPoint(baseCenter * targetScale);

        // Capsule endpoints in world space (along Y)
        float half = Mathf.Max(0f, (height * 0.5f) - radius);
        Vector3 p1 = centerWorld + Vector3.up * half;
        Vector3 p2 = centerWorld - Vector3.up * half;

        // Check overlaps
        int hits = Physics.OverlapCapsuleNonAlloc(p1, p2, radius, _overlapBuffer, solidMask);
        // We will always hit our own collider sometimes, so we filter it out:
        for (int i = 0; i < hits; i++)
        {
            Collider c = _overlapBuffer[i];
            if (!c) continue;
            if (c == capsule) continue;
            return false;
        }
        return true;
    }

    private static readonly Collider[] _overlapBuffer = new Collider[16];
}