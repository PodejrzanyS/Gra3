using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTrigger : MonoBehaviour

{
    public GameObject shopUI;

    public bool isOpen;
    public bool isPlayerNearby;
    public GameObject textShop;

    void Start()
    {
        textShop.active = false;
        shopUI.SetActive(false);
        isOpen = false;
        isPlayerNearby = false;
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
        }
        else if (isPlayerNearby && Input.GetKeyDown(KeyCode.E) && isOpen == true)
        {
            shopUI.SetActive(false);
            isOpen = false;
        }

        if(!isPlayerNearby && isOpen == true)
        {
            shopUI.SetActive(false);
            isOpen = false;
        }
    }
}
