using UnityEngine;

public class AutoMover : MonoBehaviour, ISlowable
{
    public enum MoveMode { PingPong, Loop, OneWay }

    [Header("Movement")]
    [SerializeField] private Vector3 localOffset = new Vector3(0f, 3f, 0f);
    [SerializeField] private float speed = 2f;
    [SerializeField] private MoveMode moveMode = MoveMode.PingPong;
    [SerializeField] private bool playOnStart = true;
    [SerializeField] private bool useLocalPosition = true;

    [Header("Optional Delay")]
    [SerializeField] private float startDelay = 0f;

    private Vector3 startPos;
    private Vector3 endPos;
    private bool isMoving = false;
    private float timer = 0f;
    private int direction = 1;
    private float journeyLength;

    // Vitesse effective - modifiee par ISlowable
    private float effectiveSpeed;

    private void Start()
    {
        startPos = useLocalPosition ? transform.localPosition : transform.position;
        endPos = startPos + localOffset;
        journeyLength = Vector3.Distance(startPos, endPos);
        effectiveSpeed = speed;

        if (playOnStart) StartMoving();
    }

    private void Update()
    {
        if (!isMoving) return;

        if (startDelay > 0f)
        {
            startDelay -= Time.deltaTime;
            return;
        }

        if (journeyLength <= 0.001f) return;

        // Utilise effectiveSpeed au lieu de speed directement
        timer += (Time.deltaTime * effectiveSpeed * direction) / journeyLength;

        switch (moveMode)
        {
            case MoveMode.PingPong:
                if (timer >= 1f) { timer = 1f; direction = -1; }
                else if (timer <= 0f) { timer = 0f; direction = 1; }
                break;
            case MoveMode.Loop:
                if (timer >= 1f) timer = 0f;
                break;
            case MoveMode.OneWay:
                if (timer >= 1f) { timer = 1f; isMoving = false; }
                break;
        }

        Vector3 targetPos = Vector3.Lerp(startPos, endPos, timer);
        if (useLocalPosition) transform.localPosition = targetPos;
        else transform.position = targetPos;
    }

    // --- ISlowable ---
    public void SlowDown(float factor)
    {
        effectiveSpeed = speed * factor;
    }

    public void RestoreSpeed()
    {
        effectiveSpeed = speed;
    }

    // --- API publique ---
    public void StartMoving() => isMoving = true;
    public void StopMoving() => isMoving = false;

    public void ResetToStart()
    {
        timer = 0f; direction = 1;
        if (useLocalPosition) transform.localPosition = startPos;
        else transform.position = startPos;
    }

    public void MoveToEnd()
    {
        timer = 1f;
        if (useLocalPosition) transform.localPosition = endPos;
        else transform.position = endPos;
    }
}