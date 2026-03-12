using UnityEngine;

public class PlayerTimeSlow : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private KeyCode slowKey = KeyCode.LeftShift;

    [Header("Slow Time Settings")]
    [SerializeField][Range(0.05f, 1f)] private float slowFactor = 0.3f;
    [SerializeField] private float maxDuration = 3f;
    [SerializeField] private float cooldown = 5f;

    [Header("Optional")]
    [SerializeField] private bool holdToUse = true;

    private float currentSlowTime;
    private float cooldownTimer;
    private bool isSlowing = false;
    private float defaultFixedDeltaTime;

    private void Awake()
    {
        defaultFixedDeltaTime = Time.fixedDeltaTime;
        currentSlowTime = maxDuration;
    }

    private void Update()
    {
        if (cooldownTimer > 0f)
            cooldownTimer -= Time.unscaledDeltaTime;

        bool wantsSlow = holdToUse
            ? Input.GetKey(slowKey)
            : Input.GetKeyDown(slowKey);

        bool releasedSlow = holdToUse && Input.GetKeyUp(slowKey);

        if (holdToUse)
        {
            if (wantsSlow && CanUseSlow())
            {
                StartSlow();
            }
            else if (releasedSlow || currentSlowTime <= 0f)
            {
                StopSlow();
            }
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
            currentSlowTime -= Time.unscaledDeltaTime;

            if (currentSlowTime <= 0f)
            {
                currentSlowTime = 0f;
                StopSlow();
            }
        }
        else
        {
            if (cooldownTimer <= 0f && currentSlowTime < maxDuration)
            {
                currentSlowTime += Time.unscaledDeltaTime;
                currentSlowTime = Mathf.Min(currentSlowTime, maxDuration);
            }
        }
    }

    private bool CanUseSlow()
    {
        return cooldownTimer <= 0f && currentSlowTime > 0f;
    }

    private void StartSlow()
    {
        if (isSlowing) return;

        isSlowing = true;
        Time.timeScale = slowFactor;
        Time.fixedDeltaTime = defaultFixedDeltaTime * slowFactor;
    }

    private void StopSlow()
    {
        if (!isSlowing) return;

        isSlowing = false;
        Time.timeScale = 1f;
        Time.fixedDeltaTime = defaultFixedDeltaTime;

        if (currentSlowTime <= 0f)
            cooldownTimer = cooldown;
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = defaultFixedDeltaTime;
    }

    public bool IsSlowing()
    {
        return isSlowing;
    }

    public float GetSlowPercent()
    {
        return maxDuration <= 0f ? 0f : currentSlowTime / maxDuration;
    }

    public float GetCooldownRemaining()
    {
        return Mathf.Max(0f, cooldownTimer);
    }
}