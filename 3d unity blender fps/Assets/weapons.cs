using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Com.Kawaiisun.SimpleHostile
{
    public class weapons : MonoBehaviour
    {
        public static GameObject celownik1;

        void Start()
        {
            celownik1 = GameObject.Find("Celownik1");
            celownik1.SetActive(false);
        }

        void Update()
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
