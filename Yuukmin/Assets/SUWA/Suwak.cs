using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Suwak : MonoBehaviour
{
    [SerializeField] float suwaDuration;
    [SerializeField] float suwaTimer;
    [SerializeField] Transform yuuk;
    [SerializeField] bool follow;
    [SerializeField] float distance;
    [SerializeField] bool charging;
    [SerializeField] ParticleSystem dirt;
    [SerializeField] ParticleSystem[] particles;
    [SerializeField] bool touched;
    [SerializeField] Animator suwaAnim;
    [SerializeField] Material skyMat;
    [SerializeField] Color lightColor;
    [SerializeField] Material skyMatNorm;
    [SerializeField] Color lightColorNorm;
    [SerializeField] AudioClip yTMusic;
    [SerializeField] Light lighty;
    [SerializeField] AudioClip ground;
    [SerializeField] AudioClip Ofrognata;
    [SerializeField] AudioSource musics;
    float chargeTimer;
    [SerializeField] float rad;
    [SerializeField] LayerMask fairy;

    [SerializeField] AudioClip hori1;
    [SerializeField] AudioClip hori2;

    Coroutine loopingNoises;
    int touchedHowmany;
    private void Start()
    {
        //StartCoroutine(Angry()); //DUNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNM
    }
    private void Update()
    {
        if (follow)
        {
            suwaTimer += Time.deltaTime;
            chargeTimer += Time.deltaTime;
            if(!charging && chargeTimer >= 5)
            {
                StartCoroutine(Charge());
            }
        }
        if(suwaTimer >= suwaDuration)
        {
            End(yTMusic);
        }
    }
    public void End(AudioClip music)
    {
        if(loopingNoises != null)
            StopCoroutine(loopingNoises);
        musics.clip = music;
        musics.Play();
        RenderSettings.skybox = skyMatNorm;
        lighty.color = lightColorNorm;
        follow = false;
        gameObject.SetActive(false);
    }
    private void FixedUpdate()
    {
        if(follow)
        {
            Collider[] fairyCol = Physics.OverlapSphere(transform.position, rad, fairy);
            for (int i = 0; i < fairyCol.Length; i++)
            {
                fairyCol[i].transform.GetComponent<FairyCombat>().Hurt(9999);
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(transform.position, rad);
    }

    
    IEnumerator Charge()
    {
        charging = true;
        float chargeDuration = 0;
        Vector3 destin = transform.position + (yuuk.position - transform.position).normalized * distance;
        Vector3 chargeStart = transform.position;
        while(chargeDuration <= 3)
        {
            transform.position = new Vector3(Mathf.SmoothStep(chargeStart.x, destin.x, chargeDuration / 3), transform.position.y, Mathf.SmoothStep(chargeStart.z, destin.z, chargeDuration / 3));
            chargeDuration += Time.deltaTime;
            yield return null;
        }
        charging = false;
        chargeTimer = 0;
    }
    public void Touch()
    {
        touchedHowmany++;
        if (!touched && touchedHowmany > 10)
        {
            touched = true;
            StartCoroutine(Angry());
        }
    }

    IEnumerator Angry()
    {
        dirt.Play();
        SFXSpawner.instance.PlaySFX(ground, transform, 0.8f);
        yield return new WaitForSeconds(3);

        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].Play();
        }

        yield return new WaitForSeconds(5);
        musics.clip = Ofrognata;
        musics.Play();
        suwaAnim.SetTrigger("Wake");
        follow = true;
        SFXSpawner.instance.PlaySFX(hori2, transform, 0.8f); //11
        //SFXSpawner.instance.PlaySFX(hori1, transform, 0.8f); //19
        loopingNoises = StartCoroutine(LoopSounds());
        RenderSettings.skybox = skyMat;
        lighty.color = lightColor;
    }
    IEnumerator LoopSounds()
    {
        float horiTimer = 0;
        float horiTimer2 = 0;
        while (follow)
        {
            horiTimer += Time.deltaTime;
            horiTimer2 += Time.deltaTime;
            if (horiTimer >= 9)
            {
                horiTimer = 0;
                SFXSpawner.instance.PlaySFX(hori2, transform, 0.5f); //11
                Debug.Log("9");
            }
            if (horiTimer2 >= 17)
            {
                horiTimer2 = 0;
                //SFXSpawner.instance.PlaySFX(hori1, transform, 0.8f); //19
                Debug.Log("17");
            }
            yield return null;
        }
        yield break;
    }
}
