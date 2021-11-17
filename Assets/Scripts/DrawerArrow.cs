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
    [SerializeField] WeaponManager weaponManager = null;

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
        bool shouldOpen = _mousePos.y <= _yCoord || _opening;
        weaponManager.canAttack = !shouldOpen;

        if (shouldOpen)
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

    public bool IsDrawerOpen()
    {
        return _opening;
    }

    ///b: true for open, false for closed, -1 duration for forever
    public void SetDrawerOpen(bool b, float duration)
    {
        IEnumerator ChangeDrawerState()
        {
            _opening = b;
            float x = 0.0f;
            while (x < duration || duration == -1)
            {
                yield return new WaitForEndOfFrame();
                x += Time.deltaTime;

            }
            _opening = !b;
        }
        StartCoroutine(ChangeDrawerState());
    }
}
