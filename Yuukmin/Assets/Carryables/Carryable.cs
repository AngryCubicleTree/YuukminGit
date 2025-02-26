using System.Collections.Generic;
using UnityEngine;

public class Carryable : MonoBehaviour
{
    public bool lightWeight;
    public bool heavyWeight;


    public int requiredFairyCount;

    public bool fairySunFlowerSeed;
    public bool fairyWatFlowerSeed;
    public bool scaleOfET;

    public bool beingCarried;


    List<Fairy> fae = new List<Fairy>(); //Carrying Fairy List

    public void LightWeightCarry()
    {
        beingCarried = true;
    }
    public void LightWeightDrop()
    {
        beingCarried = false;
    }
}
