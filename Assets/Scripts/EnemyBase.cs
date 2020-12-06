using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Status))]
public abstract class EnemyBase : MonoBehaviour
{
    [Header("EnemyBase")]
    [SerializeField] protected float moveSpeed = 1;
    [SerializeField] protected float turnSpeed = 1;
    [SerializeField] protected int targetingRange = 15;
    [SerializeField] protected float validAttackRange = 1f;
    [SerializeField] private float energyPoint = 0.5f;
    [SerializeField] protected Transform myBone;
    [SerializeField] protected Transform playerLooker;//プレイヤーの方を向くときの回転角取得用
    [SerializeField] protected Collider[] attackColliders;
    [SerializeField] protected AudioSource[] audioSources;

    protected float distance;

    //AttackerObjから参照可能なステータスクラス
    [HideInInspector] public Status status;


    public enum MyState
    {
        idle, chase, search, attack, death
    }
    protected MyState myState;

    
    protected Animator anim;
    private Rigidbody rigid;
    protected Rigidbody[] bones;
    private static PlayerController player;
    protected static Transform playerTrans;
    private Dictionary<AudioSource, AudioClip> audioClips = new Dictionary<AudioSource, AudioClip>();


    protected virtual void Initialize()
    {
        status = GetComponent<Status>();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        bones = myBone.GetComponentsInChildren<Rigidbody>();

        for (int i = 0; i < audioSources.Length; i++)
        {
            audioClips.Add(audioSources[i], audioSources[i].clip);
        }

        if (playerTrans == null)
        {
            player = FindObjectOfType<PlayerController>();
            playerTrans = player.transform;
        }
    }

    protected bool ObstacleCheck()
    {
        return Physics.Linecast(transform.position, playerTrans.position + Vector3.up, LayerMask.GetMask("Ground"));
    }

    protected void MoveForward()
    {
        anim.applyRootMotion = false;
        anim.SetBool("Walk", true);
        rigid.velocity = transform.forward.normalized * moveSpeed + Vector3.Scale(rigid.velocity, Vector3.up);
    }

    protected void TurnHorizontal()
    {
        var nextVector = (playerTrans.position - transform.position).normalized;
        var targetAngle = Mathf.Atan2(nextVector.x, nextVector.z) * Mathf.Rad2Deg;
        var nextAngle = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime * 100);

        transform.eulerAngles = nextAngle;

    }

    protected bool PlayerCheck()
    {
        var ray = new Ray(transform.position + Vector3.up, transform.forward);
        if(Physics.Raycast(ray, out var hit, validAttackRange * validAttackRange, LayerMask.GetMask("Player")))
        {
            return hit.transform.root.TryGetComponent<IDamageable>(out var d);
        }
        return false;
    }

    protected virtual void Death(IEnemyAI enemyAI, Vector3 force, Attackable attackable)
    {
        if (attackable.EnableHeal) player.GetEnergy(energyPoint);

        GetComponent<Collider>().enabled = false;
        GetComponent<LODGroup>().enabled = false;
        myState = MyState.death;
        anim.enabled = false;
        foreach (var bone in bones)
        {
            bone.isKinematic = false;
            bone.constraints = RigidbodyConstraints.None;
            bone.AddForce(force * bone.mass, ForceMode.Impulse);
        }

        EnemyAIUpdator.RemoveList(enemyAI);
        Destroy(gameObject, 4);
    }

    protected void AttackColliderCcontrol(bool enabled)
    {
        foreach (var collider in attackColliders)
        {
            collider.GetComponent<Attackable>().ListReset();
            collider.enabled = enabled;
        }
    }

    //Rideモード切り替え時にRideableから呼ぶ
    public static void PlayerChange(Transform t)
    {
        playerTrans = t;
    }

    //アニメーションイベント用
    public void StateReset()
    {
        myState = MyState.idle;
    }

    public void AttackColliderSwitch(string enabled)
    {
        foreach(var collider in attackColliders)
        {
            collider.GetComponent<Attackable>().ListReset();
            if (enabled == "true") collider.enabled = true;
            else if (enabled == "false") collider.enabled = false;
        }
    }

    public void PlayAudio(int num)
    {
        var AS = audioSources[num];
        float randomValue = Random.Range(-0.15f, 0.15f);
        AS.pitch += randomValue;

        AS.PlayOneShot(audioClips[AS]);
    }

}