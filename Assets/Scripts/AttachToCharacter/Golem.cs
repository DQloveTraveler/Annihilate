using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : Rideable, IDamageable
{
    [Header("固有")]
    [SerializeField] private GameObject smashEffectPrefab;
    [SerializeField] private Transform effectGeneratePoint;

    public enum State
    {
        Default, Death
    }
    private State myState = State.Default;
    private Rigidbody rigid;

    private readonly float necessarySP_attackBasic = 0.1f;
    private readonly float necessarySP_attackWalking = 0.2f;
    private readonly float necessarySP_smash = 0.5f;
 
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
                transform.position = new Vector3(pos.x, 0, pos.z);
                Walk();
                Attack();
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

    private void Attack()
    {
        bool isWalking = anim.GetCurrentAnimatorStateInfo(0).IsName("Walk");

        switch (rideableInput.AttackOfGolem)
        {
            case 0:
                break;
            case 1:
                if (isWalking)
                {
                    if (SPCheck("AttackWalking"))
                    {
                        anim.SetBool("Attack", true);
                    }
                }
                else
                {
                    if (SPCheck("AttackBasic"))
                    {
                        anim.SetBool("Attack", true);
                    }
                }
                break;
            case 2:
                if (SPCheck("Smash"))
                {
                    anim.SetBool("Smash", true);
                }
                break;
        }
    }

    protected override bool SPCheck(string attackName)
    {
        switch (attackName)
        {
            case "AttackBasic":
                return Status.SP >= necessarySP_attackBasic;
            case "AttackWalking":
                return Status.SP >= necessarySP_attackWalking;
            case "Smash":
                return Status.SP >= necessarySP_smash;
            default:
                return false;
        }
    }



    //ダメージを受ける処理
    public void GetDamage(float damage, Attackable.Atribute atribute, Attackable attacker)
    {
        if (atribute == Attackable.Atribute.Earth) damage *= 0;//攻撃が土属性ならダメージ0
        if (atribute == Attackable.Atribute.None) damage /= 2;//攻撃が無属性ならダメージ半分

        Status.Damage(damage);

        if (canvasCtrl != null) canvasCtrl.UpdateHPBar();

        if (Status.HP <= 0)
        {
            anim.SetTrigger("Death");
            myState = State.Death;
        }
    }



    //アニメーションイベント用メソッド
    public void AnimBoolFalseAll()
    {
        anim.SetBool("Attack", false);
        anim.SetBool("Smash", false);
    }

    public void Smash()
    {
        var impactEffect = Instantiate(smashEffectPrefab, effectGeneratePoint.position, effectGeneratePoint.rotation);
        Destroy(impactEffect, 10);
    }

}
