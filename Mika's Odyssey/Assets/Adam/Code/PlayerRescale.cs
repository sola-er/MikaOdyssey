using UnityEngine;
using System.Collections;

public class PlayerRescale : MonoBehaviour
{
    public enum SizeState { Small, Normal, Big }

    [Header("Refs")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private CapsuleCollider capsule;
    [SerializeField] private PlayerAction playerController; // pour sync playerHeight

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

    // Proprietes publiques - lisibles par les autres scripts
    public SizeState CurrentState => currentState;
    public float CurrentScale => transform.localScale.x;
    public float CurrentMass => rb.mass;

    // Evenement declenche quand un resize est termine
    public System.Action<SizeState> OnResizeComplete;

    // Dimensions de base du collider (etat Normal)
    private float baseHeight;
    private float baseRadius;
    private Vector3 baseCenter;
    private float basePlayerHeight; // valeur originale dans PlayerAction

    private Coroutine resizeRoutine;

    private static readonly Collider[] _overlapBuffer = new Collider[16];


    private void Reset()
    {
        rb = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();
    }

    private void Awake()
    {
        if (!rb) rb = GetComponent<Rigidbody>();
        if (!capsule) capsule = GetComponent<CapsuleCollider>();
        if (!playerController) playerController = GetComponent<PlayerAction>();

        // Avertissement si solidMask non configure
        if (solidMask == 0)
            Debug.LogWarning("[PlayerRescale] solidMask n'est pas assigne. " +
                             "L'anti-clipping ne fonctionnera pas correctement.");

        // instancier(les avoir en mémoire quoi) les dims de base
        baseHeight = capsule.height;
        baseRadius = capsule.radius;
        baseCenter = capsule.center;

        if (playerController != null)
            basePlayerHeight = playerController.playerHeight;

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

        // Anti-clipping : on verifie seulement si on grossit
        if (targetScale > transform.localScale.x && !CanResizeTo(targetScale))
            return;

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
            t += Time.unscaledDeltaTime / resizeDuration;
            float s = Mathf.Lerp(startScale, endScale, Mathf.SmoothStep(0f, 1f, t));
            ApplyScaleAndPhysics(s);
            yield return null;
        }

        // Forcer la valeur finale exacte
        ApplyScaleAndPhysics(endScale);
        currentState = target;
        resizeRoutine = null;

        OnResizeComplete?.Invoke(currentState);
    }

    private void ApplyStateInstant(SizeState state)
    {
        currentState = state;
        ApplyScaleAndPhysics(GetScale(state));
        OnResizeComplete?.Invoke(currentState);
    }

    private void ApplyScaleAndPhysics(float scale)
    {
        // Echelle visuelle
        transform.localScale = Vector3.one * scale;

        // Collider
        capsule.height = baseHeight * scale;
        capsule.radius = baseRadius * scale;
        capsule.center = new Vector3(baseCenter.x * scale, baseCenter.y, baseCenter.z * scale);

        // Masse cubique : m = m0 * s^3 (loi de volume)
        rb.mass = baseMass * Mathf.Pow(scale, 3f);

        // Sync playerHeight dans PlayerAction pour que le raycast grounded reste correct
        if (playerController != null)
            playerController.playerHeight = basePlayerHeight * scale;
    }


    private float GetScale(SizeState state) => state switch
    {
        SizeState.Small => smallScale,
        SizeState.Normal => normalScale,
        SizeState.Big => bigScale,
        _ => normalScale
    };

    private bool CanResizeTo(float targetScale)
    {
        float radius = Mathf.Max(0.01f, baseRadius * targetScale - clearancePadding);
        float height = Mathf.Max(radius * 2f + 0.01f, baseHeight * targetScale - clearancePadding);

        // On part des pieds de Mika (position reelle au sol) et on monte
        // Les pieds = position du transform - demi-hauteur du collider actuel
        float currentHalfHeight = capsule.height * 0.5f;
        Vector3 feet = transform.position - Vector3.up * currentHalfHeight;

        // Le sommet apres agrandissement
        Vector3 top = feet + Vector3.up * height;
      
        // Suffit pour detecter un plafond, et ignore completement le sol
        int hits = Physics.OverlapSphereNonAlloc(top, radius, _overlapBuffer, solidMask);

        for (int i = 0; i < hits; i++)
        {
            if (_overlapBuffer[i] == null) continue;
            if (_overlapBuffer[i] == capsule) continue;
            return false;
        }
        return true;
    }
}