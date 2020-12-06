using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Attackable
{
    [SerializeField] private ParticleSystem particle;
    private List<ParticleCollisionEvent> collisionEventList = new List<ParticleCollisionEvent>();

    private void OnParticleCollision(GameObject other)
    {
        if(other.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.GetDamage(attackPower, atribute, this);
            particle.GetCollisionEvents(other, collisionEventList);
            Destroy(particle);
            foreach (var collisionEvent in collisionEventList)
            {
                var hitPos = collisionEvent.intersection;
                var hitRot = Quaternion.FromToRotation(Vector3.up, collisionEvent.normal);
                var effect = Instantiate(hitEffect, hitPos, hitRot);
                Destroy(effect, 1);
            }
        }
    }
}
