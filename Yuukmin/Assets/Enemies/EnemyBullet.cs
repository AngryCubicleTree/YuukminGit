using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [Header("BulletCondition")]
    [SerializeField] bool fullfilledDuty;
    [SerializeField] bool move;
    [Header("BulletStatus")]
    [SerializeField] int bulletDamage;
    [SerializeField] float bulletSpeed;
    [SerializeField] float lifeTime;
    float timerLifetime;
    [SerializeField] float particleLifetime;
    [SerializeField] ParticleSystem[] particleSys;
    [SerializeField] ParticleSystem[] deathParticleSys;
    [SerializeField] GameObject[] spriteObjects;
    [SerializeField] FairyCombat targetFairy;
    [SerializeField] LayerMask fairy;

    [Header("InUnity")]
    [SerializeField] Color colorforfun;
    [SerializeField] float capsuleRad;
    [SerializeField] float lengthHalf;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = colorforfun;
        Gizmos.DrawSphere(transform.position + transform.forward * lengthHalf, capsuleRad);

        Gizmos.DrawSphere(transform.position - transform.forward * lengthHalf, capsuleRad);
    }


    private void Update()
    {
        if(!fullfilledDuty)
        {
            timerLifetime += Time.deltaTime;
            if(timerLifetime >= lifeTime)//Dies
            {
                Die();
            }
        }


        if(!fullfilledDuty && move)
        {
            transform.position += transform.forward * bulletSpeed;
            /*
            //Vector3 aim = (targetFairy.transform.position - transform.position).normalized; homming WIP~!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            //float angle = Mathf.Atan2(aim.x, aim.z) * Mathf.Rad2Deg;
            //transform.rotation = Quaternion.Euler(0, Mathf.SmoothStep(transform.rotation.y, angle, roationSpeed), 0);
            */
        }
    }
    void Die()
    {
        Debug.Log("bulletDead");
        for (int i = 0; i < spriteObjects.Length; i++)
        {
            spriteObjects[i].SetActive(false);
        }
        if (particleSys.Length >0)
            for (int i = 0; i < particleSys.Length; i++)
            {
                particleSys[i].Stop();
            }
        if (deathParticleSys.Length > 0)
            for (int i = 0; i < deathParticleSys.Length; i++)
            {
                deathParticleSys[i].Play();
            }
        fullfilledDuty = true;
        Destroy(gameObject, particleLifetime);
    }
    public void Fired(Vector3 targetPos, float speed, float lifeTimeA)
    {
        fullfilledDuty = false;
        timerLifetime = 0;
        lifeTime = lifeTimeA;
        bulletSpeed = speed;
        Vector3 aim = (targetPos - transform.position).normalized;
        float angle = Mathf.Atan2(aim.x, aim.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, angle, 0);

        move = true;
    }
    private void FixedUpdate()
    {
        if(!fullfilledDuty)
        { //Optimization Required!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!Optimization Required//
            //Collider[] fairyCol = Physics.OverlapSphere(transform.position, fairyScanRad, fairyLayer);
            Collider[] fairyCol = Physics.OverlapCapsule(transform.position + transform.forward * lengthHalf, transform.position - transform.forward * lengthHalf, capsuleRad, fairy);
            if (fairyCol.Length > 0)
            {
                if (fairyCol[0].gameObject.GetComponent<FairyCombat>() == null)
                    return; //Null? Kaboom!
                 targetFairy = fairyCol[0].gameObject.GetComponent<FairyCombat>();
                DFXSpawner.instance.SpawnDFX(fairyCol[0].transform, 1, new Color(1,0,0));
                targetFairy.Hurt(bulletDamage);

                Die();
            }
        }
    }
}
