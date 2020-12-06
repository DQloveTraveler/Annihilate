using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectCheck
{
    public class ObjectChecker
    {
        private static Rideable selectedRideable;

        public static bool FumbleableCheck(PlayerController player, out Fumbleable fumbleable)
        {
            if (ControllerMode.IsMouseAndKey)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hit, 10))
                {
                    if (hit.transform.root.TryGetComponent(out fumbleable))
                    {
                        return fumbleable.CanFumble;
                    }
                }
            }
            else if (ControllerMode.IsGamePad)
            {
                Ray ray = new Ray(player.transform.position + Vector3.up, player.transform.forward.normalized);
                if (Physics.SphereCast(ray, 1, out var hit, 3))
                {
                    if(hit.transform.root.TryGetComponent(out fumbleable))
                    {
                        return fumbleable.CanFumble;
                    }
                }
            }
            fumbleable = null;
            return false;
        }

        public static bool TalkableCheck(PlayerController player, out Talkable talkable)
        {
            if (ControllerMode.IsMouseAndKey)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hit, 10))
                {
                    if (hit.transform.root.TryGetComponent(out talkable)) 
                    {
                        return talkable.CanTalk;
                    }
                }
                talkable = null;
                return false;
            }
            else if (ControllerMode.IsGamePad)
            {
                Ray ray = new Ray(player.transform.position + Vector3.up, player.transform.forward.normalized);
                if (Physics.SphereCast(ray, 1, out var hit, 3))
                {
                    if (hit.transform.root.TryGetComponent(out talkable))
                    {
                        return talkable.CanTalk;
                    }
                }
                talkable = null;
                return false;
            }
            talkable = null;
            return false;
        }

        public static bool RideableCheck(PlayerController player, out Rideable rideable)
        {
            Ray ray = new Ray(player.transform.position + Vector3.up, player.transform.forward.normalized);
            if (Physics.SphereCast(ray, 1, out var hit, 3, LayerMask.GetMask("Player")))
            {
                if(hit.transform.root.TryGetComponent(out rideable))
                {
                    rideable.HighLightSwitch(true);
                    if(rideable != selectedRideable && selectedRideable != null)
                    {
                        selectedRideable.HighLightSwitch(false);
                    }
                    selectedRideable = rideable;
                    return true;
                }
                else
                {
                    if(selectedRideable != null)
                    {
                        selectedRideable.HighLightSwitch(false);
                        selectedRideable = null;
                    }
                    return false;
                }
            }
            else
            {
                if (selectedRideable != null)
                {
                    selectedRideable.HighLightSwitch(false);
                    selectedRideable = null;
                }
                rideable = null;
                return false;
            }
        }

    }
}

