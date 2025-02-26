using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class YuukaItemsManager : MonoBehaviour
{
    [Header("Modes")]
    public bool readTab;
    public bool decoMode;
    public bool hasPlaced;
    public bool hasFed;

    public Gap gap;

    [Header("ItemsCounts")]
    public int chosenItemIndex;
    public int sunflowerCount;
    public int watflowerCount;
    public int etScaleCount;
    [SerializeField] LayerMask faeFlowerLayer;
    [SerializeField] GameObject[] itemHighlight;
    [SerializeField] TextMeshProUGUI[] countTexts;
    [SerializeField] FairyFlowers fairySunflower;
    [SerializeField] FairyFlowers fairyWatflower;

    [Header("FairyCounts")]
    [SerializeField] int placeHolder; //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

    [SerializeField] CursorVisuals cursorVisuals;
    [SerializeField] YuukaFairyManager yuukFaeManager;

    [Header("UII")]
    [SerializeField] float swapTime;
    [SerializeField] float alpha;
    [Header("CanvasGroups")]
    [SerializeField] CanvasGroup fairyUI;
    [SerializeField] CanvasGroup itemsUI;

    [SerializeField] InputAction inputTabChange;

    [SerializeField] InputAction inputSelectChange;
    [SerializeField] InputAction inputLeftClick; //inputAction LeftClick.
    Coroutine tabSwap;
    private void Start()
    {
        inputLeftClick = InputSystem.actions.FindAction("LeftClick");//Assign action
        inputTabChange = InputSystem.actions.FindAction("TabChange");
        inputSelectChange = InputSystem.actions.FindAction("SelectChange");
    }
    void Update()
    {
        CountItems();
        if (decoMode && inputLeftClick.WasPressedThisFrame())
        {
            DecoPlace();
        }
        if (readTab)
        {
        if (inputTabChange.WasPressedThisFrame())
        {
            if (tabSwap != null)
            StopCoroutine(tabSwap);
            tabSwap = StartCoroutine(TabChange());

            decoMode = itemsUI.gameObject.activeInHierarchy;
            yuukFaeManager.decoMode = decoMode;
            if(decoMode)
                cursorVisuals.DecoStart();
            if (!decoMode)
                cursorVisuals.DecoEnd();
        }
        if (inputSelectChange.ReadValue<Vector2>().y > 0)
        {
            Debug.Log("up");
            chosenItemIndex += 1;
            if (chosenItemIndex >= itemHighlight.Length)
            {
                chosenItemIndex = 0;
            }
            HighlightItem();
        }
        if (inputSelectChange.ReadValue<Vector2>().y < 0)
        {
            Debug.Log("down");
            chosenItemIndex -= 1;
            if (chosenItemIndex < 0)
            {
                chosenItemIndex = itemHighlight.Length - 1;
            }
            HighlightItem();
        }
        }

    }
    void DecoPlace()
    {
        if (chosenItemIndex == 0 && sunflowerCount > 0)
        {
            if (!hasPlaced)
                hasPlaced = true;
            PlantSunf();
        }
        if (chosenItemIndex == 1 && watflowerCount > 0)
        {
            if (!hasPlaced)
                hasPlaced = true;
            PlantWatf();
        }
        if (chosenItemIndex == 2 && etScaleCount > 0)
        {
            UseEtScale();
        }
    }
    void PlantWatf()
    {
        watflowerCount -= 1;
        FairyFlowers ff = Instantiate(fairyWatflower, cursorVisuals.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        ff.yuuk = this.transform;
        ff.gap = gap;
    }
    void PlantSunf()
    {
        sunflowerCount -= 1;
        FairyFlowers ff = Instantiate(fairySunflower, cursorVisuals.transform.position + new Vector3(0,1,0), Quaternion.identity);
        ff.yuuk = this.transform;
        ff.gap = gap;
    }
    void UseEtScale()
    {
        Collider[] flowerCol = Physics.OverlapSphere(cursorVisuals.transform.position, 1, faeFlowerLayer); //make list of all "carryable" layer objects nearby
        if (flowerCol.Length > 0) //if there is more than one,
        {
            Debug.Log("YuukItemManager : Found!");
            ////Find nearest object
            float minDistance = Mathf.Infinity; //To calculate at start.
            int nearestIndex = 0; //Index of nearest object that will be calculated under this.
            for (int i = 0; i < flowerCol.Length; i++)
            {
                float objectDistance = Vector3.Distance(cursorVisuals.transform.position, flowerCol[i].transform.position);
                if (objectDistance < minDistance)
                {
                    minDistance = objectDistance; //to compare again.
                    nearestIndex = i; //setting nearest object's index.
                }
            }
            FairyFlowers ff = flowerCol[nearestIndex].GetComponent<FairyFlowers>();
            if (ff.canSummon)
            {
                etScaleCount -= 1;
                ff.Fed();
                if (!hasFed)
                    hasFed = true;
            }
        }
    }
    void HighlightItem()
    {
        for (int i = 0; i < itemHighlight.Length; i++)
        {
            itemHighlight[i].SetActive(false);
        }
        itemHighlight[chosenItemIndex].SetActive(true);
    }
    IEnumerator TabChange()
    {
        if (fairyUI.gameObject.activeInHierarchy)
        {
            for (float i = 0; i <= 1; i+=Time.deltaTime / swapTime)
            {
                alpha = Mathf.SmoothStep(0,1,i);
                itemsUI.gameObject.SetActive(true);
                fairyUI.alpha = Mathf.SmoothStep(1, 0, i);
                itemsUI.alpha = Mathf.SmoothStep(0, 1, i);
                fairyUI.gameObject.SetActive(false);
                yield return null;
            }
            yield break;
        }
        else
        {
            for (float i = 0; i <= 1; i += Time.deltaTime / swapTime)
            {
                alpha = Mathf.SmoothStep(0, 1, i);
                fairyUI.gameObject.SetActive(true);
                fairyUI.alpha = Mathf.SmoothStep(0, 1, i);
                itemsUI.alpha = Mathf.SmoothStep(1, 0, i);
                itemsUI.gameObject.SetActive(false);
                yield return null;
            }
            yield break;
        }
    }
    void CountItems()
    {
        countTexts[0].text = sunflowerCount.ToString();
        countTexts[1].text = watflowerCount.ToString();
        countTexts[2].text = etScaleCount.ToString();
    }
}
