using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using TMPro;

public class YuukaFairyManager : MonoBehaviour
{
    //For cursor visuals, check CursorVisuals.cs in Cursor File. :)
    public bool decoMode;

    [Header("Bases")]
    [SerializeField] Transform cursor; //Cursor Pos.
    [SerializeField] Transform yuuka;
    [SerializeField] TextMeshProUGUI sunfairyNumbText;
    [SerializeField] TextMeshProUGUI watfairyNumbText;
    [SerializeField] int sunfaeCount;
    [SerializeField] int watfaeCount;
    [Header("Throw")]
    [SerializeField] InputAction inputLeftClick; //inputAction LeftClick.
    [SerializeField] float distancel;

    [Header("Whistle")]
    [SerializeField] InputAction inputRightClick; //inputAction RightClick.
    [SerializeField] float rad; //Radius for collisionCheck
    [SerializeField] Vector3 height; //Height of capsuleCollider
    [SerializeField] LayerMask fairyLayer; // as name says.

    [Header("FairyLists")]
    public List<Fairy> fairies = new List<Fairy>(); //Fairy List

    private void OnEnable()
    {

        inputLeftClick = InputSystem.actions.FindAction("LeftClick");//Assign action
        inputRightClick = InputSystem.actions.FindAction("RightClick"); //Assign action
    }

    private void Update()
    {
        for (int i = 0; i < fairies.Count; i++)
        {
            if (!fairies[i].gameObject.activeInHierarchy)
            {
                if(fairies[i].watfae)
                {
                    watfaeCount--;
                }
                if (fairies[i].sunfae)
                {
                    sunfaeCount--;
                }
                fairies.Remove(fairies[i]);
            }
        }

        ShowNumber(); //OnUI
        if (!decoMode)
            if (inputLeftClick.WasPressedThisFrame())
                Throw();
    }
    void FixedUpdate()
    {
        if (!decoMode)
            if (inputRightClick.IsPressed())
                Whistle();
    }
    void Throw()
    {
        for (int i = 0; i < fairies.Count; i++)
        {
            if (Vector3.Distance(fairies[i].transform.position, yuuka.position) < distancel)
            {
                if (fairies[i].sunfae)
                    sunfaeCount -= 1;
                if (fairies[i].watfae)
                    watfaeCount -= 1;
                fairies[i].Thrown(cursor.position); //function handled in fairy script.

                fairies.Remove(fairies[i]); //Remove from fae List
                Debug.Log("Trhow!");
                break; //find only one and Get out
            }
        }
    }
    void Whistle()
    {
        Collider[] foundColliders = Physics.OverlapCapsule(cursor.position + height, cursor.position - height, rad, fairyLayer);
        if (foundColliders.Length > 0) //Something layerd "fairy is discovered."
        {

            for (int i = 0; i < foundColliders.Length; i++) //ForEach of disocered colliders,
            {
                Fairy fairy = foundColliders[i].gameObject.GetComponent<Fairy>(); //Temporarily save "Fairy" component,
                if (fairy.followPlayer == false) //if "Fairy"'s follow(bool) is false,
                {
                    if (fairy.sunfae)
                        sunfaeCount++;
                    if (fairy.watfae)
                        watfaeCount++;

                    fairies.Add(fairy); //Add to fae List
                    fairy.Whistled(); //it's fairy(local) because it's already inside for()
                    Debug.Log(fairies.Count);
                }
            }
        }
    }

    void ShowNumber()
    {
        sunfairyNumbText.text = sunfaeCount.ToString();
        watfairyNumbText.text = watfaeCount.ToString();
    }
    public void BringFairies(Gap newGap)
    {
        for (int i = 0; i < fairies.Count; i++)
        {
            Vector3 rngPos = new Vector3(transform.position.x + Random.Range(0, 1f), transform.position.y, transform.position.z + Random.Range(0, 1f));
            fairies[i].navAg.enabled = false;
            fairies[i].transform.position = rngPos;
            fairies[i].gap = newGap;
            Debug.Log("BringFairies" + rngPos.x + " " + rngPos.z);
            fairies[i].navAg.enabled = true;
        }
    }
}
