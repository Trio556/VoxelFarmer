using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Modify : MonoBehaviour
{
    bool isFire1Down = false;
    bool isFire2Down = false;
    double ticksBetweenFire = 150;
    DateTime lastFire1Time;
    DateTime lastFire2Time;
    DateTime currentTimeFire;

	// Use this for initialization
	void Start ()
    {
        lastFire1Time = DateTime.Now;
        lastFire2Time = DateTime.Now;
	}
	
	// Update is called once per frame
	void Update ()
    {
        currentTimeFire = DateTime.Now;
        isFire1Down = IsButtonStillDown("Fire1", isFire1Down);
        isFire2Down = IsButtonStillDown("Fire2", isFire2Down);

        if (isFire1Down && (currentTimeFire - lastFire1Time).TotalMilliseconds >= ticksBetweenFire)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 100))
            {
                EditTerrain.SetBlock(hit, new BlockAir());
            }

            lastFire1Time = DateTime.Now;
        }
        else if (isFire2Down && (currentTimeFire - lastFire2Time).TotalMilliseconds >= ticksBetweenFire)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 100))
            {
                EditTerrain.SetBlock(hit, new BlockGrass(), true);
            }

            lastFire2Time = DateTime.Now;
        }
    }

    private bool IsButtonStillDown(string actionName, bool currentValue)
    {
        if (CrossPlatformInputManager.GetButtonDown(actionName))
            return true;
        else if (CrossPlatformInputManager.GetButtonUp(actionName))
            return false;

        return currentValue;
    }
}
