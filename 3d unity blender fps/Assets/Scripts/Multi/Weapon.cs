using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

namespace Com.Kawaiisun.SimpleHostile
{
    public class Weapon : MonoBehaviourPunCallbacks
    {
        #region Variables
        public Gun[] loadout;
        public Transform weaponParent;
        private int currentIndex;
        private GameObject currentWeapon;
        public GameObject bullethholePrefab;
        public LayerMask canBeShot;
        public bool isAming = false;
        private float currentCooldown;
        private bool isReloading;
        public int currency;
        public int DoneDamage;
        public Text ui_currency;
        public Text ui_DoneDamage;
        public int level;
        private Transform ui_levelbar;
        public int expNeeded;
        private Text ui_level;
        #endregion
        #region MonoHehaviour Callbacks

        public void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        public void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }
        private void Start()
        {
            foreach (Gun a in loadout) a.Initialize();
            Equip(0);
            level = 0;
            DoneDamage = 0;
            expNeeded = 500;
        }

        // Update is called once per frame
        void Update()
        {
           
            PlayerPrefs.SetInt("Currency", currency);
            PlayerPrefs.SetInt("DoneDamage", DoneDamage);
            currency = PlayerPrefs.GetInt("Currency");
            DoneDamage = PlayerPrefs.GetInt("DoneDamage");
            PlayerPrefs.Save();

            if (photonView.IsMine)
            {
                ui_levelbar = GameObject.Find("HUD/Health/levelbar").transform;
                ui_level = GameObject.Find("HUD/Username/level").GetComponent<Text>();
                ui_DoneDamage = GameObject.Find("HUD/Stats/DMG").GetComponent<Text>();
                ui_currency = GameObject.Find("HUD/Stats/Currency").GetComponent<Text>();
                ui_DoneDamage.text = $"Zadane obra¿enia: {DoneDamage}";
                ui_currency.text = $"Gold: {currency}";

                ui_level.text = $"LEVEL {level}";
                if (DoneDamage >= 500)
                {
                    level = 1;
                    expNeeded = 2500;
                }
                if (DoneDamage >= 2500)
                {
                    level = 2;
                    expNeeded = 8000;
                }
                RefreshLevelBar();
            }
            if (Pause.paused && photonView.IsMine) return;

            if (photonView.IsMine && Input.GetKeyDown(KeyCode.Alpha1)) { photonView.RPC("Equip", RpcTarget.All, 0); }
            if (photonView.IsMine && Input.GetKeyDown(KeyCode.Alpha2)) { photonView.RPC("Equip", RpcTarget.All, 1); }
            if (currentWeapon != null)
            {
                if (photonView.IsMine)
                {
                    Aim(Input.GetMouseButton(1));

                    if (loadout[currentIndex].burst != 1)
                    {
                        if (Input.GetMouseButtonDown(0) && currentCooldown <= 0 && !isReloading)
                        {
                            if (loadout[currentIndex].FireBullet()) photonView.RPC("Shoot", RpcTarget.All);
                            else StartCoroutine(Reload(loadout[currentIndex].reload));

                        }
                    }
                    else
                    {
                        if (Input.GetMouseButton(0) && currentCooldown <= 0 && !isReloading)
                        {
                            if (loadout[currentIndex].FireBullet()) photonView.RPC("Shoot", RpcTarget.All);
                            else StartCoroutine(Reload(loadout[currentIndex].reload));

                        }

                    }
                    if (Input.GetKeyDown(KeyCode.R) && !isReloading) StartCoroutine(Reload(loadout[currentIndex].reload));

                    //cd
                    if (currentCooldown > 0) currentCooldown -= Time.deltaTime;
                }
                //weapon position elastic
                currentWeapon.transform.localPosition = Vector3.Lerp(currentWeapon.transform.localPosition, Vector3.zero, Time.deltaTime * 4f);
            }
            Debug.Log((int)DoneDamage / (int)expNeeded);
            Debug.Log(DoneDamage);
            Debug.Log(expNeeded);
        }

        #region Public Metods
        public void RefreshAmmo(Text p_text)
        {
            int t_clip = loadout[currentIndex].GetClip();
            int t_stache = loadout[currentIndex].GetStash();

            p_text.text = t_clip.ToString("D2") + " / " + t_stache.ToString("D2");

        }

        #endregion
        #endregion
        #region Private Metods

        void RefreshLevelBar()
        {
            int t_level_ratio = (int)DoneDamage / (int)expNeeded;
            ui_levelbar.localScale = Vector3.Lerp(ui_levelbar.localScale, new Vector3(t_level_ratio, 1, 1), Time.deltaTime * 8f);
        }
        IEnumerator Reload(float p_wait)
        {
            isReloading = true;
            currentWeapon.SetActive(false);

            yield return new WaitForSeconds(p_wait);

            loadout[currentIndex].Reload();
            currentWeapon.SetActive(true);
            isReloading = false;
        }
        [PunRPC]
        void Equip(int p_ind)
        {
            if (currentWeapon != null)
            {
                //if(isReloading) StartCoroutine("Reload");
                Destroy(currentWeapon);
            }

            currentIndex = p_ind;
            GameObject t_newWeapon = Instantiate(loadout[p_ind].prefab, weaponParent.position, weaponParent.rotation, weaponParent) as GameObject;
            t_newWeapon.transform.localPosition = Vector3.zero;
            t_newWeapon.transform.localEulerAngles = Vector3.zero;
            t_newWeapon.GetComponent<Sway>().isMine = photonView.IsMine;
            currentWeapon = t_newWeapon;
        }

        void Aim(bool p_isAiming)
        {
            isAming = p_isAiming;
            Transform t_anchor = currentWeapon.transform.Find("Anchor");
            Transform t_state_ads = currentWeapon.transform.Find("States/ADS");
            Transform t_state_hip = currentWeapon.transform.Find("States/Hip");
            if (p_isAiming)
            {
                t_anchor.position = Vector3.Lerp(t_anchor.position, t_state_ads.position, Time.deltaTime * loadout[currentIndex].aimSpeed);
            }
            else;
            {
                t_anchor.position = Vector3.Lerp(t_anchor.position, t_state_hip.position, Time.deltaTime * loadout[currentIndex].aimSpeed);
            }
        }

        [PunRPC]
        void Shoot()
        {
            Transform t_spawn = transform.Find("Cameras/Normal Camera");

            //bloom
            Vector3 t_bloom = t_spawn.position + t_spawn.forward * 1000f;
            t_bloom += Random.Range(-loadout[currentIndex].bloom, loadout[currentIndex].bloom) * t_spawn.up;
            t_bloom += Random.Range(-loadout[currentIndex].bloom, loadout[currentIndex].bloom) * t_spawn.right;
            t_bloom -= t_spawn.position;
            t_bloom.Normalize();

            //raycast
            RaycastHit t_hit = new RaycastHit();
            if (Physics.Raycast(t_spawn.position, t_bloom, out t_hit, 1000f, canBeShot))
            {
                GameObject t_newHole = Instantiate(bullethholePrefab, t_hit.point + t_hit.normal * 0.001f, Quaternion.identity) as GameObject;
                t_newHole.transform.LookAt(t_hit.point + t_hit.normal);
                Destroy(t_newHole, 5f);

                if (photonView.IsMine)
                {

                    //shooting other player on network
                    if (t_hit.collider.gameObject.layer == 11)
                    {

                        t_hit.collider.transform.root.gameObject.GetPhotonView().RPC("TakeDamage", RpcTarget.All, loadout[currentIndex].damage, PhotonNetwork.LocalPlayer.ActorNumber);
                        currency += Random.Range(0, 2);
                        DoneDamage += loadout[currentIndex].damage;
                        PlayerPrefs.Save();

                    }
                }
            }
            currentCooldown = loadout[currentIndex].firerate;

            // gun fx
            currentWeapon.transform.Rotate(-loadout[currentIndex].recoil, 0, 0);
            currentWeapon.transform.position -= currentWeapon.transform.forward * loadout[currentIndex].kickback;
            //cooldown

        }
        [PunRPC]
        private void TakeDamage(int p_damage, int p_actor)
        {
            GetComponent<Player>().TakeDamage(p_damage, p_actor);
        }


        #endregion
     
    }

}