using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDissoveSettings : MonoBehaviour
{
    [SerializeField] private float delay = 2;
    [SerializeField] private float dissolveDuration = 2;
    [SerializeField] private string variableName = "_DissolveAmount";
    private Material material;
    private WaitForSeconds waitForSeconds;
    private float dissolveVariable;


    private void Awake()
    {
        material = GetComponent<ParticleSystemRenderer>().material;
        waitForSeconds = new WaitForSeconds(dissolveDuration / 50);
    }

    private void Start()
    {
        StartCoroutine(Dissolve());
    }


    private IEnumerator Dissolve()
    {
        yield return new WaitForSeconds(delay);

        material.SetFloat(variableName, 0);
        for (int i = 0; i < 50; i++)
        {
            material.SetFloat(variableName, material.GetFloat(variableName) + (1f / 50));
            yield return waitForSeconds;
        }
    }
}
