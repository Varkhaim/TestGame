using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TMPro.TextMeshProUGUI deathCounterTextMesh = null;
    [SerializeField] TMPro.TextMeshProUGUI gameTimerTextMesh = null;
    [SerializeField] TMPro.TextMeshProUGUI finishGameTextMesh = null;

    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<UIManager>();
            }

            return instance;
        }
    }

    public void UpdateDeathCounter(int value)
    {
        deathCounterTextMesh.text = "Deaths: " + value;
    }

    public void UpdateGameTimer(int value)
    {
        gameTimerTextMesh.text = ConvertIntToTimer(value);
    }

    private string ConvertIntToTimer(int value)
    {
        int minutes = Mathf.FloorToInt(value / 60F);
        int seconds = Mathf.FloorToInt(value - minutes * 60);
        string timerText = string.Format("{0:0}:{1:00}", minutes, seconds);
        return timerText;
    }

    public void ShowFinishText(string finishText, int timer)
    {
        string finalFinishText = finishText + ConvertIntToTimer(timer);
        finishGameTextMesh.text = finalFinishText;
        finishGameTextMesh.gameObject.SetActive(true);
    }
}
