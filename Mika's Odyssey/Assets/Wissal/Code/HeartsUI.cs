using UnityEngine;
using UnityEngine.UI;

public class HeartsUI : MonoBehaviour
{
    [SerializeField] private Image[] hearts;

    private void OnEnable()
    {
        HeartsManager.OnHeartsChanged += UpdateHearts;
    }

    private void OnDisable()
    {
        HeartsManager.OnHeartsChanged -= UpdateHearts;
    }

    private void UpdateHearts(int currentHearts)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].enabled = i < currentHearts;
            // hearts[i].color = i < currentHearts ? Color.white : Color.gray;
        }
    }
}