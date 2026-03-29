using UnityEngine;
using UnityEngine.UI;

public class SpaceTravelToggle : MonoBehaviour
{
    private Toggle toggle;
    private MovementWithGravity playerMovement;

    void Start()
    {
        playerMovement = FindFirstObjectByType<MovementWithGravity>();
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(OnToggleChanged);
    }

    void OnToggleChanged(bool val)
    {
        playerMovement.SetSpaceTravellingMode(val);
    }
}