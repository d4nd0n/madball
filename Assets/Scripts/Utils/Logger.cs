using UnityEngine;

public static class Logger
{
    public static void logWithTime(string text)
    {
        float timer = TimeManager.timer;
        float roundedTimer = Mathf.Round(timer);
        Debug.Log(string.Format("===== {0} : {1} =====", roundedTimer, text));
    }
}
