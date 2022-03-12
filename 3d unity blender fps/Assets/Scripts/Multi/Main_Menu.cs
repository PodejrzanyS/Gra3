using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Com.Kawaiisun.SimpleHostile
{
    public class Main_Menu : MonoBehaviour
    {


        private void Start()
        {
            Pause.paused = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        public void PlayGame()
        {
            SceneManager.LoadScene(1);
        }
        public void PlayGameMulti()
        {
            SceneManager.LoadScene(2);
        }
        public void QuitGame()
        {
            Debug.Log("Quit");
            Application.Quit();
        }
    }
}
