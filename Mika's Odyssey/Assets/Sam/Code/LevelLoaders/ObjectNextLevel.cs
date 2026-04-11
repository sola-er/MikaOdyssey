using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectNextLevel : MonoBehaviour, IInteractable
{
    public void Interact()
    {
       
        
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); 
        
    }

  
    
        
      
    
    private void FixedUpdate()
    {
        
    }
}
