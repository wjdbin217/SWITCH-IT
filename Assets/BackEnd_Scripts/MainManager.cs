using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using BackEnd;


public class MainManager : MonoBehaviour
{
    void Start()
    {
        PhotonNetwork.NickName = Backend.UserNickName;
    }
    public void GoToNextScene()
    {
        SceneManager.LoadScene("IngameScene");
    }

    public void GoToNextScene2()
    {
        SceneManager.LoadScene("PvPLobby");
    }
}
