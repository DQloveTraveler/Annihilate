using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] private GameObject EnemyPrefab;
    private List<Transform> generatePoints = new List<Transform>();
    private WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Generate());
    }

    private IEnumerator Generate()
    {
        var children = GetComponentsInChildren<Renderer>();

        foreach (var child in children)
        {
            generatePoints.Add(child.transform);
            child.gameObject.SetActive(false);
        }

        foreach (var point in generatePoints)
        {
            var random = Random.Range(0, 360);
            var angle = Quaternion.Euler(0, random, 0);
            Instantiate(EnemyPrefab, point.position, angle);
            yield return waitForEndOfFrame;
        }
    }
}
