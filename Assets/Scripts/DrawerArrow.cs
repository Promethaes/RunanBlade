using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class DrawerArrow : MonoBehaviour
{
    [SerializeField] AnimationCurve lerpCurve = null;
    [SerializeField] float lerpSpeed = 1.0f;
    [Header("References")]
    [SerializeField] new Camera camera = null;
    [SerializeField] GameObject arrow = null;
    [SerializeField] GameObject background = null;
    [SerializeField] Transform bgLerpPos = null;

    Vector2 _originalPos = new Vector2();
    Vector2 _mousePos = new Vector2();
    float _yCoord = 0.0f;
    bool _drawerOpen = false;

    Coroutine _openCR = null;
    Coroutine _closeCR = null;
    private void Start()
    {
        _yCoord = arrow.transform.position.y;
        
        
        _originalPos = background.transform.position;
    }


    //maybe revise this code
    private void Update()
    {
        if (!_drawerOpen && _mousePos.y <= _yCoord)
        {
            _drawerOpen = true;
            OnOpenDrawer();
        }
        else if (_drawerOpen && _mousePos.y > _yCoord)
        {
            _drawerOpen = false;
            OnCloseDrawer();
        }
    }

    //hook up to player in editor
    public void MousePosition(CallbackContext ctx)
    {
        _mousePos = ctx.ReadValue<Vector2>();
    }

    public void OnOpenDrawer()
    {
        if (_closeCR != null)
            StopCoroutine(_closeCR);
        _openCR = StartCoroutine(LerpForward());
    }

    public void OnCloseDrawer()
    {
        if (_openCR != null)
            StopCoroutine(_openCR);
        StartCoroutine(LerpBackward());
    }

    IEnumerator LerpForward()
    {
        float x = 0.0f;
        while (x < 1.0f)
        {
            yield return new WaitForEndOfFrame();
            x += Time.deltaTime * lerpSpeed;
            background.transform.position = Vector3.Lerp(_originalPos, bgLerpPos.position, lerpCurve.Evaluate(x));
            background.transform.position = new Vector3(_originalPos.x, background.transform.position.y, background.transform.position.z);
        }
    }

    IEnumerator LerpBackward()
    {
        float x = 1.0f;
        while (x > 0.0f)
        {
            yield return new WaitForEndOfFrame();
            x -= Time.deltaTime * lerpSpeed;
            background.transform.position = Vector3.Lerp(_originalPos, bgLerpPos.position, lerpCurve.Evaluate(x));
            background.transform.position = new Vector3(_originalPos.x, background.transform.position.y, background.transform.position.z);
        }
    }
}
