using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FairyFlowers : MonoBehaviour
{
    [SerializeField] bool assigned;
    [SerializeField] ParticleSystem Boosted;
    [SerializeField] ParticleSystem Burst;
    public bool canSummon;
    [SerializeField] List<Fairy> fairies = new List<Fairy>();
    [SerializeField] Fairy fairySummon;
    [SerializeField] int fairyNumber;
    [SerializeField] TextMeshPro count;
    [SerializeField] bool summoning;
    public Transform yuuk;
    public Gap gap;
    //[SerializeField] Transform moveTarget;
    //[SerializeField] Transform centerPos;
    //[SerializeField] float radius;
    //[SerializeField] float moveInterval;
    //float moveelapsedTime;

    private void OnEnable()
    {

    }
    void Update()
    {
        if(!assigned)
            if(yuuk != null)
            {
                if (fairies.Count <= 0)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        Fairy fae = Instantiate(fairySummon, transform.position, Quaternion.identity);
                        fae.gameObject.GetComponent<FairyCombat>().hitPoint = 10;
                        fae.yuuka = yuuk;
                        fae.gap = gap;
                        fae.gameObject.SetActive(false);
                        fae.homeFlower = this;
                        fairies.Add(fae);
                    }
                }
                assigned = true;
            }

        if (fairyNumber == 5)
            canSummon = false;
        if(!summoning && fairyNumber < 5)
            canSummon = true;
        /*
        moveelapsedTime += Time.deltaTime;
        if(moveelapsedTime > moveInterval)
        {
            moveelapsedTime = 0;
            moveTarget.position = new Vector3(centerPos.position.x + Random.Range(-radius, radius), moveTarget.position.y, centerPos.position.z + Random.Range(-radius, radius));
        }
        */
        CountFairies();
        count.text = (fairyNumber + " / 5");
    }
    void CountFairies()
    {
        int activeFairyCount = 0;
        for (int i = 0; i < 5; i++)
        {
            if (fairies[i].gameObject.activeInHierarchy)
            {
                activeFairyCount++;
            }
        }
        fairyNumber = activeFairyCount;
    }
    public void Fed()
    {
        canSummon = false;
        Debug.Log(gameObject.name + " : Fed!");
        StartCoroutine(Summon());
    }
    IEnumerator Summon()
    {
        summoning = true;
        count.gameObject.SetActive(true);
        Boosted.Play();
        yield return new WaitForSeconds(2);

        Vector3 randOffset = new Vector3(Random.Range(-1f,1f),0, Random.Range(-1f, 1f));
        //Fairy fairy = Instantiate(fairySummon, transform.position + randOffset, Quaternion.identity);
        for (int i = 0; i < 5; i++)
        {
            if (!fairies[i].gameObject.activeInHierarchy)
            {
                Debug.Log("Activate!");
                fairies[i].gameObject.SetActive(true);
                fairies[i].gameObject.GetComponent<FairyCombat>().hitPoint = 10;
                fairies[i].transform.position = transform.position + randOffset;
                break;
            }
        }
        Boosted.Stop();
        Burst.Play();
        yield return null;
        Burst.Stop();
        yield return new WaitForSeconds(2);
        count.gameObject.SetActive(false);
        canSummon = true;
        summoning = false;
    }
}
