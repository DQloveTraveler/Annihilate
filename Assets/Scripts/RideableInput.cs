using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RideableInput
{

    private readonly float validTime_SpreadFire = 0.3f;
    private readonly float validTime_Smash = 0.3f;
    private float validTimeCounter_SpreadFire = 0;
    private float validTimeCounter_Smash = 0;

    public bool TakeOff
    {
        get
        {
            if (ControllerMode.IsGamePad)
            {
                return Input.GetButtonDown("Y");
            }
            else if (ControllerMode.IsMouseAndKey)
            {
                return Input.GetKeyDown(KeyCode.T);
            }
            return false;
        }
    }

    public bool LandOn
    {
        get
        {
            if (ControllerMode.IsGamePad)
            {
                return Input.GetButtonDown("A");
            }
            else if (ControllerMode.IsMouseAndKey)
            {
                return Input.GetKeyDown(KeyCode.T);
            }
            return false;
        }
    }

    public bool Attack1
    {
        get
        {
            if (ControllerMode.IsGamePad)
            {
                return Input.GetButtonDown("B");
            }
            else if (ControllerMode.IsMouseAndKey)
            {
                return Input.GetKeyDown(KeyCode.Space);
            }
            return false;
        }
    }

    public int FireAttack
    {
        get
        {
            if (ControllerMode.IsGamePad)
            {
                if (Input.GetButtonDown("B")) return 1;
                if (Input.GetKeyDown("Y")) return 2;
            }
            else if (ControllerMode.IsMouseAndKey)
            {
                if (validTimeCounter_SpreadFire <= validTime_SpreadFire)
                {
                    if (Input.GetKey(KeyCode.Space))
                    {
                        validTimeCounter_SpreadFire += Time.deltaTime;
                    }
                    else if (Input.GetKeyUp(KeyCode.Space))
                    {
                        validTimeCounter_SpreadFire = 0;
                        return 1;
                    }
                }
                else
                {
                    if (Input.GetKey(KeyCode.Space))
                    {
                        validTimeCounter_SpreadFire = 0;
                        return 2;
                    }
                }
            }
            return 0;
        }
    }

    public bool ThunderAttack
    {
        get
        {
            if (ControllerMode.IsGamePad)
            {
                return Input.GetButtonDown("B");
            }
            else if (ControllerMode.IsMouseAndKey)
            {
                return Input.GetKeyDown(KeyCode.Space);
            }
            return false;
        }
    }

    public int AttackOfGolem
    {
        get
        {
            if (ControllerMode.IsGamePad)
            {
                if (Input.GetButtonDown("B")) return 1;
                if (Input.GetButtonDown("Y")) return 2;
            }
            else if (ControllerMode.IsMouseAndKey)
            {
                if (validTimeCounter_Smash <= validTime_Smash)
                {
                    if (Input.GetKey(KeyCode.Space))
                    {
                        validTimeCounter_Smash += Time.deltaTime;
                    }
                    else if (Input.GetKeyUp(KeyCode.Space))
                    {
                        validTimeCounter_Smash = 0;
                        return 1;
                    }
                }
                else
                {
                    if (Input.GetKey(KeyCode.Space))
                    {
                        validTimeCounter_Smash = 0;
                        return 2;
                    }
                }
            }
            return 0;
        }
    }


}
