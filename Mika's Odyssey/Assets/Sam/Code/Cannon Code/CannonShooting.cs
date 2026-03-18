using UnityEngine;

public class CannonShooting : MonoBehaviour, IInteractable
{
    [SerializeField]
    Projection projection;
    bool canonON = false;

  
    void Update()
    {
        //Controls();
        if (canonON) // quand je vais faire le mode canon, je vais faire une touche speciale pour partir du canon
        projection.TrajectorySimulation(projectillePrefab, ProjectilleSpawn.position, ProjectilleSpawn.forward * shootForce);
    }



    [SerializeField] Projectille projectillePrefab;
    [SerializeField] float shootForce;
    [SerializeField] Transform ProjectilleSpawn;


    [SerializeField] KeyCode shootingKeycode = KeyCode.None; // a changer 

    //private void Controls() 
    //{
    //    if (Input.GetKeyDown(shootingKeycode))
    //    {
    //        var spawned = Instantiate(projectillePrefab,ProjectilleSpawn.position,ProjectilleSpawn.rotation);
    //        spawned.Init(ProjectilleSpawn.forward * shootForce, false);
    //    }
    //}

    public void Interact()
    {
        canonON = true;
        shootingKeycode = KeyCode.E;
        if (Input.GetKeyDown(shootingKeycode))
        {
            var spawned = Instantiate(projectillePrefab, ProjectilleSpawn.position, ProjectilleSpawn.rotation);
            spawned.Init(ProjectilleSpawn.forward * shootForce, false);
        }
    }
}
