using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatIceDragon : Rideable, IDamageable
{
    [Header("固有")]
    [SerializeField] private GameObject IceBallPrefab;
    [SerializeField] private GameObject spreadBlizzardPrefab;
    [SerializeField] private Transform IceGeneratePosi;

    public enum State
    {
        Default, Flying, Death
    }
    private State myState = State.Default;
    private Rigidbody rigid;
    private GameObject instanceBlizzard;

    private readonly float necessarySP_Bite = 0.1f;
    private readonly float necessarySP_IceBall = 0.3f;
    private readonly float necessarySP_SpreadBlizzard = 0.5f;

    // Start is called before the first frame update
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


    // Update is called once per frame
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
                Walk();
                OnGroundAttack();
                TakeOff();
                break;
            case State.Flying:
                transform.position = new Vector3(pos.x, Mathf.Clamp(pos.y, 0, 6), pos.z);
                Fly();
                IceBall();
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
            rigid.velocity = new Vector3(0, rigid.velocity.y, 0);
            anim.SetBool("Walk", false);
            GetOff();
        }
        else
        {
            anim.SetBool("Walk", true);
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
            {
                TurnHorizontal(inputH, inputV);
                rigid.velocity = transform.forward.normalized * moveSpeed;
            }
        }
    }

    private void Fly()
    {
        float inputH = Input.GetAxis("Horizontal");
        float inputV = Input.GetAxis("Vertical");

        TurnHorizontal(inputH, inputV, 1.5f);

        if (inputH == 0 && inputV == 0)
        {
            rigid.velocity = Vector3.zero;
            LandOn();
        }
        else
        {
            // 移動方向にスピードを掛ける
            rigid.velocity = transform.forward.normalized * moveSpeed * 2;
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
            if (SPCheck("SpreadBlizzard"))
            {
                anim.SetBool("SpreadBlizzard", true);
            }
            if (SPCheck("Bite"))
            {
                anim.SetBool("BiteAttack", true);
            }
        }
    }

    private void IceBall()
    {
        if (rideableInput.Attack1)
        {
            if (SPCheck("IceBall"))
            {
                anim.SetBool("IceBall", true);
            }
        }
    }

    protected override bool SPCheck(string attackName)
    {
        switch (attackName)
        {
            case "Bite":
                return Status.SP >= necessarySP_Bite;
            case "IceBall":
                return Status.SP >= necessarySP_IceBall;
            case "SpreadBlizzard":
                return Status.SP >= necessarySP_SpreadBlizzard;
            default:
                return false;
        }
    }



    //ダメージを受ける処理
    public void GetDamage(float damage, Attackable.Atribute atribute, Attackable attacker)
    {
        if (atribute == Attackable.Atribute.Ice) damage *= 0;//攻撃が氷属性ならダメージ０

        Status.Damage(damage);

        if (canvasCtrl != null) canvasCtrl.UpdateHPBar();

        if (Status.HP <= 0)
        {
            anim.applyRootMotion = false;
            rigid.useGravity = true;
            anim.SetTrigger("Death");
            myState = State.Death;

        }
    }



    //アニメーションイベント用メソッド
    public void AnimBoolFalseAll()
    {
        anim.SetBool("BiteAttack", false);
        anim.SetBool("IceBall", false);
        anim.SetBool("SpreadBlizzard", false);
        anim.SetBool("TakeOff", false);
        anim.SetBool("Land", false);
    }

    public void StateChange(string state)
    {
        if (state == "Default")
        {
            myState = State.Default;
            StartCamPos.position += Vector3.down * 4;
            CamTarget.position += Vector3.down * 4;
            nearCamPosi.position += new Vector3 (0, -5.2f, 0);
            nearCamTarget.position += new Vector3(0, -5.2f, 0);
        }
        if (state == "Flying")
        {
            myState = State.Flying;
            StartCamPos.position += Vector3.up * 4;
            CamTarget.position += Vector3.up * 4;
            nearCamPosi.position += new Vector3(0, 5.2f, 0);
            nearCamTarget.position += new Vector3(0, 5.2f, 0);
        }
    }

    public void ShootIceBall()
    {
        var iceBall = Instantiate(IceBallPrefab, IceGeneratePosi.position, IceGeneratePosi.rotation);

        Destroy(iceBall, 3);

    }

    public void SpreadBlizzard(string state)
    {
        if(state == "Start")
        {
            instanceBlizzard = Instantiate(spreadBlizzardPrefab, IceGeneratePosi);
        }
        if(state == "Stop")
        {
            instanceBlizzard.GetComponentInChildren<SpreadBlizzard>().StopParticle();

        }
    }
}
