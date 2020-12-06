using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    [SerializeField] private Texture2D[] cursorTextures;
    [SerializeField] private Vector2[] hotspots;
    private static int currentCursorNum = 0;
    private GameObject cursorImage = null;
    private static string[] gamePads = null;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        ControllerCheck();
    }

    public void SetCursor(int cursorNum)
    {
        if(currentCursorNum != cursorNum)
        {
            currentCursorNum = cursorNum;
            Cursor.SetCursor(cursorTextures[cursorNum], hotspots[cursorNum], CursorMode.Auto);
        }
    }


    private void ControllerCheck()
    {
        gamePads = Input.GetJoystickNames();
        if(gamePads.Length >= 1) ControllerIsGamePad();
        else ControllerIsMouseAndKey();
    }

    private void ControllerIsGamePad()
    {
        if(cursorImage == null) cursorImage = FindObjectOfType<CursorImage>().gameObject;

        if (!cursorImage.activeSelf)
        {
            cursorImage.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        ControllerMode.Mode = ControllerMode.Controller.GamePad;
    }

    private void ControllerIsMouseAndKey()
    {
        if (cursorImage == null) cursorImage = FindObjectOfType<CursorImage>().gameObject;

        if (cursorImage.activeSelf)
        {
            cursorImage.SetActive(false);
            Cursor.SetCursor(cursorTextures[0], new Vector2(0, 0), CursorMode.Auto);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        ControllerMode.Mode = ControllerMode.Controller.MouseAndKey;
    }


    public void ExitGame()
    {
        Application.Quit();
    }

}