using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTrigger : MonoBehaviour

{

    public GameObject shopUI;


    public bool isOpen;

    public bool isPlayerNearby;

    bool cos = false;

    #region Singleton



    public static ShopTrigger instance;



    private void Awake()

    {

        if (instance != null)

        {

            Debug.LogWarning("More than one instance of Inventory found!");

            return;



        }

        instance = this;
    }



    #endregion





    public void Start()

    {



        shopUI.SetActive(false);

        isOpen = false;

        isPlayerNearby = false;

    }









    public void OpenShop()

    {

        shopUI.SetActive(true);



    }



    public void CloseShop()

    {

        shopUI.SetActive(false);
         

    }



 /*   void OnTriggerStay(Collider col)
    {
        if (col.transform.gameObject.tag == "Player")

        {

            if(!isOpen && Input.GetKeyDown(KeyCode.E))
            {
                    Debug.Log("Pressed to Open");

                OpenShop();
                isOpen = true;
            }
            if (isOpen && Input.GetKeyDown(KeyCode.E))
            {
                    Debug.Log("Pressed to Close");

                CloseShop();
                isOpen = false;
            }


        }
    }
    

   void OnTriggerEnter(Collider col)
    {
        if (col.transform.gameObject.tag == "Player")

        {
            isPlayerNearby = true;
            OpenShop();
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.transform.gameObject.tag == "Player")

        {
            isPlayerNearby = false;
            CloseShop();
        }
    }
    */
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isOpen = true;
        }
    }


    public void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            isOpen = false;

        }
    }

    private void Update()
    {
        if (isOpen && Input.GetKeyDown(KeyCode.E) && (!shopUI.activeSelf))
        {
            shopUI.SetActive(true);

           
        }
        if (!isOpen  && (shopUI.activeSelf))
        {
            shopUI.SetActive(false);
        }
    }
}
