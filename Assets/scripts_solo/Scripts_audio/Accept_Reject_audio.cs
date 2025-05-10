using UnityEngine;

public class AudioClipController : MonoBehaviour
{
    public AudioSource audioSource;  // AudioSource를 Inspector에 연결
    [SerializeField] [Range(0f, 1f)] private float volume = 1f;  // 볼륨을 Inspector에서 조절 가능
    [SerializeField] private float startTime = 0f;  // 시작 시간
    [SerializeField] private float endTime = 5f;    // 종료 시간 

    public bool isPlayingSegment = false;  // 현재 구간 재생 여부

    public void AudioStart()
    {
        // AudioSource의 볼륨을 설정
        audioSource.volume = volume;
        // Inspector에서 설정한 구간을 재생
        PlayAudioSegment();
        Debug.Log("오디오 실행 완료");
    }

    void Update()
    {
        // 오디오가 종료 시간에 도달하면 정지
        if (isPlayingSegment && audioSource.isPlaying && audioSource.time >= endTime)
        {
            audioSource.Stop();
            isPlayingSegment = false;
        }
    }

    // 오디오 클립의 특정 구간을 재생하는 함수
    public void PlayAudioSegment()
    {
        if (audioSource != null)
        {
            // AudioSource 컴포넌트가 비활성화된 경우 활성화
            if (!audioSource.enabled)
            {
                audioSource.enabled = true;
            }
            Debug.Log("특정구간 실행");
            audioSource.time = startTime;    // 시작 시간으로 이동
            audioSource.Play();         // 재생 시작
            isPlayingSegment = true;       // 구간 재생 중임을 표시
        }
    }

}
