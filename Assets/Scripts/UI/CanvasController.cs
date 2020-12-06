using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayerInputManagement;

public class CanvasController : MonoBehaviour
{
    [SerializeField] private Transform cursorImage;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private Image blurPanel;
    [SerializeField] private GameObject guideOfRide;
    [SerializeField] private ComparePanel comparePanel;

    private SceneBlurEffect sceneBlurEffect;
    public enum State
    {
        Default, MainMenu
    }
    private State myState = State.Default;
    private HPBar hpBar;
    private SPBar spBar;

    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        TalkSystemManager.SetMainCanvas(this);
        sceneBlurEffect = FindObjectOfType<SceneBlurEffect>();
        hpBar = GetComponentInChildren<HPBar>();
        spBar = GetComponentInChildren<SPBar>();
        mainMenu.SetActive(false);
        blurPanel.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerInput.Pause)
        {
            switch (myState)
            {
                case State.Default:
                    ActivateMainMenu();
                    break;
                case State.MainMenu:
                    DeActivateMainMenu();
                    break;
            }
        }
    }

    private void ActivateMainMenu()
    {
        StartCoroutine(_ActivateMainMenu());
    }

    private IEnumerator _ActivateMainMenu()
    {
        Time.timeScale = 0;
        var waitForSeconds = new WaitForSecondsRealtime(0.01f);
        blurPanel.enabled = true;
        for(int i = 0; i < 25; i++)
        {
            yield return waitForSeconds;
            if (sceneBlurEffect.Intencity >= 1)
            {
                sceneBlurEffect.Intencity = 1;
            }
            else
            {
                sceneBlurEffect.Intencity += 0.02f;
            }
        }
        mainMenu.SetActive(true);
        myState = State.MainMenu;
    }

    private void DeActivateMainMenu()
    {
        StartCoroutine(_DeActivateMainMenu());
    }

    private IEnumerator _DeActivateMainMenu()
    {
        var waitForSeconds = new WaitForSecondsRealtime(0.01f);
        mainMenu.SetActive(false);
        for (int i = 0; i < 25; i++)
        {
            yield return waitForSeconds;
            if (sceneBlurEffect.Intencity <= 0)
            {
                sceneBlurEffect.Intencity = 0;
            }
            else
            {
                sceneBlurEffect.Intencity -= 0.02f;
            }
        }
        blurPanel.enabled = false;
        myState = State.Default;
        Time.timeScale = 1;
    }

    public void UpdateHPBar()
    {
        if(hpBar != null)
        {
            hpBar.Damage();
        }
    }

    public void SetActiveGuideMessage(string guideOf, bool isActive)
    {
        switch (guideOf)
        {
            case "RideOn":
                guideOfRide.SetActive(isActive);
                break;
        }
    }

}
