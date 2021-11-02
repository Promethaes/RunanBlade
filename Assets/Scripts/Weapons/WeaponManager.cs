using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.InputSystem.InputAction;

public class WeaponManager : MonoBehaviour
{

    [SerializeField] float downtime = 0.25f;
    public UnityEvent OnChangeWeapon;
    public UnityEvent OnBeginAttack;
    public UnityEvent OnFinishAttack;

    [Header("References")]
    [SerializeField] List<Weapon> weapons = new List<Weapon>();

    Weapon _currentWeapon = null;

    float _internalDowntime = 0.0f;

    private void Start()
    {
        _currentWeapon = weapons[0];
        foreach (var w in weapons)
        {
            if (_currentWeapon == w)
                continue;
        }
    }

    private void Update()
    {
        _internalDowntime += Time.deltaTime;
        if (_internalDowntime >= downtime)
            OnFinishAttack.Invoke();
    }

    public void AttackWithCurrentWeapon(CallbackContext ctx)
    {
        if (_currentWeapon.GetAmmo() == 0 || !ctx.performed)
            return;
        _internalDowntime = 0.0f;
        OnBeginAttack.Invoke();
        _currentWeapon.Attack();
        // if (_currentWeapon.GetAmmo() == 0)
        //     ChangeWeapon(0);
    }

    public void ChangeWeapon(Weapon weapon)
    {
        IEnumerator Wait()
        {
            while (_currentWeapon.attacking)
                yield return new WaitForEndOfFrame();
            _currentWeapon = weapon;
            OnChangeWeapon.Invoke();
        }
        StartCoroutine(Wait());
    }
    public void AddWeapon(Weapon weapon)
    {
        weapons.Add(weapon);
    }
    public void RemoveWeapon(Weapon weapon)
    {
        weapons.Remove(weapon);
    }
}
