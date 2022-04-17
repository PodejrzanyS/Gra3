using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;
using UnityEngine.UI;
namespace Com.Kawaiisun.SimpleHostile
{
    [System.Serializable]
    public class ProfileData
    {
        public string username;
        public int level;
        public int xp;
      
        public ProfileData()
        {
            this.username = "DEFAULT USERNAME";
            this.level = 0;
            this.xp = 0;
        }

        public ProfileData(string u, int l, int x)
        {
            this.username = u;
            this.level = l;
            this.xp = x;
        }
        object[] ConvertToObjectArr()
        {
            object[] ret = new object[3];

            return ret;
        }
    }

   
    public class Launcher : MonoBehaviourPunCallbacks
    {
        public TMP_InputField usernameField;
        public static ProfileData myProfile = new ProfileData();

        public int curr;
        public int lvl;
        public int DoneDamage;
        [SerializeField] TMP_Text ui_level;
        [SerializeField] TMP_Text ui_DoneDamage;
        [SerializeField] TMP_Text ui_currency;
        public static bool Scope1;
        public static bool Silancer1;
        public static bool Magazynek1;



        public static Launcher Instance;

        [SerializeField] TMP_InputField roomNameInputField;
        [SerializeField] TMP_Text errorText;
        [SerializeField] TMP_Text roomNameText;
        [SerializeField] Transform roomListContent;
        [SerializeField] GameObject roomListItemPrefab;
        [SerializeField] Transform playerListContent;
        [SerializeField] GameObject PlayerListItemPrefab;
        [SerializeField] GameObject startGameButton;

        public void Awake()
        {
            myProfile = Data.LoadProfile();
            usernameField.text = myProfile.username;
            Instance = this;
        }

        void Start()
        {
            
                Scope1 = true;
            Silancer1 = true;
            Magazynek1 = true;
            lvl = PlayerPrefs.GetInt("level");
            curr = PlayerPrefs.GetInt("Currency");
            DoneDamage = PlayerPrefs.GetInt("DoneDamage");
            ui_currency.text = $"Gold: {curr}";
            ui_level.text = $"LEVEL {lvl}";
            Debug.Log("Connecting to Master");
            PhotonNetwork.ConnectUsingSettings();

        }
        private void Update()
        {
            curr = PlayerPrefs.GetInt("Currency");
            ui_currency.text = $"Gold: {curr}";
        }
        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected to Master");
            PhotonNetwork.JoinLobby();
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        public override void OnJoinedLobby()
        {
            MenuManager.Instance.OpenMenu("title");
            Debug.Log("Joined Lobby");
        }

        public void CreateRoom()
        {
            if (string.IsNullOrEmpty(roomNameInputField.text))
            {
                return;
            }
            PhotonNetwork.CreateRoom(roomNameInputField.text);
            MenuManager.Instance.OpenMenu("loading");
        }

        public override void OnJoinedRoom()
        {
            MenuManager.Instance.OpenMenu("room");
            roomNameText.text = PhotonNetwork.CurrentRoom.Name;

            Photon.Realtime.Player[] players = PhotonNetwork.PlayerList;

            foreach (Transform child in playerListContent)
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < players.Count(); i++)
            {
                Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
            }

            startGameButton.SetActive(PhotonNetwork.IsMasterClient);
        }

        public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
        {
            startGameButton.SetActive(PhotonNetwork.IsMasterClient);
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            errorText.text = "Room Creation Failed: " + message;
            Debug.LogError("Room Creation Failed: " + message);
            MenuManager.Instance.OpenMenu("error");
        }

        public void StartGame()
        {
            if (string.IsNullOrEmpty(usernameField.text))
            {
                myProfile.username = "PLAYER" + Random.Range(100, 1000);
            }
            else
            {
                myProfile.username = usernameField.text;
            }
            Data.SaveProfile(myProfile);
            PhotonNetwork.LoadLevel(3);
        }
        public void OnUsernameInputValueChanged()
        {
            PhotonNetwork.NickName = usernameField.text;
            PlayerPrefs.SetString("username", usernameField.text);
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
            MenuManager.Instance.OpenMenu("loading");
        }

        public void JoinRoom(RoomInfo info)
        {
            if (string.IsNullOrEmpty(usernameField.text))
            {
                myProfile.username = "PLAYER" + Random.Range(100, 1000);
            }
            else
            {
                myProfile.username = usernameField.text;
            }
            Data.SaveProfile(myProfile);
            PhotonNetwork.JoinRoom(info.Name);
            MenuManager.Instance.OpenMenu("loading");
        }

        public override void OnLeftRoom()
        {
            MenuManager.Instance.OpenMenu("title");
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            foreach (Transform trans in roomListContent)
            {
                Destroy(trans.gameObject);
            }

            for (int i = 0; i < roomList.Count; i++)
            {
                if (roomList[i].RemovedFromList)
                    continue;
                Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
            }
        }

        public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
        {
            Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
        }
        public void BuyScope1()
        {
           
                if (curr >= 300 && lvl >= 4 && Scope1 == false)
                {
                    curr -= 300;
                    PlayerPrefs.SetInt("Currency", curr);
                    PlayerPrefs.Save();
                Scope1 = true;
                PlayerPrefs.SetInt("Scope1", Scope1 ? 1 : 0);
                curr = PlayerPrefs.GetInt("Currency");
                }
            
        }
        public void SellScope1()
        {

            if (Scope1 == true)
            {
                curr += 100;
                PlayerPrefs.SetInt("Currency", curr);
                PlayerPrefs.Save();
                Scope1 = false;
                PlayerPrefs.SetInt("Scope1", Scope1 ? 1 : 0);
                curr = PlayerPrefs.GetInt("Currency");
            }

        }
        public void BuySilancer1()
        {
            if (curr >= 500 && lvl >= 5 && Silancer1 == false)
            {
                curr -= 200;
                PlayerPrefs.SetInt("Currency", curr);
                PlayerPrefs.Save();
                Silancer1 = true;
                PlayerPrefs.SetInt("Scope1", Silancer1 ? 1 : 0);
                curr = PlayerPrefs.GetInt("Currency");
            }
        }
        public void SellSilancer1()
        {

            if (Silancer1 == true)
            {
                curr += 50;
                PlayerPrefs.SetInt("Currency", curr);
                PlayerPrefs.Save();
                Silancer1 = false;
                PlayerPrefs.SetInt("Scope1", Silancer1 ? 1 : 0);
                curr = PlayerPrefs.GetInt("Currency");
            }

        }
        public void BuyMagazynek1()
        {
            if (curr >= 600 && lvl >= 7 && Magazynek1 == false)
            {
                curr -= 600;
                PlayerPrefs.SetInt("Currency", curr);
                PlayerPrefs.Save();
                Magazynek1 = true;
                PlayerPrefs.SetInt("Scope1", Magazynek1 ? 1 : 0);
                curr = PlayerPrefs.GetInt("Currency");
            }
        }
        public void SelllMagazynek()
        {

            if (Magazynek1 == true)
            {
                curr += 50;
                PlayerPrefs.SetInt("Currency", curr);
                PlayerPrefs.Save();
                Magazynek1 = false;
                PlayerPrefs.SetInt("Scope1", Magazynek1 ? 1 : 0);
                curr = PlayerPrefs.GetInt("Currency");
            }

        }
    }
}