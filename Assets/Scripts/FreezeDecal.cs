using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeDecal : MonoBehaviour
{
    [SerializeField] private Renderer myRenderer;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(_RendererON(myRenderer));
    }

    private IEnumerator _RendererON(Renderer renderer)
    {
        yield return new WaitForEndOfFrame();
        renderer.enabled = true;
    }

}
