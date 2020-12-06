using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthImpact : MonoBehaviour
{
    [SerializeField] private GameObject delayEffect;
    [SerializeField] private Transform[] generatePoints;
    [SerializeField] private float delayTime = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(_Generate());
    }

    private IEnumerator _Generate()
    {
        for(int i = 0; i < generatePoints.Length; i++)
        {
            yield return new WaitForSeconds(delayTime);
            Instantiate(delayEffect, generatePoints[i]);
        }
    }

}
