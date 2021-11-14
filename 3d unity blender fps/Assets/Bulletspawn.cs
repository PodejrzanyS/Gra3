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

    bool canShoot = true;

    public int currentAmmo;
    public int allAmmo;
    int magazineSizeAmmo;
    int weaponType;
    public Text ammo;
    public Text weapon;
    public Animator weaponAnimation;

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
        if (Input.GetMouseButton(0) && canShoot == true && currentAmmo > 0)
        {
            animator.SetBool("isFire", true);
            StartCoroutine(Shoot());
        }

        if (Input.GetMouseButton(1) && weaponAnimation.GetBool("scoping") == false)
        {
            weaponAnimation.SetBool("scoping", true);
        }
        else if(Input.GetMouseButtonUp(1) && weaponAnimation.GetBool("scoping") == true)
        {
            weaponAnimation.SetBool("scoping", false);
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
            }
            else
            {
                currentAmmo += allAmmo;
                allAmmo = 0;
                weaponAnimation.SetTrigger("przeladowac");
            }
            WyswietlanieAmmoUI();
        }
    }

    void WyswietlanieAmmoUI()
    {
        ammo.text = currentAmmo + " / " + allAmmo;
    }
    IEnumerator Shoot()
    {
        canShoot = false;
        currentAmmo--;
        WyswietlanieAmmoUI();
        GameObject bulletInstance = Instantiate(bulletPrefab, shootPos.position, shootPos.rotation);
        bulletInstance.GetComponent<Rigidbody>().AddForce(shootForce * shootPos.up, ForceMode.Impulse);
        // bulletInstance.transform.SetPositionAndRotation(shootPos.position, shootPos.rotation);
        yield return new WaitForSeconds(shootInterval);
        canShoot = true;
    } 
}
