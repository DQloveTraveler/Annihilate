using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadBlizzard : BreathAttack
{
    [SerializeField] private GameObject freezeEffect;
    [SerializeField] private float attackPower = 0.5f;

    private void Awake()
    {
        Initialize();
    }


    private void Initialize()
    {
        particle = GetComponent<ParticleSystem>();
        effectAudio = GetComponent<AudioSource>();
    }

    public void StopParticle()
    {
        particle.Stop();
        StartCoroutine(AudioFadeOut(0.02f));
        StartCoroutine(DestroyParent(3f));
    }

    private void OnParticleCollision(GameObject other)
    {
        particle.GetCollisionEvents(other, collisionEventList);

        foreach (var collisionEvent in collisionEventList)
        {
            Vector3 hitPos = collisionEvent.intersection;
            var instanceEffect = Instantiate(freezeEffect, hitPos, Quaternion.identity);
            Destroy(instanceEffect, 4);
        }
    }

}
