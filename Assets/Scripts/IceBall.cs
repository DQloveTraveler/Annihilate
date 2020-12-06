using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBall : MonoBehaviour
{
    [SerializeField] private float startSpeed = 10;
    [SerializeField] private float rotationSpeed = 30;
    [SerializeField] private GameObject explosionPrefab;

    private Transform meshTransform;
    private Vector3 hitPoint;
    private Quaternion hitRotation;

    private void Awake()
    {
        var rigidB = GetComponent<Rigidbody>();
        rigidB.AddForce(transform.forward * startSpeed, ForceMode.Impulse);
        rigidB.angularVelocity = transform.forward * rotationSpeed;
        meshTransform = GetComponentInChildren<Renderer>().transform;
        var ray = new Ray(meshTransform.position, transform.forward);
        if (Physics.Raycast(ray, out var hit, 100, LayerMask.GetMask("Ground")))
        {
            hitPoint = hit.point;
            hitRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Instantiate(explosionPrefab, hitPoint, hitRotation);
            Destroy(gameObject);
        }
    }

}
