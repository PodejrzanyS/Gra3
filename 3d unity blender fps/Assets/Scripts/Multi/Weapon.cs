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
        private Slider ui_levelbar;
        public int expNeeded;
        private Text ui_level;
        public int curr;
        public int diddamage;
        public int lvl;
        public Text LevelUpText;
        public Text HealthAndDamage;
        private GameObject NEWLEVEL;
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
            lvl = PlayerPrefs.GetInt("level");
            curr = PlayerPrefs.GetInt("Currency");
            diddamage = PlayerPrefs.GetInt("DoneDamage");
            if (photonView.IsMine)
            {
                ui_levelbar = GameObject.Find("HUD/Health/levelbar").GetComponent<Slider>();
                ui_level = GameObject.Find("HUD/Username/level").GetComponent<Text>();
                ui_DoneDamage = GameObject.Find("HUD/Stats/DMG").GetComponent<Text>();
                ui_currency = GameObject.Find("HUD/Stats/Currency").GetComponent<Text>();
                LevelUpText = GameObject.Find("HUD/LevelUpText").GetComponent<Text>();
                HealthAndDamage = GameObject.Find("HUD/Damage+Health").GetComponent<Text>();
                LevelUpText.enabled = false;
                HealthAndDamage.enabled = false;
                ui_DoneDamage.text = $"Zadane obra¿enia: {diddamage}";
                ui_currency.text = $"Gold: {curr}";
                NEWLEVEL = GameObject.Find("NEWLEVEL");
                ui_level.text = $"LEVEL {lvl}";
            }
            
            foreach (Gun a in loadout) a.Initialize();
            Equip(0);
            if (lvl == 0)
            {
                level = 0;
                expNeeded = 500;
            }
            else
            {
                level = lvl;
                
            }
            
        }

        // Update is called once per frame
        void Update()
        {
            lvl = PlayerPrefs.GetInt("level");
            curr = PlayerPrefs.GetInt("Currency");
            diddamage = PlayerPrefs.GetInt("DoneDamage");


            if (photonView.IsMine)
            {
                NEWLEVEL = GameObject.Find("NEWLEVEL");
                LevelUpText = GameObject.Find("HUD/LevelUpText").GetComponent<Text>();
                HealthAndDamage = GameObject.Find("HUD/Damage+Health").GetComponent<Text>();
                ui_levelbar = GameObject.Find("HUD/Health/levelbar").GetComponent<Slider>();
                ui_level = GameObject.Find("HUD/Username/level").GetComponent<Text>();
                ui_DoneDamage = GameObject.Find("HUD/Stats/DMG").GetComponent<Text>();
                ui_currency = GameObject.Find("HUD/Stats/Currency").GetComponent<Text>();
                ui_DoneDamage.text = $"Zadane obra¿enia: {diddamage}";
                ui_currency.text = $"Gold: {curr}";
                ui_level.text = $"LEVEL {lvl}";
                levelUp();
                RefreshLevelBar();

                if (level >= 1)
                {
                    
                }


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
          
            PlayerPrefs.Save();
        }

        #region Public Metods
        IEnumerator MyIEnumerator()
        {
            LevelUpText.enabled = true;
            HealthAndDamage.enabled = true;
            yield return new WaitForSeconds(1);
            LevelUpText.enabled = false;
            HealthAndDamage.enabled = false;
            yield return new WaitForSeconds(1);
            LevelUpText.enabled = true;
            HealthAndDamage.enabled = true;
            yield return new WaitForSeconds(1);
            LevelUpText.enabled = false;
            HealthAndDamage.enabled = false;
            yield return new WaitForSeconds(1);
            LevelUpText.enabled = true;
            HealthAndDamage.enabled = true;
            yield return new WaitForSeconds(1);
            LevelUpText.enabled = false;
            HealthAndDamage.enabled = false;
            yield return new WaitForSeconds(1);
            LevelUpText.enabled = true;
            HealthAndDamage.enabled = true;
            yield return new WaitForSeconds(1);
            LevelUpText.enabled = false;
            HealthAndDamage.enabled = false;
        }
        public void levelUp()
        {
            if (DoneDamage >= 500)
            {
                
                level = 1;
                PlayerPrefs.SetInt("level", level);
                expNeeded = 2500;
                DoneDamage = 0;
                PlayerPrefs.SetInt("DoneDamage", DoneDamage);
                PlayerPrefs.Save();
                StartCoroutine(MyIEnumerator());
                
            }
            if (DoneDamage >= 2500)
            {
                level = 2;
                PlayerPrefs.SetInt("level", level);
                expNeeded = 8000;
                DoneDamage = 0;
                PlayerPrefs.SetInt("DoneDamage", DoneDamage);
                PlayerPrefs.Save();
            }
            if (DoneDamage >= 8000)
            {
                level = 3;
                PlayerPrefs.SetInt("level", level);
                expNeeded = 20000;
                DoneDamage = 0;
                PlayerPrefs.SetInt("DoneDamage", DoneDamage);
                PlayerPrefs.Save();
            }
            if (DoneDamage >= 20000)
            {
                level = 4;
                PlayerPrefs.SetInt("level", level);
                expNeeded = 100000;
                DoneDamage = 0;
                PlayerPrefs.SetInt("DoneDamage", DoneDamage);
                PlayerPrefs.Save();
            }
            if (DoneDamage >= 100000)
            {
                level = 5;
                PlayerPrefs.SetInt("level", level);
                expNeeded = 1000000;
                DoneDamage = 0;
                PlayerPrefs.SetInt("DoneDamage", DoneDamage);
                PlayerPrefs.Save();
            }
        }
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
            float t_level_ratio = (float)DoneDamage / (float)expNeeded;
            ui_levelbar.value = t_level_ratio; 
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

                        t_hit.collider.transform.root.gameObject.GetPhotonView().RPC("TakeDamage", RpcTarget.All, loadout[currentIndex].damage + (lvl*4), PhotonNetwork.LocalPlayer.ActorNumber);
                        currency =curr + Random.Range(0, 2);
                        DoneDamage = loadout[currentIndex].damage + diddamage;
                        RefreshLevelBar();
                        Debug.Log(loadout[currentIndex].damage + (lvl * 4));
                        PlayerPrefs.SetInt("Currency", currency);
                        PlayerPrefs.SetInt("DoneDamage", DoneDamage);
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