using UnityEngine;
using System.Collections;

public class CameraShaker : MonoBehaviour
{
    private static CameraController cameraCtrl;

    public static void SetController(CameraController controller)
    {
        cameraCtrl = controller;
    }

    void OnEnable()
    {
        cameraCtrl.Shake();
    }

    private void Update()
    {
        //Debug.Log(cameraCtrl.Camera.transform.localRotation.eulerAngles);
    }
}
