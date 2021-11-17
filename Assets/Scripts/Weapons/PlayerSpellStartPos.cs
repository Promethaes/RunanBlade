using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class PlayerSpellStartPos : SpellStartPos
{
    [Header("References")]
    [SerializeField] new Camera camera = null;
    Vector2 _mousePos = Vector2.zero;
    Vector2 _dir = Vector2.zero;
    protected override void RotateStartPos()
    {
        var _currentDir = (startPos.transform.position - transform.position).normalized;
        var rot = Vector3.RotateTowards(_currentDir, (Vector2)_dir, 10.0f, 0.0f);
        var angle = Vector3.SignedAngle(_currentDir, rot, Vector3.forward);
        startPos.RotateAround(transform.position, Vector3.forward, angle);
    }

    public void MousePosition(CallbackContext ctx)
    {
        _mousePos = ctx.ReadValue<Vector2>();
        _mousePos = camera.ScreenToWorldPoint(_mousePos);
        _dir = _mousePos - (Vector2)transform.position;
        _dir = _dir.normalized;
    }
}
