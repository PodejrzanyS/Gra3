using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health;
    Rigidbody rb;
    GameObject target;
    float moveSpeed;
    Vector3 directionToTarget;
    public GameObject explosion;
    void Start()
    {
        target = GameObject.Find("Drzewo");
        rb = GetComponent<Rigidbody>();
        moveSpeed = Random.Range(1f, 5f);
        transform.position = new Vector3(transform.position.x + Random.Range(-10, 10), transform.position.y, transform.position.z + Random.Range(-10, 10));
    }
    // Update is called once per frame
    void Update()
    {
        MoveMonster();

        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(this.gameObject);
    }

    void MoveMonster()
    {
        if (target != null)
        {
            directionToTarget = (target.transform.position - transform.position).normalized;
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);
            transform.LookAt(target.transform.position);

        }
        else
        {
         
            rb.velocity = Vector3.zero;
        }
    }
}
