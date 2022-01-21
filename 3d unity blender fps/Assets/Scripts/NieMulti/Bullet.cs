using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float aliveTime;
    public float moveSpeed;


   // public GameObject bulletSpawn;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        aliveTime -= 1 * Time.deltaTime;

        if(aliveTime<=0)
        {
            Destroy(this.gameObject);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Destroy(this.gameObject);
        }
    }
}
