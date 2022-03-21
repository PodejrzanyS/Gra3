using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

namespace Com.Kawaiisun.SimpleHostile
{
    public class Player : MonoBehaviourPunCallbacks
    {
        #region Variables
        public float speed;
        public float sprintModifier;
        public float jumpForce;
        public float slideModifier;
        public int max_health;
        public Transform weaponParent;
        public Transform groundDetector;
        public Camera normalCam;
        public GameObject cameraParent;
        public LayerMask ground;
        private TMP_Text ui_username;
        private Transform ui_healthbar;
        private Text ui_ammo;
        public Rigidbody rig;
        private float baseFOV;
        private float sprintFOVModifier = 1.2f;
        private Vector3 weaponParentOrigin;
        private Vector3 weaponParentCurrentPos;
        private Vector3 targetWeaponBobPosition;
        private float movementCounter;
        private float idleCounter;
        private int current_health;
        private Manager manager;
        private Weapon weapon;
        private bool sliding;
        private float slide_time;
        public float lengthofSlide;
        private Vector3 slide_dir;
        private Vector3 origin;
        private Vector3 velocity;
        [HideInInspector]public ProfileData playerProfile;
        public TextMeshPro playerUsername;
        public Transform LookAtMe;
        public int lvl;
        public int kills;
        public static GameObject scoreboard;
        public bool animating = false;
        private Animator anim;
        #endregion

        #region Monobehaviour Callback


        private void Start()
        {
            
            velocity = rig.velocity;
            manager = GameObject.Find("Manager").GetComponent<Manager>();
            weapon = GetComponent<Weapon>();



            cameraParent.SetActive(photonView.IsMine);

            if (!photonView.IsMine)
            {
                gameObject.layer = 11;
            }

            baseFOV = normalCam.fieldOfView;
            origin = normalCam.transform.localPosition;
            if (Camera.main) normalCam.enabled = false;
            rig = GetComponent<Rigidbody>();
            weaponParentOrigin = weaponParent.localPosition;
            weaponParentCurrentPos = weaponParentOrigin;

            if (photonView.IsMine)
            {


                anim = GetComponent<Animator>();
                lvl = PlayerPrefs.GetInt("level");
                ui_healthbar = GameObject.Find("HUD/Health/Bar").transform;
                ui_ammo = GameObject.Find("HUD/Ammo/Text").GetComponent<Text>();
                ui_username = GameObject.Find("HUD/Username/Text").GetComponent<TMP_Text>();
                
                RefreshHealthBar();

                ui_username.text = Launcher.myProfile.username;

                photonView.RPC("SyncProfile", RpcTarget.All, Launcher.myProfile.username, Launcher.myProfile.level, Launcher.myProfile.xp);
            }
            current_health = max_health + lvl*20;
        }
        [PunRPC]
        private void SyncProfile(string p_username,int p_level,int p_xp)
        {
            playerProfile = new ProfileData(p_username,p_level,p_xp);
            playerUsername.text = playerProfile.username;
        }
        private void Update()
        {
            if (!photonView.IsMine) return;

       
            lvl = PlayerPrefs.GetInt("level");
            float t_hmove = Input.GetAxis("Horizontal");
            float t_vmove = Input.GetAxis("Vertical");
        
            bool sprint = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            bool jump = Input.GetKey(KeyCode.Space);
            bool pause = Input.GetKeyDown(KeyCode.Escape);

            bool isGrounded = Physics.Raycast(groundDetector.position, Vector3.down, 0.1f, ground);
            bool isJumping = jump && isGrounded;
            bool isSprinting = sprint && t_vmove > 0 && !isJumping && isGrounded;
            bool isMoving = t_vmove > 0 && isGrounded;
            //Pause
            if (pause)
            {

                GameObject.Find("Pause").GetComponent<Pause>().TogglePause();
              
            }
            if (Pause.paused)
            {
                t_hmove = 0f;
                t_vmove = 0f;
                sprint = false;
                jump = false;
                pause = false;
                isGrounded = false;
                isJumping = false;
                isSprinting = false;
               

            }

            if (Input.GetKeyDown(KeyCode.U)) TakeDamage(100,-1);
            if(Input.GetKey(KeyCode.Space))
            {
                GetComponent<Animator>().Play("Jump");
            }
            if (isJumping && rig.velocity.y <= 0)
            {
                
                velocity.y = 0.2f;
                rig.velocity = velocity;
                rig.AddForce(Vector3.up * jumpForce);
            }
            //head bob
            float t_aim_adjust = 1f;
            if (sliding) 
            {
                HeadBob(movementCounter, 0.15f, 0.075f);
                weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, targetWeaponBobPosition, Time.deltaTime * 10f);
            }
            else if (t_hmove == 0 && t_vmove == 0)
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
            if (Input.GetKeyDown(KeyCode.Tab) && scoreboard == false)
            {
                scoreboard.SetActive(false);
            } else if(Input.GetKeyDown(KeyCode.Tab) && scoreboard == true)
            {
                scoreboard.SetActive(false);
            }

           

           
                if(isMoving && !(GetComponent<Animator>().GetNextAnimatorStateInfo(0).length > GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime))
                {

                    GetComponent<Animator>().Play("Move");

                }
                

            RefreshHealthBar();

            weapon.RefreshAmmo(ui_ammo);

        }
        void FixedUpdate()
        {
            if (!photonView.IsMine) return;
            float t_hmove = Input.GetAxis("Horizontal");
            float t_vmove = Input.GetAxis("Vertical");

            bool sprint = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            bool jump = Input.GetKey(KeyCode.Space);
            bool slide = Input.GetKey(KeyCode.LeftControl);

            bool isGrounded = Physics.Raycast(groundDetector.position, Vector3.down, 0.1f, ground);
            bool isJumping = jump && isGrounded;
            bool isSprinting = sprint && t_vmove > 0 && !isJumping && isGrounded;
            bool isSliding = isSprinting && slide && !sliding;
            //movement
            Vector3 t_direction = Vector3.zero;
            float t_adjustedSpeed = speed;


            if (Pause.paused)
            {

                t_hmove = 0f;
                t_vmove = 0f;
                sprint = false;
                jump = false;
                isGrounded = false;
                isJumping = false;
                isSprinting = false;
                isSliding = false;
                slide = false;

            }


            if (!sliding)
            {

                t_direction = new Vector3(t_hmove, 0, t_vmove);
                //t_direction.Normalize();
                t_direction = transform.TransformDirection(t_direction);


                if (isSprinting) t_adjustedSpeed *= sprintModifier;
            }
            else
            {
                t_direction = slide_dir;
                t_adjustedSpeed *= slideModifier;
                slide_time -= Time.deltaTime;
                if (slide_time <= 0)
                {
                    sliding = false;
                    weaponParentCurrentPos += Vector3.up * 0.5f;
                }

            }

            Vector3 t_targetVelocity = t_direction * t_adjustedSpeed * Time.deltaTime;
            t_targetVelocity.y = rig.velocity.y;
            rig.velocity = t_targetVelocity;

            //sliding
            if (isSliding)
            {
                sliding = true;
                slide_dir = t_direction;
                slide_time = lengthofSlide;

                weaponParentCurrentPos += Vector3.down * 0.5f;
            }

            // camera stuff
            if (sliding)
            {
                normalCam.fieldOfView = Mathf.Lerp(normalCam.fieldOfView, baseFOV * sprintFOVModifier * 1.25f, Time.deltaTime * 8f);
                normalCam.transform.localPosition = Vector3.Lerp(normalCam.transform.localPosition, origin + Vector3.down * 0.5f, Time.deltaTime * 6f);
            }
            else
            {

                if (isSprinting) { normalCam.fieldOfView = Mathf.Lerp(normalCam.fieldOfView, baseFOV * sprintFOVModifier, Time.deltaTime * 8f); }
                else { normalCam.fieldOfView = Mathf.Lerp(normalCam.fieldOfView, baseFOV, Time.deltaTime * 8f); }

                normalCam.transform.localPosition = Vector3.Lerp(normalCam.transform.localPosition, origin, Time.deltaTime * 6f);
            }
            float t_anim_horizontal = 0f;
            float t_anim_vertical = 0f;

            if (isGrounded)
            {
                t_anim_horizontal = t_direction.x;
                t_anim_vertical = t_direction.z;
            }
            anim.SetFloat("Horizontal", t_anim_horizontal);
            anim.SetFloat("Vertical", t_anim_vertical);
        }
        #endregion

        #region Private Methods

        void HeadBob(float p_z, float p_x_intensity, float p_y_intensity)
        {
            float t_aim_adjust = 1f;
            if (weapon.isAming) t_aim_adjust = 0.1f;
            targetWeaponBobPosition = weaponParentCurrentPos + new Vector3(Mathf.Cos(p_z) * p_x_intensity*t_aim_adjust, Mathf.Sin(p_z * 2) * p_y_intensity*t_aim_adjust, 0);
        }

        void RefreshHealthBar()
        {
            float t_health_ratio = (float)current_health / (float)max_health;
            ui_healthbar.localScale =Vector3.Lerp( ui_healthbar.localScale ,new Vector3(t_health_ratio, 1, 1),Time.deltaTime*8f);
        }


        #endregion


        #region public methods

       
        public void TakeDamage(int p_damage,int actor)
        {
            if (photonView.IsMine)
            {
                current_health -= p_damage;
                RefreshHealthBar();
                if (current_health <= 0)
                {
                    Debug.Log("Actor: "+actor);
                    manager.Spawn();
                  
                    manager.ChangeStat_S(PhotonNetwork.LocalPlayer.ActorNumber, 1, 1);
                    Debug.Log("dead  "+Launcher.myProfile.username);

                    if (actor >= 0) { manager.ChangeStat_S(actor, 0, 1); }
                    PhotonNetwork.Destroy(gameObject);
                }
            }
        }
       
        #endregion


    }
}
