using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalEffect : MonoBehaviour
{
    [SerializeField] private Gradient color = new Gradient();
    [SerializeField] private float lifeTime = 1;
    private Renderer render;
    private Color baseColor;
    private float time = 0;

    private void Awake()
    {
        render = GetComponent<Renderer>();
        baseColor = render.material.GetColor("_TintColor");
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        var multiplyColor = color.Evaluate(time / lifeTime);
        render.material.SetColor("_TintColor", baseColor * multiplyColor);
    }
}
