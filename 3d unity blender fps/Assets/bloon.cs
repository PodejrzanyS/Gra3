using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Com.Kawaiisun.SimpleHostile
{
    public class bloon : MonoBehaviour
    {
         public static GameObject balon;
        // Start is called before the first frame update
        void Start()
        {
            balon = this.gameObject;
            
            balon.SetActive(true);
        }

        // Update is called once per frame
        void Update()
        {
            
           

        } 
    }
}