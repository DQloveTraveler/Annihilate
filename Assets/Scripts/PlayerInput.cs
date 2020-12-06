using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlayerInputManagement
{
    public class PlayerInput
    {
        //キーボード操作時の回避入力判定用
        public enum Key
        {
            NULL, W, S, A, D
        }

        public static Key InputKey { get; set; } = Key.NULL;

        //各入力の受付有効時間とカウンター
        private static readonly float validTime_doubleClick = 0.2f;
        private static readonly float validTime_combo1 = 0.3f;
        private static readonly float validTime_combo2 = 0.3f;
        private static float validTimeCounter_doubleClick = 0;
        private static float validTimeCounter_combo1 = 0;
        private static float validTimeCounter_combo2 = 0;

        public static void Updating()
        {
            validTimeCounter_doubleClick += Time.deltaTime;
            validTimeCounter_combo1 += Time.deltaTime;
            validTimeCounter_combo2 += Time.deltaTime;
        }


        private static bool DoubleClick
        {
            get
            {
                if (Input.GetMouseButtonUp(0))
                {
                    if (validTimeCounter_doubleClick < validTime_doubleClick)
                    {
                        return true;
                    }
                    validTimeCounter_doubleClick = 0;
                }
                return false;
            }
        }

        public static bool Pause
        {
            get
            {
                if (ControllerMode.IsGamePad)
                {
                    return Input.GetButtonUp("Menu");
                }
                else if (ControllerMode.IsMouseAndKey)
                {
                    return Input.GetKeyUp(KeyCode.Escape);
                }
                return false;
            }
        }

        public static bool Jump
        {
            get
            {
                if (ControllerMode.IsGamePad)
                {
                    return Input.GetButtonDown("Y");
                }
                else if (ControllerMode.IsMouseAndKey)
                {
                    return Input.GetKeyDown(KeyCode.Space);
                }
                return false;
            }
        }

        public static bool Avoid
        {
            get
            {
                if (ControllerMode.IsGamePad)
                {
                    return Input.GetButtonDown("A");
                }
                else if (ControllerMode.IsMouseAndKey)
                {
                    return Input.GetKeyDown(KeyCode.F);
                }
                return false;
            }
        }

        public static bool AutoLockOn
        {
            get
            {
                if (ControllerMode.IsGamePad)
                {
                    return Input.GetAxis("Trigger") < 0; 
                }
                else if (ControllerMode.IsMouseAndKey)
                {
                    return Input.GetMouseButton(2);
                }
                return false;
            }
        }

        public static bool LockOn
        {
            get
            {
                if (ControllerMode.IsGamePad)
                {
                    return Input.GetButtonUp("RightAnalogButton");
                }
                else if (ControllerMode.IsMouseAndKey)
                {
                    return DoubleClick;
                }
                return false;
            }
        }

        public static bool EquipSwitch
        {
            get
            {
                if (ControllerMode.IsGamePad)
                {
                    return Input.GetButtonDown("X");
                }
                else if (ControllerMode.IsMouseAndKey)
                {
                    return Input.GetMouseButtonDown(1);
                }
                return false;
            }
        }

        public static bool LightAttack
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

        public static bool Combo1
        {
            get { return validTimeCounter_combo1 < validTime_combo1; }
        }

        public static bool Combo2
        {
            get { return validTimeCounter_combo2 < validTime_combo2; }
        }

        public static void ResetComboTime(int comboNum)
        {
            switch (comboNum)
            {
                case 1:
                    validTimeCounter_combo1 = 0;
                    break;
                case 2:
                    validTimeCounter_combo2 = 0;
                    break;
            }
        }

        public static bool HeavyAttack
        {
            get
            {
                if (ControllerMode.IsGamePad)
                {
                    return Input.GetButtonDown("Y");
                }
                else if (ControllerMode.IsMouseAndKey)
                {
                    return Input.GetKeyDown(KeyCode.LeftShift);
                }
                return false;
            }
        }

        public static bool RideOn
        {
            get
            {
                if (ControllerMode.IsGamePad)
                {
                    return Input.GetButtonUp("B");
                }
                else if (ControllerMode.IsMouseAndKey)
                {
                    return Input.GetKeyUp(KeyCode.R);
                }
                return false;
            }
        }

        public static bool GetOff
        {
            get
            {
                if (ControllerMode.IsGamePad)
                {
                    return Input.GetButtonUp("A");
                }
                else if (ControllerMode.IsMouseAndKey)
                {
                    return Input.GetKeyUp(KeyCode.G);
                }
                return false;
            }
        }

        public static bool Talk
        {
            get
            {
                if (ControllerMode.IsGamePad)
                {
                    return Input.GetButtonUp("B");
                }
                else if (ControllerMode.IsMouseAndKey)
                {
                    return DoubleClick;
                }
                return false;
            }
        }

        public static bool FumbleSomething
        {
            get
            {
                if (ControllerMode.IsGamePad)
                {
                    return Input.GetButtonUp("B");
                }
                else if (ControllerMode.IsMouseAndKey)
                {
                    return DoubleClick;
                }
                return false;
            }
        }

        public static bool SkipTalkingText
        {
            get
            {
                if (ControllerMode.IsGamePad)
                {
                    return Input.GetButtonUp("B") || Input.GetButtonUp("A");
                }
                else if (ControllerMode.IsMouseAndKey)
                {
                    return Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Return);
                }
                return false;
            }
        }

    }
}