using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    [SerializeField] private float speedOfHealingSP = 0.2f;

    [SerializeField] private int maxHP = 1;

    public int MaxHP => maxHP;
    public float HP { get; private set; }
    public float SP { get; private set; } = 1;
    public bool HealStop { get; set; } = false;

    protected virtual void Start()
    {
        HP = maxHP;
    }

    protected virtual void Update()
    {
        HealSP();
    }

    public void HealHP(float healAmount)
    {
        HP = Mathf.Clamp(HP + healAmount, 0, maxHP);
    }

    public void Damage(float damage)
    {
        HP -= damage;
    }

    private void HealSP()
    {
        if (!HealStop)
        {
            if (SP < 1) SP += speedOfHealingSP * Time.deltaTime;
            else SP = 1;
        }
    }

    public void UseSP(float useAmount)
    {
        SP -= useAmount;
        if (SP < 0) SP = 0;
    }
}
