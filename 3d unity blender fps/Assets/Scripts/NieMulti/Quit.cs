using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Quit : MonoBehaviour
{
    public bool isOpen;
    public GameObject textQuit;
    public bool isPlayerNearby;
    public GameObject gracz;
    public GameObject sensitivity;

    void Start()
    {
        textQuit.active = false;
       
        isOpen = false;
        isPlayerNearby = false;
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            textQuit.active = true;
        }
    }

    void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            textQuit.active = false;
        }
    }

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.Q) && isOpen == false)
        {
            SceneManager.LoadScene(0);
        }
        
        if (!isPlayerNearby && isOpen == true)
        {
           
            isOpen = false;
            Cursor.lockState = CursorLockMode.Locked;
            sensitivity.GetComponent<MouseLook>().wrocSensitivity();
            gracz.GetComponent<Bulletspawn>().canShoot = true;
        }
    }
}
