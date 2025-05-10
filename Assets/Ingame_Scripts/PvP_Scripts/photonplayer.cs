using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class photonplayer : MonoBehaviourPun
{
    public Game game;
    PhotonView PV;
    NetworkManager NM;
    // Start is called before the first frame update
    void Start()
    {
        NM = GameObject.FindWithTag("NetworkManager").GetComponent<NetworkManager>();
        PV = photonView;
        game = GameObject.Find("Game").GetComponent<Game>();
    }
    public void GameStart()
    {
        PV.RPC("allcardset", RpcTarget.All);
        if (PhotonNetwork.IsMasterClient)
        {
            PV.RPC("ReceiveData", RpcTarget.Others, GameObject.Find("Game").GetComponent<Game>().playersCards[0].ToArray(),GameObject.Find("Game").GetComponent<Game>().playersCards[1].ToArray(),GameObject.Find("Game").GetComponent<Game>().boxcards.ToArray());
            PV.RPC("allsethand", RpcTarget.All);
        }
        
        PV.RPC("turnset", RpcTarget.All);
    }

    void Update()
    {
        PV.RPC("ReceiveEnemyNickname", RpcTarget.Others,GameObject.Find("Game").GetComponent<Game>().MyNickname);
    }

    public void GameStart2()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PV.RPC("GameStartRPC", RpcTarget.All);
        }
    }

    [PunRPC]
    public void GameStartRPC()
    {
        StartCoroutine(GameObject.Find("Game").GetComponent<Game>().GameStart());
    }

    // Update is called once per frame
    
    [PunRPC]
    public void ReceiveData(string[] receivedData1,string[] receivedData2,string[] receivedData3)
    {
        // 수신된 데이터를 리스트에 저장
        GameObject.Find("Game").GetComponent<Game>().GetComponent<Game>().playersCards[0] = (new List<string>(receivedData2));
        GameObject.Find("Game").GetComponent<Game>().GetComponent<Game>().playersCards[1] = (new List<string>(receivedData1));
        GameObject.Find("Game").GetComponent<Game>().GetComponent<Game>().boxcards = new List<string>(receivedData3);
    }
    [PunRPC]
    public void ReceiveEnemyNickname(string enemynickname)
    {
        // 수신된 데이터를 리스트에 저장
        GameObject.Find("Game").GetComponent<Game>().GetComponent<Game>().EnemyNickname = enemynickname;
    }

    [PunRPC]
    public void allsethand()
    {
        GameObject.Find("Game").GetComponent<Game>().allsethand();
    }
    [PunRPC]
    public void allcardset()
    {
        GameObject.Find("Game").GetComponent<Game>().CardSet();
    }
    [PunRPC]
    public void turnset()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            game.IsMyTurn = true;
            game.situation = "card_selecting";
        }
        else
        {
            game.IsMyTurn = false;
            game.situation = "card_selecting";
        }
    }

    [PunRPC]
    public void Card_selectRPC()
    {
        game.Card_select();
    }

    public void Card_select()
    {
        PV.RPC("Card_selectRPC", RpcTarget.All);
    }
    [PunRPC]
    public void rejectRPC()
    {
        game.reject();
    }
    public void reject()
    {
        PV.RPC("rejectRPC", RpcTarget.All);
    }
    [PunRPC]
    public void acceptRPC()
    {
        game.accept();
    }
    public void accept()
    {
        PV.RPC("acceptRPC", RpcTarget.All);
    }
    [PunRPC]
    public void SelectedCardUpdateRPC(string a)
    {
        game.selected_card = a;
    }
    public void SelectedCardUpdate(string a)
    {
        PV.RPC("SelectedCardUpdateRPC", RpcTarget.All, a);
    }
    [PunRPC]
    public void playbyRPC()
    {
        GameObject.Find("Game").GetComponent<Game>().Play.SetActive(false);
    }
    public void playby()
    {
        PV.RPC("playbyRPC", RpcTarget.All);
    }
}