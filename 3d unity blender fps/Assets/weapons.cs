using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
namespace Com.Kawaiisun.SimpleHostile
{
    public class weapons : MonoBehaviourPunCallbacks
    {
        public static GameObject celownik1;

        void Start()
        {
            if (photonView.IsMine)
            {
                celownik1 = GameObject.Find("Celownik1");
                celownik1.SetActive(false);
            }
        }

        void Update()
        {
            if (photonView.IsMine)
            {
                if (Launcher.Scope1.Equals(true))
                {
                    celownik1.SetActive(true);
                }
                else
                {
                    celownik1.SetActive(false);
                }
            }
    }
    }
}
