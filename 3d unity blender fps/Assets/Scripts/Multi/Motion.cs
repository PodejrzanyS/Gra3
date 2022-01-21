using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Motion : MonoBehaviourPunCallbacks
{
    #region Variables
    public float speed;
    public float sprintModifier;
    public float jumpForce;
    public Transform weaponParent;
    public Transform groundDetector;
    public Camera normalCam;
    public GameObject cameraParent;
    public LayerMask ground;

    public Rigidbody rig;
    private float baseFOV;
    private float sprintFOVModifier = 1.5f;
    private Vector3 WeaponParentOrigin;
    private Vector3 targetWeaponBobPosition;
    private float movementCounter;
    private float idleCounter;
    #endregion

    #region Monobehaviour Callback
    private void Start()
    {
        
        cameraParent.SetActive(photonView.IsMine);

        baseFOV = normalCam.fieldOfView;
        if (Camera.main) normalCam.enabled = false;
        rig = GetComponent<Rigidbody>();
        WeaponParentOrigin = weaponParent.localPosition;
    }
    private void Update()
    {
        if (!photonView.IsMine) return;
        float t_hmove = Input.GetAxis("Horizontal");
        float t_vmove = Input.GetAxis("Vertical");

        bool sprint = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        bool jump = Input.GetKey(KeyCode.Space);

        bool isGrounded = Physics.Raycast(groundDetector.position, Vector3.down, 0.1f, ground);
        bool isJumping = jump && isGrounded;
        bool isSprinting = sprint && t_vmove > 0 && !isJumping && isGrounded;


        if (isJumping)
        {
            rig.AddForce(Vector3.up * jumpForce);
        }

        if (t_hmove == 0 && t_vmove == 0)
        {
            HeadBob(idleCounter, 0.015f, 0.015f);
            idleCounter += Time.deltaTime;
            weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, targetWeaponBobPosition, Time.deltaTime * 2f);
        }
        else if (!isSprinting)
        {
            HeadBob(movementCounter, 0.015f, 0.15f);
            movementCounter += Time.deltaTime * 7f;
            weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, targetWeaponBobPosition, Time.deltaTime * 10f);
        }
        else
        {
            HeadBob(movementCounter, 0.15f, 0.075f);
            movementCounter += Time.deltaTime * 7f;
            weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, targetWeaponBobPosition, Time.deltaTime * 10f);
        }

        
    }
    void FixedUpdate()
    {

        float t_hmove = Input.GetAxis("Horizontal");
        float t_vmove = Input.GetAxis("Vertical");

        bool sprint = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        bool jump = Input.GetKey(KeyCode.Space);

        bool isGrounded = Physics.Raycast(groundDetector.position, Vector3.down, 0.1f, ground);
        bool isJumping = jump && isGrounded;
        bool isSprinting = sprint && t_vmove > 0 && !isJumping && isGrounded;






        Vector3 t_direction = new Vector3(t_hmove, 0, t_vmove);
        

        float t_adjustedSpeed = speed;
        if (isSprinting) t_adjustedSpeed *= sprintModifier;

        Vector3 t_targetVelocity = transform.TransformDirection(t_direction) * t_adjustedSpeed;
        t_targetVelocity.y = rig.velocity.y;
        rig.velocity = t_targetVelocity;

        if (isSprinting) { normalCam.fieldOfView = Mathf.Lerp(normalCam.fieldOfView, baseFOV * sprintFOVModifier, Time.deltaTime * 8f); }
        else { normalCam.fieldOfView = Mathf.Lerp(normalCam.fieldOfView, baseFOV, Time.deltaTime * 8f); }
    }
    #endregion

    #region Private Methods
    void HeadBob(float p_z, float p_x_intensity,float p_y_intensity)
    {
        weaponParent.localPosition = WeaponParentOrigin + new Vector3(Mathf.Cos(p_z)*p_x_intensity, Mathf.Sin(p_z * 2) *p_y_intensity,0);
    }

    #endregion


}
