using UnityEngine;

public class Timer_audio : MonoBehaviour
{
    public Animator animator;
    public AudioSource audioSource;  // AudioSource를 Inspector에 연결
    [SerializeField] [Range(0f, 1)] private float volume = 1f;  // 볼륨을 Inspector에서 조절 가능
    [SerializeField] private float startTime = 0f;  // 시작 시간
    [SerializeField] private float endTime = 5f;    // 종료 시간 

    public bool isPlayingSegment = false;  // 현재 구간 재생 여부

    public void AudioStart()
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
        }

        // Animator의 현재 상태 확인
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        
        // 특정 애니메이션이 실행되고 있는지 확인
        if (stateInfo.IsName("Ring_Clock"))
        {
            // 애니메이션이 실행 중일 때 오디오를 시작
            if (!isPlayingSegment)
            {
                AudioStart(); // AudioStart 메서드 호출
            }
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
