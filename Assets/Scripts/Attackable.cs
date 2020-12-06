using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attackable : MonoBehaviour
{
    [SerializeField] protected float attackPower = 1;
    [SerializeField] private float forceMultiplier = 1;
    [SerializeField] private bool enableHeal = false;
    public bool EnableHeal => enableHeal;
    public float ForceMultiplier { get { return forceMultiplier; } }
    [SerializeField] protected GameObject hitEffect;
    public enum Atribute
    {
        None, Fire, Ice, Electro, Earth
    }
    [SerializeField] protected Atribute atribute = Atribute.None;

    //一度接触したコライダーとの当たり判定を無視するためのリスト
    protected List<IDamageable> alreadyHit = new List<IDamageable>();

    //PlayerControllerから参照
    public void SetAttackPower(int power)
    {
        attackPower = power;
    }

    public void ListReset()
    {
        alreadyHit.Clear();
    }

    //ヒットエフェクトの生成メソッド
    //boolの引数は「ひっかかり」をするかどうか
    protected void EffectInstantiate(Collider other, bool delay)
    {
        Vector3 hitPos = other.ClosestPointOnBounds(transform.position);
        if(hitEffect != null)
        {
            var instanceEffect = Instantiate(hitEffect, hitPos, Quaternion.identity);
            Destroy(instanceEffect, 0.5f);
            if (delay) StartCoroutine(_TimeDelay(4));
        }
    }

    private IEnumerator _TimeDelay(float delayFrame)
    {
        Time.timeScale = 0.1f;
        for(int i = 0; i < delayFrame; i++)
        {
            yield return new WaitForEndOfFrame();
        }
        Time.timeScale = 1;
    }
}
