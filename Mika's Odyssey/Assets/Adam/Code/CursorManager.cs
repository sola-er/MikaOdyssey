using UnityEngine;

<<<<<<< HEAD

    
=======
public class CursorManager : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
    }

    private void Update()
    {
        // Echap pour liberer le curseur (utile en dev)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
>>>>>>> e61b94d262d097f3a571c78e117335caf3e28dd5
