using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class sniper1 : MonoBehaviourPunCallbacks
{
    public static GameObject ScopeOverlay;
    public static GameObject Sniper1;
    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine)
        {
            Sniper1 = GameObject.Find("Anchor").gameObject;
            ScopeOverlay = GameObject.Find("Scope").gameObject;
            ScopeOverlay.SetActive(false);
            Sniper1.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
