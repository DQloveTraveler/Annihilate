using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAI : EnemyBase, IEnemyAI, IDamageable
{
    [Header("Skeleton固有")]
    [SerializeField] private Animator weaponAnim;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform generateArrowPoint;

    private Collider myCollider;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    protected override void Initialize()
    {
        base.Initialize();
        myCollider = GetComponent<Collider>();
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

                var target = playerTrans.position + new Vector3(0, 1.5f, 0);
                var ray = new Ray(transform.position , target - transform.position);
                if (Physics.Raycast(ray, out RaycastHit hit, 100, LayerMask.GetMask("Player")))
                {
                    if(hit.transform == playerTrans)
                    {
                        transform.eulerAngles = Vector3.Scale(playerLooker.eulerAngles, Vector3.up);
                        var deltaY = Mathf.Abs((transform.position.y - playerTrans.position.y) / 2);
                        generateArrowPoint.LookAt(target + Vector3.up * deltaY);
                        myState = MyState.attack;
                    }
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
        switch (atribute)
        {
            case Attackable.Atribute.Fire:
                damage *= 2;
                break;
            case Attackable.Atribute.Ice:
                damage /= 3;
                break;
            case Attackable.Atribute.Electro:
                damage /= 3;
                break;
        }

        status.Damage(damage);
        AttackColliderCcontrol(false);
        var getForce = (transform.position - attacker.transform.position).normalized * 3 + Vector3.up;
        getForce *= attacker.ForceMultiplier;
        if (status.HP <= 0) Death(this, getForce, attacker);
        else anim.SetTrigger("GetHit");
    }


    //アニメーションイベントで呼ぶ
    public void WeaponAnimPlay()
    {
        weaponAnim.SetTrigger("Play");
    }

    public void ShootArrow()
    {
        var arrow = Instantiate(arrowPrefab, generateArrowPoint.position, generateArrowPoint.rotation);
        Destroy(arrow, 2);
    }
    
}
