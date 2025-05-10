using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;    // 포톤 사용 시 필수
using Photon.Realtime;
using Unity.Properties;    // 포톤 방 생성 옵션
//using UnityEngine.UI;


public class NetworkManager : MonoBehaviourPunCallbacks // 포톤의 함수들을 오버라이딩 할 수 있게 됨.
{
    public static NetworkManager instance;  // GameManager 사용할때랑 같은 방법(싱글 패턴 이라고 부름.)
    // public TMP_Text userCount;
    private List<RoomInfo> roomList = new List<RoomInfo>(); // 여기에 방이 추가됨.
    private bool inLobby; // 현재 로비에 참여했는지에 대한 여부
    public photonplayer mine;

    PhotonView PV;

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);  // 씬 전환 시에도 지워지지 않도록 하기 위해서
    }

    // void Update()
    // {
    //     Debug.Log("현재 상태: " + PhotonNetwork.NetworkClientState);

    // }






    // Start is called before the first frame update
    void Start()
    {
        // PhotonNetwork.ConnectTimeout = 10000;  // 연결 대기 시간 10초
        // PhotonNetwork.DisconnectTimeout = 5000; // 끊어진 후 대기 시간 5초
        // PhotonNetwork.ServerTimeout = 15000;    // 서버 응답 대기 시간 15초
    }

    // 유저 수 가져오기
    public int GetUserCount()
    {
        return PhotonNetwork.CountOfPlayers;
    }

    // 방 수 가져오기
    public int GetRoomsCount()
    {
        return PhotonNetwork.CountOfRooms;
    }

    // 방 생성
    public void CreateRoom(string room_Name)
    {
        RoomOptions options = new RoomOptions(); // 방의 옵션을 가진 클래스임.
        options.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(room_Name, options); // 포톤 네트워크에서 방을 생성하는 함수.
    }

    // 로비 참가
    // public IEnumerator JoinLobby() // 4번 시도에 실패하면 포기
    // {
    //     int count = 0;
    //     while (!inLobby)
    //     {
    //         count++;
    //         yield return StartCoroutine(TryJoinLobby());
            
    //         if (count < 4)
    //         {
    //             break;
    //         }
    //     }
    // }
    public IEnumerator JoinLobby() // 4번 시도에 실패하면 포기
{
    int count = 0;

    // 로비에 진입 시도
    while (!inLobby)
    {
        count++;
        Debug.Log($"[JoinLobby] 로비 접속 시도: {count}회차");

        yield return StartCoroutine(TryJoinLobby());

        // 로비에 성공적으로 접속했는지 확인
        if (inLobby)
        {
            Debug.Log($"[JoinLobby] 로비에 성공적으로 접속했습니다. (시도 횟수: {count})");
            break;
        }
        else
        {
            Debug.Log($"[JoinLobby] 로비 접속 실패. (시도 횟수: {count})");
        }

        // 시도 횟수가 4회를 넘으면 반복 중단
        if (count >= 4)
        {
            Debug.Log($"[JoinLobby] 로비 접속을 4번 시도했지만 실패했습니다. 포기합니다.");
            break;
        }

        // 잠시 대기 (필요한 경우)
        yield return new WaitForSeconds(0.5f);
    }
}


    public bool CheckInLobby()  // 현재 로비에 있는지 확인
    {
        return inLobby;
    }

    // 랜덤 방 참가
    public bool JoinRoom()
    {
        PhotonNetwork.JoinRandomRoom(); // 랜덤 방에 참여(Photon)
        if (PhotonNetwork.InRoom)
            return true;
        else
            return false;
    }

    // 방 참가
    public bool JoinRoom(string room_Name)
    {
        return PhotonNetwork.JoinRoom(room_Name); // 특정 방에 참여(Photon)
    }


    // 방 목록 가져오기
    public List<LobbyRoom> GetRoomList()
    {
        List<LobbyRoom> room_list = new List<LobbyRoom>();
        foreach (RoomInfo room_info in roomList)
        {
            LobbyRoom lobby_room = new LobbyRoom();
            lobby_room.roomName = room_info.Name;
            lobby_room.maxPlayers = room_info.MaxPlayers;
            lobby_room.playerCount = room_info.PlayerCount;
            room_list.Add(lobby_room);
        }
        return room_list; // 방들의 정보에 해당하는 room_list 반환
    }




//  & PhotonNetwork.NetworkClientState == Photon.Realtime.ClientState.Disconnected



    // 포톤 서버 및 로비에 접속 시도
    private IEnumerator TryJoinLobby()
    {
        if (!PhotonNetwork.IsConnectedAndReady) // 이미 포톤에 접속되어 있는지 확인
        {
            Debug.Log("포톤 접속 시도");
            PhotonNetwork.ConnectUsingSettings();   // 포톤 네트워크 접속(Photon)
            // 연결 확인 디버깅
            if (PhotonNetwork.IsConnected)      
            {
                Debug.Log("Photon 네트워크에 연결되었습니다.");
            }
            else
            {
                Debug.LogWarning("Photon 네트워크에 연결되지 않았습니다.");
            }
            yield return new WaitForSeconds(0.5f);
        }
        else if (!inLobby)
        {
            Debug.Log("로비 접속 시도");
            PhotonNetwork.JoinLobby();       // 포톤 로비에 참여(Photon)
            yield return new WaitForSeconds(0.5f);
        }
    }
        // Photon에 연결 완료 후 로비 접속
    public override void OnConnectedToMaster()
    {
        Debug.Log("Photon 네트워크에 연결되었습니다. 로비 접속 시도 중...");
        PhotonNetwork.JoinLobby();
    }


    public override void OnConnected()
    {
        Debug.Log("연결 상태: " + PhotonNetwork.NetworkClientState);
    }


    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogError("Photon 서버 연결 실패: " + cause.ToString());
    }


    // MonoBehaviourPunCallbacks에서 오버라이딩한 함수
    public override void OnJoinedLobby() // 로비에 참가
    {
        base.OnJoinedLobby();
        inLobby = true;
        Debug.Log("로비에 참가했습니다.");
    }

    public override void OnLeftLobby()  // 로비를 떠남
    {
        base.OnLeftLobby();
        inLobby = false;
        Debug.Log("로비에서 나갔습니다.");
    }

    public override void OnCreatedRoom()  // 방 생성 완료
    {
        base.OnCreatedRoom();
        Debug.Log("방 생성 완료");
    }

    public override void OnCreateRoomFailed(short returnCode, string message) // 방 생성 실패
    {
        base.OnCreateRoomFailed(returnCode, message);
        Debug.LogError("방 생성 실패 : " + message);
    }

    // 방에 참가한 것이 확인 되면 바로 Player 생성
    public override void OnJoinedRoom() // 방에 참가
    {
        base.OnJoinedRoom();
        Debug.Log(PhotonNetwork.CurrentRoom.Name + " 방에 참가했습니다.");
        PlayerSpawn();
    }

    public void PlayerSpawn()
    {
        // 방에 들어온 플레이어 수를 확인
        int playerNumber = PhotonNetwork.CurrentRoom.PlayerCount;
        // 플레이어 프리팹 생성
        GameObject player = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
        // PhotonView 가져오기(player)
        PhotonView playerPV = player.GetComponent<PhotonView>();
        // 이름을 Player_1, Player_2 으로 지정하여 모든 클라이언트에 동기화
        // if (playerPV != null)
        // {
        //     string playerName = "Player_" + playerNumber;
        //     playerPV.RPC("SetPlayerName", RpcTarget.AllBuffered, playerName);  // 이름 동기화
        //     playerPV.RPC("SetParent", RpcTarget.AllBuffered);  // 캔버스의 자식 오브젝트로 위치
        // }
        Debug.Log("플레이어 생성 완료 : " + player.name);
        if (playerNumber == 2)
        {
            GameObject.Find("Game").GetComponent<Game>().realstart();
        }
    }




    public override void OnJoinRoomFailed(short returnCode, string message) // 방 참가 실패
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.LogError("방 참가에 실패했습니다. : " + message);
    }

    public override void OnJoinRandomFailed(short returnCode, string message) // 랜덤 방 참가에 실패
    {
        base.OnJoinRandomFailed(returnCode, message);
        Debug.LogError("랜덤 방 참가 실패했습니다. : " + message);
    }

    public override void OnLeftRoom()  // 방을 떠남
    {
        base.OnLeftRoom();
        Debug.Log("방에서 나갔습니다.");
    }

    public override void OnRoomListUpdate(List<RoomInfo> room_List)
    {
        base.OnRoomListUpdate(room_List);
        roomList = room_List;
        Debug.Log($"방 목록 업데이트: {roomList.Count}개의 방이 있습니다.");
    }
    public void FindPlayer()
    {
        foreach(GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if(player.GetPhotonView().IsMine)
            {

                mine = player.GetComponent<photonplayer>();
            }
        }
    }

    public void GameStart()
    {
        FindPlayer();
        mine.GameStart();
    }

    public void GameStart2()
    {
        FindPlayer();
        mine.GameStart2();
    }

    [PunRPC]
    public void SetPlayerName(string playerName)
    {
    // 플레이어 이름 설정 코드
    }




}




