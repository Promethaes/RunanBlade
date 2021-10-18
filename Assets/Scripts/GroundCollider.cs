using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GroundCollider : MonoBehaviour
{
    //may unhide later
    [HideInInspector] public UnityEvent OnTouchGround;
    [HideInInspector] public UnityEvent OnStayGround;
    [HideInInspector] public UnityEvent OnLeaveGround;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Terrain"))
            OnTouchGround.Invoke();
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Terrain"))
            OnStayGround.Invoke();
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Terrain"))
            OnLeaveGround.Invoke();
    }
}
