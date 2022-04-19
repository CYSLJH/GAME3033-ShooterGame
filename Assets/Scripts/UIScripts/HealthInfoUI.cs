using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthInfoUI : MonoBehaviour
{

    [SerializeField]
    private Slider healthBar;
    HealthComponent playerHealthComponent;

    private void OnEnable()
    {
        PlayerEvents.OnHealthInitialized += OnHealthInitialized;
    }

    private void OnDisable()
    {
        PlayerEvents.OnHealthInitialized -= OnHealthInitialized;
    }

    // Start is called before the first frame update
    void Start()
    {
        healthBar = GetComponent<Slider>();
    }

    private void OnHealthInitialized(HealthComponent healthComponent)
    {
        playerHealthComponent = healthComponent;
    }
    public void updateHealthBar()
    {
        healthBar.value = playerHealthComponent.CurrentHealth / playerHealthComponent.MaxHealth;
    }


    // Update is called once per frame
    void Update()
    {
        updateHealthBar();
    }
}
