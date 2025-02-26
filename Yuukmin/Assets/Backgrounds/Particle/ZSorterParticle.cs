using System;
using UnityEngine;

public class ZSorterParticle : MonoBehaviour
{
    [SerializeField] int extraDepth;
    [SerializeField] Transform main;

    [SerializeField] bool moving;
    ParticleSystemRenderer PSR;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PSR = GetComponent<ParticleSystemRenderer>();
        PSR.sortingOrder = (int)(30000 - (main.position.z * 100)) + extraDepth;
    }
    private void Update()
    {
        if(moving)
            PSR.sortingOrder = (int)(30000 - (main.position.z * 100)) + extraDepth;
    }
}
