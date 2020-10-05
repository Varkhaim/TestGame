using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Transform playerRespawnPoint = null;

    public float DeathZoneHeight = -50f;
    public float MinimumCameraHeight = -20f;
    public float DeathTime = 3f;

    private int deathCounter = 0;
    private float gameTimer = 0f;
    public bool isGameFinished = false;

    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<GameManager>();
            }

            return instance;
        }
    }

    private void Update()
    {
        UpdateGameTimer();
    }

    private void UpdateGameTimer()
    {
        gameTimer += Time.deltaTime;
        UIManager.Instance.UpdateGameTimer((int)gameTimer);
    }

    public void RespawnPlayer(Transform player)
    {
        player.position = playerRespawnPoint.position;
        player.rotation = playerRespawnPoint.rotation;
    }

    public void AddDeath()
    {
        deathCounter++;
        UIManager.Instance.UpdateDeathCounter(deathCounter);
    }

    public void FinishGame()
    {
        isGameFinished = true;
        UIManager.Instance.ShowFinishText("You finished in: ", (int)gameTimer);
    }
}
