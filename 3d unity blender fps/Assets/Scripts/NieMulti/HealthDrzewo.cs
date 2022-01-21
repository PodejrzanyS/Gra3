using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HealthDrzewo : MonoBehaviour
{
    [SerializeField]
    public int maxHealth;
    public int damage;
    public int currentHealth;
    private GameObject enemyTriggered;
    //public GameObject manager;

    public event Action<float> OnHealthPctChanged = delegate { };

    private void OnEnable()
    {
        currentHealth = maxHealth;
    }

    public void ModifyHealth(int amount)
    {
        currentHealth += amount;

        float currentHealthPct = (float)currentHealth / (float)maxHealth;
        OnHealthPctChanged(currentHealthPct);
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            Die();
     //       manager.GetComponent<Manager>().End();
        }
    }
    public void Die()
    {
        Destroy(this.gameObject); 
    }
    void OnCollisionEnter(Collision other)
    {
        Debug.Log("a");
        if (other.transform.gameObject.tag == "Enemy")
        {
            ModifyHealth(-damage);
            Debug.Log("bijom");
            Destroy(other.gameObject);
        }
    }
}
