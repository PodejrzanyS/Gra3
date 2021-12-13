using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTrigger : MonoBehaviour

{
    public GameObject shopUI;

    public bool isOpen;
    public bool isPlayerNearby;
    public GameObject textShop;
    public GameObject sensitivity;
    public GameObject gracz;
    public GameObject drzewo;

    void Start()
    {
        textShop.active = false;
        shopUI.SetActive(false);
        isOpen = false;
        isPlayerNearby = false;
    }

    public void KupAmmo()
    {
        gracz.GetComponent<Bulletspawn>().allAmmo += 100;
        gracz.GetComponent<Bulletspawn>().WyswietlanieAmmoUI();
        gracz.GetComponent<PlayerMovement>().coins -= 10;
    }
    public void KupHealth()
    {
        if(drzewo.GetComponent<HealthDrzewo>().currentHealth < 100)
        {
            drzewo.GetComponent<HealthDrzewo>().ModifyHealth(10);
            gracz.GetComponent<PlayerMovement>().coins -= 20;

        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            textShop.active = true;
        }
    }

    void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            textShop.active = false;
        }
    }

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E) && isOpen == false)
        {
            shopUI.SetActive(true);
            isOpen = true;
            Cursor.lockState = CursorLockMode.None;
            MouseLook.CurrentMouseSensitivity = 0f;
            gracz.GetComponent<Bulletspawn>().canShoot = false;
        }
        else if (isPlayerNearby && Input.GetKeyDown(KeyCode.E) && isOpen == true)
        {
            shopUI.SetActive(false);
            isOpen = false;
            Cursor.lockState = CursorLockMode.Locked;
            sensitivity.GetComponent<MouseLook>().wrocSensitivity();
            gracz.GetComponent<Bulletspawn>().canShoot = true;
        }

        if(!isPlayerNearby && isOpen == true)
        {
            shopUI.SetActive(false);
            isOpen = false;
            Cursor.lockState = CursorLockMode.Locked;
            sensitivity.GetComponent<MouseLook>().wrocSensitivity();
            gracz.GetComponent<Bulletspawn>().canShoot = true;
        }
    }
}
