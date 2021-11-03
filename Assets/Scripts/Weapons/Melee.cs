using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Melee : Weapon
{
    [SerializeField] float attackSpeed = 1.0f;
    [SerializeField] int comboLimit = 3;
    [SerializeField] float comboCooldown = 1.0f;
    [Header("References")]
    [SerializeField] GameObject hitbox = null;
    [SerializeField] new Camera camera = null;

    Vector2 _mousePos = new Vector2();
    Vector2 _dir = new Vector2();

    int _comboCounter = 0;
    float _comboCooldown = 0.0f;

    private void Start()
    {
        IEnumerator Cooldown()
        {
            while (true)
            {
                yield return new WaitForEndOfFrame();
                if (_comboCounter > 0)
                {
                    _comboCooldown -= Time.deltaTime;
                    if (_comboCooldown <= 0.0f)
                    {
                        attacking = false;
                        _comboCounter = 0;
                        _comboCooldown = comboCooldown;
                    }
                }
            }
        }
        StartCoroutine(Cooldown());
    }

    // Update is called once per frame
    void Update()
    {
        if (hitbox.activeSelf)
            return;
        var _currentDir = (hitbox.transform.position - transform.position).normalized;
        var rot = Vector3.RotateTowards(_currentDir, (Vector2)_dir, 10.0f, 0.0f);
        var angle = Vector3.SignedAngle(_currentDir, rot, Vector3.forward);
        transform.RotateAround(transform.position, Vector3.forward, angle);
    }

    public void MousePosition(CallbackContext ctx)
    {
        _mousePos = ctx.ReadValue<Vector2>();
        _mousePos = camera.ScreenToWorldPoint(_mousePos);
        _dir = _mousePos - (Vector2)transform.position;
        _dir = _dir.normalized;
        //_dir = _dir.normalized;
    }

    public override void Attack()
    {
        if (hitbox.activeSelf || _comboCounter == 3)
            return;
        IEnumerator Wait()
        {
            attacking = true;
            _comboCooldown = comboCooldown;
            _comboCounter++;
            hitbox.SetActive(true);
            yield return new WaitForSeconds(1.0f / attackSpeed);
            hitbox.SetActive(false);
        }
        StartCoroutine(Wait());
    }
}
