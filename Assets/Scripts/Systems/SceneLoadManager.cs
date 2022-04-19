using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    private static SceneLoadManager instance;
    public static SceneLoadManager Instance => instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        } else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void LoadStartScreen()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void LoadMenuScreen()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void LoadWinScreen()
    {
        SceneManager.LoadScene("WinScene");

    }
    public void LoadLoseScreen()
    {
        SceneManager.LoadScene("LoseScene");

    }
}
