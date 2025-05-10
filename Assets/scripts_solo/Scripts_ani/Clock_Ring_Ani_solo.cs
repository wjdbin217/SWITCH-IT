using UnityEngine;

public class TimerAnimationController : MonoBehaviour
{
    public Animator circleAnimator;  // 원 애니메이터
    public Animator clockAnimator;   // 시계 애니메이터



    void Update()
    {
        // 원 애니메이션이 끝났는지 확인
        AnimatorStateInfo circleStateInfo = circleAnimator.GetCurrentAnimatorStateInfo(0);
        
        if (circleStateInfo.IsName("Clock_pink_start") && circleStateInfo.normalizedTime >= 1f)
        {
            // 원 애니메이션이 끝났을 때 시계 애니메이션 시작
            clockAnimator.SetTrigger("Ring_Clock");
        }
    }
}
