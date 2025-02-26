using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField] float volum;
    [SerializeField] AudioMixer mixer;
    [SerializeField] CanvasGroup title;
    [SerializeField] float appearTime;
    [SerializeField] GameObject HelpUI;
    [SerializeField] GameObject ButtonsUI;
    void Start()
    {
        StartCoroutine(titleAppear());
    }
    IEnumerator titleAppear()
    {
        for (float i = 0; i <= 1; i += Time.deltaTime / appearTime)
        {
            title.alpha = i;
            yield return null;
        }
        Debug.Log("transitionClear");
    }
    public void OnSliderChanged(float value)
    {
        volum = Mathf.Log10(value) * 20f;
        mixer.SetFloat("masterVolume", Mathf.Log10(value) * 20f);
    }
    public void ButtonPlay()
    {
        SceneManager.LoadScene(1);
    }
    public void doExitGame()
    {
        Application.Quit();
    }
    public void ButtonHelp()
    {
        ButtonsUI.SetActive(false);
        HelpUI.SetActive(true);
    }
    public void ButtonOk()
    {
        ButtonsUI.SetActive(true);
        HelpUI.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        if (HelpUI.activeInHierarchy)
        {
            ButtonOk();
        }
    }
}
