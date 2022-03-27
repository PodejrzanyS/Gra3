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
        private bool isEquiping;
        public int exp;
        public static bool bloonhit;
        public static GameObject whathit;
        public GameObject bloon;
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
            

          
            if (photonView.IsMine)
            {
                lvl = PlayerPrefs.GetInt("level");
                curr = PlayerPrefs.GetInt("Currency");
                diddamage = PlayerPrefs.GetInt("DoneDamage");
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
                bloonhit = false;
                if (lvl == 0)
                {
                    level = 0;
                    expNeeded = 500;
                    PlayerPrefs.SetInt("expNeeded", expNeeded);
                    PlayerPrefs.Save();
                }
                else
                {
                    level = lvl;
                    DoneDamage = PlayerPrefs.GetInt("DoneDamage");
                    expNeeded = PlayerPrefs.GetInt("expNeeded");
                    PlayerPrefs.Save();
                }
                foreach (Gun a in loadout) a.Initialize();
                Equip(0);
            }

            


        }

        // Update is called once per frame
        void Update()
        {


            if (photonView.IsMine)
            {
                lvl = PlayerPrefs.GetInt("level");
                curr = PlayerPrefs.GetInt("Currency");
                diddamage = PlayerPrefs.GetInt("DoneDamage");
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
            if (DoneDamage >= 500 && lvl == 0)
            {
                
                level = 1;
                PlayerPrefs.SetInt("level", level);
                expNeeded = 1000;
                DoneDamage = 0;
                PlayerPrefs.SetInt("DoneDamage", DoneDamage);
                PlayerPrefs.SetInt("expNeeded", expNeeded);
                PlayerPrefs.Save();
                StartCoroutine(MyIEnumerator());
        

            }
            if (DoneDamage >= 1000 && lvl ==1)
            {
                level = 2;
                PlayerPrefs.SetInt("level", level);
                expNeeded = 1500;
                DoneDamage = 0;
                PlayerPrefs.SetInt("DoneDamage", DoneDamage);
                PlayerPrefs.SetInt("expNeeded", expNeeded);
                PlayerPrefs.Save();
                StartCoroutine(MyIEnumerator());
  
            }
            if (DoneDamage >= 1500 && lvl == 2)
            {
                level = 3;
                PlayerPrefs.SetInt("level", level);
                expNeeded = 2000;
                DoneDamage = 0;
                PlayerPrefs.SetInt("DoneDamage", DoneDamage);
                PlayerPrefs.SetInt("expNeeded", expNeeded);
                PlayerPrefs.Save();
                StartCoroutine(MyIEnumerator());
   
            }
            if (DoneDamage >= 2000 && lvl == 3)
            {
                level = 4;
                PlayerPrefs.SetInt("level", level);
                expNeeded = 2500;
                DoneDamage = 0;
                PlayerPrefs.SetInt("DoneDamage", DoneDamage);
                PlayerPrefs.SetInt("expNeeded", expNeeded);
                PlayerPrefs.Save();
                StartCoroutine(MyIEnumerator());

            }
            if (DoneDamage >= 2500 && lvl == 4)
            {
                level = 5;
                PlayerPrefs.SetInt("level", level);
                expNeeded = 2500;
                DoneDamage = 0;
                PlayerPrefs.SetInt("DoneDamage", DoneDamage);
                PlayerPrefs.SetInt("expNeeded", expNeeded);
                PlayerPrefs.Save();
                StartCoroutine(MyIEnumerator());

            }
            if (DoneDamage >= 3000 && lvl == 5)
            {
                level = 6;
                PlayerPrefs.SetInt("level", level);
                expNeeded = 3500;
                DoneDamage = 0;
                PlayerPrefs.SetInt("DoneDamage", DoneDamage);
                PlayerPrefs.SetInt("expNeeded", expNeeded);
                PlayerPrefs.Save();
                StartCoroutine(MyIEnumerator());

            }
            if (DoneDamage >= 3500 && lvl == 6)
            {
                level = 7;
                PlayerPrefs.SetInt("level", level);
                expNeeded = 4000;
                DoneDamage = 0;
                PlayerPrefs.SetInt("DoneDamage", DoneDamage);
                PlayerPrefs.SetInt("expNeeded", expNeeded);
                PlayerPrefs.Save();
                StartCoroutine(MyIEnumerator());

            }
            if (DoneDamage >= 4000 && lvl == 7)
            {
                level = 8;
                PlayerPrefs.SetInt("level", level);
                expNeeded = 4500;
                DoneDamage = 0;
                PlayerPrefs.SetInt("DoneDamage", DoneDamage);
                PlayerPrefs.SetInt("expNeeded", expNeeded);
                PlayerPrefs.Save();
                StartCoroutine(MyIEnumerator());

            }
            if (DoneDamage >= 4500 && lvl == 8)
            {
                level = 9;
                PlayerPrefs.SetInt("level", level);
                expNeeded = 5000;
                DoneDamage = 0;
                PlayerPrefs.SetInt("DoneDamage", DoneDamage);
                PlayerPrefs.SetInt("expNeeded", expNeeded);
                PlayerPrefs.Save();
                StartCoroutine(MyIEnumerator());

            }
            if (DoneDamage >= 5000 && lvl == 9)
            {
                level = 10;
                PlayerPrefs.SetInt("level", level);
                expNeeded = 100000;
                DoneDamage = 0;
                PlayerPrefs.SetInt("DoneDamage", DoneDamage);
                PlayerPrefs.SetInt("expNeeded", expNeeded);
                PlayerPrefs.Save();
                StartCoroutine(MyIEnumerator());

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
            if (isEquiping == true) { yield break; }
            isReloading = true;
            if (currentWeapon != null)
            {
                if (currentWeapon.GetComponent<Animator>())
            {
                currentWeapon.GetComponent<Animator>().Play("Reload", 0, 0);
            }
            else
                currentWeapon.SetActive(false);
            } 
            yield return new WaitForSeconds(p_wait);

            loadout[currentIndex].Reload();
            currentWeapon.SetActive(true);
            isReloading = false;
        }
        [PunRPC]
        IEnumerator Equip(int p_ind)
        {
          
            if (isEquiping == true || isReloading == true || isAming == true) { yield break; }
            
            if (currentWeapon != null)
            {
                //if(isReloading) StartCoroutine("Reload");
                Destroy(currentWeapon);
            }
            
            isEquiping = true;   
            currentIndex = p_ind;
            GameObject t_newWeapon = Instantiate(loadout[p_ind].prefab, weaponParent.position, weaponParent.rotation, weaponParent) as GameObject;
            
            t_newWeapon.transform.localPosition = Vector3.zero;
            t_newWeapon.transform.localEulerAngles = Vector3.zero;
           
            t_newWeapon.GetComponent<Sway>().isMine = photonView.IsMine;

            t_newWeapon.GetComponent<Animator>().keepAnimatorControllerStateOnDisable = true;
            t_newWeapon.GetComponent<Animator>().Play("Equip", 0, 0);

            
            yield return new WaitForSeconds(1);

            currentWeapon = t_newWeapon;
            isEquiping = false;
            if (loadout[p_ind].name == "Sniper")
            {
                if (sniper1.Sniper1 != null)
                {
                    sniper1.Sniper1.SetActive(true);
                }
            }
        }
        IEnumerator Wait()
        {
            yield return new WaitForSeconds(3);
            Instantiate(bloon, new Vector3(Random.Range(42, 56), Random.Range(-1, 9), 11), Quaternion.identity);
        }
        IEnumerator WaitM()
        {
            if (sniper1.Sniper1 != null)
            {
                sniper1.Sniper1.SetActive(false);
            }
            yield return new WaitForSeconds(.15f);
            if (sniper1.ScopeOverlay != null)
            {
                sniper1.ScopeOverlay.SetActive(true);
            }
        }
        IEnumerator WaitF()
        {
             
            if (sniper1.Sniper1 != null)
            {
                sniper1.Sniper1.SetActive(true);
            }
            yield return new WaitForSeconds(.15f);
            if (sniper1.ScopeOverlay != null)
            {
                sniper1.ScopeOverlay.SetActive(false);
            }
        }
        void Aim(bool p_isAiming)
        {
            if (isEquiping == true) { return; }
            isAming = p_isAiming;
            Transform t_anchor = currentWeapon.transform.Find("Anchor");
            Transform t_state_ads = currentWeapon.transform.Find("States/ADS");
            Transform t_state_hip = currentWeapon.transform.Find("States/Hip");
            if (p_isAiming)
            {
                t_anchor.position = Vector3.Lerp(t_anchor.position, t_state_ads.position, Time.deltaTime * loadout[currentIndex].aimSpeed);
            }
            else
            {
                t_anchor.position = Vector3.Lerp(t_anchor.position, t_state_hip.position, Time.deltaTime * loadout[currentIndex].aimSpeed);
            }
           
        }

        [PunRPC]
        void Shoot()
        {
            if (isEquiping == true) { return; }
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

                        t_hit.collider.transform.root.gameObject.GetPhotonView().RPC("TakeDamage", RpcTarget.All, loadout[currentIndex].damage + (lvl * 4), PhotonNetwork.LocalPlayer.ActorNumber);
                        currency = curr + Random.Range(0, 5);
                        DoneDamage = loadout[currentIndex].damage + diddamage;
                        RefreshLevelBar();

                        PlayerPrefs.SetInt("Currency", currency);
                        PlayerPrefs.SetInt("DoneDamage", DoneDamage);
                        PlayerPrefs.Save();


                    }
                    if (t_hit.collider.gameObject.layer == 12 && bloonhit == false)
                    {

                        whathit = t_hit.collider.gameObject;
                        Destroy(whathit);
                        StartCoroutine(Wait());
                        currency = curr + Random.Range(0, 1);

                        Debug.Log(bloonhit);

                    }
                }
               
            }
            currentCooldown = loadout[currentIndex].firerate;

            // gun fx
            if (currentWeapon != null)
            {
                currentWeapon.transform.Rotate(-loadout[currentIndex].recoil, 0, 0);
                currentWeapon.transform.position -= currentWeapon.transform.forward * loadout[currentIndex].kickback;
            }

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