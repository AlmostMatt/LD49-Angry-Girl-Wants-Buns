﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    private bool mIsBroken = false;

    public void Break()
    {
        if (mIsBroken)
        {
            return;
        }
        mIsBroken = true;
        // TODO - make objects shatter or spawn particles.
        // https://answers.unity.com/questions/1006318/script-to-break-mesh-into-smaller-pieces.html
        // TODO - destroy the gameobject after a delay, or change layers
        foreach (Collider coll in GetComponentsInChildren<Collider>())
        {
            coll.enabled = false;
        }
        foreach (MeshRenderer renderer in GetComponentsInChildren<MeshRenderer>())
        {
            renderer.enabled = false;
        }
    }
}