using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactLookAtMe : MonoBehaviour
{
    void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
    }
}
