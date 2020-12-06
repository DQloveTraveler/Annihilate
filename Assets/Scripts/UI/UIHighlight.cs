using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHighlight : MonoBehaviour
{
    [SerializeField] private Material highLightMaterial;
    [SerializeField] private AnimationCurve animationCurve;
    [SerializeField] private bool isHighlighted = false;
    public bool IsHighlighted
    {
        get { return isHighlighted; }
        set { isHighlighted = value; }
    }

    private static float SimulationSpeed = 10f;
    private Image image;
    private float value = 0;
    private float elapsedTime = 0;

    // Start is called before the first frame update
    private void Awake()
    {
        image = GetComponent<Image>();
        image.material = highLightMaterial;
    }

    private void Update()
    {
        if (isHighlighted)
        {
            image.material = highLightMaterial;
            image.material.SetFloat("_Angle", value);
            var speed = SimulationSpeed * animationCurve.Evaluate(elapsedTime);
            value += speed;
        }
        else
        {
            image.material = null;
        }
        if (value >= 360) value -= 360;
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= 1) elapsedTime--;
    }
}
