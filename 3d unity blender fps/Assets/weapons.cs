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

        public static GameObject silancer1;
        public static GameObject silancerr1;

        public static GameObject magazynek1;
        public static GameObject magazynekk1;
        void Start()
        {
            //celownik
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

            //silancer
            if (photonView.IsMine)
            {
                weapons.silancer1 = GameObject.Find("Silancer1");

                if (Launcher.Silancer1.Equals(true))
                {
                    weapons.silancer1.SetActive(true);
                }
                else
                {
                    weapons.silancer1.SetActive(false);
                }


            }
            else
            {

                weapons.silancerr1 = GameObject.Find("Silancer1");
                if (Launcher.Silancer1.Equals(true))
                {
                    weapons.silancerr1.SetActive(true);
                }
                else
                {
                    weapons.silancerr1.SetActive(false);
                }

            }
            // magazynek
            if (photonView.IsMine)
            {
                weapons.magazynek1 = GameObject.Find("Magazynek1");

                if (Launcher.Magazynek1.Equals(true))
                {
                    weapons.magazynek1.SetActive(true);
                }
                else
                {
                    weapons.magazynek1.SetActive(false);
                }


            }
            else
            {

                weapons.magazynekk1 = GameObject.Find("Magazynek1");
                if (Launcher.Magazynek1.Equals(true))
                {
                    weapons.magazynekk1.SetActive(true);
                }
                else
                {
                    weapons.magazynekk1.SetActive(false);
                }

            }
        }

        void Update()
        {
           
        }
    }
}
