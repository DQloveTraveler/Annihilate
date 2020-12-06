using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadFire : BreathAttack
{
    [SerializeField] private GameObject smallFirePrefab;

    // Start is called before the first frame update
    void Start()
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
            var smallfire = Instantiate(smallFirePrefab, hitPos, Quaternion.identity);
            Destroy(smallfire, 4);
        }
    }
}
