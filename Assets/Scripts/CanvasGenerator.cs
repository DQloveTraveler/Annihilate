using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasGenerator
{
    public static GameObject Generate(GameObject canvasPrefab, Transform target)
    {
        var targetStatus = target.GetComponent<Status>();
        var canvas = Object.Instantiate(canvasPrefab);
        var statusUIs = canvas.GetComponentsInChildren<StatusUI>();
        foreach(var sui in statusUIs)
        {
            sui.Initialize(targetStatus);
        }
        if (target.TryGetComponent<WeaponEnergy>(out var weaponEnergy))
        {
            canvas.GetComponentInChildren<EnergyGage>().Initialize(weaponEnergy);
        }
        return canvas;
    }
}
