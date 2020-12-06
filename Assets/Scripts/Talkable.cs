using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using HighlightPlus;

[RequireComponent(typeof(HighlightEffect))]
[RequireComponent(typeof(Status))]
public abstract class Talkable : Character, IDamageable
{
    public bool CanTalk { get; set; } = false;
    [SerializeField] protected Flowchart myFlowChart;
    [SerializeField] private AudioSource[] audioSources;
    private Dictionary<AudioSource, AudioClip> audioClips = new Dictionary<AudioSource, AudioClip>();
    protected Animator anim;
    private HighlightEffect highlight;
    protected Quaternion startRotation;
    private Status status;




    protected void Initialize()
    {
        startRotation = transform.rotation;
        anim = GetComponent<Animator>();
        status = GetComponent<Status>();
        highlight = GetComponent<HighlightEffect>();
        setSayDialog = SayDialog.Instance;

        for (int i = 0; i < audioSources.Length; i++)
        {
            audioClips.Add(audioSources[i], audioSources[i].clip);
        }
    }

    protected bool SerchPlayer()
    {
        float search_radius = 2f;
        var ray = new Ray(transform.position, transform.forward);
        var hits = Physics.SphereCastAll(ray, search_radius, 0.01f, LayerMask.GetMask("Player"));
        return hits.Length >= 1;
    }

    public abstract void Talk(PlayerController player);

    public abstract void TalkEnd();

    protected IEnumerator _LookAt(Vector3 targetPos)
    {
        var targetVector = Quaternion.LookRotation(targetPos - transform.position);
        var waitForEndOfFrame = new WaitForEndOfFrame();
        for(int i = 0; i < 30; i++)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetVector, 6);
            yield return waitForEndOfFrame;
        }
    }

    protected IEnumerator _LookRotation(Quaternion targetRot)
    {
        var waitForEndOfFrame = new WaitForEndOfFrame(); for (int i = 0; i < 30; i++)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, 6);
            yield return waitForEndOfFrame;
        }
    }

    protected void HighLightSwitch(bool active)
    {
        highlight.highlighted = active;
    }

    public void GetDamage(float damage, Attackable.Atribute atribute, Attackable attacker)
    {
        status.Damage(damage);

        if (status.HP <= 0)
        {
            anim.SetTrigger("Death");
            GetComponent<Collider>().enabled = false;
            HighLightSwitch(false);
            this.enabled = false;
        }
        else
        {
            anim.SetTrigger("GetHit");
        }
    }

    //アニメーションイベントから参照
    public void PlayAudio(int num)
    {
        var AS = audioSources[num];
        AS.PlayOneShot(audioClips[AS]);
    }


    //FlowChartから参照
    public virtual void TryChoiceRideable(SelectingRideable.RideableCharacter rideableChara)
    {
        SelectingRideable.TrySet(rideableChara);
        FindObjectOfType<ComparePanel>().Enabled = true;
    }

}