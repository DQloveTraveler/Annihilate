using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RideableGenerator : MonoBehaviour
{
    [SerializeField] private Transform generatePoint;

    [Header("RideablePrefabs")]
    [SerializeField] private GameObject dragonPrefab;
    [SerializeField] private GameObject griffonPrefab;
    [SerializeField] private GameObject fatIceDragonPrefab;
    [SerializeField] private GameObject golemPrefab;


    // Start is called before the first frame update
    void Start()
    {
        if (SelectingRideable.IsDragon)
        {
            Instantiate(dragonPrefab, generatePoint.position, generatePoint.rotation);
        }
        if (SelectingRideable.IsGriffon)
        {
            Instantiate(griffonPrefab, generatePoint.position, generatePoint.rotation);
        }
        if (SelectingRideable.IsFatIceDragon)
        {
            Instantiate(fatIceDragonPrefab, generatePoint.position, generatePoint.rotation);
        }
        if (SelectingRideable.IsGolem)
        {
            Instantiate(golemPrefab, generatePoint.position, generatePoint.rotation);
        }

        Destroy(generatePoint.gameObject);
    }
}
