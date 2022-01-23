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
    public class ProfileData
    {
        public string username;
        public int level;
        public int xp;
    }

    public class Launcher : MonoBehaviourPunCallbacks
    {
        public TMP_InputField usernameField;
        public static ProfileData myProfile = new ProfileData();







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

            Instance = this;
        }

        void Start()
        {
            Debug.Log("Connecting to Master");
            PhotonNetwork.ConnectUsingSettings();
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
    }
}