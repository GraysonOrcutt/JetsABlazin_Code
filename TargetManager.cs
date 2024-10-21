using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public int shotsFired;
    public int shotsLanded;
    public int targetsBroken;
    public int RyeShotsLanded;

    public RyeGunScript RGR;
    public RyeGunScript RGL;
    public rbescript RBER;
    public rbescript RBEL;

    void Start()
    { 

    }

    public void RyeGunUpdateLeft()
    {
        RGL.ShotsUntilOverheat += 3;
    }
    public void RyeGunUpdateRight()
    {
        RGR.ShotsUntilOverheat += 3;
    }

    public void RBEGunUpdateLeft()
    {
        RBEL.strength += RBER.strengthChange * 2;
    }
    public void RBEGunUpdateRight()
    {
        RBER.strength += RBEL.strengthChange * 2;
    }
}
