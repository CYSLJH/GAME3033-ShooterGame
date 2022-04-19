using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHealthComponent : HealthComponent
{
    ZombieStateMachine zombieStateMachine;

    AudioSource audioSource;

    private void Awake()
    {
        zombieStateMachine = GetComponent<ZombieStateMachine>();
        audioSource = GetComponent<AudioSource>();
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        if (audioSource != null)
        {
            audioSource.clip = gameObject.GetComponent<ZombieComponent>().audioClips[0];
            audioSource.Play();
        }
    }

    // Start is called before the first frame update
    public override void Destroy()
    {
        zombieStateMachine.ChangeState(ZombieStateType.isDead);
    }

   
}
