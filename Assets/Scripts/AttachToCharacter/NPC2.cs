﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC2 : Talkable
{
    private void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        myFlowChart.SetBooleanVariable("selecting_NPC2", SelectingRideable.IsGriffon);
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
    public void ChoiceRideable(SelectingRideable.RideableCharacter rideableChara)
    {
        SelectingRideable.Set(rideableChara);
    }

    //FlowChartから参照
    public override void TryChoiceRideable(SelectingRideable.RideableCharacter rideableChara)
    {
        base.TryChoiceRideable(rideableChara);
    }


}
