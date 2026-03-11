using UnityEngine;

public class CannonShooting : MonoBehaviour
{
    [SerializeField]
    Projection projection;

  
    void Update()
    {
        Controls();
        projection.TrajectorySimulation(projectillePrefab, ProjectilleSpawn.position, ProjectilleSpawn.forward * shootForce);
    }



    [SerializeField] Projectille projectillePrefab;
    [SerializeField] float shootForce;
    [SerializeField] Transform ProjectilleSpawn;


    [SerializeField] KeyCode shootingKeycode = KeyCode.Space;

    private void Controls() 
    {
        if (Input.GetKeyDown(shootingKeycode))
        {
            var spawned = Instantiate(projectillePrefab,ProjectilleSpawn.position,ProjectilleSpawn.rotation);
            spawned.Init(ProjectilleSpawn.forward * shootForce, false);
        }
    }
}
