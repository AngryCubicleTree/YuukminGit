using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Cirno : MonoBehaviour
{
    [SerializeField] int healthPoint;
    [SerializeField] bool cirnoAttack;
    [SerializeField] bool inCombat;
    [SerializeField] float boredTimer;
    [SerializeField] float shotSpeed;

    [Header("Main")]
    [SerializeField] YuukaFairyManager YFM;
    [SerializeField] EnemyBullet icicle1;
    [SerializeField] Transform sprites;
    [SerializeField] Transform yuuk;
    [SerializeField] Animator cirnoAnima;
    [SerializeField] ParticleSystem[] shootingParticles;
    [SerializeField] SphereCollider coll;
    [SerializeField] AudioSource musicPl;
    [SerializeField] AudioClip cirnoMusic;
    [SerializeField] Transform[] destinations;
    [SerializeField] NavMeshAgent nvAG;
    [SerializeField] bool defeated;
    [SerializeField] AbsoluteCinema absCine;

    [Header("sounds")]
    [SerializeField] AudioClip pew;
    [SerializeField] AudioClip vwomp;
    [SerializeField] AudioClip defeat;

    Coroutine attackRoutine;
    Coroutine loopCoro;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (inCombat)
        {
            boredTimer += Time.deltaTime;
            if(boredTimer >= 309)
            {
                inCombat = false;
                StartCoroutine(absCine.CirnoBored());
            }
        }
        Facing();
    }
    public void Hurt()
    {
        healthPoint--;
        if (!defeated & healthPoint <= 0)
            StartCoroutine(Defeat());
    }
    public void CirnoStart()
    {
        StartCoroutine(CombaStart());
    }

    IEnumerator Defeat()
    {
        coll.enabled = false;
        SFXSpawner.instance.PlaySFX(defeat, transform, 1);
        defeated = true;
        if(loopCoro != null)
        StopCoroutine(loopCoro);
        if (attackRoutine != null)
            StopCoroutine(attackRoutine);
        Debug.LogWarning("DEFEAT!");
        StartCoroutine(absCine.CirnoDefeat());
        yield return null;
    }
    public IEnumerator CombaStart()
    {
        if (YFM.fairies.Count >= 35)
            healthPoint = 1999;
        musicPl.clip = cirnoMusic;
        musicPl.Play();
        for (int i = 0; i < shootingParticles.Length; i++)
        {
            shootingParticles[i].Play();
        }

        cirnoAnima.SetBool("Shooting", true);
        cirnoAnima.SetTrigger("Shoot");
        SFXSpawner.instance.PlaySFX(vwomp, transform, 1);
        for (int i = 0; i < 10; i++)
        {
            SFXSpawner.instance.PlaySFX(pew, transform, 1);
            EnemyBullet icicle =  Instantiate(icicle1,transform.position + new Vector3(0, -1.435f,0),Quaternion.identity);
            icicle.Fired(yuuk.position, 0.02f, 10);
            yield return new WaitForSeconds(1f);
        }

        for (int i = 0; i < shootingParticles.Length; i++)
        {
            shootingParticles[i].Stop();
        }
        cirnoAnima.SetBool("Shooting", false);
        inCombat = true;
        loopCoro = StartCoroutine(AttackLoop());
    }

    IEnumerator AttackLoop()
    {
        nvAG.enabled = true;
        for (int i = 0; i < destinations.Length; i++)
        {
            int rng = Random.Range(0, 2);
            nvAG.SetDestination(destinations[i].position);
            yield return new WaitForSeconds(10f); //getting there

            coll.enabled = true;
            if (rng == 0)
                attackRoutine=StartCoroutine(AttackType1());
            else if (rng == 1)
                attackRoutine=StartCoroutine(AttackType2());
            while (cirnoAttack)
            {
                yield return null;
            }
            yield return new WaitForSeconds(2.9f); 
            coll.enabled = false;
        }
    }

    IEnumerator AttackType1()
    {
        /////Starting!
        cirnoAttack = true;
        cirnoAnima.SetBool("Shooting", true);
        cirnoAnima.SetTrigger("Shoot");
        SFXSpawner.instance.PlaySFX(vwomp, transform, 1);
        for (int i = 0; i < shootingParticles.Length; i++)
        {
            shootingParticles[i].Play();
        }
        /////Shooting!
        for (int i = 0; i < 20; i++)
        {
            SFXSpawner.instance.PlaySFX(pew, transform, 1);
            EnemyBullet icicle = Instantiate(icicle1, transform.position + new Vector3(0, -1.435f, 0), Quaternion.identity);
            icicle.Fired(yuuk.position, shotSpeed, 20);
            yield return new WaitForSeconds(0.5f);
        }
        /////END
        for (int i = 0; i < shootingParticles.Length; i++)
        {
            shootingParticles[i].Stop();
        }
        cirnoAnima.SetBool("Shooting", false);
        yield return new WaitForSeconds(1f);
        cirnoAttack = false;
    }
    IEnumerator AttackType2()
    {
        /////Starting!
        cirnoAttack = true;
        cirnoAnima.SetBool("Shooting", true);
        cirnoAnima.SetTrigger("Shoot");
        SFXSpawner.instance.PlaySFX(vwomp, transform, 1);
        for (int i = 0; i < shootingParticles.Length; i++)
        {
            shootingParticles[i].Play();
        }
        /////Shooting!
        for (int i = 0; i < 45; i++)
        {
            SFXSpawner.instance.PlaySFX(pew, transform, 1);
            EnemyBullet icicle = Instantiate(icicle1, transform.position + new Vector3(0, -1.435f, 0), Quaternion.identity);
            Vector3 rngVec = new Vector3(Random.Range(0, 3f), 0, Random.Range(0, 3f));
            icicle.Fired(yuuk.position + rngVec, shotSpeed, 20);
            yield return new WaitForSeconds(0.25f);
        }
        /////END
        for (int i = 0; i < shootingParticles.Length; i++)
        {
            shootingParticles[i].Stop();
        }
        cirnoAnima.SetBool("Shooting", false);
        yield return new WaitForSeconds(1f);
        cirnoAttack = false;
    }

    void Facing()
    {
        if (yuuk.position.x < transform.position.x)
            sprites.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        else
            sprites.localScale = new Vector3(-0.1f, 0.1f, 0.1f);
    }
}
