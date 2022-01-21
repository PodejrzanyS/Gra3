using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Manager2 : MonoBehaviour
{
    public GameObject gracz;
    int health;
    public Text tekst;
    public GameObject end;
    int counting = 5;

    public float restartDelay = 100f;

    void Start()
    {
        end.active = false;
    }

    public void End()
    {
        health=gracz.GetComponent<HealthDrzewo>().currentHealth;
        if(health<=0)
        {
            Debug.Log("dead");
            Restart();
            end.active = true;
           
            StartCoroutine(Restart());
        }
    }

    IEnumerator Restart()
    {
        tekst.text = "NEW GAME WILL START IN " + 5 + " SECONDS";
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}