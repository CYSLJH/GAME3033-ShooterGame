using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class WeaponAmmoUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI weaponNameText;
    [SerializeField] private TextMeshProUGUI currentClipAmmoText;
    [SerializeField] private TextMeshProUGUI totalAmmoText;
    
    [SerializeField] WeaponComponent weaponComponent;
    private void Start()
    {
        PlayerEvents.OnWeaponEquipped += this.OnWeaponEquipped;
    }

    private void OnDestroy()
    {
        PlayerEvents.OnWeaponEquipped -= this.OnWeaponEquipped;

    }

    void OnWeaponEquipped(WeaponComponent _weaponComponent)
    {
        weaponComponent = _weaponComponent;
    }

    // Update is called once per frame
    void Update()
    {
        if (!weaponComponent)
        {
            return;
        }

        weaponNameText.text = weaponComponent.weaponStats.weaponName;
        currentClipAmmoText.text = weaponComponent.weaponStats.bulletsInClip.ToString();
        totalAmmoText.text = weaponComponent.weaponStats.totalBullets.ToString();


    }
}
