using FSG.MeshAnimator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HobGoblinAI : EnemyBase, IEnemyAI, IDamageable
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

    public void Updating()
    {
        playerLooker.LookAt(playerTrans.position);
        distance = (transform.position - playerTrans.position).sqrMagnitude;
        StateChange();
        ActionControl();
    }

    public void LateUpdating()
    {
        anim.SetBool("Attack", false);
    }



    private void StateChange()
    {
        switch (myState)
        {
            case MyState.idle:
                if (distance < targetingRange * targetingRange)
                {
                    myState = MyState.chase;
                }
                break;
            case MyState.chase:
                if (targetingRange * targetingRange < distance)
                {
                    myState = MyState.idle;
                }
                if (distance < validAttackRange * validAttackRange)
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
                anim.SetBool("Attack", true);
                break;
            case MyState.death:
                myCollider.enabled = false;
                break;
        }
    }


    public void GetDamage(float damage, Attackable.Atribute atribute, Attackable attacker)
    {
        if (atribute == Attackable.Atribute.None) damage /= 2;

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
            PlayAudio(1);
        }
    }

    protected override void Death(IEnemyAI enemyAI, Vector3 force, Attackable attackable)
    {
        PlayAudio(2);
        meshAnimator.gameObject.SetActive(false);
        base.Death(enemyAI, force, attackable);
    }

}
