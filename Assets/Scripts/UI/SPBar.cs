using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SPBar : StatusUI
{
    [SerializeField] private Image fillMask;
    [SerializeField] private Image lackingFillMask;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        fillMask.fillAmount = targetStatus.SP;
    }

    private void LateUpdate()
    {
        anim.SetBool("Blink", false);
    }
}
