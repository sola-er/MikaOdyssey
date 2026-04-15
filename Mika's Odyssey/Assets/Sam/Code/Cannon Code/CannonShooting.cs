using Unity.VisualScripting;
using UnityEngine;

public class CannonShooting : MonoBehaviour, IInteractable
{
    
    bool canonON = false;

    [Header("Mode")]
    [SerializeField]
    bool cannonControls = true;

    [Header("Ui Related")]
    [SerializeField]
    GameObject forceUi;
  
    void Update()
    {
        if (cannonControls) { Controls(); }
        else if (!cannonControls)
            forceUi.SetActive(false);

        if (canonON) // quand je vais faire le mode canon, je vais faire une touche speciale pour partir du canon
        projection.TrajectorySimulation(projectillePrefab, ProjectilleSpawn.position, ProjectilleSpawn.forward * shootForce);
    }

    private void Start()
    {
        currentRotationY = BarrelPivot.localEulerAngles.y;
        if (currentRotationY > 180)
            currentRotationY -= 360;
    }

    [Header("Cannon shooting")]
    [SerializeField] Projection projection;
    [SerializeField] Projectille projectillePrefab;
    [SerializeField] float shootForce;
    [SerializeField] Transform ProjectilleSpawn;
    [SerializeField] KeyCode shootingKeycode = KeyCode.None; // a changer 

    [Header("Cannon part Movement")]
    [SerializeField] Transform rightwheel;
    [SerializeField] Transform leftwheel;
    [SerializeField] Transform BarrelPivot;
    [SerializeField] float rotatespeed = 2;

    float currentRotationY;
    public float sensitivity = 50f;

    private void Controls() // ajouter le bouton escape pour switcher de controls
    {
        forceUi.SetActive(true);
         //////////////// (Up, Down)
        if (Input.GetKey(KeyCode.S))
        {
            currentRotationY -= rotatespeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            currentRotationY += rotatespeed * Time.deltaTime;
        }

        currentRotationY = Mathf.Clamp(currentRotationY, -99f, 0f); // on veut pas que sa aille plus bas que le sol
        BarrelPivot.localRotation = Quaternion.Euler(0, currentRotationY, 0);
        ////////////////////

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.back * rotatespeed * Time.deltaTime); // this is all good
            leftwheel.Rotate(Vector3.up * rotatespeed * Time.deltaTime);
            rightwheel.Rotate(Vector3.down * rotatespeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.forward * rotatespeed * Time.deltaTime); // this is all good
            leftwheel.Rotate(Vector3.down * rotatespeed * Time.deltaTime);
            rightwheel.Rotate(Vector3.up * rotatespeed * Time.deltaTime);
        }


        ///////////////////////
        if (Input.GetKeyDown(shootingKeycode))
        {
            var spawned = Instantiate(projectillePrefab, ProjectilleSpawn.position, ProjectilleSpawn.rotation);
            spawned.Init(ProjectilleSpawn.forward * shootForce, false);

            //_launchParticles.Play();
            //_source.PlayOneShot(_clip);
        }
        ///////////////////////
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            cannonControls = false;
            
        }
    }

    public void Interact() // faire en sorte que lon aille en mode cannon
    {

        cannonControls = true;

            //if (Input.GetKeyDown(shootingKeycode))
            //{
            //    //var spawned = Instantiate(projectillePrefab, ProjectilleSpawn.position, ProjectilleSpawn.rotation);
            //    //spawned.Init(ProjectilleSpawn.forward * shootForce, false);
            //}
            canonON = true;
            
        
      
    }

    public void UpdatedForce(float sliderNewValue) // to update the force in the UI
    {
        shootForce = sliderNewValue;
        Debug.Log("Cannon Force set to:" + shootForce);
    }
}
