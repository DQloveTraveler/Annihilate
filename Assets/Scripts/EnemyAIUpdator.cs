using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIUpdator : MonoBehaviour
{
    private static List<IEnemyAI> enemyList = new List<IEnemyAI>();

    void Awake()
    {
        ResetList();
    }

    // Update is called once per frame
    void Update()
    {
        foreach(var ai in enemyList)
        {
            ai.Updating();
        }
    }

    void LateUpdate()
    {
        foreach(var ai in enemyList)
        {
            ai.LateUpdating();
        }
    }

    public static void AddList(IEnemyAI enemyAI)
    {
        enemyList.Add(enemyAI);
    }

    public static void RemoveList(IEnemyAI enemyAI)
    {
        enemyList.Remove(enemyAI);
    }

    private static void ResetList()
    {
        enemyList.Clear();
    }
}
