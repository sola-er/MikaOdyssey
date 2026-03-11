using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Projection : MonoBehaviour // this is a made in progress, might not actually use this one cause the fps gets tanked
{
    private Scene simulationScene;
    private PhysicsScene physicsScene;
    [SerializeField] private Transform obstaclesParent;

    [SerializeField] private LineRenderer lineR;
    [SerializeField] private int maxPhysicsFrameIteration; // this will also be how far the ghost balls go, the less it is the less we will see in the future so less work in the end

    void CreatePhysicsScene()
    {
        simulationScene = SceneManager.CreateScene("Simulation", new CreateSceneParameters(LocalPhysicsMode.Physics3D));
        physicsScene = simulationScene.GetPhysicsScene();

        foreach (Transform obj in obstaclesParent) // we need to attribute every things that will collide with it to this main object parent
        {
            var ghostObj = Instantiate(obj.gameObject,obj.position,obj.rotation);
            ghostObj.GetComponent<Renderer>().enabled = false;
            SceneManager.MoveGameObjectToScene(ghostObj, simulationScene);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TrajectorySimulation(Projectille ProjectillePrefb, Vector3 pos, Vector3 velocity)
    {
        var ghostObj = Instantiate(ProjectillePrefb, pos, Quaternion.identity);
        SceneManager.MoveGameObjectToScene(ghostObj.gameObject, simulationScene); // probably best to put in a function

        ghostObj.Init(velocity);

        lineR.positionCount = maxPhysicsFrameIteration;

        for (int i = 0; i < maxPhysicsFrameIteration; i++) // i need to add object pooling right here, else my computer will crash
        {
            physicsScene.Simulate(Time.fixedDeltaTime);
            lineR.SetPosition(i,ghostObj.transform.position);
        }
        Destroy(ghostObj.gameObject);
    }
}
