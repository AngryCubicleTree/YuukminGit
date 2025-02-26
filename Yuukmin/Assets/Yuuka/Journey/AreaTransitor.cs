using UnityEngine;

public class AreaTransitor : MonoBehaviour
{
    //[SerializeField] LayerMask YuukPlayer;
    [SerializeField] Gap gap;
    [SerializeField] Vector3 destination;
    [SerializeField] YuukaJourneyManager YuukJM;
    [SerializeField] string yuukTag;
    [SerializeField] bool traveled;
    [SerializeField] float interval;
    [SerializeField] float tiemr;
    [SerializeField] AudioClip areaMusic;
    private void OnTriggerEnter(Collider other)
    {
        if (!traveled)
            if (other.transform.gameObject.tag == yuukTag)
        {
            traveled = true;
            Debug.Log("Yuuk!");
            YuukJM.Transport(destination, gap, areaMusic);
        }
    }
    private void Update()
    {
        if (traveled)
        {
            tiemr += Time.deltaTime;
            if(tiemr > interval)
            {
                traveled = false;
                tiemr = 0;
            }
        }
    }
}
