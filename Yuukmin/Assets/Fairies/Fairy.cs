using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Splines.ExtrusionShapes;

public class Fairy : MonoBehaviour
{
    public FairyFlowers homeFlower;
    public bool sunfae;
    public bool watfae;

    [Header("Idling")]
    //[SerializeField] float idleTimer;
    [SerializeField] bool idling;
    [SerializeField] ParticleSystem idleParticle;

    [Header("Carry")]

    [SerializeField] AudioClip hai;
    public Gap gap; //destination
    [SerializeField] float carrySpeedLW; //carry speed with LightWeight object
    [SerializeField] bool carryingLightWeight;
    [SerializeField] float carryableScanRad;
    [SerializeField] LayerMask carryableLayer; //Change to Tag!!
    [SerializeField] Carryable lightTarget;
    float carryStartY;

    [Header("Enemy")]
    [SerializeField] FairyCombat combat;
    [SerializeField] LayerMask enemyLayer; //Change to Tag!!

    [Header("General")]
    [SerializeField] AudioClip[] unya;
    [SerializeField] AudioClip fairy1;
    [SerializeField] float normalSpeed;
    public Transform yuuka;
    public bool followPlayer;
    [SerializeField] bool movingToCursor;
    [SerializeField] Vector3 offset;
    public NavMeshAgent navAg;
    [SerializeField] Transform spriteContainer;

    [SerializeField] Vector3 followSphereOffset; //PlayerFollowOffset
    [SerializeField] float followSphereScale; //Radius!

    [Header("Anims")]
    [SerializeField] Animator animator;
    [SerializeField] float blikTime;
    float blinkElapsedTime;

    [Header("Debug")]
    [SerializeField] float navVelMag;

    private void OnEnable()
    {
        followSphereOffset = new Vector3(Random.Range(-followSphereScale, followSphereScale), 0, Random.Range(-followSphereScale, followSphereScale));
        SFXSpawner.instance.PlaySFX(unya[Random.Range(0, 3)], transform, 1);
    }
    private void Update()
    {
        //Show...
        navVelMag = navAg.velocity.magnitude; //Magnitude of speed.

        Blink();
        //flip sprites according to velocity if moving.
        if (navVelMag > 0)
        {
            if (navAg.velocity.x > 0)
                spriteContainer.localScale = new Vector3(-1, 1, 1);
            else
                spriteContainer.localScale = new Vector3(1, 1, 1);
        }

        IdleCheck();

        if (followPlayer)
            navAg.SetDestination(yuuka.position + offset + followSphereOffset);
            //navAg.destination = yuuka.position + offset;

        if (!followPlayer && movingToCursor) //isn't following player But moving toward cursor,
            if(navVelMag <= 0)
                if (Vector3.Distance(transform.position, navAg.destination) <= navAg.stoppingDistance) //stopped and close enough
                {
                    movingToCursor = false;
                    SearchDestination(); //Start searching for things to do
                }
        if(carryingLightWeight) //if carried lightW object to destination, 
            if (navVelMag <= 0)
                if (Vector3.Distance(transform.position, navAg.destination) <= navAg.stoppingDistance) //stopped and close enough
                {
                    DeliverLightWeight();
                }
    }
    void IdleCheck()
    {
        if (!idling)
            if (!followPlayer && !movingToCursor && !carryingLightWeight && !combat.attacking)
            {
                idling = true;
                idleParticle.Play();
            }

        if (idling)
            if (followPlayer || movingToCursor || carryingLightWeight || combat.attacking)
            {
                idling = false;
                idleParticle.Stop();
            }
    }

    void SearchDestination()
    { //                                                                             PlaceHolder !1111111111111111111111111111111111!
        Collider[] enemyCol = Physics.OverlapSphere(transform.position, carryableScanRad + 1, enemyLayer); //make list of all "enemy" layer objects nearby
        Collider[] carryableCol = Physics.OverlapSphere(transform.position, carryableScanRad, carryableLayer); //make list of all "carryable" layer objects nearby
        if (enemyCol.Length > 0) //Detected Enemy !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! Detected Enemy
        {
            float minDistance = Mathf.Infinity; //To calculate at start.
            int nearestIndex = 0; //Index of nearest object that will be calculated under this.
            for (int i = 0; i < enemyCol.Length; i++)
            {
                float objectDistance = Vector3.Distance(transform.position, enemyCol[i].transform.position);
                if (objectDistance < minDistance)
                {
                    minDistance = objectDistance; //to compare again.
                    nearestIndex = i; //setting nearest object's index.
                }
            }
            AttackStart(enemyCol[nearestIndex].transform);
            Debug.Log("Fairy : Enemies!!!");
            return; //break out of this.
        }
        Debug.Log("Fairy : Search!");
        if (carryableCol.Length > 0) //if there is more than one,
        {
            ////Find nearest object
            float minDistance = Mathf.Infinity; //To calculate at start.
            int nearestIndex = 0; //Index of nearest object that will be calculated under this.
            for (int i = 0; i < carryableCol.Length; i++)
            {
                float objectDistance = Vector3.Distance(transform.position, carryableCol[i].transform.position);
                if (objectDistance < minDistance)
                {
                    minDistance = objectDistance; //to compare again.
                    nearestIndex = i; //setting nearest object's index.
                }
            }


            //Set carryable component
            Carryable carryTarget = carryableCol[nearestIndex].gameObject.GetComponent<Carryable>();

            if (!carryTarget.beingCarried) //if it's not being carried
            {
                if (carryTarget.lightWeight) //and LightWeight,
                {
                    lightTarget = carryTarget; //assign as lightW carryable
                    lightTarget.LightWeightCarry(); //tell it that it's being carried.
                    CarryLightWieght();
                }
            }
        }
    }

    void AttackStart(Transform target)
    {
        combat.attackTarget = target;
        combat.attacking = true;
    }
    void AttackCancel()
    {
        combat.attacking = false;
    }

    #region LightWeightStuffs
    void CarryLightWieght()
    {
        SFXSpawner.instance.PlaySFX(hai, transform, 1);
        navAg.speed = carrySpeedLW;
        carryingLightWeight = true;
        lightTarget.transform.SetParent(transform); //set parent
        carryStartY = lightTarget.transform.localPosition.y;
        lightTarget.transform.localPosition = new Vector3(0, 10f, 0); //put it on top of fairy's head

        navAg.SetDestination(gap.transform.position);
    }
    void DropLightWeight()
    {
        navAg.speed = normalSpeed;
        lightTarget.LightWeightDrop(); //Tell it's dropped
        lightTarget.transform.localPosition = new Vector3(0, carryStartY, 0); //Put down
        lightTarget.transform.SetParent(null); //unparent

        lightTarget = null; //empty target
        carryingLightWeight = false; //Not anymore!
    }
    void DeliverLightWeight()
    {
        lightTarget.LightWeightDrop(); //Tell it's dropped
        lightTarget.transform.SetParent(gap.transform); //SetParent
        lightTarget.gameObject.SetActive(false); //deactivate

        gap.LightItemDeposited(lightTarget);

        lightTarget = null; //empty target
        carryingLightWeight = false; //Not anymore!
        Debug.Log("Fairy : Delivery!");
    }
    #endregion
    //Yuuk-Fairy Functions
    public void Whistled()
    {
        Invoke(nameof(WhistleNoise), Random.Range(0, 0.4f));
        followPlayer = true;
        if(carryingLightWeight)
            DropLightWeight();
        if (combat.attacking)
            AttackCancel();
    }
    void WhistleNoise()
    {
        SFXSpawner.instance.PlaySFX(fairy1, transform, 1);
    }
    public void Thrown(Vector3 destination)
    {
        int audioRng = Random.Range(0, 3);
        SFXSpawner.instance.PlaySFX(unya[audioRng], transform, 1);
        movingToCursor = true;
        followPlayer = false;
        navAg.SetDestination(destination);
    }



    //BlinkingAnimation
    void Blink()
    {
        blinkElapsedTime += Time.deltaTime;
        if (blinkElapsedTime > blikTime)
        {
            animator.SetTrigger("Blink");
            blikTime = Random.Range(4, 10);
            blinkElapsedTime = 0;
        }
    }
}
