using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    [Header("Achievements")]
    public bool HaruDesuYo;
    public bool HaruDeathYo;

    private void OnEnable()
    {
        LoadAll();
    }
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.M))
            //SaveAll();
    }
    void LoadAll()
    {
        AchievData data = SaveManager.LoadAchieve();
        HaruDeathYo = data.HaruDeathYo;
        HaruDesuYo = data.HaruDesuYo;
    }
    void SaveAll()
    {
        SaveManager.SaveAchieve(this);
    }
}
