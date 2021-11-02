using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class DrawerArrow : MonoBehaviour
{
    [SerializeField] AnimationCurve lerpCurve = null;
    [SerializeField] float lerpSpeed = 1.0f;
    [Header("References")]
    [SerializeField] Transform drawerOpenThreshold = null;
    [SerializeField] GameObject background = null;
    [SerializeField] Transform bgLerpPos = null;

    Vector2 _originalPos = new Vector2();
    Vector2 _mousePos = new Vector2();
    float _yCoord = 0.0f;
    bool _drawerOpen = false;


    bool _opening = false;
    float _x = 0.0f;
    private void Start()
    {
        _yCoord = drawerOpenThreshold.transform.position.y;
        _originalPos = background.transform.position;
    }

    //maybe revise this code
    private void Update()
    {
        _opening = _mousePos.y <= _yCoord;

        if (_opening)
            _x += Time.deltaTime * lerpSpeed;
        else
            _x -= Time.deltaTime * lerpSpeed;

        _x = Mathf.Clamp(_x, 0.0f, 1.0f);
        background.transform.position = Vector3.Lerp(_originalPos, bgLerpPos.position, lerpCurve.Evaluate(_x));
        background.transform.position = new Vector3(_originalPos.x, background.transform.position.y, background.transform.position.z);
    }

    //hook up to player in editor
    public void MousePosition(CallbackContext ctx)
    {
        _mousePos = ctx.ReadValue<Vector2>();
    }
}
