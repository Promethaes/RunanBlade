using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellStartPos : MonoBehaviour
{
    [HideInInspector] public Transform startPos = null;
    // Update is called once per frame
    void Update()
    {
        RotateStartPos();
    }

    protected virtual void RotateStartPos()
    {

    }
}
