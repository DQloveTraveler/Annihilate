using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyGage : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    private Text amountText;

    private WeaponEnergy weaponEnergy;
    private Animator anim;
    private float currentValue = 0;
    private int maxValue;

    // Start is called before the first frame update
    void Start()
    {
        if (weaponEnergy != null)
        {
            maxValue = weaponEnergy.MaxValue;
            amountText = GetComponentInChildren<Text>();
            anim = GetComponent<Animator>();
        }
        else Destroy(gameObject);
    }

    public void Initialize(WeaponEnergy weaponEnergy)
    {
        this.weaponEnergy = weaponEnergy;
    }

    // Update is called once per frame
    void Update()
    {
        currentValue = weaponEnergy.Value;
        fillImage.fillAmount = Mathf.MoveTowards(fillImage.fillAmount, currentValue / maxValue, Time.deltaTime / 4);
        amountText.text = ((int)(fillImage.fillAmount * 100)).ToString();

        anim.SetBool("MaxCharge", currentValue >= weaponEnergy.MaxValue);
    }
}
