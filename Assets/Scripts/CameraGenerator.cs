using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraGenerator
{
    public static CameraController Generate(GameObject cameraPrefab, Transform startTransform, Transform target)
    {
        var cameraObj = Object.Instantiate(cameraPrefab, startTransform.position, startTransform.rotation);

        var cameraCtrl = cameraObj.GetComponent<CameraController>();
        cameraCtrl.SetStartTransform(startTransform);
        cameraCtrl.SetTarget(target);
        return cameraCtrl;
    }
}
