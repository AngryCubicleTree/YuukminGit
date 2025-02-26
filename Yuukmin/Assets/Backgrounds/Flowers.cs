using UnityEngine;

public class Flowers : MonoBehaviour
{
    [SerializeField] float lastTime;
    [SerializeField] float interval;
    [SerializeField] bool reverse;

    [SerializeField] float maxAngleZ;
    [SerializeField] float randomRange;
    float randomIntervalAdd;

    private void OnEnable()
    {
        randomIntervalAdd = Random.Range(0, randomRange);
    }

    // Update is called once per frame
    void Update()
    {

        if(Time.time >= lastTime + interval + randomIntervalAdd)
        {
            lastTime = Time.time;
            reverse = !reverse;
            //startAngleZ = transform.rotation.z;
        }
        if(!reverse)
            transform.rotation = Quaternion.Euler(0,0,Mathf.SmoothStep(-maxAngleZ, maxAngleZ, (Time.time - lastTime) / (interval + randomIntervalAdd)));
        if (reverse)
            transform.rotation = Quaternion.Euler(0, 0, Mathf.SmoothStep(maxAngleZ, -maxAngleZ, (Time.time - lastTime) / (interval + randomIntervalAdd)));

    }
}
