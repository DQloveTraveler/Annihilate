using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using HighlightPlus;

public class Lever : Fumbleable
{
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    protected override void Initialize()
    {
        highlight = GetComponent<HighlightEffect>();
        if (sayDialog != null)
        {
            sayDialog = FindObjectOfType<SayDialog>();
        }
        setSayDialog = sayDialog;

        for (int i = 0; i < audioSources.Length; i++)
        {
            audioClips.Add(audioSources[i], audioSources[i].clip);
        }
    }

    // Update is called once per frame
    void Update()
    {
        HighLightSwitch(canFumble);
    }

    private void FixedUpdate()
    {
        canFumble = SerchPlayer();
    }

    //FlowChartから参照
    public override void FumbleEnd()
    {
        Player.FumbleEnd();
    }

    //FlowChartから参照
    public override void PlayAudio(int num)
    {
        var AS = audioSources[num];
        AS.PlayOneShot(audioClips[AS]);
    }
}
