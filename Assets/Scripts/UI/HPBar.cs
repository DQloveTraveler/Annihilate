using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : StatusUI
{
    [SerializeField] private Image fillMask;
    [SerializeField] private Image redMask;
    
    private int maxHP;
    private bool coroutineIsActive = false;
    private WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();


    void Start()
    {
        maxHP = targetStatus.MaxHP;
        fillMask.fillAmount = targetStatus.HP / maxHP;
        redMask.fillAmount = fillMask.fillAmount;
    }

    public void Heal()
    {
        fillMask.fillAmount = targetStatus.HP / maxHP;
        redMask.fillAmount = fillMask.fillAmount;
    }

    public void Damage()
    {
        StartCoroutine(_Damage());
    }

    private IEnumerator _Damage()
    {
        fillMask.fillAmount = targetStatus.HP / maxHP;
        if (!coroutineIsActive)
        {
            coroutineIsActive = true;

            yield return new WaitForSecondsRealtime(2);

            while (true)
            {
                yield return waitForEndOfFrame;
                redMask.fillAmount -= 0.005f;
                if (redMask.fillAmount < fillMask.fillAmount)
                {
                    redMask.fillAmount = fillMask.fillAmount;
                    coroutineIsActive = false;
                    break;
                }
            }
        }
    }

}
