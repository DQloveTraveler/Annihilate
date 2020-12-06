using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunder : MonoBehaviour
{
    private ParticleSystem thunderParticle;
    [SerializeField] private GameObject hitEffect;

    // Start is called before the first frame update
    void Start()
    {
        thunderParticle = GetComponent<ParticleSystem>();
        if(Physics.Raycast(transform.position, transform.forward, out var hit, 20f, LayerMask.GetMask("Ground")))
        {
            StartCoroutine(_GenerateHitEffect(hit));
        }
    }

    private IEnumerator _GenerateHitEffect(RaycastHit hit)
    {
        yield return new WaitForSeconds(thunderParticle.main.startDelayMultiplier);
        var decal = Instantiate(hitEffect, hit.point + hit.transform.up * 0.1f, Quaternion.Euler(hit.normal));
        Destroy(decal, 5);
    }
}
