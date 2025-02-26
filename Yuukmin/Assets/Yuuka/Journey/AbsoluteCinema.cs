using System.Collections;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class AbsoluteCinema : MonoBehaviour
{
    public bool IsCinemaTime;

    [SerializeField] float timerToDebug;
    [Header("TutorialStuffs")]
    [SerializeField] TutorialManager tutManager;

    [Header("Yuuka")]
    [SerializeField] Animator animYuuka;
    [SerializeField]
    GameObject[] disablethese;

    [Header("Bases")]
    [SerializeField] CanvasGroup blockCanvas;//
    [SerializeField] CanvasGroup endingCanvas;
    [SerializeField] CinemachineCamera cinema;
    [SerializeField] CinemachineFollow cinemaFollower;
    [SerializeField] CanvasGroup canvas;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] RectTransform box;
    [SerializeField] Cirno cirno;
    [Header("Targets")]
    [SerializeField] Transform Yuuka;
    [SerializeField] Transform Cirno;
    [SerializeField] Transform SunflowerTarget;
    [SerializeField] Transform YoukaiTrail;
    [SerializeField] Transform YTEnemy;
    [SerializeField] Transform field; //
    [SerializeField] Material skyMat; //
    [SerializeField] Color lightColor; //
    [SerializeField] Light lighty;
    [SerializeField] AudioClip nightFairy;
    [SerializeField] AudioClip cirnoMusic;
    [SerializeField] AudioSource musicer; //

    [Header("BasicVariables")]
    [SerializeField] Vector3 beforeDamping;
    [SerializeField] Vector3 beforeOffset;
    [SerializeField] float beforeRota;
    //
    [SerializeField] Vector3 afterDamping;
    [SerializeField] float afterRota;
    [Header("SpecialVariables")]
    [SerializeField] Vector3 YuukaOffset;
    [SerializeField] Vector3 CirnoOffset;
    [SerializeField] Vector3 SunfOffset;
    [Header("Dialogues")]
    [SerializeField] float waitTime;
    [SerializeField] DialogueContainer startDia;
    [SerializeField] DialogueContainer fairyReactDia;
    [SerializeField] DialogueContainer ytReactDia;
    [SerializeField] DialogueContainer enemyReactDia;
    [SerializeField] DialogueContainer cirnoMeet;
    [SerializeField] DialogueContainer cirnoDefeat;
    [SerializeField] DialogueContainer cirnoBored;
    [SerializeField] DialogueContainer testDia1;

    InputAction inputNextDialogue;
    private void Start()
    {
        inputNextDialogue = InputSystem.actions.FindAction("NextDialogue");
        StartCoroutine(StartDialogue());
    }
    IEnumerator StartDialogue()
    {
        float timer = 0;
        StartCoroutine(StartCinema());
        SetCamFollow(Yuuka, YuukaOffset);

        for (int i = 0; i < startDia.paragraphs.Length; i++)
        {
            timer = 0;
            text.text = startDia.paragraphs[i];
            if (i == 0)
            {
                SetCamFollow(Yuuka, YuukaOffset);
            }
            if (i == 1)
            {
                animYuuka.SetLayerWeight(1, 1);
                animYuuka.SetLayerWeight(2, 0);
                //Debug.Log("2"+animYuuka.GetLayerName(2)); eyes
                //Debug.Log("1" + animYuuka.GetLayerName(1)); emote??
                SetCamFollow(SunflowerTarget, SunfOffset);
            }
            if(i == 2)
            {
                SetCamFollow(Yuuka, YuukaOffset);
            }
            if (i == 3)
            {
                animYuuka.SetLayerWeight(1, 0);
                animYuuka.SetLayerWeight(2, 1);
                SetCamFollow(Yuuka, YuukaOffset);
            }

            while (!inputNextDialogue.WasPressedThisFrame() && timer <= waitTime)
            {
                timerToDebug = timer;
                timer += Time.deltaTime;
                yield return null;
            }
            Debug.Log("NextParagraph");
            yield return null;
        }

        //yield return new WaitForSeconds(2);
        StartCoroutine(EndCinema());
        SetCamFollow(Yuuka, beforeOffset);

        StartCoroutine(tutManager.TUTMove()); //Tuto start
    }
    public IEnumerator YuukFairyReaction()
    {
        float timer = 0;
        StartCoroutine(StartCinema());
        SetCamFollow(Yuuka, YuukaOffset);

        for (int i = 0; i < fairyReactDia.paragraphs.Length; i++)
        {
            timer = 0;
            text.text = fairyReactDia.paragraphs[i];

            while (!inputNextDialogue.WasPressedThisFrame() && timer <= waitTime)
            {
                timerToDebug = timer;
                timer += Time.deltaTime;
                yield return null;
            }
            Debug.Log("NextParagraph");
            yield return null;
        }

        //yield return new WaitForSeconds(2);
        StartCoroutine(EndCinema());
        SetCamFollow(Yuuka, beforeOffset);

        StartCoroutine(tutManager.TUTFairy2()); //Tuto start
    }
    //public IEnumerator FairyInteraction1/2
    public IEnumerator YuukYTReaction()
    {
        float timer = 0;
        StartCoroutine(StartCinema());
        SetCamFollow(Yuuka, YuukaOffset);

        for (int i = 0; i < ytReactDia.paragraphs.Length; i++)
        {
            timer = 0;
            text.text = ytReactDia.paragraphs[i];

            if (i == 1)
            {
                SetCamFollow(YoukaiTrail, beforeOffset);
            }
            if (i == 2)
            {
                SetCamFollow(Yuuka, YuukaOffset);
            }

            while (!inputNextDialogue.WasPressedThisFrame() && timer <= waitTime)
            {
                timerToDebug = timer;
                timer += Time.deltaTime;
                yield return null;
            }
            Debug.Log("NextParagraph");
            yield return null;
        }
        tutManager.YTBlock.SetActive(false);
        //yield return new WaitForSeconds(2);
        StartCoroutine(EndCinema());
        SetCamFollow(Yuuka, beforeOffset);
    }
    public IEnumerator YuukEnemReaction()
    {
        float timer = 0;
        StartCoroutine(StartCinema());
        SetCamFollow(Yuuka, YuukaOffset);

        for (int i = 0; i < enemyReactDia.paragraphs.Length; i++)
        {
            timer = 0;
            text.text = enemyReactDia.paragraphs[i];

            if (i == 1)
            {
                SetCamFollow(YTEnemy, beforeOffset + new Vector3(0,1f,0));
            }
            if (i == 2)
            {
                SetCamFollow(Yuuka, YuukaOffset);
            }

            while (!inputNextDialogue.WasPressedThisFrame() && timer <= waitTime)
            {
                timerToDebug = timer;
                timer += Time.deltaTime;
                yield return null;
            }
            Debug.Log("NextParagraph");
            yield return null;
        }
        tutManager.YTBlock.SetActive(false);
        //yield return new WaitForSeconds(2);
        StartCoroutine(EndCinema());
        SetCamFollow(Yuuka, beforeOffset);
        StartCoroutine(tutManager.TUTEnemy());
    }
    public IEnumerator YuukCirnoMeet()
    {
        float timer = 0;
        StartCoroutine(StartCinema());
        SetCamFollow(Yuuka, YuukaOffset);

        for (int i = 0; i < cirnoMeet.paragraphs.Length; i++)
        {
            timer = 0;
            text.text = cirnoMeet.paragraphs[i];
            if (cirnoMeet.speakerID[i] == 0)
            {
                SetCamFollow(Yuuka, YuukaOffset);
            }
            else if (cirnoMeet.speakerID[i] == 1)
            {
                SetCamFollow(Cirno, CirnoOffset);
            }
            while (!inputNextDialogue.WasPressedThisFrame() && timer <= waitTime)
            {
                timerToDebug = timer;
                timer += Time.deltaTime;
                yield return null;
            }
            Debug.Log("NextParagraph");
            yield return null;
        }
        
        yield return new WaitForSeconds(2);
        StartCoroutine(EndCinema());
        cirno.CirnoStart();
        SetCamFollow(Yuuka, beforeOffset);
    }
    public IEnumerator CirnoDefeat()
    {
        musicer.clip = cirnoMusic;
        musicer.Play();
        float timer = 0;
        StartCoroutine(StartCinema());
        SetCamFollow(Yuuka, YuukaOffset);
        for (int i = 0; i < cirnoDefeat.paragraphs.Length; i++)
        {
            timer = 0;
            text.text = cirnoDefeat.paragraphs[i];
            SetCamFollow(Cirno, CirnoOffset);

            while (!inputNextDialogue.WasPressedThisFrame() && timer <= waitTime)
            {
                timerToDebug = timer;
                timer += Time.deltaTime;
                yield return null;
            }
            Debug.Log("NextParagraph");
            yield return null;
        }

        yield return new WaitForSeconds(2);
        StartCoroutine(EndCinema());
        musicer.clip = nightFairy;
        musicer.Play();
        tutManager.YIM.readTab = false;
        tutManager.cursor.SetActive(false);
        tutManager.yuukaMove.move = false;
        for (int i = 0; i < disablethese.Length; i++)
        {
            disablethese[i].SetActive(false);
        }
        blockCanvas.gameObject.SetActive(true);
        for (float i = 0; i < 1; i+= Time.deltaTime / 3)
        {
            blockCanvas.alpha = i;
            yield return null;
        }
        blockCanvas.alpha = 1;

        endingCanvas.gameObject.SetActive(true);
        SetCamFollow(field, beforeOffset);
        RenderSettings.skybox = skyMat;
        lighty.color = lightColor;

        for (float i = 1; i > 0; i-=Time.deltaTime)
        {
            blockCanvas.alpha = i;
            yield return null;
        }
        blockCanvas.alpha = 0;
    }
    public IEnumerator CirnoBored()
    {
        musicer.clip = cirnoMusic;
        musicer.Play();
        float timer = 0;
        StartCoroutine(StartCinema());
        SetCamFollow(Yuuka, YuukaOffset);
        for (int i = 0; i < cirnoBored.paragraphs.Length; i++)
        {
            timer = 0;
            text.text = cirnoBored.paragraphs[i];
            SetCamFollow(Cirno, CirnoOffset);

            if(i == 2)
                SetCamFollow(Yuuka, YuukaOffset);
            while (!inputNextDialogue.WasPressedThisFrame() && timer <= waitTime)
            {
                timerToDebug = timer;
                timer += Time.deltaTime;
                yield return null;
            }
            Debug.Log("NextParagraph");
            yield return null;
        }

        yield return new WaitForSeconds(2);
        StartCoroutine(EndCinema());
        musicer.clip = nightFairy;
        musicer.Play();
        tutManager.YIM.readTab = false;
        tutManager.cursor.SetActive(false);
        tutManager.yuukaMove.move = false;
        blockCanvas.gameObject.SetActive(true);
        for (int i = 0; i < disablethese.Length; i++)
        {
            disablethese[i].SetActive(false);
        }
        for (float i = 0; i < 1; i += Time.deltaTime / 3)
        {
            blockCanvas.alpha = i;
            yield return null;
        }
        blockCanvas.alpha = 1;

        endingCanvas.gameObject.SetActive(true);
        SetCamFollow(field, beforeOffset);
        RenderSettings.skybox = skyMat;
        lighty.color = lightColor;

        for (float i = 1; i > 0; i -= Time.deltaTime)
        {
            blockCanvas.alpha = i;
            yield return null;
        }
        blockCanvas.alpha = 0;
    }
    /*
    IEnumerator TestDialogue3()
    {
        StartCoroutine(StartCinema());
        SetCamFollow(Yuuka, YuukaOffset);

        for (int i = 0; i < testDia1.paragraphs.Length; i++)
        {
            text.text = testDia1.paragraphs[i];
            if (testDia1.speakerID[i] == 0)
            {
                SetCamFollow(Yuuka, YuukaOffset);
            }
            else if (testDia1.speakerID[i] == 1)
            {
                SetCamFollow(Cirno, CirnoOffset);
            }
            yield return new WaitForSeconds(2);
        }

        yield return new WaitForSeconds(2);
        StartCoroutine(EndCinema());
        SetCamFollow(Yuuka, beforeOffset);
    }
    */
    IEnumerator StartCinema()
    {
        IsCinemaTime = true;
        cinemaFollower.TrackerSettings.PositionDamping = afterDamping;
        cinema.transform.rotation = Quaternion.Euler(afterRota, 0, 0);

        canvas.gameObject.SetActive(true);
        for (float i = 0; i < 1; i += Time.deltaTime)
        {
            canvas.alpha = i;
            box.sizeDelta = new Vector2(1670, Mathf.SmoothStep(0, 315f, i));
            yield return null;
        }
        canvas.alpha = 1;
        box.sizeDelta = new Vector2(1670, 315f);
    }
    IEnumerator EndCinema()
    {
        cinemaFollower.TrackerSettings.PositionDamping = beforeDamping;
        cinema.transform.rotation = Quaternion.Euler(beforeRota, 0, 0);

        for (float i = 1; i > 0; i -= Time.deltaTime)
        {
            canvas.alpha = i;
            box.sizeDelta = new Vector2(1670, Mathf.SmoothStep(0, 315f, i));
            yield return null;
        }
        canvas.alpha = 0;
        box.sizeDelta = new Vector2(1670, 0);

        canvas.gameObject.SetActive(false);
        IsCinemaTime = false;
    }

    void SetCamFollow(Transform protag, Vector3 offset)
    {
        cinemaFollower.FollowOffset = offset;
        cinema.Follow = protag;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
        }
        if (Input.GetKeyDown(KeyCode.Equals))
        {
        }
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            //StartCoroutine(TestDialogue3());
        }
    }

    public void ButtonOK()
    {
        SceneManager.LoadScene(0);
    }
}
