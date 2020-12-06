using FSG.MeshAnimator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhoulAI : EnemyBase, IEnemyAI, IDamageable
{
    private Collider myCollider;
    private MeshAnimator meshAnimator;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    protected override void Initialize()
    {
        base.Initialize();
        myCollider = GetComponent<Collider>();
        meshAnimator = GetComponentInChildren<MeshAnimator>();
        EnemyAIUpdator.AddList(this);

    }

    //EnemyAIUpDatorのUpdateで呼ぶ
    public void Updating()
    {
        playerLooker.LookAt(playerTrans.position);
        distance = (transform.position - playerTrans.position).sqrMagnitude;
        StateChange();
        ActionControl();
    }

    //EnemyAIUpDatorのLateUpdateで呼ぶ
    public void LateUpdating()
    {
        anim.SetBool("Attack", false);
    }

    private void StateChange()
    {
        switch (myState)
        {
            case MyState.idle:
                if (distance < targetingRange * targetingRange && !ObstacleCheck())
                {
                    myState = MyState.chase;
                }
                break;
            case MyState.chase:
                if (targetingRange * targetingRange < distance)
                {
                    myState = MyState.idle;
                }
                if (distance < validAttackRange * validAttackRange || PlayerCheck())
                {
                    myState = MyState.search;
                }
                break;
            case MyState.search:
                if (validAttackRange * validAttackRange < distance)
                {
                    myState = MyState.chase;
                }

                var deltaAngle = Mathf.DeltaAngle(transform.eulerAngles.y, playerLooker.eulerAngles.y);
                if (Mathf.Abs(deltaAngle) < 5)
                {
                    myState = MyState.attack;
                }
                break;
        }
    }

    private void ActionControl()
    {
        switch (myState)
        {
            case MyState.idle:
                anim.SetBool("Walk", false);
                break;
            case MyState.chase:
                MoveForward();
                TurnHorizontal();
                break;
            case MyState.search:
                TurnHorizontal();
                break;
            case MyState.attack:
                anim.applyRootMotion = true;
                anim.SetBool("Attack", true);
                break;
            case MyState.death:
                myCollider.enabled = false;
                break;
        }
    }


    public void GetDamage(float damage, Attackable.Atribute atribute, Attackable attacker)
    {
        status.Damage(damage);
        AttackColliderCcontrol(false);
        var getForce = (transform.position - attacker.transform.position).normalized * 5 + Vector3.up * 2;
        getForce *= attacker.ForceMultiplier;
        if (status.HP <= 0)
        {
            Death(this, getForce, attacker);
        }
        else
        {
            anim.SetTrigger("GetHit");
        }
    }

    protected override void Death(IEnemyAI enemyAI, Vector3 force, Attackable attackable)
    {
        PlayAudio(1);
        meshAnimator.gameObject.SetActive(false);
        base.Death(enemyAI, force, attackable);
    }

}
