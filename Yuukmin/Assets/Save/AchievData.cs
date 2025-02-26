using UnityEngine;

[System.Serializable]
public class AchievData
{
    public bool HaruDesuYo;
    public bool HaruDeathYo;

    public AchievData (AchievementManager aM)
    {
        HaruDesuYo = aM.HaruDesuYo;
        HaruDeathYo = aM.HaruDeathYo;
    }
}
