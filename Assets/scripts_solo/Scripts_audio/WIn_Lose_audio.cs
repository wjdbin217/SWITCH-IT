using UnityEngine;
using UnityEngine.SceneManagement;


public class Win_Lose_audio : MonoBehaviour
{
    public AudioSource audioSource;  // AudioSource를 Inspector에 연결
    [SerializeField] [Range(0f, 1)] private float volume = 1f;  // 볼륨을 Inspector에서 조절 가능
    [SerializeField] private float startTime = 0f;  // 시작 시간
    [SerializeField] private float endTime = 5f;    // 종료 시간 

    public bool isPlayingSegment = false;  // 현재 구간 재생 여부

    void Start()
    {
        // AudioSource의 볼륨을 설정
        audioSource.volume = volume;
        // Inspector에서 설정한 구간을 재생
        PlayAudioSegment();
    }

    void Update()
    {
        // 오디오가 종료 시간에 도달하면 정지
        if (isPlayingSegment && audioSource.isPlaying && audioSource.time >= endTime)
        {
            audioSource.Stop();
            isPlayingSegment = false;
            // SceneManager.LoadScene("GameChooseScene"); // 마지막 이제 다시 홈화면으로
        }
    }

    // 오디오 클립의 특정 구간을 재생하는 함수
    public void PlayAudioSegment()
    {
        if (audioSource != null)
        {
            audioSource.time = startTime;    // 시작 시간으로 이동
            audioSource.Play();         // 재생 시작
            isPlayingSegment = true;       // 구간 재생 중임을 표시
        }
    }

}
