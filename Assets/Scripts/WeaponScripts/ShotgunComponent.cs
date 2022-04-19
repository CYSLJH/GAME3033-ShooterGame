using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunComponent : WeaponComponent
{
    [SerializeField] private ShotgunDamageArea damageArea;

    // Start is called before the first frame update
    void Start()
    {
        damageArea.GetComponent<Collider>().isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void FireWeapon()
    {
        base.FireWeapon();

        if (firingEffect)
        {
            firingEffect.Play();
        }
        if (audioSource != null)
        {
            audioSource.clip = audioClips[0];
            audioSource.Play();
        }
        List<GameObject> targets = damageArea.GetTargets;

        //foreach (GameObject target in targets.ToArray())
        //{
        //    DealDamage(target);
        //}
        for (int i = 0; i < targets.Count; i++)
        {
            DealDamage(targets[i]);
        }
    }

    void DealDamage(GameObject zombie)
    {
        if (zombie != null)
        {
            IDamageable damageable = zombie.GetComponent<IDamageable>();
            damageable?.TakeDamage(weaponStats.damage);
            if (zombie.GetComponent<HealthComponent>().CurrentHealth <= 0)
            {
                damageArea.GetTargets.Remove(zombie);
            }
            if (zombie.GetComponent<ZombieComponent>() != null)
            {
                zombie.GetComponent<ZombieComponent>().StunEnemy();
            }
        }
    }

    //IEnumerator FireDelay()
    //{
    //    damageArea.gameObject.SetActive(true);
    //    List<GameObject> targets = damageArea.GetTargets;

    //    foreach (GameObject target in targets)
    //    {
    //        DealDamage(target);
    //    }
    //    yield return new WaitForSeconds(0.06f);
    //    damageArea.gameObject.SetActive(false);
    //}
}
