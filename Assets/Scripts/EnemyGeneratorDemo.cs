using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGeneratorDemo : MonoBehaviour
{
    [SerializeField] private GameObject EnemyPrefab;
    private List<Transform> generatePoints = new List<Transform>();
    private WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
    private List<GameObject> enemyList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        var children = GetComponentsInChildren<Renderer>();

        foreach (var child in children)
        {
            generatePoints.Add(child.transform);
            child.gameObject.SetActive(false);
            enemyList.Add(null);
        }
    }

    void Update()
    {
        for(int i = 0; i < enemyList.Count; i++)
        {
            if(enemyList[i] == null)
            {
                enemyList[i] = Instantiate(EnemyPrefab, generatePoints[i].position, generatePoints[i].rotation);
            }
        }
    }


    private IEnumerator Generate()
    {
        foreach (var point in generatePoints)
        {
            var random = Random.Range(0, 360);
            var angle = Quaternion.Euler(0, random, 0);
            Instantiate(EnemyPrefab, point.position, angle);
            yield return waitForEndOfFrame;
        }
    }
}
