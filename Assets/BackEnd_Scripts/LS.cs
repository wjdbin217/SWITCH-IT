using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LS : MonoBehaviour
{
  

    [SerializeField]
    Image progressBar;
    [SerializeField]
    private SceneName nextScene;

    private void Start()
    {
        StartCoroutine(LoadScene());
    }

  

    IEnumerator LoadScene()
    {
        yield return null;

        AsyncOperation oper = SceneManager.LoadSceneAsync("LoginScene");
//LoadSceneAsync ("게임씬이름"); 입니다.
        oper.allowSceneActivation = false;
//allowSceneActivation 이 true가 되는 순간이 바로 다음 씬으로 넘어가는 시점입니다.

        float timer = 0.0f;
        while (!oper.isDone)
        {
            yield return null;

            timer += Time.deltaTime;

            if (oper.progress >= 0.9f)
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);
                //Image가 아니라 Slider의 경우 progressBar.value로 간단히 구현이 가능합니다만
               // 이미지가 찌그러진 것이 펴지는 것처럼 나오기 때문에 비추천하는 방법입니다.

                if (progressBar.fillAmount == 1.0f)
                    oper.allowSceneActivation = true;
            }
            else
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, oper.progress, timer);
                if (progressBar.fillAmount >= oper.progress)
                {
                    timer = 0f;
                }
            }
        }
    }
}
//[출처] 유니티 5 - 로딩씬 구현하기|작성자 러드

