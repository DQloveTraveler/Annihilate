using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform enemyLooker;

    public Camera Camera { get; private set; }
    public Transform StartTrans { get; private set; } = null;
    public Transform Target { get; private set; } = null;
    private Vector3 targetPos;
    private float startDistance;
    private Animator anim;
    private RadialBlurEffect radial;

    private RaycastHit frontHit;
    private RaycastHit backHit;


    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        OperatedByPlayer();
    }

    void FixedUpdate()
    {
        AvoidObstacle();
    }

    private void Initialize()
    {
        Camera = transform.GetComponentInChildren<Camera>();
        anim = transform.GetComponentInChildren<Animator>();
        radial = transform.GetComponentInChildren<RadialBlurEffect>();
        Camera.depthTextureMode = DepthTextureMode.Depth;
        CameraShaker.SetController(this);
        transform.position = StartTrans.position;
        transform.eulerAngles = StartTrans.eulerAngles;
        targetPos = Target.position;
        startDistance = (transform.position - targetPos).sqrMagnitude;
    }

    public void SetStartTransform(Transform startTransform)
    {
        StartTrans = startTransform;
    }

    public void SetTarget(Transform tage)
    {
        Target = tage;
    }

    private void OperatedByPlayer()
    {
        float inputH = 0;
        float inputV = 0;
        switch (ControllerMode.Mode)
        {
            case ControllerMode.Controller.MouseAndKey:
                if (Input.GetMouseButton(0)) inputH = Input.GetAxis("MouseHorizontal");
                if (Input.GetMouseButton(0)) inputV = Input.GetAxis("MouseVertical");
                break;
            case ControllerMode.Controller.GamePad:
                inputH = Input.GetAxis("Horizontal2");
                inputV = Input.GetAxis("Vertical2");
                break;
        }

        transform.position += Target.position - targetPos;
        targetPos = Target.position;
        transform.RotateAround(targetPos, Vector3.up, inputH * Time.deltaTime * 200f);
        transform.RotateAround(targetPos, transform.right, inputV * Time.deltaTime * 100f);
        transform.LookAt(targetPos);
    }

    private void AvoidObstacle()
    {
        var distance = (transform.position - targetPos).sqrMagnitude;

        if(FrontRayCheck())
        {
            transform.position = frontHit.point;
        }
        else
        {
            if (!BackRayCheck())
            {
                if (distance < startDistance)
                {
                    transform.position += -transform.forward.normalized * Time.deltaTime * 2;
                }
            }
        }
    }

    private bool FrontRayCheck()
    {
        return Physics.Linecast(Target.position, transform.position, out frontHit, LayerMask.GetMask("Ground", "NPC"));
    }

    private bool BackRayCheck()
    {
        return Physics.Raycast(transform.position, -transform.forward, out backHit, 0.3f, LayerMask.GetMask("Ground", "NPC"));
    }

    public void LockOn(int loopValue, Vector3 enemyPosi)
    {
        StartCoroutine(_LockOn(loopValue, enemyPosi));
    }

    public void LockOn(int loopValue)
    {
        StartCoroutine(_LockOn(loopValue));
    }

    private IEnumerator _LockOn(int loopValue, Vector3 enemyPosi)
    {
        enemyLooker.LookAt(enemyPosi);
        float angleDiff = Mathf.DeltaAngle(transform.eulerAngles.y, enemyLooker.eulerAngles.y);

        for (int i = loopValue; i > 0; i--)
        {
            yield return new WaitForEndOfFrame();
            transform.RotateAround(targetPos, Vector3.up, angleDiff / loopValue);
        }
    }

    private IEnumerator _LockOn(int loopValue)
    {
        float angleDiff = Mathf.DeltaAngle(transform.eulerAngles.y, Target.eulerAngles.y);

        for (int i = loopValue; i > 0; i--)
        {
            yield return new WaitForEndOfFrame();
            transform.RotateAround(targetPos, Vector3.up, angleDiff / loopValue);
        }
    }

    public void ShortShake()
    {
        StartCoroutine(_ShortShake(0.01f, 0.1f, 0.15f));
    }

    private IEnumerator _ShortShake(float duration, float magnitudeX, float magnitudeY)
    {
        int loopNum = 10;
        var waitForSeconds = new WaitForSeconds(duration / loopNum);

        for(int i = 0; i < loopNum; i++)
        {
            var x = Random.Range(-1f, 1f) * magnitudeX;
            var y = Random.Range(-1f, 1f) * magnitudeY;
            Camera.transform.localPosition = new Vector3(x, y, 0);
            yield return waitForSeconds;
        }

        Camera.transform.localPosition = Vector3.zero;
    }

    public void Shake()
    {
        anim.SetTrigger("Shake");
    }

    public void RadialBlurEffect()
    {
        StopCoroutine("_RadialBlurEffect");
        StartCoroutine("_RadialBlurEffect", 0.2f);
    }

    private IEnumerator _RadialBlurEffect(float duration)
    {
        radial.blurDegree = 0.02f;
        float loopNum = 10;
        var waitForSeconds = new WaitForSecondsRealtime(duration / loopNum);
        for(int i = 0; i < loopNum; i++)
        {
            radial.blurDegree -= 0.02f / loopNum;
            yield return waitForSeconds;
        }
        radial.blurDegree = 0;
    }
}
