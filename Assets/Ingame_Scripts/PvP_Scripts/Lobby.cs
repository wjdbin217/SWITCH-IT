using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Lobby : MonoBehaviour
{
    public string mod;

    public TMP_Text userCount;
    public TMP_Text roomsCount;
    public TMP_InputField roomName;
    public GameStart gameStart;
    public Transform content;
    public GameObject room;
    public GameObject lobby;
    private bool inLobby;
    private float currentTime = 0f;
    private float updateDelay = 2f;

    public void multiset()
    {
        mod = "multi";
    }
    public void soloset()
    {
        mod = "solo";
    }
    public void playbutton()
    {
        if(mod=="multi")
        {
            GameObject.Find("PvE").GetComponent<btnToLogin>().OnclickPvE();
        }
        else if (mod == "solo")
        {
            GameObject.Find("LobbyScript").GetComponent<Lobby>().ClickNetworkPlay();
        }
        else
        {

        }
    }
    void Start()
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/Room");
    }

    void Update()
    {   
        if (inLobby)   // �κ� ���� ��
        {
            currentTime += Time.deltaTime;
            if (currentTime >= updateDelay)
            {
                UpdateLobby();
                currentTime = 0.0f;
            }
        }
        // NetworkManager.cs�� �Լ��� ������ ��.
        userCount.text = NetworkManager.instance.GetUserCount().ToString();
        roomsCount.text = NetworkManager.instance.GetRoomsCount().ToString();
    }

    // ��Ʈ��ũ �÷��� Ŭ��
    public void ClickNetworkPlay()
    {
        StartCoroutine(JoinLobby());
    }



    // �ݱ� ��ư�� Ŭ���ϸ� �޴� â�� ������ �Լ�.
    public void ClickCloseMenu(GameObject game_object)
    {
        game_object.SetActive(false);  // ������Ʈ�� ��Ȱ��ȭ �ؼ� �޴� â�� �ݴ´�.
        roomName.text = "";   // â�� ���� �� �� ���� â�� �� �̸� ���뵵 ���ش�.

        // if(game_object.name == "Lobby") //���� ������Ʈ �̸��� "Lobby"�̸� �� �ڽ� ������Ʈ �� "CreateOption" ������Ʈ�� ��Ȱ��ȭ �Ѵ�.
        // {
        //     inLobby = false;
        //     game_object.transform.Find("CreateOption").gameObject.SetActive(false);
        // }
    }

    // �� ���� Ŭ��
    public void ClickCreate()
    {
        lobby.transform.GetChild(5).gameObject.SetActive(true);
        
    } // �� ��ũ��Ʈ�� ��� �ִ� ������Ʈ�� 7��° �ڽ� ������Ʈ�� Ȱ��ȭ �Ѵ�. ����� â ����


    // �� ���� ���� Ŭ��(�� ���� â�� Create ��ư�� ����)
    public void ClickCreateRoom()
    {
        NetworkManager.instance.CreateRoom(roomName.text);
        gameStart.StartGame(2);
    }

    // �κ� �����ϴ� �Լ�
    private IEnumerator JoinLobby()
    {
        yield return StartCoroutine(NetworkManager.instance.JoinLobby());
        inLobby = NetworkManager.instance.CheckInLobby();
        
        if (inLobby)
        {
            lobby.SetActive(true);
        }
        else
        {
            GameManager.instance.FailedToConnect();
        }
    }


        // ���� ����
    public void ClickJoin()
    {
        bool joined = NetworkManager.instance.JoinRoom();
        if (joined)
            gameStart.StartGame(2);
        else
            GameManager.instance.JoinFailed();
    }

    // �� Ŭ��
    public void ClickRoom()
    {
        GameObject room_object = EventSystem.current.currentSelectedGameObject;
        string room_name = room_object.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
        string[] room_count = room_object.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text.Split('/');
        string player_count_str = room_count[0].Trim();
        string max_players_str = room_count[1].Trim();
        int player_count = int.Parse(player_count_str);
        int max_players = int.Parse(max_players_str);
        if (player_count >= max_players)
            GameManager.instance.RoomIsFull();
        else
        {
            bool joined = NetworkManager.instance.JoinRoom(room_name);
            if (joined)
                gameStart.StartGame(2);
            else
                GameManager.instance.JoinFailed();
        }
    }

    // �κ� ������Ʈ
    private void UpdateLobby()
    {
        // �� ��� ����
        foreach (Transform room_object in content)
        {
            Destroy(room_object.gameObject);
        }
        List<LobbyRoom> room_list = NetworkManager.instance.GetRoomList();
        roomsCount.text = room_list.Count.ToString();
        // �� ��� ����
        foreach (LobbyRoom lobby_room in room_list)
        {
            GameObject temp = Instantiate(room, content);
            temp.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = lobby_room.roomName;
            string count = string.Format("{0} / {1}", lobby_room.playerCount, lobby_room.maxPlayers);
            temp.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = count;
            temp.GetComponent<Button>().onClick.AddListener(delegate { ClickRoom(); });
        }
    }


}
