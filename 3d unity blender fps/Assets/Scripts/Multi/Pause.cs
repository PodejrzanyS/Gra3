using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

namespace Com.Kawaiisun.SimpleHostile
{
    public class Pause : MonoBehaviourPunCallbacks
    {
        public static bool paused = false;
        private bool disconnecting = false;
        public GameObject player;

        public void TogglePause()
        {
            if (disconnecting) return;
            paused = !paused;

            transform.GetChild(0).gameObject.SetActive(paused);
            Cursor.lockState = (paused) ? CursorLockMode.None : CursorLockMode.Confined;
            Cursor.visible = paused;

        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();
           
        }

        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);

            base.OnLeftRoom();
        }



    }
}
