using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class MenuDialogOriginal : MenuDialog
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
}
