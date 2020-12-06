using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class ComparePanel : MonoBehaviour
{
    [SerializeField]
    private GameObject decoy;

    [SerializeField] private Color positiveColor;
    [SerializeField] private Color negativeColor;

    [SerializeField] private ElementSprites elementSprites;

    public SelectingRideable.RideableCharacter CurrentRideable { private get; set; } = SelectingRideable.RideableCharacter.Null;
    public SelectingRideable.RideableCharacter NewRideable { private get; set; } = SelectingRideable.RideableCharacter.Null;

    [SerializeField] private CurrentValueImages currentValues;
    [SerializeField] private NewValueImages newValues;
    private Sprite currentElementSprite;
    private Sprite newElementSprite;


    private RideablesData RideablesData = new RideablesData();

    public bool Enabled
    { 
        get 
        {
            return decoy.activeSelf;
        }
        set
        {
            if(value)
            {
                CurrentRideable = SelectingRideable.Value;
                NewRideable = SelectingRideable.NewValue;
                newValues.OnEnable();
                ColorUpdate();
                FillUpdate();
            }
            decoy.SetActive(value);
        }
    }

    public void DeActivate()
    {
        Enabled = false;
    }

    private void ColorUpdate()
    {
        var Cdata = RideablesData.GetData(CurrentRideable);
        var Ndata = RideablesData.GetData(NewRideable);

        if (Cdata.ATK < Ndata.ATK) newValues.ATK.color = positiveColor;
        else if (Cdata.ATK > Ndata.ATK) newValues.ATK.color = negativeColor;

        if (Cdata.DEF < Ndata.DEF) newValues.DEF.color = positiveColor;
        else if (Cdata.DEF > Ndata.DEF) newValues.DEF.color = negativeColor;

        if (Cdata.VIT < Ndata.VIT) newValues.VIT.color = positiveColor;
        else if (Cdata.VIT > Ndata.VIT) newValues.VIT.color = negativeColor;

        if (Cdata.AGI < Ndata.AGI) newValues.AGI.color = positiveColor;
        else if (Cdata.AGI > Ndata.AGI) newValues.AGI.color = negativeColor;

    }

    private void FillUpdate()
    {
        var Cdata = RideablesData.GetData(CurrentRideable);
        var Ndata = RideablesData.GetData(NewRideable);

        switch (Cdata.ELE)
        {
            case Attackable.Atribute.Fire:
                currentElementSprite = elementSprites.fire;
                break;
            case Attackable.Atribute.Ice:
                currentElementSprite = elementSprites.ice;
                break;
            case Attackable.Atribute.Electro:
                currentElementSprite = elementSprites.electro;
                break;
            case Attackable.Atribute.Earth:
                currentElementSprite = elementSprites.earth;
                break;
            case Attackable.Atribute.None:
                currentElementSprite = elementSprites.none;
                break;
        }
        switch (Ndata.ELE)
        {
            case Attackable.Atribute.Fire:
                newElementSprite = elementSprites.fire;
                break;
            case Attackable.Atribute.Ice:
                newElementSprite = elementSprites.ice;
                break;
            case Attackable.Atribute.Electro:
                newElementSprite = elementSprites.electro;
                break;
            case Attackable.Atribute.Earth:
                newElementSprite = elementSprites.earth;
                break;
            case Attackable.Atribute.None:
                newElementSprite = elementSprites.none;
                break;
        }

        currentValues.Update(Cdata.ATK, Cdata.DEF, Cdata.VIT, Cdata.AGI, currentElementSprite);
        newValues.Update(Ndata.ATK, Ndata.DEF, Ndata.VIT, Ndata.AGI, newElementSprite);
    }

    [Serializable]
    public class ElementSprites
    {
        public Sprite fire;
        public Sprite ice;
        public Sprite earth;
        public Sprite electro;
        public Sprite none;
    }

    [Serializable]
    public class CurrentValueImages
    {
        [SerializeField]
        private int baseValue = 0;

        [SerializeField]
        private Image Atk;
        public Image ATK { get { return Atk; } private set { Atk = value; } }

        [SerializeField]
        private Image Def;
        public Image DEF { get { return Def; } private set { Def = value; } }

        [SerializeField]
        private Image Vit;
        public Image VIT { get { return Vit; } private set { Vit = value; } }

        [SerializeField]
        private Image Agi;
        public Image AGI { get { return Agi; } private set { Agi = value; } }

        [SerializeField]
        private Image Ele;
        public Image ELE { get { return Ele; } private set { Ele = value; } }


        public void Update(float atk, float def, float vit, float agi, Sprite ele)
        {
            ATK.fillAmount = atk / baseValue;
            DEF.fillAmount = def / baseValue;
            VIT.fillAmount = vit / baseValue;
            AGI.fillAmount = agi / baseValue;
            ELE.sprite = ele;
        }

    }

    [Serializable]
    public class NewValueImages
    {
        [SerializeField]
        private int baseValue = 0;

        [SerializeField]
        private Color defaultColor;

        [SerializeField]
        private Image Atk;
        public Image ATK { get { return Atk; } private set { Atk = value; } }

        [SerializeField]
        private Image Def;
        public Image DEF { get { return Def; } private set { Def = value; } }

        [SerializeField]
        private Image Vit;
        public Image VIT { get { return Vit; } private set { Vit = value; } }

        [SerializeField]
        private Image Agi;
        public Image AGI { get { return Agi; } private set { Agi = value; } }

        [SerializeField]
        private Image Ele;
        public Image ELE { get { return Ele; } private set { Ele = value; } }

        private Image[] images = new Image[4];


        public void Update(float atk, float def, float vit, float agi, Sprite ele)
        {
            ATK.fillAmount = atk / baseValue;
            DEF.fillAmount = def / baseValue;
            VIT.fillAmount = vit / baseValue;
            AGI.fillAmount = agi / baseValue;
            ELE.sprite = ele;
        }

        public void OnEnable()
        {
            images[0] = ATK;
            images[1] = DEF;
            images[2] = VIT;
            images[3] = AGI;

            foreach(var i in images)
            {
                i.color = defaultColor;
            }
        }
    }
}
