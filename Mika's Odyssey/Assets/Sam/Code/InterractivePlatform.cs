using UnityEngine;
using UnityEngine.Events;

public class steppingplatform : MonoBehaviour
{
    [SerializeField] GameObject InterractiveGameObject;

    [Header("Color States")]
    public Color activeColor = Color.green;


    private Renderer _renderer;
    public Color inactiveColor;


    bool IsActivated = false;   // its unactivated by default

    [SerializeField]
    UnityEvent actionToPerform;

    // its activated with the gameobject is on the platform
    public void OnTriggerStay(Collider other) //maybe switch this to onTriggerEnter
    {
        if (InterractiveGameObject != null)
        {
            if (other.CompareTag("InterractiveGameObject"))
            {
                IsActivated = true;
                UpdateVisuals();
            }
        }


    }

    // the platform is disabled
    private void OnTriggerExit(Collider other)
    {
        IsActivated = false ;
        UpdateVisuals() ;
    }

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        inactiveColor = _renderer.material.color;
        UpdateVisuals();
    }

    public void UpdateVisuals()
    {
        if (_renderer != null)
        {
            _renderer.material.color = IsActivated ? activeColor : inactiveColor;
        }
    }

    public void InterractiveTest()
    {
        Debug.Log("the platform is operational!!");
    }


    // thing that happens here
    private void Update()
    {
        if (IsActivated) // if the platform is on
            actionToPerform.Invoke();
            

    }
}
