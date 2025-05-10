using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    // 0 : AI, 1 : Hot Seat, 2: Network
    public int gameMode;
    public Transform canvas;
    public GameObject infoText;

    void Awake()
    {
        instance = this;
        gameMode = 0;
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 포톤 접속 실패
    public void FailedToConnect()
    {
        StartCoroutine(GameManager.instance.PrintMessage("서버 접속 실패"));
    }

    // 방이 가득 참
    public void RoomIsFull()
    {
        StartCoroutine(GameManager.instance.PrintMessage("방이 가득 찼습니다."));
    }

    // 방 참가 실패
    public void JoinFailed()
    {
        StartCoroutine(GameManager.instance.PrintMessage("방에 참가할 수 없습니다."));
    }
    
    // 화면에 1초간 메세지 출력
    private IEnumerator PrintMessage(string message)
    {
        GameObject temp = Instantiate(infoText, canvas);
        temp.GetComponent<TextMeshProUGUI>().text = message;
        temp.SetActive(true);
        yield return new WaitForSeconds(1f);
        Destroy(temp);
    }


}    
