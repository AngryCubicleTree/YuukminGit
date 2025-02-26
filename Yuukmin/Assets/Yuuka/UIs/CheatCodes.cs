using UnityEngine;

public class CheatCodes : MonoBehaviour
{
    [SerializeField] GameObject cheatCanv;
    [SerializeField] YuukaItemsManager YIM;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            Debug.LogError("BackQuote");
            cheatCanv.SetActive(!cheatCanv.activeInHierarchy);
        }
    }
    public void MoreScale()
    {
        YIM.etScaleCount ++;
    }
    public void MoreSun()
    {
        YIM.sunflowerCount++;

    }
    public void MoreWat()
    {
        YIM.watflowerCount++;
    }
    public void MoreSpeed()
    {
        YIM.gameObject.GetComponent<YuukaMovement>().Speed += 1;
    }
    public void MoreNoSpeed()
    {
        YIM.gameObject.GetComponent<YuukaMovement>().Speed -= 1;
    }
    public void MoreNoGame()
    {
        Application.Quit();
    }
}
