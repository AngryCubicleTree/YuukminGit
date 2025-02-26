using System.Collections.Generic;
using UnityEngine;

public class Gap : MonoBehaviour
{
    [SerializeField] YuukaItemsManager YuukIM;
    [SerializeField] AudioClip pong1;
    [SerializeField] Animator animator;
    [SerializeField] ParticleSystem burstParticles;
    public Carryable depositedItem;
    public List<Carryable> seedsList = new List<Carryable>();
    public bool gotItem;
    public void LightItemDeposited(Carryable item)
    {
        if (!gotItem)
            gotItem = true;
        SFXSpawner.instance.PlaySFX(pong1, transform, 1);
        burstParticles.Play();
        animator.SetTrigger("Boing");
        depositedItem = item;
        CheckType();


        seedsList.Add(item);//Placeholder
        Debug.Log("Gap Contains : " + seedsList.Count);
    }

    void CheckType()
    {
        if (depositedItem.fairyWatFlowerSeed)
        {
            YuukIM.watflowerCount += 1;
        }
        if (depositedItem.fairySunFlowerSeed)
        {
            YuukIM.sunflowerCount += 1;
        }
        if (depositedItem.scaleOfET)
        {
            YuukIM.etScaleCount += 1;
        }
        depositedItem = null;
    }
}
