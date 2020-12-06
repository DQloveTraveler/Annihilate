using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectingSignal : MonoBehaviour
{
    [SerializeField] private SelectingRideable.RideableCharacter myChara;

    private ParticleSystem[] particleSystems;

    private void Start()
    {
        particleSystems = GetComponentsInChildren<ParticleSystem>();
    }

    private void Update()
    {
        if (myChara == SelectingRideable.Value)
        {
            if (!particleSystems[0].isPlaying)
            {
                foreach (var p in particleSystems)
                {
                    p.Play();
                }
            }
        }
        else
        {
            if (particleSystems[0].isPlaying)
            {
                foreach (var p in particleSystems)
                {
                    p.Stop();
                }
            }
        }
    }
}
