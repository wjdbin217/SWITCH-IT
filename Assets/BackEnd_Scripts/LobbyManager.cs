using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private TextMeshProUGUI textMessage;

    public GameObject playerPrefab;  // 플레이어 프리팹
    //private bool isPlayerAssigned = false; // 플레이어가 오브젝트에 할당되었는지 확인하는 플래그

    // 게임 버전 체크
    private readonly string gameVersion = "1";

    // 플레이 버튼
    public Button joinButton;


    //public TextMeshProUGUI userCount;
    public TextMeshProUGUI roomsCount;
    public TMP_InputField roomName;

    private List<RoomInfo> roomList = new List<RoomInfo>();
    private bool inLobby;

    public Transform content;
    public GameObject room;
    public GameObject lobby;

    private float currentTime = 0f;
    private float updateDelay = 2f;

    protected void SetMessage(string msg)
    {
        textMessage.text = msg;
    }

    // 게임 버전 체크 후 마스터 서버 연결 시도
    void Start()
    {
        PhotonNetwork.NickName = Backend.UserNickName;
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();

        joinButton.interactable = false;
        SetMessage("Connecting To Server...");

        Application.runInBackground = true;
        PhotonNetwork.JoinLobby();
//        UpdateLobby();
    }

    void Update()
    {
        if (inLobby)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= updateDelay)
            {
//                UpdateLobby();
                currentTime = 0.0f;
            }
        }
        //userCount.text = GetUserCount().ToString();
//        roomsCount.text = GetRoomsCount().ToString();
        // UpdateLobby();

    }

    // 연결 버튼 활성화, 연결 성공 메시지
    public override void OnConnectedToMaster()
    {
        joinButton.interactable = true;
        SetMessage("Online : Connected to Server");
    }

    // 연결 실패 시 버튼 비활성화, 이유 표시, 재연결 시도
    public override void OnDisconnected(DisconnectCause cause)
    {
        joinButton.interactable = false;
        SetMessage($"Offline : Connection Disabled {cause.ToString()} - Try reconnecting...");
        PhotonNetwork.ConnectUsingSettings();
    }

    public void Connect()
    {
        joinButton.interactable = false;

        if(PhotonNetwork.IsConnected)
        {
            Debug.Log("Connecting to Random Room...");
            SetMessage("Connecting to Random Room...");
            PhotonNetwork.JoinRandomRoom(); 
        }
        else
        {
            SetMessage($"Offline : Connection Disabled - Try reconnecting...");
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("There is no empty room, Creating new room...");
        SetMessage("There is no empty room, Creating new room...");
        PhotonNetwork.CreateRoom(null, new RoomOptions{MaxPlayers = 2});
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("There's room, Connected with Room");
        SetMessage("There's room, Connected with Room");
        PhotonNetwork.LoadLevel("PvPIngameScene");
    }

    public void GoToNextScene()
    {
        SceneManager.LoadScene("PvE_IngameScene");
    }

    // 로비 조인
    // private IEnumerator TryJoinLobby()
    // {
    //     if (!PhotonNetwork.IsConnectedAndReady)
    //     {
    //         Debug.Log("포톤 접속 시도");
    //         PhotonNetwork.ConnectUsingSettings();
    //         yield return new WaitForSeconds(0.5f);
    //     }
    //     else if (!inLobby)
    //     {
    //         Debug.Log("로비접속시도");
    //         PhotonNetwork.JoinLobby();
    //         yield return new WaitForSeconds(0.5f);
    //     }
    //     else
    //     {
    //         Debug.Log("접속 완료");
    //     }
    // }

    // // 로비 업데이트
    // private void UpdateLobby()
    // {
    //     Debug.Log("UpdateLobby() 호출됨"); // 메서드가 호출되는지 확인

    //     // 방 목록 삭제
    //     if(content == null)
    //     {
    //         foreach (Transform room_object in content)
    //         {
    //             Destroy(room_object.gameObject);
    //         }
    //     }
    //     List<LobbyRoom> room_list = GetRoomList();
    //     roomsCount.text = room_list.Count.ToString();
    //     // 방 목록 생성
    //     foreach (LobbyRoom lobby_room in room_list)
    //     {
    //         GameObject temp = Instantiate(room, content);
    //         RectTransform rectTransform = temp.GetComponent<RectTransform>();
    //         //Debug.Log($"{lobby_room.roomName} 방 추가됨.");
    //         temp.transform.GetChild(0).GetComponent<Text>().text = lobby_room.roomName;
    //         string count = string.Format("{0} / {1}", lobby_room.playerCount, lobby_room.maxPlayers);
    //         temp.transform.GetChild(1).GetComponent<Text>().text = count;
    //         temp.GetComponent<Button>().onClick.AddListener(delegate { ClickRoom(); });
    //         if (temp != null)
    //         {
    //             Debug.Log($"{lobby_room.roomName} 방이 Content 트랜스폼에 추가되었습니다.");
    //         }
    //         else
    //         {
    //             Debug.LogError("방 객체가 생성되지 않았습니다.");
    //         }
    //     }

    // }

    // public override void OnRoomListUpdate(List<RoomInfo> room_List)
    // {
    //     Debug.Log("OnRoomListUpdate() 호출됨"); // 메서드가 호출되는지 확인
    //     base.OnRoomListUpdate(room_List);
    //     roomList = room_List;
    //     Debug.Log($"OnRoomListUpdate() 호출됨, 방 개수: {roomList.Count}"); // 콜백 호출 확인
    //     UpdateLobby();
    // }


    // public bool CheckInLobby()
    // {
    //     return inLobby;
    // }

    // // 방수 가져오기
    // public int GetRoomsCount()
    // {
    //     return PhotonNetwork.CountOfRooms;
    // }

    // // 방 목록 가져오기
    // public List<LobbyRoom> GetRoomList()
    // {
    //     Debug.Log($"roomList에 {roomList.Count}개의 방이 있습니다."); // roomList의 개수 확인
    //     List<LobbyRoom> room_list = new List<LobbyRoom>();
    //     foreach (RoomInfo room_info in roomList)
    //     {
    //         LobbyRoom lobby_room = new LobbyRoom();
    //         //room options
    //         lobby_room.roomName = room_info.Name;
    //         lobby_room.maxPlayers = room_info.MaxPlayers;
    //         lobby_room.playerCount = room_info.PlayerCount;
    //         //lobby_room에 room_list 추가
    //         room_list.Add(lobby_room);
    //     }

    //     return room_list;
    // }


    // // 방 생성
    // public void CreateRoom(string room_Name)
    // {
    //     RoomOptions options = new RoomOptions();
    //     options.MaxPlayers = 2;
    //     options.IsVisible = true;
    //     PhotonNetwork.CreateRoom(room_Name, options);
    // }

    // public void GoToNextScene()
    // {
    //     PhotonNetwork.LoadLevel("IngameScene");
    // }

    // // public void GoToNextScene2()
    // // {
    // //     PhotonNetwork.LoadLevel("PvPLobby");
    // // }

    // // 방 최종 생성 클릭
    // public void ClickCreateRoom()
    // {
    //     CreateRoom(roomName.text);
    //     Invoke("GoToNextScene", 3.0f);
    //     Debug.Log($"{PhotonNetwork.NickName} entered");
    //     if (content == null)
    //     {
    //         Debug.Log("Content 트랜스폼이 할당되지 않았습니다.");
    //         return;
    //     }

    // }


    // // 방 클릭
    // public void ClickRoom()
    // {
    //     GameObject room_object = EventSystem.current.currentSelectedGameObject;
    //     string room_name = room_object.transform.GetChild(0).GetComponent<Text>().text;
    //     string[] room_count = room_object.transform.GetChild(1).GetComponent<Text>().text.Split('/');
    //     string player_count_str = room_count[0].Trim();
    //     string max_players_str = room_count[1].Trim();
    //     int player_count = int.Parse(player_count_str);
    //     int max_players = int.Parse(max_players_str);
    //     if (player_count >= max_players)
    //        RoomIsFull();
    //     else
    //     {
    //         JoinRoom(room_name);
    //     }
    // }

    // // 방 참가
    // public bool JoinRoom(string room_Name)
    // {
    //     return PhotonNetwork.JoinRoom(room_Name);
    // }



    // // 방이 가득참
    // public void RoomIsFull()
    // {
    //     SetMessage("방이 가득 찼습니다");
    // }

    // // 방 참가 실패
    // public void JoinFailed()
    // {
    //     SetMessage("방에 참가할 수 없습니다");
    // }
}