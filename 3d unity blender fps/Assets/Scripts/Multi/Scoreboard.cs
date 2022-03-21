using System.Collections;
using System.Collections.Generic;
using Com.Kawaiisun.SimpleHostile;
using UnityEngine;

public class Scoreboard : MonoBehaviour
{
    void OnEnable()
    {
        PlayerInfo[] playerInfo = Manager.GetAllPlayers();

        foreach (PlayerInfo player in playerInfo)
        {
            Debug.Log(Launcher.myProfile.username + " | " + player.deaths);
        }
    }

    void OnDisable()
    {
        PlayerInfo[] playerInfo = Manager.GetAllPlayers();

        foreach (PlayerInfo player in playerInfo)
        {
            Debug.Log(Launcher.myProfile.username + " | " + player.deaths);
        }
    }
}

