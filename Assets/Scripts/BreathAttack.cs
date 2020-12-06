using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BreathAttack : MonoBehaviour
{
    protected ParticleSystem particle;
    protected List<ParticleCollisionEvent> collisionEventList = new List<ParticleCollisionEvent>();
    protected AudioSource effectAudio;


    protected IEnumerator DestroyRoot(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(transform.root.gameObject);
    }

    protected IEnumerator DestroyParent(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(transform.parent.gameObject);
    }

    protected IEnumerator AudioFadeOut(float fadeAmount)
    {
        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
        for (int i = 0; i < 30; i++)
        {
            yield return waitForEndOfFrame;
            if(effectAudio.volume < 0.3)
            {
                effectAudio.volume = 0;
            }
            else
            {
                effectAudio.volume -= fadeAmount;
            }
        }
    }

}
