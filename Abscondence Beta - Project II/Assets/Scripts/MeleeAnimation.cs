using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAnimation : MonoBehaviour
{
    public Animator anim;
    bool allowToggleActive = true;

    public void ToggleAnimation()
    {
        if (anim.GetBool("Active") && allowToggleActive)
        {
            anim.SetBool("Active", false); // Plays Idle Animation
            allowToggleActive = false;
        }
        else if (allowToggleActive)
        {
            anim.SetBool("Active", true); // Plays Swipe animation
            allowToggleActive = false;
        }
    }

    // Called by AnimationEvent on last frame of each animation
    public void SetToggleOk()
    {
        allowToggleActive = true;
    }
}
