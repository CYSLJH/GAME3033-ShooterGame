using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealthComponent : HealthComponent
{

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        PlayerEvents.InvokeOnHealthInitialized(this);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
     
    }

    public override void Destroy()
    {
        Debug.Log("Player dead");
        //Application.Quit();

        // Load a new Game Over Scene when at 0 
        // ... Play a death animation?
        GameManager.Instance.GetComponent<AudioSource>().Stop();
        SceneLoadManager.Instance.LoadLoseScreen();
    }

}
