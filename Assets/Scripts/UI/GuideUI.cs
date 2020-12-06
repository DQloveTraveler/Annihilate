using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideUI : MonoBehaviour
{
    [SerializeField] private GameObject buttonUI;
    [SerializeField] private GameObject keyUI;

    void Update()
    {
        buttonUI.SetActive(ControllerMode.IsGamePad);
        keyUI.SetActive(ControllerMode.IsMouseAndKey);
    }
}
