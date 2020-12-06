using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RideablesData
{
    public RideableData GetData(SelectingRideable.RideableCharacter rideableCharacter)
    {
        switch (rideableCharacter)
        {
            case SelectingRideable.RideableCharacter.Null:
                return new NULL().GetData();
            case SelectingRideable.RideableCharacter.Dragon:
                return new MountainDragon().GetData();
            case SelectingRideable.RideableCharacter.Griffon:
                return new Griffon().GetData();
            case SelectingRideable.RideableCharacter.Golem:
                return new Golem().GetData();
            case SelectingRideable.RideableCharacter.FatIceDragon:
                return new FatIceDragon().GetData();
            default:
                return null;
        }
    }


    public abstract class RideableData
    {
        public float ATK { get; protected set; }
        public float DEF { get; protected set; }
        public float VIT { get; protected set; }
        public float AGI { get; protected set; }

        public Attackable.Atribute ELE { get; protected set; }

        public abstract RideableData GetData();
    }

    public class NULL : RideableData
    {
        public NULL()
        {
            ATK = 0;
            DEF = 0;
            VIT = 0;
            AGI = 0;
            ELE = Attackable.Atribute.None;
        }
        public override RideableData GetData()
        {
            return this;
        }
    }

    public class MountainDragon : RideableData
    {
        public MountainDragon()
        {
            ATK = 8;
            DEF = 6;
            VIT = 10;
            AGI = 3f;
            ELE = Attackable.Atribute.Fire;
        }

        public override RideableData GetData()
        {
            return this;
        }
    }

    public class Griffon : RideableData
    {
        public Griffon()
        {
            ATK = 7;
            DEF = 4;
            VIT = 5;
            AGI = 10;
            ELE = Attackable.Atribute.Electro;
        }

        public override RideableData GetData()
        {
            return this;
        }
    }

    public class Golem : RideableData
    {
        public Golem()
        {
            ATK = 8;
            DEF = 10;
            VIT = 3;
            AGI = 5;
            ELE = Attackable.Atribute.Earth;
        }

        public override RideableData GetData()
        {
            return this;
        }
    }

    public class FatIceDragon : RideableData
    {
        public FatIceDragon()
        {
            ATK = 4;
            DEF = 6;
            VIT = 6;
            AGI = 8;
            ELE = Attackable.Atribute.Ice;
        }

        public override RideableData GetData()
        {
            return this;
        }
    }
}
