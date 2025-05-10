using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using UnityEngine.SceneManagement;


public class Progress : MonoBehaviour
{
    [SerializeField]
    private Image       sliderProgress;
    [SerializeField]
    private Image       textProgressData;
    [SerializeField]
    private float       progressTime;       // 로딩바 재생 시간

    public void Play(UnityAction action = null)
    {
        StartCoroutine(OnProgress(action));
    }

    private IEnumerator OnProgress(UnityAction action)
    {
        float current = 0;
        float percent = 0;
//        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
//        op.allowSceneActivation = false;
//        if (sliderProgress.fillAmount >= op.progress)
                {
                    current = 0f;
                }
        while ( percent < 1)
        {
            current += Time.deltaTime;
            percent = current / progressTime;
//            sliderProgress.fillAmount = Mathf.Lerp(sliderProgress.fillAmount, op.progress, current);

            yield return null;
        }

        // action이 null이 아니면 action 메소드 실행
        action?.Invoke();
    }

    
}
