using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Panneaux")]
    [SerializeField] private AutoMover leftPanel;
    [SerializeField] private AutoMover rightPanel;

    [Header("Settings")]
    [SerializeField] private bool startOpen = false;

    private bool isOpen = false;

    private void Start()
    {
        if (startOpen) Open();
        else Close();
    }

    public void Open()
    {
        if (isOpen) return;
        isOpen = true;
        leftPanel?.MoveToEnd();
        rightPanel?.MoveToEnd();
    }

    public void Close()
    {
        if (!isOpen) return;
        isOpen = false;
        leftPanel?.ResetToStart();
        rightPanel?.ResetToStart();
    }

    public void Toggle()
    {
        if (isOpen) Close();
        else Open();
    }

    public bool IsOpen() => isOpen;
}