using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAT : MonoBehaviour
{
    private Transform target;
    void LateUpdate()
    {
        target = GameObject.FindWithTag("LookAt").transform;
        transform.LookAt(target);
    }
}
