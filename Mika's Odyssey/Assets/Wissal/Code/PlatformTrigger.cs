using UnityEngine;
using System.Collections;

public class PlatformTrigger : MonoBehaviour
{
    [SerializeField] private GameObject fourthPlatform;
    [SerializeField] private Vector3 raisedPosition;
    [SerializeField] private float riseSpeed = 2f;
    private bool hasTriggered = false;
    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
            Planet1Manager.Singleton.mochiQuest = MochiQuestState.PlatformIsGone;
            StartCoroutine(RaisePlatform());
        }
    }
    private IEnumerator RaisePlatform()
    {
        while (Vector3.Distance(fourthPlatform.transform.position, raisedPosition) > 0.01f)
        {
            fourthPlatform.transform.position = Vector3.MoveTowards(
                fourthPlatform.transform.position,
                raisedPosition,
                riseSpeed * Time.deltaTime
            );
            yield return null;
        }
        fourthPlatform.transform.position = raisedPosition;
    }
}