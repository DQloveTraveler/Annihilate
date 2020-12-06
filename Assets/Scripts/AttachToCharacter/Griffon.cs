using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Griffon : Rideable, IDamageable
{
    [Header("Griffon固有")]
    [SerializeField] private GameObject thunderPrefab;
    [SerializeField] private Transform thunderGeneratePoint;

    public enum State
    {
        Default, Flying, Death
    }
    private State myState = State.Default;
    private Rigidbody rigid;

    private readonly float necessarySP_Bite = 0.1f;
    private readonly float necessarySP_ClawAttack = 0.2f;
    private readonly float necessarySP_Thunder = 0.5f;


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
                if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Landing"))
                {
                    transform.position = new Vector3(pos.x, 0, pos.z);
                }
                WalkControl();
                OnGroundAttack();
                TakeOff();
                break;
            case State.Flying:
                transform.position = new Vector3(pos.x, Mathf.Clamp(pos.y, 0, 4), pos.z);
                Fly();
                ThunderAttack();
                LandOn();
                break;
            case State.Death:
                GetOff();
                break;
        }
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }

    private void WalkControl()
    {
        float inputH = Input.GetAxis("Horizontal");
        float inputV = Input.GetAxis("Vertical");
        anim.applyRootMotion = true;

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
        bool isFlying = anim.GetCurrentAnimatorStateInfo(0).IsName("Flying");

        if (isFlying)
        {
            TurnHorizontal(0, 0, 1.2f);
        }

        float flyHorizontal = Mathf.MoveTowards(anim.GetFloat("FlyHorizontal"), inputH, 0.1f);
        float flyVertical = Mathf.MoveTowards(anim.GetFloat("FlyVertical"), inputV, 0.1f);
        anim.SetFloat("FlyHorizontal", flyHorizontal);
        anim.SetFloat("FlyVertical", flyVertical);

        if (inputH == 0 && inputV == 0)
        {
            anim.applyRootMotion = true;
        }
        else 
        {
            if (isFlying)
            {
                anim.applyRootMotion = false;
                var moveVector = (transform.right * inputH + transform.forward * inputV).normalized;
                if (inputV == 1)
                {
                    moveVector *= 1.5f;
                }
                // 移動方向にスピードを掛ける
                rigid.velocity = moveVector * moveSpeed;
            }
            else
            {
                rigid.velocity = Vector3.zero;
            }
        }
    }

    private void TakeOff()
    {
        if (rideableInput.TakeOff)
        {
            anim.SetBool("TakeOff", true);
        }
    }

    private void LandOn()
    {
        if (rideableInput.LandOn)
        {
            anim.SetBool("Land", true);
        }
    }

    private void OnGroundAttack()
    {
        if (rideableInput.Attack1)
        {
            if (SPCheck("Bite"))
            {
                anim.SetBool("Bite", true);
            }
            if (SPCheck("ClawAttack"))
            {
                anim.SetBool("ClawAttack", true);
            }
        }
    }

    private void ThunderAttack()
    {
        if (rideableInput.ThunderAttack)
        {
            if(SPCheck("Thunder"))
            {
                anim.SetBool("Thunder", true);
            }
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
            case "Thunder":
                return Status.SP >= necessarySP_Thunder;
            default:
                return false;
        }
    }

    //ダメージを受ける処理
    public void GetDamage(float damage, Attackable.Atribute atribute, Attackable attacker)
    {
        Status.Damage(damage);
        if (canvasCtrl != null)
        {
            canvasCtrl.UpdateHPBar();
        }

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
        anim.SetBool("Thunder", false);
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
            case "Flying":
                myState = State.Flying;
                break;
        }
    }

    public void UseGravitySwitch(string useGravity)
    {
        if (useGravity == "true")
        {
            rigid.useGravity = true;
        }
        if (useGravity == "false")
        {
            rigid.useGravity = false;
        }
    }

    public void JumpToFly()
    {
        StartCoroutine(_JumpToFly());
    }

    private IEnumerator _JumpToFly()
    {
        var waitForEndOfFrame = new WaitForEndOfFrame();
        for(int i = 0; i < 20; i++)
        {
            transform.position += (Vector3.up / 20) * 4;
            transform.position += (transform.forward.normalized / 20) * 4;
            yield return waitForEndOfFrame;
        }
    }

    public void Thunder()
    {
        var thunder = Instantiate(thunderPrefab, thunderGeneratePoint.position, thunderGeneratePoint.rotation);
        Destroy(thunder, 6);
    }
}
