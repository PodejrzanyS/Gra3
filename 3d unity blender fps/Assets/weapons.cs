using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
namespace Com.Kawaiisun.SimpleHostile
{
    public class weapons : MonoBehaviourPunCallbacks
    {
        public static GameObject celownik1;
        public static GameObject celownikk1;

        void Start()
        {
            if (photonView.IsMine)
            {
                weapons.celownik1 = GameObject.Find("Celownik1");

                if (Launcher.Scope1.Equals(true))
                {
                    weapons.celownik1.SetActive(true);
                }
                else
                {
                    weapons.celownik1.SetActive(false);
                }

                
            }
            else
            {
                
                    weapons.celownikk1 = GameObject.Find("Celownik1");
                if (Launcher.Scope1.Equals(true))
                {
                    weapons.celownikk1.SetActive(true);
                }
                else
                {
                    weapons.celownikk1.SetActive(false);
                }

            }


        }

        void Update()
        {
           
        }
    }
}
