using System.Collections;
using System.Collections.Generic;
using Photon.Pun.Demo.PunBasics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    
    }
    
    // public void StartGame()
    // {
    //     SceneManager.LoadScene("SingleLane");
    // }

    public void LoginMenu()
    {
        SceneManager.LoadScene("LoginMenu");
    }

    // 게임 시작  ----    0 : AI, 1 : Hot Seat(구현 안됨), 2 : Network 플레이

    // public void StartGame(int play_Mode)
    // {
    //     GameManager.instance.gameMode = play_Mode;
    //     SceneManager.LoadScene("SingleLane");
    // }

    public void StartGame(int play_Mode)
    {
        // gameMode 설정
        GameManager.instance.gameMode = play_Mode;
        // gameMode 설정 후 상태 출력
        Debug.Log("New gameMode set to : " + GameManager.instance.gameMode);
        // 씬 로드하기 전에 현재 씬을 출력
        Debug.Log("Loading scene : multi");
        // 씬 로드
        SceneManager.LoadScene("multi");
    }

}
