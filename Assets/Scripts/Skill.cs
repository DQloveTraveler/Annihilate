using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : Attackable
{
    [SerializeField] protected Collider myCollider;
    [SerializeField] protected float colliderOFFTime = 0.1f;
    // Start is called before the first frame update
    protected void Init()
    {
        if (myCollider != null)
        {
            Destroy(myCollider, colliderOFFTime);
        }
        else
        {
            Destroy(GetComponent<Collider>(), colliderOFFTime);
        }
    }
}
