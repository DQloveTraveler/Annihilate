using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject Decal;
    bool input0 = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        input0 = Input.GetKeyDown(KeyCode.Alpha0);
    }

    private void FixedUpdate()
    {
        if (input0)
        {
            for (int i = 0; i < 50; i++)
            {
                var x = Random.Range(-3, 3);
                var z = Random.Range(-3, 3);
                var instantDecal = Instantiate(Decal, new Vector3(x, 0, z), Quaternion.identity);
                Destroy(instantDecal, 4);
            }
        }
    }
}
