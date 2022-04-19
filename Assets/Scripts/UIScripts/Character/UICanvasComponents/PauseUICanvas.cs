using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseUICanvas : GameHUDWidget
{
    [SerializeField] private Button ResumeButton;
    [SerializeField] private Button MenuButton;

    private void Start()
    {
        ResumeButton.onClick.AddListener(Unpause);
        MenuButton.onClick.AddListener(ReturnToMenu);
    }

    public void Unpause()
    {
        AppEvents.InvokeOnPauseEvent(false);
    }

    public void ReturnToMenu()
    {
        if (SceneLoadManager.Instance.isActiveAndEnabled)
        {
            DisableWidget();
            SceneLoadManager.Instance.LoadMenuScreen();
        }
    }
    public override void EnableWidget()
    {
        base.EnableWidget();
    }

    public override void DisableWidget()
    {
        base.DisableWidget();
    }
}
