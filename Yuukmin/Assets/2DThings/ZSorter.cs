using System;
using UnityEngine;

public class ZSorter : MonoBehaviour
{
    [SerializeField] bool doMove = false;
    [SerializeField] int extraDepth;
    [SerializeField] Transform main;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<SpriteRenderer>().sortingOrder = (int)(30000 - (transform.position.z * 100)) + extraDepth;
    }

    // Update is called once per frame
    void Update()
    {
        if(doMove) //+ (int)(Mathf.Abs((transform.localPosition.z * 1000) % 10) FAILURE
        {
            GetComponent<SpriteRenderer>().sortingOrder = (int)(30000 - (main.position.z * 100)) + extraDepth;

        }

    }
}
