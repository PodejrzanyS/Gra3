using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using ExitGames.Client.Photon;
using TMPro;

namespace Com.Kawaiisun.SimpleHostile
{
    public class PlayerInfo
    {
        public ProfileData profile;
        public int actor;
        public short kills;
        public short deaths;

        public PlayerInfo(ProfileData p, int a, short k, short d)
        {
            this.profile = p;
            this.actor = a;
            this.kills = k;
            this.deaths = d;
        }

    }


    public class Manager : MonoBehaviourPunCallbacks, IOnEventCallback
    {

        public int currency;
        public int DoneDamage;
        public enum EventCodes : byte
        {
            NewPlayer,
            UpdatePlayers,
            ChangeStat
        }

        public string player_prefab;
        public Transform[] spawn_points;

        public static List<PlayerInfo> playerInfo = new List<PlayerInfo>();
        public int myind;

        private TMP_Text ui_mykills;
        private TMP_Text ui_mydeaths;
        private Transform ui_leaderboard;
        private void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        private void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        private void Start()
        {
            ValidateConnection();
            InitializeUI();
            NewPlayer_S(Launcher.myProfile);
            Spawn();
        }
        private void Update()
        {
            
                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    if (ui_leaderboard != null)
                    {
                        if (ui_leaderboard.gameObject.activeSelf) ui_leaderboard.gameObject.SetActive(false);
                        else Leaderboard(ui_leaderboard);
                    }
                }

                  }
        public void Spawn()
        {
            Transform t_spawn = spawn_points[Random.Range(0, spawn_points.Length)];
            PhotonNetwork.Instantiate(player_prefab, t_spawn.position, t_spawn.rotation);
        }

        public void OnEvent(EventData photonEvent)
        {
            if (photonEvent.Code >= 200) return;

            EventCodes e = (EventCodes)photonEvent.Code;
            object[] o = (object[])photonEvent.CustomData;

            switch (e)
            {
                case EventCodes.NewPlayer:
                    NewPlayer_R(o);
                    break;

                case EventCodes.UpdatePlayers:
                    UpdatePlayers_R(o);
                    break;

                case EventCodes.ChangeStat:
                    ChangeStat_R(o);
                    break;
            }

        }

        private void ValidateConnection()
        {
            if (PhotonNetwork.IsConnected) return;
            SceneManager.LoadScene(0);
        }

        public void NewPlayer_S(ProfileData p)
        {
            object[] package = new object[6];

            package[0] = p.username;
            package[1] = p.level;
            package[2] = p.xp;
            package[3] = PhotonNetwork.LocalPlayer.ActorNumber;
            package[4] = (short)0;
            package[5] = (short)0;

            NewPlayer_R(package);
        }




        public void NewPlayer_R(object[] data)
        {
            PlayerInfo p = new PlayerInfo(
                new ProfileData(
                    (string)data[0],
                    (int)data[1],
                    (int)data[2]
                ),
                (int)data[3],
                (short)data[4],
                (short)data[5]
            );

            playerInfo.Add(p);

            UpdatePlayers_S(playerInfo);
        }


        public void UpdatePlayers_S(List<PlayerInfo> info)
        {
            object[] package = new object[info.Count];

            for (int i = 0; i < info.Count; i++)
            {
                object[] piece = new object[6];

                piece[0] = info[i].profile.username;
                piece[1] = info[i].profile.level;
                piece[2] = info[i].profile.xp;
                piece[3] = info[i].actor;
                piece[4] = info[i].kills;
                piece[5] = info[i].deaths;



                package[i] = piece;
            }

            UpdatePlayers_R(package);


        }

        public void UpdatePlayers_R(object[] data)
        {
            playerInfo = new List<PlayerInfo>();

            for (int i = 0; i < data.Length; i++)
            {
                object[] extract = (object[])data[i];

                PlayerInfo p = new PlayerInfo(
                    new ProfileData(
                        (string)extract[0],
                        (int)extract[1],
                        (int)extract[2]
                        ),
                    (int)extract[3],
                    (short)extract[4],
                    (short)extract[5]
                    );


                playerInfo.Add(p);

                if (PhotonNetwork.LocalPlayer.ActorNumber == p.actor) myind = i;
            }



        }

        public void ChangeStat_S(int actor, byte stat, byte amt)
        {
            Debug.Log("zmiana");
            object[] package = new object[] { actor, stat, amt };

            ChangeStat_R(package);


        }
        public void ChangeStat_R(object[] data)
        {
            Debug.Log("zmiana2");
            int actor = (int)data[0];
            byte stat = (byte)data[1];
            byte amt = (byte)data[2];
            for (int i = 0; i < playerInfo.Count; i++)
            {
                Debug.Log("for");
                Debug.Log(actor);
                Debug.Log(stat);
                Debug.Log(amt);
                if (playerInfo[i].actor == actor)
                {
                    switch (stat)
                    {
                        case 0: //kills
                            Debug.Log("kill++");
                            playerInfo[i].kills += amt;
                            Debug.Log($"Player {playerInfo[i].profile.username} : kills = {playerInfo[i].kills}");
                            break;

                        case 1: //deaths
                            Debug.Log("death++");
                            playerInfo[i].deaths += amt;
                            Debug.Log($"Player {playerInfo[i].profile.username} : deaths = {playerInfo[i].deaths}");
                            break;
                    }

                    if (i == myind) RefreshMyStats();
                    if (ui_leaderboard.gameObject.activeSelf) Leaderboard(ui_leaderboard);
                    return;
                }
            }

        }
        private void InitializeUI()
        {
            ui_mykills = GameObject.Find("HUD/Stats/Kills/Text").GetComponent<TMP_Text>();
            ui_mydeaths = GameObject.Find("HUD/Stats/Deaths/Text").GetComponent<TMP_Text>();
            ui_leaderboard = GameObject.Find("HUD").transform.Find("Leaderboard").transform;

            RefreshMyStats();

        }
        private void RefreshMyStats()
        {
            Debug.Log(playerInfo.Count.ToString() + "  " + myind.ToString() + "  " + Time.time.ToString());

            if (playerInfo.Count > myind)
            {
                ui_mykills.text = $"{playerInfo[myind].kills} kills";
                ui_mydeaths.text = $"{playerInfo[myind].deaths} deaths";
            }
            else
            {
                ui_mykills.text = $"0 kills";
                ui_mydeaths.text = $"0 deaths";
            }
            if (ui_leaderboard.gameObject.activeSelf) Leaderboard(ui_leaderboard);
        }

        public static PlayerInfo[] GetAllPlayers()
        {
            return playerInfo.ToArray();
        }




        private void Leaderboard(Transform p_lb)
        {

            // clean up
            for (int i = 2; i < p_lb.childCount; i++)
            {
                Destroy(p_lb.GetChild(i).gameObject);
            }

            // set details
            



            // cache prefab
            GameObject playercard = p_lb.GetChild(1).gameObject;
            playercard.SetActive(false);

            // sort
            List<PlayerInfo> sorted = SortPlayers(playerInfo);

            // display
            bool t_alternateColors = false;
            foreach (PlayerInfo a in sorted)
            {
                GameObject newcard = Instantiate(playercard, p_lb) as GameObject;

                if (t_alternateColors) newcard.GetComponent<Image>().color = new Color32(0, 0, 0, 180);
                t_alternateColors = !t_alternateColors;
              
                newcard.transform.Find("Level").GetComponent<Text>().text = $"LEVEL {a.profile.level}";
                newcard.transform.Find("Username").GetComponent<Text>().text = $"Nickname: {a.profile.username}";
                newcard.transform.Find("Deaths").GetComponent<Text>().text = $"Deaths: {a.deaths}";

                newcard.SetActive(true);
            }

            // activate
            p_lb.gameObject.SetActive(true);
        }

        private List<PlayerInfo> SortPlayers (List<PlayerInfo> p_info)
        {
            List<PlayerInfo> sorted = new List<PlayerInfo>();


            while (sorted.Count < p_info.Count)
            {
                // set defaults
                short highest = -1;
                PlayerInfo selection = p_info[0];

                // grab next highest player
                foreach (PlayerInfo a in p_info)
                {
                    if (sorted.Contains(a)) continue;
                    if (a.deaths > highest)
                    {
                        selection = a;
                        highest = a.deaths;
                    }
                }


                // add player
                sorted.Add(selection);
            }
            return sorted;
        }

         
            
        






    }
}
