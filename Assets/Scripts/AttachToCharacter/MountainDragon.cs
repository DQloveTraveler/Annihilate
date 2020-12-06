using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MountainDragon : Rideable, IDamageable
{
    [Header("MountainDragon")]
    [SerializeField] private GameObject fireBallPrefab;
    [SerializeField] private GameObject spreadFirePrefab;
    [SerializeField] private Transform fireGeneratePosi;

    public enum State
    {
        Default, TakeOff, Flying, Landing, Death
    }
    private State myState = State.Default;
    private Rigidbody rigid;
    private GameObject InstanceFire;


    private readonly float necessarySP_Bite = 0.1f;
    private readonly float necessarySP_ClawAttack = 0.15f;
    private readonly float necessarySP_FireBall = 0.3f;
    private readonly float necessarySP_SpreadFire = 0.4f;


    void Start()
    {
        Initialize();
    }

    protected override void Initialize()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        Status = GetComponent<Status>();

        for (int i = 0; i < audioSources.Length; i++)
        {
            audioClips.Add(audioSources[i], audioSources[i].clip);
        }
    }

    void Update()
    {
        if (Riding)
        {
            RidingAction();
        }
    }

    void LateUpdate()
    {
        AnimBoolFalseAll();
    }

    private void RidingAction()
    {
        CameraChange();

        var pos = transform.position;
        switch (myState)
        {
            case State.Default:
                transform.position = new Vector3(pos.x, 0, pos.z);
                Walk();
                OnGroundAttack();
                TakeOff();
                break;
            case State.Landing:
                break;
            case State.Flying:
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(pos.x, 6, pos.z), 0.1f);
                Fly();
                FireAttack();
                LandOn();
                break;
            case State.Death:
                GetOff();
                break;
        }
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }

    private void Walk()
    {
        float inputH = Input.GetAxis("Horizontal");
        float inputV = Input.GetAxis("Vertical");

        if (inputH == 0 && inputV == 0)
        {
            anim.SetBool("Walk", false);
            GetOff();
        }
        else
        {
            anim.SetBool("Walk", true);
            TurnHorizontal(inputH, inputV);
        }
    }

    private void Fly()
    {
        float inputH = Input.GetAxis("Horizontal");
        float inputV = Input.GetAxis("Vertical");
        
        TurnHorizontal(inputH, inputV, 2f);

        if (inputH == 0 && inputV == 0)
        {
            anim.SetBool("Fly", false);
            anim.applyRootMotion = true;
            rigid.velocity = Vector3.zero;
        }
        else
        {
            anim.SetBool("Fly", true);

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                anim.applyRootMotion = false;
            }
            // 移動方向にスピードを掛ける
            rigid.velocity = transform.forward.normalized * moveSpeed;
        }
    }
    
    private void TakeOff()
    {
        if(rideableInput.TakeOff)
        {
            anim.SetBool("TakeOff", true);
        }
    }

    private void LandOn()
    {
        if (rideableInput.LandOn)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                anim.SetBool("Land", true);
            }
        }
    }

    private void OnGroundAttack()
    {
        if(rideableInput.Attack1)
        {
            if (SPCheck("ClawAttack"))
            {
                anim.SetBool("ClawAttack", true);
            }
            if (SPCheck("Bite"))
            {
                anim.SetBool("Bite", true);
            }
        }
    }

    private void FireAttack()
    {
        switch (rideableInput.FireAttack)
        {
            case 0:
                break;
            case 1:
                if (SPCheck("FireBall"))
                {
                    anim.SetBool("FireBall", true);
                }
                break;
            case 2:
                if (SPCheck("SpreadFire"))
                {
                    anim.SetBool("SpreadFire", true);
                }
                break;
        }
    }

    protected override bool SPCheck(string attackName)
    {
        switch (attackName)
        {
            case "Bite":
                return Status.SP >= necessarySP_Bite;
            case "ClawAttack":
                return Status.SP >= necessarySP_ClawAttack;
            case "FireBall":
                return Status.SP >= necessarySP_FireBall;
            case "SpreadFire":
                return Status.SP >= necessarySP_SpreadFire;
            default:
                return false;
        }
    }

    //ダメージを受ける処理
    public void GetDamage(float damage, Attackable.Atribute atribute, Attackable attacker)
    {
        if(atribute == Attackable.Atribute.Fire) damage *= 0;//攻撃が火属性ならダメージ０

        Status.Damage(damage);

        if(canvasCtrl != null) canvasCtrl.UpdateHPBar();

        if (Status.HP <= 0)
        {
            anim.applyRootMotion = false;
            rigid.useGravity = true;
            anim.SetTrigger("Death");
            myState = State.Death;
            
        }
    }




    //以下、アニメーションイベント用メソッド
    public void AnimBoolFalseAll()
    {
        anim.SetBool("ClawAttack", false);
        anim.SetBool("Bite", false);
        anim.SetBool("FireBall", false);
        anim.SetBool("SpreadFire", false);
        anim.SetBool("TakeOff", false);
        anim.SetBool("Land", false);
    }

    public void StateChange(string state)
    {
        switch (state)
        {
            case "Default":
                myState = State.Default;
                break;
            case "TakeOff":
                myState = State.TakeOff;
                break;
            case "Flying":
                myState = State.Flying;
                break;
            case "Landing":
                myState = State.Landing;
                break;
        }
    }

    public void ShootFireBall()
    {
        Instantiate(fireBallPrefab, fireGeneratePosi.position, fireGeneratePosi.rotation);
    }

    public void SpreadFire(string state)
    {
        if(state == "Start")
        {
           InstanceFire = Instantiate(spreadFirePrefab, fireGeneratePosi.position, fireGeneratePosi.rotation, fireGeneratePosi);
        }
        else if(state == "Stop")
        {
            InstanceFire.GetComponentInChildren<SpreadFire>().StopParticle();
        }
    }

}
