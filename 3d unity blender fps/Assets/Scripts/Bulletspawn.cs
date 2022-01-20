using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Bulletspawn : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float shootInterval;
    public float shootForce;
    public Transform shootPos;
    Animator animator;

    public bool canShoot = true;

    public int currentAmmo;
    public int allAmmo;
    int magazineSizeAmmo;
    int weaponType;
    public Text ammo;
    public Text weapon;
    public Animator weaponAnimation;
    public Camera kamera;
    public ParticleSystem blyskStrzalu;
    public int bron;
    public GameObject ak47;
    public GameObject noz;
    public bool canStab;
    bool canstabdamage;
    public Animator nozAnimator;


    void Start()
    {
        animator = GetComponent<Animator>();

        //przykladowe wartosci do testu broni
        weaponType = 1;
        currentAmmo = 0;
        allAmmo = 420;

        if (weaponType == 1)
        {
            weapon.text = "AK-47";
            magazineSizeAmmo = 30;
            shootInterval = 0.12f;
        }
        WyswietlanieAmmoUI();
    }
    void Update()
    {
        if (shakeDuration > 0)
        {
            camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shakeDuration = 0f;
            camTransform.localPosition = originalPos;
        }










        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            bron = 3;
            noz.SetActive(true);
            ak47.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            bron = 1;
            noz.SetActive(false);
            ak47.SetActive(true);
        }


        if (bron == 1)
        {
            canStab = false;
            if (Input.GetMouseButton(0) && canShoot == true && currentAmmo > 0 && noShootingWhileReloading.shootinggg == true)
            {
                animator.SetBool("isFire", true);
                StartCoroutine(Shoot());
            }

            if (Input.GetMouseButton(1) && weaponAnimation.GetBool("scoping") == false)
            {
                weaponAnimation.SetBool("scoping", true);
                kamera.fieldOfView = 20;
            }
            else if (Input.GetMouseButtonUp(1) && weaponAnimation.GetBool("scoping") == true)
            {
                weaponAnimation.SetBool("scoping", false);
                kamera.fieldOfView = 60;
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                int brakujaceAmmo;
                brakujaceAmmo = magazineSizeAmmo - currentAmmo;
                if (allAmmo >= brakujaceAmmo)
                {
                    currentAmmo += brakujaceAmmo;
                    allAmmo -= brakujaceAmmo;
                    weaponAnimation.SetTrigger("przeladowac");
                    noShootingWhileReloading.shootinggg = false;
                }
                else
                {
                    currentAmmo += allAmmo;
                    allAmmo = 0;
                    weaponAnimation.SetTrigger("przeladowac");
                    noShootingWhileReloading.shootinggg = false;
                }
                WyswietlanieAmmoUI();
            }
        }
        else if(bron == 3)
        {
            canStab = true;
            if (Input.GetMouseButton(0) && canStab == true)
            {  
                StartCoroutine(Stab());
                Stabb();
            }
        }
    }

    public void WyswietlanieAmmoUI()
    {
        ammo.text = currentAmmo + " / " + allAmmo;
    }

    IEnumerator Stab()
    {
        canStab = false;

        nozAnimator.SetTrigger("stabbing");
        yield return new WaitForSeconds(.2f);
        nozAnimator.ResetTrigger("stabbing");
        
        canStab = true;
    }

    void Stabb()
    {
        canstabdamage = true;
        RaycastHit hit;
        if (Physics.Raycast(kamera.transform.position, kamera.transform.forward, out hit, 4))
        {   
            Health przeciwnik = hit.transform.GetComponent<Health>(); 
            if (canstabdamage==true)
            {
                przeciwnik.currentHealth -= 10;
                przeciwnik.ModifyHealth(0);
                canstabdamage = false;
            }
        }
    }
    IEnumerator Shoot()
    {
        canShoot = false;
        currentAmmo--;
        OnEnable();
        blyskStrzalu.Play();
        weaponAnimation.SetTrigger("shooting");
        WyswietlanieAmmoUI();
        GameObject bulletInstance = Instantiate(bulletPrefab, shootPos.position, shootPos.rotation);
        bulletInstance.GetComponent<Rigidbody>().AddForce(shootForce * shootPos.up, ForceMode.Impulse);
        // bulletInstance.transform.SetPositionAndRotation(shootPos.position, shootPos.rotation);
        yield return new WaitForSeconds(shootInterval);
        canShoot = true;
    }























    // Transform of the camera to shake. Grabs the gameObject's transform
    // if null.
    public Transform camTransform;

    // How long the object should shake for.
    public float shakeDuration;

    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount;
    public float decreaseFactor;

    Vector3 originalPos;

    void Awake()
    {
        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    void OnEnable()
    {
        shakeDuration += 0.1f;
        originalPos = camTransform.localPosition;
    }

}
