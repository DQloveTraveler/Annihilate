using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePadDetector : SingletonMonoBehaviour<GamePadDetector>
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
    }
}
