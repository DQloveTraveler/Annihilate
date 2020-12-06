using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using PlayerInputManagement;
using ObjectCheck;
using static UnityEngine.TouchScreenKeyboard;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(WeaponEnergy))]
public class PlayerController : Playable, IDamageable
{
    [Header("パラメータ")]
    [SerializeField] private float jumpPower;
    [SerializeField] private bool autoLockOn = false;

    [Header("その他")]
    [SerializeField] private Transform swordTrans_UnEquipped;
    [SerializeField] private Transform swordTrans_Equipped;
    [SerializeField] private Collider swordCollider;
    [SerializeField] private GameObject swordTrail;
    [SerializeField] private Transform enemyLooker;
    [SerializeField] private GameObject[] effectList;
    [SerializeField] private Transform[] effectInstantPoint;

    private readonly int necessaryWE_HeavyAttack = 20;


    private Collider playerCollider;
    private bool isIdle = false;
    [HideInInspector] public bool isRun = false;
    private bool isJump = false;
    private Rigidbody ribo;
    private Attackable sword;
    private HighlightPlus.HighlightEffect swordHighlight;

    private Rideable selectingRideable;
    private Talkable selectingTalkable;
    private WeaponEnergy weaponEnergy;

    //プレイヤーの状態
    public enum State
    {
        Default, Equiping, IgnoreInput, 
    }
    private State myState = State.Default;


    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        //カメラ生成
        GenerateCamera();

        //キャンバス生成
        GenerateCanvas();


        for (int i = 0; i < audioSources.Length; i++)
        {
            audioClips.Add(audioSources[i], audioSources[i].clip);
        }
        weaponEnergy = GetComponent<WeaponEnergy>();
        Status = GetComponent<Status>();
        anim = GetComponent<Animator>();
        ribo = GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();
        sword = swordCollider.GetComponent<Attackable>();
        swordHighlight = sword.GetComponent<HighlightPlus.HighlightEffect>();
        swordTrail.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInput.Updating();
        PlayingAnimCheck();
        WeaponHighlightSwitch();

        switch (myState)
        {
            case State.Default:
                UnEquipingAction();
                RideableCheck();
                TalkableCheck();
                FumbleableCheck();
                Move();
                break;
            case State.Equiping:
                canvasCtrl.SetActiveGuideMessage("RideOn", false);
                if(selectingRideable != null) selectingRideable.HighLightSwitch(false);
                EquipingAction();
                Move();
                AutoLockOnSwitch();
                LockOn();
                break;
            case State.IgnoreInput:
                break;
        }
    }

    void FixedUpdate()
    {
        GroundCheck();
    }

    void LateUpdate()
    {
        AnimBoolAllOFF();
    }



    //再生中のアニメーションをチェック
    private void PlayingAnimCheck()
    {
        isIdle = anim.GetCurrentAnimatorStateInfo(0).IsName("Idle");
        isRun = anim.GetCurrentAnimatorStateInfo(0).IsName("Run");
        Status.HealStop = isRun;
        if (isRun) Status.UseSP(0.002f);
    }

    private void AnimBoolAllOFF()
    {
        anim.SetBool("Attack1", false);
        anim.SetBool("Attack2", false);
        anim.SetBool("Attack3", false);
        anim.SetBool("Attack4", false);
        anim.SetBool("Avoid", false);
        anim.SetBool("Eqip", false);
        anim.SetBool("UnEqip", false);
        anim.SetBool("Jump", false);
        anim.SetBool("Jump_End", false);
        anim.SetBool("Fall", false);
        anim.SetBool("Fall_End", false);
        anim.SetBool("GetHit", false);
    }

    private void Move()
    {
        float inputH = Input.GetAxis("Horizontal");
        float inputV = Input.GetAxis("Vertical");
        
        if (inputH == 0 && inputV == 0)
        {
            if(!isJump) ribo.velocity = new Vector3(0, ribo.velocity.y, 0);
            anim.SetBool("Run", false);
        }
        if (isIdle || isRun)
        {
            if(inputH != 0 || inputV != 0)
            {
                Vector3 moveForward = transform.forward.normalized;

                moveForward *= moveSpeed;
                anim.SetBool("Run", true);


                if (myState == State.Default) moveForward *= 1.3f;
                if (Status.SP <= 0.02f)
                {
                    moveForward *= 0.7f;
                    anim.SetFloat("RunSpeed", 0.7f);
                }
                else anim.SetFloat("RunSpeed", 1);

                ribo.velocity = moveForward + new Vector3(0, ribo.velocity.y, 0);
                TurnHorizontal(inputH, inputV);
            }
        }
        else if(!isJump)
        {
            ribo.velocity = new Vector3(0, ribo.velocity.y, 0);
        }
    }
    
    private void TurnHorizontal(float inputH, float inputV)
    {
        // カメラの方向から、X-Z平面の単位ベクトルを取得
        var cameraForward = Vector3.Scale(cameraCtrl.transform.forward, new Vector3(1, 0, 1)).normalized;
        //回転方向を算出
        var nextVector = cameraForward * inputV + cameraCtrl.transform.right * inputH;

        var targetAngle = Mathf.Atan2(nextVector.x, nextVector.z) * Mathf.Rad2Deg;
        var nextAngle = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime * 100);
        //回転処理
        transform.eulerAngles = nextAngle;
    }

    private void AutoLockOnSwitch()
    {
        autoLockOn = PlayerInput.AutoLockOn;
    }

    private void LockOn()
    {
        if (PlayerInput.LockOn)
        {
            try
            {
                var lockonTarget = GetLockOnTarget();
                enemyLooker.LookAt(lockonTarget.position);

                if (isIdle)
                {
                    transform.eulerAngles = new Vector3(0, enemyLooker.eulerAngles.y, 0);
                    cameraCtrl.LockOn(10);

                }
                else if (isRun)
                {
                    cameraCtrl.LockOn(10, lockonTarget.position);
                }
            }
            catch
            { }
        }else if (autoLockOn)
        {
            try
            {
                var lockonTarget = GetLockOnTarget();
                enemyLooker.LookAt(lockonTarget.position);

                transform.eulerAngles = new Vector3(0, enemyLooker.eulerAngles.y, 0);
            }
            catch
            { }
        }
    }

    private Transform GetLockOnTarget()
    {
        float search_radius = 30f;

        var hits = Physics.SphereCastAll(
            transform.position,
            search_radius,
            transform.forward,
            0.01f,
            LayerMask.GetMask("Enemy")
        ).Select(h => h.transform.gameObject).ToList();

        if (0 < hits.Count())
        {
            float min_target_distance = float.MaxValue;
            Transform target = null;

            foreach (var hit in hits)
            {
                if(hit.gameObject != gameObject)
                {
                    float target_distance = Vector3.Distance(transform.position, hit.transform.position);
                    if (target_distance < min_target_distance)
                    {
                        min_target_distance = target_distance;
                        target = hit.transform;
                    }
                }
            }

            return target;
        }
        else
        {
            return null;
        }
    }

    private void GroundCheck()
    {
        Ray ray = new Ray(transform.position + Vector3.up, Vector3.down);
        //RaycastHit hit;
        if (Physics.SphereCast(ray, 0.3f, 1f, LayerMask.GetMask("Ground", "Enemy", "Default", "Player")))
        {
            anim.SetBool("Fall", false);
            anim.SetBool("Jump_End", true);
            anim.SetBool("Fall_End", true);
        }
        else
        {
            anim.SetBool("Fall", true);
            anim.SetBool("Jump_End", false);
            anim.SetBool("Fall_End", false);
        }
    }

    private void UnEquipingAction()
    {
        if(isIdle || isRun)
        {
            if (PlayerInput.EquipSwitch)
            {
                anim.SetBool("Eqip", true);
            }
            if (PlayerInput.Jump)
            {
                anim.SetBool("Jump", true);
            }
        }
    }

    private void EquipingAction()
    {
        if(isIdle || isRun)
        {
            if (PlayerInput.EquipSwitch)
            {
                anim.SetBool("UnEqip", true);
            }
            if (PlayerInput.Avoid)
            {
                if(Status.SP > 0.2f)
                {
                    anim.SetBool("Avoid", true);
                    anim.applyRootMotion = true;
                }
                PlayerInput.InputKey = PlayerInput.Key.NULL;
            }
            if (isRun)
            {
                if (PlayerInput.LightAttack)
                {
                    anim.SetBool("Attack1", true);
                    swordTrail.SetActive(true);
                }
                if (PlayerInput.HeavyAttack)
                {
                    if (weaponEnergy.Value >= necessaryWE_HeavyAttack)
                    {
                        anim.SetBool("Attack2", true);
                        swordTrail.SetActive(true);
                    }
                }
            }
            if (isIdle)
            {
                if (PlayerInput.LightAttack)
                {
                    anim.SetBool("Attack4", true);
                    PlayerInput.ResetComboTime(1);
                    swordTrail.SetActive(true);
                }
                if (PlayerInput.HeavyAttack)
                {
                    if (weaponEnergy.Value >= necessaryWE_HeavyAttack)
                    {
                        anim.SetBool("Attack3", true);
                        swordTrail.SetActive(true);
                    }
                }
            }
        }
        else
        {
            if (PlayerInput.LightAttack)
            {
                if (PlayerInput.Combo2)
                {
                    anim.SetBool("Combo2", true);
                }
                else if (PlayerInput.Combo1)
                {
                    anim.SetBool("Combo1", true);
                    PlayerInput.ResetComboTime(2);
                }
            }
        }
    }

    private void WeaponHighlightSwitch()
    {
        swordHighlight.highlighted = weaponEnergy.Value >= weaponEnergy.MaxValue;
    }

    private void RideableCheck()
    {
        bool canRide = ObjectChecker.RideableCheck(this, out selectingRideable);
        canvasCtrl.SetActiveGuideMessage("RideOn", canRide);

        if (PlayerInput.RideOn && canRide)
        {
            RideOn(selectingRideable);
        }
    }

    private void TalkableCheck()
    {
        bool canTalk = ObjectChecker.TalkableCheck(this, out selectingTalkable);

        if (canTalk)
        {
            UIManager.Instance.SetCursor(1);
            if (PlayerInput.Talk)
            {
                UIManager.Instance.SetCursor(0);
                Talk(selectingTalkable);
            }
        }
        else
        {
            UIManager.Instance.SetCursor(0);
        }
    }

    private void FumbleableCheck()
    {
        bool canFumble = ObjectChecker.FumbleableCheck(this, out var fumbleable);

        if(canFumble && PlayerInput.FumbleSomething)
        {
            FumbleSomething(fumbleable);
        }
    }

    private void FumbleSomething(Fumbleable fumbleable)
    {
        myState = State.IgnoreInput;
        cameraCtrl.enabled = false;
        StartCoroutine(_LookAt(fumbleable.transform.position));
        fumbleable.Fumbled(this);
    }

    //Fumbleableから参照
    public void FumbleEnd()
    {
        myState = State.Default;
        cameraCtrl.enabled = true;
    }



    //NPCと会話するときの処理
    private void Talk(Talkable talkable)
    {
        myState = State.IgnoreInput;
        cameraCtrl.enabled = false;
        StartCoroutine(_LookAt(talkable.transform.position));
        TalkSystemManager.TalkStart(this, talkable);
    }

    //TalkSystemManagerから参照
    public void TalkEnd()
    {
        myState = State.Default;
        cameraCtrl.enabled = true;
    }

    private IEnumerator _LookAt(Vector3 target)
    {
        var targetVector = Quaternion.LookRotation(target - transform.position);
        var waitForEndOfFrame = new WaitForEndOfFrame();
        for(int i = 0; i < 30; i++)
        {
            var nextRotation = Quaternion.RotateTowards(transform.localRotation, targetVector, 8);
            transform.localEulerAngles = Vector3.Scale(nextRotation.eulerAngles, Vector3.up);
            yield return waitForEndOfFrame;
        }
    }



    //モンスターに乗るときの処理
    private void RideOn(Rideable rideable)
    {
        myState = State.IgnoreInput;
        Destroy(cameraCtrl.gameObject);
        Destroy(canvasObj.gameObject);
        canvasCtrl.SetActiveGuideMessage("RideOn", false);
        rideable.Ridden(this);
    }

    //モンスターから降りるかどうか判定
    //Rideableから確認
    public bool GetOff()
    {
        if (PlayerInput.GetOff)
        {
            GenerateCamera();
            GenerateCanvas();
            myState = State.Default;
            return true;
        }
        else return false;
    }


    //EnemyBaseから参照
    public void GetEnergy(float energyPoint)
    {
        PlayAudio(6);
        cameraCtrl.RadialBlurEffect();
        weaponEnergy.GetEnergy(energyPoint);
        if(weaponEnergy.Value >= weaponEnergy.MaxValue && Status.HP > 0)
        {
            Status.HealHP(energyPoint / 2);
            canvasCtrl.UpdateHPBar();
        }
    }


    //ダメージを受ける処理
    public void GetDamage(float damage, Attackable.Atribute atribute, Attackable attacker)
    {
        Status.Damage(damage);
        canvasCtrl.UpdateHPBar();
        cameraCtrl.ShortShake();

        if (Status.HP <= 0)
        {
            anim.SetTrigger("Death");
            myState = State.IgnoreInput;
        }
    }
    


    //以下、アニメーションイベント用メソッド
    public void swordTrailOFF()
    {
        swordTrail.SetActive(false);
        anim.SetBool("Combo1", false);
        anim.SetBool("Combo2", false);
    }

    public void Avoid(string start_end)
    {
        if (start_end == "start")
        {
            playerCollider.enabled = false;
            Status.UseSP(0.2f);
        }
        else if (start_end == "end")
        {
            playerCollider.enabled = true;
        }
    }

    public void Jump()
    {
        ribo.AddForce(Vector3.up * jumpPower * 10, ForceMode.Impulse);
    }

    public void Death()
    {
        playerCollider.enabled = false;
        PlayAudio(7);
    }

    public void Switch_of_isJump(string true_false)
    {
        if (true_false == "true") isJump = true;
        if (true_false == "false") isJump = false;
    }

    public void ApplyLootMotionSwitch(string true_false)
    {
        if (true_false == "true") anim.applyRootMotion = true;
        else if (true_false == "false") anim.applyRootMotion = false;
    }

    public void SwordColliderSwitch(string true_false)
    {
        sword.ListReset();
        if (true_false == "true") swordCollider.enabled = true;
        else if (true_false == "false") swordCollider.enabled = false;
    }

    public void SwordEquipSwitch()
    {
        if (swordCollider.transform.parent == swordTrans_Equipped)
        {
            swordCollider.transform.parent = swordTrans_UnEquipped;
            swordCollider.transform.localPosition = Vector3.zero;
            swordCollider.transform.localRotation = Quaternion.identity;
            myState = State.Default;
        }
        else if (swordCollider.transform.parent == swordTrans_UnEquipped)
        {
            swordCollider.transform.parent = swordTrans_Equipped;
            swordCollider.transform.localPosition = Vector3.zero;
            swordCollider.transform.localRotation = Quaternion.identity;
            myState = State.Equiping;
        }
    }

    public void SwordTrailActive()
    {
        swordTrail.SetActive(true);
    }

    public void InstantEffect(int effectNum)
    {
        GameObject instanceEffect = Instantiate(effectList[effectNum], effectInstantPoint[effectNum].position, transform.rotation);

        if (effectNum == 1)
        {
            Destroy(instanceEffect, 1.2f);
        }
        else
        {
            Destroy(instanceEffect, 4);
        }
    }

    public void SetAttackPower(int power)
    {
        sword.SetAttackPower(power);
    }

}
