using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class WeaponHolder : MonoBehaviour
{
    [Header("Weapon to Spawn"), SerializeField]
    GameObject weaponToSpawn;

    public PlayerController playerController;
    Animator playerAnimator;
    Sprite crosshairImage;
    WeaponComponent equippedWeapon;
    public WeaponComponent GetEquippedWeapon => equippedWeapon;

    [SerializeField]
    GameObject weaponSocketLocation;
    //[SerializeField]
    //Transform GripIKSocketLocation;

    bool firingPressed = false;
    GameObject spawnedWeapon;
    public readonly int isFiringHash = Animator.StringToHash("isFiring");
    public readonly int isReloadingHash = Animator.StringToHash("isReloading");

    [SerializeField]
    private WeaponScriptable startWeapon;

    [SerializeField]
    private WeaponAmmoUI weaponAmmoUI;

    public Dictionary<WeaponType, WeaponStats> weaponAmmoDictionary;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        playerAnimator = GetComponent<Animator>();
        weaponAmmoDictionary = new Dictionary<WeaponType, WeaponStats>();

        playerController.inventory.AddItem(startWeapon, 1);
        weaponAmmoDictionary.Add(startWeapon.weaponStats.weaponType, startWeapon.weaponStats);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnFire(InputValue value)
    {
        firingPressed = value.isPressed;

        if (firingPressed)
        {
            StartFiring();
        }
        else
        {
            StopFiring();
        }

    }

    public void OnReload(InputValue value)
    {
        playerController.isReloading = value.isPressed;
        StartReloading();
    }

    private void StartFiring()
    {
        if (!equippedWeapon) return;

        if (equippedWeapon.weaponStats.bulletsInClip <= 0)
        {
            StartReloading();
            return;
        };

        playerController.isFiring = true;
        playerAnimator.SetBool(isFiringHash, true);
        equippedWeapon.StartFiringWeapon();
    }

    private void StopFiring()
    {
        if (!equippedWeapon) return;

        playerController.isFiring = false;
        playerAnimator.SetBool(isFiringHash, false);
        equippedWeapon.StopFiringWeapon();
    }

    public void StartReloading()
    {
        if (!equippedWeapon) return;

        if (equippedWeapon.isReloading || equippedWeapon.weaponStats.bulletsInClip == equippedWeapon.weaponStats.clipSize) return;

        if (playerController.isFiring)
        {
            StopFiring();
        }
        if (equippedWeapon.weaponStats.totalBullets <= 0) return;
        playerAnimator.SetBool(isReloadingHash, true);
        equippedWeapon.StartReloading();
        weaponAmmoDictionary[equippedWeapon.weaponStats.weaponType] = equippedWeapon.weaponStats;
        InvokeRepeating(nameof(StopReloading), 0, 0.1f);

    }

    public void StopReloading()
    {
        if (!equippedWeapon) return;

        if (playerAnimator.GetBool(isReloadingHash)) return;

        playerController.isReloading = false;
        equippedWeapon.StopReloading();
        playerAnimator.SetBool(isReloadingHash, false);
        CancelInvoke(nameof(StopReloading));
    }

    public void EquipWeapon(WeaponScriptable weaponScriptable)
    {
        if (!weaponScriptable) return;

        spawnedWeapon = Instantiate(weaponScriptable.itemPrefab, weaponSocketLocation.transform.position, weaponSocketLocation.transform.rotation, weaponSocketLocation.transform);

        if (!spawnedWeapon) return;

        equippedWeapon = spawnedWeapon.GetComponent<WeaponComponent>();

        if (!equippedWeapon) return;

        equippedWeapon.Initialize(this, weaponScriptable);
        if (weaponAmmoDictionary.ContainsKey(equippedWeapon.weaponStats.weaponType))
        {
            equippedWeapon.weaponStats = weaponAmmoDictionary[equippedWeapon.weaponStats.weaponType];
        }
        PlayerEvents.InvokeOnWeaponEquipped(equippedWeapon);
    }

    public void UnquipWeapon()
    {
        if (!equippedWeapon) return;
        if (weaponAmmoDictionary.ContainsKey(equippedWeapon.weaponStats.weaponType))
        {
            weaponAmmoDictionary[equippedWeapon.weaponStats.weaponType] = equippedWeapon.weaponStats;
        }

        Destroy(equippedWeapon.gameObject);
        equippedWeapon = null;
    }
}