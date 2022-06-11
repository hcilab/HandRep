using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CharacterMoveOverride : MonoBehaviour
{
    private XRRig xrrig => GetComponent<XRRig>();
    private CharacterController cc => GetComponent<CharacterController>();
    private CharacterControllerDriver driver => GetComponent<CharacterControllerDriver>();

    private void Update()
    {
        UpdateCharacterController();
    }

    protected virtual void UpdateCharacterController()
    {
        if(xrrig == null || cc == null)
        {
            return;
        }

        float height = Mathf.Clamp(xrrig.cameraInRigSpaceHeight, driver.minHeight, driver.maxHeight);
        Vector3 centre = xrrig.cameraInRigSpacePos;
        centre.y = height / 2f + cc.skinWidth;
        cc.height = height;
        cc.center = centre;
    }
}
