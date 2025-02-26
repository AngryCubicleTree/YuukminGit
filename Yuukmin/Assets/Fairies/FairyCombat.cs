using UnityEngine;

public class FairyCombat : MonoBehaviour
{
    public int hitPoint;
    [SerializeField] bool immune;
    [SerializeField] float immuneTime;
    public bool attacking;
    float immunityTimer;
    public Transform attackTarget;

    [SerializeField] float attackDelay;
    [SerializeField] float attacktimer;

    [Header("Sounds")]
    [SerializeField] AudioClip hurt1;
    [SerializeField] AudioClip[] ya;

    [SerializeField] Gap spawnAreaGap;
    [SerializeField]Fairy mainF;
    private void Start()
    {
        spawnAreaGap = mainF.gap;
    }
    void Update()
    {
        if (attacking)
        {
            attacktimer += Time.deltaTime;
            mainF.navAg.SetDestination(attackTarget.position);

            if (attacktimer >= attackDelay)
                if (Vector3.Distance(attackTarget.position, transform.position) <= mainF.navAg.stoppingDistance + 1.5f)
                {
                    if (!attackTarget.GetComponent<SphereCollider>().enabled)
                        attacking = false;
                    if (attackTarget.GetComponent<Spirit>() != null)
                    {
                        SFXSpawner.instance.PlaySFX(ya[Random.Range(0, 1)], transform, 1);
                        Spirit spirit = attackTarget.GetComponent<Spirit>();
                        spirit.hitPoint -= 1;
                        DFXSpawner.instance.SpawnDFX(attackTarget, 1, new Color(1, 1, 0));
                        if (spirit.hitPoint <= 0)
                        {
                            attackTarget = null;
                            attacking = false;
                        }
                        attacktimer = 0;
                    }
                    else if (attackTarget.GetComponent<Cirno>() != null)
                    {
                        SFXSpawner.instance.PlaySFX(ya[Random.Range(0, 1)], transform, 1);
                        attackTarget.GetComponent<Cirno>().Hurt();
                        DFXSpawner.instance.SpawnDFX(attackTarget, 1, new Color(1, 1, 0));

                        attacktimer = 0;
                    }
                    else if (attackTarget.GetComponent<Suwak>() != null)
                    {
                        SFXSpawner.instance.PlaySFX(ya[Random.Range(0, 1)], transform, 1);
                        attackTarget.GetComponent<Suwak>().Touch();
                        DFXSpawner.instance.SpawnDFX(attackTarget, 1, new Color(1, 1, 0));

                        attacktimer = 0;
                    }
                }
        }

        if (immune)
        {
            immunityTimer += Time.deltaTime;
            if(immunityTimer >= immuneTime)
            {
                immunityTimer = 0;
                immune = false;
            }
        }
    }
    public void Hurt(int damage)
    {
        if (!immune)
        {
            SFXSpawner.instance.PlaySFX(hurt1, transform, 1);
            immune = true;
            hitPoint -= damage;

            if(hitPoint <= 0)
            {
                mainF.followPlayer = false;
                mainF.gap = spawnAreaGap;
                gameObject.SetActive(false);
                Debug.Log(gameObject.name + " : Pichuun!");
            }
        }
    }
}
