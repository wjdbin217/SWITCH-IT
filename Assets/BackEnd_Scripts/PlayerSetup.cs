using UnityEngine;
using Photon.Pun;

public class PlayerSetup : MonoBehaviour
{
    public Camera[] cameras;        // 각 플레이어에 할당될 카메라 배열
    public Canvas[] canvases;       // 각 플레이어에 할당될 캔버스 배열

    private void Start()
    {
        // 씬이 변경된 후 카메라와 캔버스 설정
        SetupPlayer();
    }

    private void SetupPlayer()
    {
        // 로컬 플레이어인지 확인
        // if (GetComponent<PhotonView>().IsMine)
        // {
        //     // 플레이어가 사용할 카메라와 캔버스 인덱스 결정 (네트워크 플레이어 ID 기반)
        //     int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;

        //     if (playerIndex >= 0 && playerIndex < cameras.Length && playerIndex < canvases.Length)
        //     {
        //         // 선택된 카메라와 캔버스 활성화
        //         cameras[playerIndex].enabled = true;
        //         canvases[playerIndex].enabled = true;
        //     }
        // }

            // 플레이어가 사용할 카메라와 캔버스 인덱스 결정 (네트워크 플레이어 ID 기반)
            int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;

            if (playerIndex >= 0 && playerIndex < cameras.Length && playerIndex < canvases.Length)
            {
                // 선택된 카메라와 캔버스 활성화
                cameras[playerIndex].enabled = true;
                canvases[playerIndex].enabled = true;
            }
    }
}