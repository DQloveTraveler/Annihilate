using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : Skill
{
    private void Start()
    {
        Init();
        Destroy(gameObject, 5);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.TryGetComponent<IDamageable>(out var damageable))
        {
            if (!alreadyHit.Contains(damageable))//既衝突なら判定を行わない
            {
                alreadyHit.Add(damageable);
                if(other.tag != "Player")
                {
                    damageable.GetDamage(attackPower, atribute, this);
                    EffectInstantiate(other, false);
                }
            }
        }
    }
}
