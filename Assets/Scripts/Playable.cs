using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Status))]
public abstract class Playable : MonoBehaviour
{
    [Header("Playable")]
    [SerializeField] protected float moveSpeed = 1f;
    [SerializeField] protected float turnSpeed = 1f;
    [SerializeField] protected GameObject cameraPrefab;
    [SerializeField] protected GameObject canvasPrefab;
    [SerializeField] protected AudioSource[] audioSources;
    protected Dictionary<AudioSource, AudioClip> audioClips = new Dictionary<AudioSource, AudioClip>();

    [Header("CameraContorollerに渡す用")]
    [SerializeField] protected Transform StartCamPos;
    [SerializeField] protected Transform CamTarget;

    protected Status Status { get; set; }
    protected Animator anim;
    protected CameraController cameraCtrl;
    protected GameObject canvasObj;
    protected CanvasController canvasCtrl;
    protected HPBar hPBar;

    protected void GenerateCamera()
    {
        cameraCtrl = CameraGenerator.Generate(cameraPrefab, StartCamPos, CamTarget);
    }

    protected void GenerateCanvas()
    {
        canvasObj = CanvasGenerator.Generate(canvasPrefab, transform);
        canvasCtrl = canvasObj.GetComponent<CanvasController>();
    }

    ////以下、アニメーションイベント用メソッド
    public void PlayAudio(int num)
    {
        var AS = audioSources[num];
        AS.PlayOneShot(audioClips[AS]);
    }

}
