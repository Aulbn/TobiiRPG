using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static bool isRunning = true;
    public static bool isEyeTracking = false;
    public Transform spawnPoint;

    private void Awake()
    {
        Instance = this;
    }

    public static void QuitGame()
    {
        Application.Quit();
    }

    public static void RestartGame()
    {
        EnemyController.LivingEnemies = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static void PauseGame(bool value)
    {
        Time.timeScale = value ? 0 : 1;
        isRunning = Time.timeScale == 1 ? true : false;
    }

    public static void WinGame()
    {
        Time.timeScale = 0;
        UIManager.ShowWinScreen(true);
    }

    public static void Respawn()
    {
        Time.timeScale = 1;
        PlayerController.Respawn(Instance.spawnPoint.position);
        CameraController.LookAtRespawn();
    }

}
