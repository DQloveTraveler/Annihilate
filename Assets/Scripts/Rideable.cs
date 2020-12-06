using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HighlightPlus;

public abstract class Rideable : Playable
{
    protected bool Riding = false;
    private bool cameraIsNear = false;

    [Header("Rideable")]
    [SerializeField] protected Transform nearCamPosi;
    [SerializeField] protected Transform nearCamTarget;
    [SerializeField] private GameObject ridingPlayerObj;
    [SerializeField] private HighlightEffect highlight;
    [SerializeField] private Transform getOffPoint;
    [SerializeField] private Collider[] attackColliders;

    protected RideableInput rideableInput = new RideableInput();
    private PlayerController myPlayer;


    protected abstract void Initialize();

    protected void TurnHorizontal(float inputH, float inputV)
    {
        // カメラの方向から、X-Z平面の単位ベクトルを取得
        Vector3 cameraForward = Vector3.Scale(cameraCtrl.transform.forward, new Vector3(1, 0, 1)).normalized;
        //回転方向を算出
        Vector3 nextVector = cameraForward * inputV + cameraCtrl.transform.right * inputH;

        float targetAngle;
        if (inputH == 0 && inputV == 0)
        {
            targetAngle = cameraCtrl.transform.eulerAngles.y;
        }
        else
        {
            targetAngle = Mathf.Atan2(nextVector.x, nextVector.z) * Mathf.Rad2Deg;
        }
        var nextAngle = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime * 100);

        transform.eulerAngles = nextAngle;
    }

    protected void TurnHorizontal(float inputH, float inputV, float multiplier)
    {
        // カメラの方向から、X-Z平面の単位ベクトルを取得
        Vector3 cameraForward = Vector3.Scale(cameraCtrl.transform.forward, new Vector3(1, 0, 1)).normalized;
        //回転方向を算出
        Vector3 nextVector = cameraForward * inputV + cameraCtrl.transform.right * inputH;

        float targetAngle;
        if (inputH == 0 && inputV == 0)
        {
            targetAngle = cameraCtrl.transform.eulerAngles.y;
        }
        else
        {
            targetAngle = Mathf.Atan2(nextVector.x, nextVector.z) * Mathf.Rad2Deg;
        }
        var nextAngle = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnSpeed * multiplier * Time.deltaTime * 100);

        transform.eulerAngles = nextAngle;
    }

    protected void CameraChange()
    {
        if (Input.GetMouseButtonDown(1) || Input.GetButtonDown("RightAnalogButton"))
        {

            Destroy(cameraCtrl.gameObject);

            if (cameraIsNear)
            {
                cameraCtrl = CameraGenerator.Generate(cameraPrefab, StartCamPos, CamTarget);
            }
            else
            {
                cameraCtrl = CameraGenerator.Generate(cameraPrefab, nearCamPosi, nearCamTarget);
            }
            cameraIsNear = !cameraIsNear;
        }
    }

    public void Ridden(PlayerController player)
    {
        myPlayer = player;
        player.gameObject.SetActive(false);
        ridingPlayerObj.SetActive(true);
        EnemyBase.PlayerChange(transform);
        cameraIsNear = false;
        Riding = true;
        HighLightSwitch(false);

        //カメラ生成
        GenerateCamera();

        //キャンバス生成
        GenerateCanvas();
    }

    public void HighLightSwitch(bool active)
    {
        highlight.highlighted = active;
    }

    protected void GetOff()
    {
        if (GetOffPointCheck() && myPlayer.GetOff())
        {
            ridingPlayerObj.SetActive(false);
            myPlayer.gameObject.SetActive(true);
            myPlayer.transform.position = getOffPoint.position;
            myPlayer.transform.rotation = getOffPoint.rotation;
            EnemyBase.PlayerChange(myPlayer.transform);
            Destroy(cameraCtrl.gameObject);
            Destroy(canvasObj.gameObject);
            Riding = false;
        }
    }

    private bool GetOffPointCheck()
    {
        return !Physics.Linecast(transform.position, getOffPoint.position, LayerMask.GetMask("Ground"));
    }

    protected abstract bool SPCheck(string attackName);

    ////以下、アニメーションイベント用メソッド
    public void AnimBoolFalse(string parameter)
    {
        anim.SetBool(parameter, false);
    }

    public void AttackColliderON(int num)
    {
        foreach (var collider in attackColliders)
        {
            collider.GetComponent<Attackable>().ListReset();
        }
        attackColliders[num].enabled = true;
    }

    public void AttackColliderOFF()
    {
        foreach (var collider in attackColliders)
        {
            collider.GetComponent<Attackable>().ListReset();
            collider.enabled = false;
        }
    }

}
