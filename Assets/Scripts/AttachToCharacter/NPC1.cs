using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC1 : Talkable
{
    private SceneTransitioner sceneTransitioner;

    private void Start()
    {
        sceneTransitioner = FindObjectOfType<SceneTransitioner>();
        Initialize();
    }


    private void Update()
    {
        HighLightSwitch(CanTalk);
    }
    private void FixedUpdate()
    {
        CanTalk = SerchPlayer();
    }



    //TalkSystemManagerから参照
    public override void Talk(PlayerController player)
    {
        HighLightSwitch(false);
        StartCoroutine(_LookAt(player.transform.position));
        anim.SetBool("Talk", true);
        myFlowChart.ExecuteBlock("Start");
    }

    //FlowChartから参照
    public override void TalkEnd()
    {
        anim.SetBool("Talk", false);
        StartCoroutine(_LookRotation(startRotation));
        TalkSystemManager.TalkEnd();
    }

    //FlowChartから参照
    public void SceneTransition(int sceneNum)
    {
        sceneTransitioner.TransitionStart(sceneNum);
    }
}
