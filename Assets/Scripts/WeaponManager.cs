using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] List<Weapon> weapons = new List<Weapon>();
    [SerializeField] float downtime = 0.25f;
    public UnityEvent OnChangeWeapon;
    public UnityEvent OnBeginAttack;
    public UnityEvent OnFinishAttack;

    int _weaponIndex = 0;
    Weapon _currentWeapon = null;
    bool _canBackstab = false;

    float _internalDowntime = 0.0f;

    private void Start()
    {
        _currentWeapon = weapons[0];
        foreach (var w in weapons)
        {
            if (_currentWeapon == w)
                continue;
            w.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        _internalDowntime += Time.deltaTime;
        if (_internalDowntime >= downtime)
            OnFinishAttack.Invoke();
    }

    public void AttackWithCurrentWeapon()
    {
        if (_currentWeapon.GetAmmo() == 0)
            return;
        _internalDowntime = 0.0f;
        OnBeginAttack.Invoke();
        _currentWeapon.Attack();
        // if (_currentWeapon.GetAmmo() == 0)
        //     ChangeWeapon(0);
    }

    public void ChangeWeapon(int index)
    {
        IEnumerator Wait()
        {
            while (_currentWeapon.attacking)
                yield return new WaitForEndOfFrame();
            _weaponIndex = index;
            _currentWeapon.gameObject.SetActive(false);
            _currentWeapon = weapons[_weaponIndex];
            _currentWeapon.gameObject.SetActive(true);
            OnChangeWeapon.Invoke();
        }
        StartCoroutine(Wait());
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Terrain") || other.gameObject.CompareTag("Unassigned"))
            return;
        _canBackstab = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Terrain") || other.gameObject.CompareTag("Unassigned"))
            return;
        _canBackstab = false;
    }
}
