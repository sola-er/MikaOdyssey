using UnityEngine;
using System.Collections;

public class PlatformTrigger : MonoBehaviour
{
    [SerializeField] private GameObject fourthPlatform;
    [SerializeField] private Vector3 raisedPosition;
    [SerializeField] private float riseSpeed = 2f;
    private Vector3 normalPosition;
    private bool hasTriggered = false;
    private void Awake()
    {
        normalPosition = fourthPlatform.transform.position;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
            StartCoroutine(MovePlatform(raisedPosition));
            Planet1Manager.Singleton.AdvanceQuest(MochiQuestState.PlatformIsGone);
        }
    }
    private IEnumerator MovePlatform(Vector3 end)
    {
        while (Vector3.Distance(fourthPlatform.transform.position, end) > 0.01f)
        {
            fourthPlatform.transform.position = Vector3.MoveTowards(
                fourthPlatform.transform.position,
                end,
                riseSpeed * Time.deltaTime
            );
            yield return null;
        }
        fourthPlatform.transform.position = end;
    }
    public void MoveBack()
    {
        StartCoroutine(MovePlatform(normalPosition));
    }
}