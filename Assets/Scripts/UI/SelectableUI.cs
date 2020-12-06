using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableUI : MonoBehaviour
{
    private UIHighlight myHighLight;

    private void Awake()
    {
        myHighLight = GetComponent<UIHighlight>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Cursor")
        {
            myHighLight.IsHighlighted = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Cursor")
        {
            myHighLight.IsHighlighted = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Cursor")
        {
            if (Input.GetButtonUp("B"))
            {

            }
        }
    }


}
