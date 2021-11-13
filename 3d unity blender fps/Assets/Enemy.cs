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
    public int crh;
    Animator animator;
    bool wylaczony = false;


    void Start()
    {
        animator = GetComponent<Animator>();
        target = GameObject.Find("Drzewo");
        rb = GetComponent<Rigidbody>();
        moveSpeed = Random.Range(1f, 5f);
        transform.position = new Vector3(transform.position.x + Random.Range(-10, 10), transform.position.y, transform.position.z + Random.Range(-10, 10));
    }
    // Update is called once per frame
    void Update()
    {
        crh =GetComponent<Health>().currentHealth;
        MoveMonster();

        if (crh <= 0)
        {
            WylaczCollider();
            Die();
        }
    }
    void WylaczCollider()
    {
        if (wylaczony == false)
        {
            Collider kolider = GetComponent<Collider>();
            kolider.enabled = !kolider.enabled;
            wylaczony = true;
        }
    }
    public void Die()
    {
        
        StartCoroutine(ExampleCoroutine());
    }

    void MoveMonster()
    {
        if ((target != null) && crh > 0)
        {
            animator.SetBool("isRunning", true);
            directionToTarget = (target.transform.position - transform.position).normalized;
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);
            transform.LookAt(target.transform.position);

        }
        else
        {
            animator.SetBool("isRunning", false);
            rb.velocity = Vector3.zero;
        }
    }
    IEnumerator ExampleCoroutine()
    {
        
        animator.SetBool("isDead", true);
        yield return new WaitForSeconds(3f);
        Destroy(this.gameObject);
    }
}
