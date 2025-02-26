using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines.ExtrusionShapes;

public class TutorialManager : MonoBehaviour
{
    public bool Waiting;
    [SerializeField] float checkingTimer;
    [SerializeField] float checkTime;

    [Header("FlowerCast")]
    [SerializeField] Transform flowerPos;
    [SerializeField] float flowerRange;
    [SerializeField] LayerMask yuukLayer;
    [SerializeField] FairyFlowers ff;

    [Header("EnemyCast")]
    [SerializeField] Transform enemyPos;
    [SerializeField] float enemyrange;

    [Header("References")]
    [SerializeField] CanvasGroup canvas;
    [SerializeField] TextMeshProUGUI tutText;
    public YuukaMovement yuukaMove;
    [SerializeField] AbsoluteCinema absCine;
    public GameObject cursor;
    public GameObject YTBlock;
    public YuukaItemsManager YIM;
    [SerializeField] GameObject fairyUI;
    [SerializeField] YuukaFairyManager YFM;
    [SerializeField] Gap gap;
    [SerializeField] Spirit spirit;
    [SerializeField] Cirno cirno;
    [SerializeField] float cirnoRange;
    [SerializeField] Transform MistyCheck;
    [SerializeField] float mistyRange;
    [SerializeField] Suwak suwak;
    [SerializeField] AudioClip mistyLMusic;

    [Header("DoCheck")]
    public bool DotutMove;
    public bool DotutRClick;
    public bool DotutLClick;

    [Header("Checks")]
    public bool tutMove = true;
    public bool tutSunflower = true;
    public bool tutRClick = true;
    public bool tutLClick = true;
    public bool tutEnemy = true;
    public bool tutCirno = true;
    public bool tutMisty = true;
    [Header("InputSystems")]
    [SerializeField] InputAction inputMoveKeys;
    [SerializeField] InputAction inputLeftClick;
    [SerializeField] InputAction inputRightClick;
    [SerializeField] InputAction inputTabChange;

    Coroutine tutCoro1;
    Coroutine tutCoro2;
    private void Start()
    {
        inputMoveKeys = InputSystem.actions.FindAction("Move");
        inputLeftClick = InputSystem.actions.FindAction("LeftClick");
        inputRightClick = InputSystem.actions.FindAction("RightClick");
        inputTabChange = InputSystem.actions.FindAction("TabChange");
    }
    // Update is called once per frame
    void Update()
    {
        if (!absCine.IsCinemaTime)
        {
                ReadTutorials();
        }
    }
    void ReadTutorials()
    {
        if (DotutMove && tutMove && inputMoveKeys.WasPressedThisFrame())
        {
            tutMove = false;
        }
        else if (DotutLClick && tutLClick && inputLeftClick.WasPressedThisFrame())
        {
            cursor.SetActive(true);
            tutLClick = false;
        }
        else if (DotutRClick && tutRClick && inputRightClick.WasPressedThisFrame())
        {
            cursor.SetActive(true);
            tutRClick = false;
        }
    }

    public IEnumerator TUTMove()
    {
        tutText.text = "Use (W, A ,S, D) key to Move Yuuka!";
        CheckUI();
        tutCoro1 = StartCoroutine(ShowTUT());
        while (!inputMoveKeys.IsPressed())
        {
            Waiting = true;
            yield return null;
        }
        CheckUI();
        tutCoro2 = StartCoroutine(UnShowTUT());
        yuukaMove.move = true;
        Waiting = false;
    }
    public IEnumerator TUTFairy1() //WIP
    {
        cursor.SetActive(true);
        tutText.text = "Use your Right mouse to call Fairy";
        CheckUI();
        tutCoro1 = StartCoroutine(ShowTUT());
        while (!inputRightClick.IsPressed())
        {
            Waiting = true;
            yield return null;
        }
        while (YFM.fairies.Count <= 0)
        {
            Waiting = true;
            yield return null;
        }
        CheckUI();
        tutCoro2 = StartCoroutine(UnShowTUT());
        fairyUI.SetActive(true);
        YIM.readTab = true;
        Waiting = false;
        StartCoroutine(absCine.YuukFairyReaction());
    }
    public IEnumerator TUTFairy2()
    {

        tutText.text = "Send your fairy With Left CLick\r\nand make them carry items for you!"; 
        CheckUI();
        tutCoro1 = StartCoroutine(ShowTUT());
        while (!gap.gotItem)
        {
            Waiting = true;
            yield return null;
        }
        CheckUI();
        tutCoro2 = StartCoroutine(UnShowTUT());
        StartCoroutine(TUTPlacing());
    }
    public IEnumerator TUTPlacing()
    {

        tutText.text = "Press Tab to change to 'deco mode'\r\nthis is tab where you can place new flowers!";
        CheckUI();
        tutCoro1 = StartCoroutine(ShowTUT());
        while (!inputTabChange.IsPressed())
        {
            Waiting = true;
            yield return null;
        }
        CheckUI();
        tutCoro2 = StartCoroutine(UnShowTUT());/////////////////////////////////////////////////

        tutText.text = "In deco mode, you cannot command fairies\r\nso be sure to swap between two!"; 
        CheckUI();
        tutCoro1 = StartCoroutine(ShowTUT());
        yield return new WaitForSeconds(7);
        CheckUI();
        tutCoro2 = StartCoroutine(UnShowTUT());/////////////////////////////////////////////////

        tutText.text = "You can select item with mouse scroll and \r\nplace them with left click! (if you have one)";
        CheckUI();
        tutCoro1 = StartCoroutine(ShowTUT());
        while (!YIM.hasPlaced)
        {
            Waiting = true;
            yield return null;
        }
        CheckUI();
        tutCoro2 = StartCoroutine(UnShowTUT());/////////////////////////////////////////////////

        tutText.text = "the you can use scale item to \r\nsummon fairies! (the yellow squres)";
        CheckUI();
        tutCoro1 = StartCoroutine(ShowTUT());
        while (!YIM.hasFed)
        {
            Waiting = true;
            yield return null;
        }
        CheckUI();
        tutCoro2 = StartCoroutine(UnShowTUT());/////////////////////////////////////////////////

        tutText.text = "Try and gather more than \r\ntwo fairy!";
        CheckUI();
        tutCoro1 = StartCoroutine(ShowTUT());
        while (YFM.fairies.Count <= 2)
        {
            Waiting = true;
            yield return null;
        }
        CheckUI();
        tutCoro2 = StartCoroutine(UnShowTUT());/////////////////////////////////////////////////

        StartCoroutine(absCine.YuukYTReaction());
    }

    public IEnumerator TUTEnemy() ////aaaaaaaaaaaasdwadsda
    {

        tutText.text = "You can also send fairies to attack\r\nenemies!";
        CheckUI();
        tutCoro1 = StartCoroutine(ShowTUT());
        while (!spirit.dead)
        {
            Waiting = true;
            yield return null;
        }
        CheckUI();
        tutCoro2 = StartCoroutine(UnShowTUT());

        yield return new WaitForSeconds(2);
        tutText.text = "Defeated enemies drops scales,\r\nwich you can use to make more fairies!";
        CheckUI();
        tutCoro1 = StartCoroutine(ShowTUT());
        yield return new WaitForSeconds(7);
        CheckUI();
        tutCoro2 = StartCoroutine(UnShowTUT());

 
        tutText.text = "There's also blue water fairies\r\nsummon them and see what they do yourself!";
        CheckUI();
        tutCoro1 = StartCoroutine(ShowTUT());
        yield return new WaitForSeconds(6);
        CheckUI();
        tutCoro2 = StartCoroutine(UnShowTUT());

        yield return new WaitForSeconds(1);
        tutText.text = "that's all you need to know!\r\nEnjoy!";
        CheckUI();
        tutCoro1 = StartCoroutine(ShowTUT());
        yield return new WaitForSeconds(5);
        CheckUI();
        tutCoro2 = StartCoroutine(UnShowTUT());
        Debug.LogWarning("EEEEEEEEENDDDDDDDDDDDDD");
    }
    private void FixedUpdate()
    {
        if(tutSunflower && Physics.OverlapSphere(flowerPos.position, flowerRange, yuukLayer).Length > 0)
        {
            ff.Fed();
            tutSunflower = false;
            StartCoroutine(TUTFairy1());
            Debug.Log("YUUK!");
        }
        if (tutEnemy && Physics.OverlapSphere(enemyPos.position, enemyrange, yuukLayer).Length > 0)
        {
            tutEnemy = false;
            StartCoroutine(absCine.YuukEnemReaction());
            //AAAAAAAAAAAAAAAAAAAAAAAA
        }
        if(tutCirno && Physics.OverlapSphere(cirno.transform.position, cirnoRange, yuukLayer).Length > 0)
        {
            tutCirno = false;
            StartCoroutine(absCine.YuukCirnoMeet());
        }
        if (tutMisty && Physics.OverlapSphere(MistyCheck.position, mistyRange, yuukLayer).Length > 0)
        {
            tutMisty = false;
            suwak.End(mistyLMusic);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(flowerPos.position, flowerRange);

        Gizmos.DrawSphere(enemyPos.position, enemyrange);

        Gizmos.DrawSphere(cirno.transform.position, cirnoRange);
        Gizmos.DrawSphere(MistyCheck.position, mistyRange);
    }
    void CheckUI()
    {
        if (tutCoro1 != null)
            StopCoroutine(tutCoro1);
        if (tutCoro2 != null)
            StopCoroutine(tutCoro2);
    }
    IEnumerator ShowTUT()
    {
        canvas.gameObject.SetActive(true);
        for (float i = 0; i < 1; i+= Time.deltaTime / 0.5f)
        {
            canvas.alpha = i;
            yield return null;
        }
        canvas.alpha = 1;
    }
    IEnumerator UnShowTUT()
    {
        for (float i = 1; i > 0; i -= Time.deltaTime / 2)
        {
            canvas.alpha = i;
            yield return null;
        }
        canvas.alpha = 0;
        canvas.gameObject.SetActive(false);
    }
}
