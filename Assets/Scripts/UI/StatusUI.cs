using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusUI : MonoBehaviour
{
    protected Status targetStatus;

    public void Initialize(Status status)
    {
        targetStatus = status;
    }
}
