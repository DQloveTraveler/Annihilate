using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkSystemManager
{
    public enum Options {Null, Yes, No}
    public static Options Option { get; set; }
    private static CanvasController mainCanvas;
    private static PlayerController talkingPlayer;
    private static Talkable talkingNPC;

    public static void SetMainCanvas(CanvasController canvas)
    {
        mainCanvas = canvas;
    }

    public static void TalkStart(PlayerController player, Talkable talkable)
    {
        talkingPlayer = player;
        talkingNPC = talkable;
        talkable.Talk(player);
    }

    public static void TalkEnd()
    {
        talkingPlayer.TalkEnd();
    }
}
