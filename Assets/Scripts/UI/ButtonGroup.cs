using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonGroup : MonoBehaviour
{
    private Image myImage;
    private Button[] buttons;
    // Start is called before the first frame update

    private void Awake()
    {
        myImage = GetComponent<Image>();
        myImage.enabled = false;
        buttons = GetComponentsInChildren<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i].gameObject.activeSelf)
            {
                myImage.enabled = true;
            }
        }
    }
}
