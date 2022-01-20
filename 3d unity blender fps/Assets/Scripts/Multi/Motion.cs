using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class Motion : MonoBehaviour
    {
        public float speed;

    public Camera cmaera;

        public Rigidbody rig;

        private void Start()
        {
        cmaera.enabled = false;
            rig = GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {
            float t_hmove = Input.GetAxis("Horizontal");
            float t_vmove = Input.GetAxis("Vertical");

            Vector3 t_direction = new Vector3(t_hmove, 0, t_vmove);
            t_direction.Normalize();
            rig.velocity = transform.TransformDirection(t_direction) * speed * Time.fixedDeltaTime;
        }


    }
