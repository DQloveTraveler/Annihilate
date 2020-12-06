using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEnergy: MonoBehaviour
{
    public readonly int MaxValue = 100;

    public float Value { get; private set; } = 30;

    private float elapsedTime = 0;
    //private bool isDecreasing = false;


    // Update is called once per frame
    void Update()
    {
        if (Value >= MaxValue)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= 15)
            {
                Value = 0;
                //isDecreasing = true;
            }
        }
        /*
        else
        {
            if (elapsedTime >= 10)
            {
                isDecreasing = true;
            }
        }
        if (isDecreasing)
        {
            DecreaseWeaponEnergy();
        }
        */
    }

    private void DecreaseWeaponEnergy()
    {
        var decreaseAmount = Time.deltaTime;
        Value -= decreaseAmount;
        if (Value < 0) Value = 0;
    }

    public void GetEnergy(float energy)
    {
        Value = Mathf.Clamp(Value + energy, 0, MaxValue);
        //isDecreasing = false;
        if (Value < MaxValue) elapsedTime = 0;
    }

    public void UseEnergy(int useAmount)
    {
        if(Value < MaxValue) Value = Mathf.Clamp(Value - useAmount, 0, MaxValue);
    }

}
