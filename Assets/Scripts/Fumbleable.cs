using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using HighlightPlus;

public abstract class Fumbleable : Character
{
    protected bool canFumble = false;
    public bool CanFumble => canFumble;
    [SerializeField] protected Flowchart myFlowChart;
    [SerializeField] protected AudioSource[] audioSources;
    protected Dictionary<AudioSource, AudioClip> audioClips = new Dictionary<AudioSource, AudioClip>();
    protected HighlightEffect highlight;
    protected static SayDialog sayDialog;

    protected PlayerController Player { get; set; } = null;


    protected abstract void Initialize();

    protected bool SerchPlayer()
    {
        float search_radius = 2f;
        var ray = new Ray(transform.position, transform.forward);
        var hits = Physics.SphereCastAll(ray, search_radius, 0.01f, LayerMask.GetMask("Player"));
        return hits.Length >= 1;
    }

    public void Fumbled(PlayerController player)
    {
        HighLightSwitch(false);
        myFlowChart.ExecuteBlock("Start");
        Player = player;
    }

    //FlowChartから参照
    public abstract void FumbleEnd();

    protected void HighLightSwitch(bool active)
    {
        highlight.highlighted = active;
    }

    //FlowChartから参照
    public abstract void PlayAudio(int num);

}
