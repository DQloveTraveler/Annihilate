using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class CursorImage : MonoBehaviour
{
    [SerializeField] private int sensitivity = 20;
    float cursorSpeed = 1;

    float screenSize_H = 0;
    float screenSize_V = 0;


    // Start is called before the first frame update
    void Start()
    {
        screenSize_H = Screen.width / 2 - 30;
        screenSize_V = Screen.height / 2 - 20;
    }

    // Update is called once per frame
    void Update()
    {
        float inputH = Input.GetAxis("Horizontal");
        float inputV = Input.GetAxis("Vertical");

        Vector3 moveVector = new Vector3(inputH, inputV, 0).normalized * sensitivity * cursorSpeed;
        Vector3 nextPosition = transform.localPosition + moveVector;

        nextPosition.x = Mathf.Clamp(nextPosition.x, -screenSize_H, screenSize_H);
        nextPosition.y = Mathf.Clamp(nextPosition.y, -screenSize_V, screenSize_V);

        transform.localPosition = nextPosition;

    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        cursorSpeed = 0.5f;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        cursorSpeed = 1;
    }

}
