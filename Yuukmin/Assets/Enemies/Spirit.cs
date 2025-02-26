using Unity.VisualScripting;
using UnityEngine;

public class Spirit : MonoBehaviour
{
    [Header("Enemy")]
    public bool dead;
    public int hitPoint;
    [SerializeField] ParticleSystem deathParticle;
    [SerializeField] GameObject etScale;

    [Header("Bullets")]
    [SerializeField] EnemyBullet bullet;
    [SerializeField] float bulletSpeed;
    [SerializeField] float bulletLifeT;

    [SerializeField] float checkTime;
    float checkTimer;

    [SerializeField] LayerMask fairyLayer;
    [SerializeField] float fairyScanRad;
    void Update()
    {
        if (!dead)
            if (hitPoint <= 0)
            {
                gameObject.SetActive(false);
                dead = true;
                for (int i = 0; i < Random.Range(2,7); i++)
                {
                    Vector3 randomPos = new Vector3(transform.position.x + Random.Range(0, 2f), 0.41f, transform.position.z + Random.Range(0, 2f));
                    Instantiate(etScale, randomPos, Quaternion.identity);
                }
                PFXSpawner.instance.PlayPFX(deathParticle, transform.position, 3);
            }

        checkTimer += Time.deltaTime;
        if(checkTimer >= checkTime)
        {
            checkTimer = 0;
            NearbyCheck();
        }
    }
    void NearbyCheck()
    {
        Collider[] fairyCol = Physics.OverlapSphere(transform.position, fairyScanRad, fairyLayer);
        if(fairyCol.Length > 0)
        {
            SHoot(fairyCol[0].transform.position);
            int rngFairyIndex = Random.Range(0, fairyCol.Length);
            Debug.Log(fairyCol[rngFairyIndex].gameObject.name);
        }
    }
    void SHoot(Vector3 targetPos)
    {
        EnemyBullet firedBullet = Instantiate(bullet, transform.position, Quaternion.identity);
        firedBullet.Fired(targetPos,bulletSpeed, bulletLifeT);
    }
}
