using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class CardCanvas : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int view;
    public Vector2 showdownposition;
    public float moveDuration = 2f;
    public Vector3 startPosition;

    public Vector2 startScale;
    public Vector2 targetScale;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if ((view==0) && (GameObject.Find("Game").GetComponent<Game>().IsMyTurn) && (GameObject.Find("Game").GetComponent<Game>().situation == "card_selecting"))
        {
            transform.GetChild(0).GetComponent<Card>().OnPointer1();
            transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    // 마우스가 UI에서 벗어났을 때
    public void OnPointerExit(PointerEventData eventData)
    {
        if (view==0)
        {
            transform.GetChild(0).GetComponent<Card>().OnPointer2();
            transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    public void Card_select()
    {
        GameObject.FindWithTag("Player").GetComponent<photonplayer>().Card_select();
        // if (GameObject.Find("Game").GetComponent<Game>().turn%2 ==1)
        // {
        //     GameObject.Find("Me").transform.Find("Card_" + GameObject.Find("Game").GetComponent<Game>().selected_card + "_canvas").transform.GetChild(0).GetComponent<Card>().OnPointer2();
        //     GameObject.Find("Me").transform.Find("Card_" + GameObject.Find("Game").GetComponent<Game>().selected_card + "_canvas").transform.GetChild(1).gameObject.SetActive(false);
        //     GameObject.Find("Me").transform.Find("Card_" + GameObject.Find("Game").GetComponent<Game>().selected_card + "_canvas").GetComponent<CardCanvas>().view = 1;
        //     GameObject.Find("Me").transform.Find("Card_" + GameObject.Find("Game").GetComponent<Game>().selected_card + "_canvas").GetComponent<CardCanvas>().StartMoving();
        //     GameObject.Find("Me").transform.Find("Card_" + GameObject.Find("Game").GetComponent<Game>().selected_card + "_canvas").transform.GetChild(0).GetComponent<Card>().Front_Back = false;
        //     //GameObject.Find("Me").transform.Find("Card_" + GameObject.Find("Game").GetComponent<Game>().selected_card + "_canvas").transform.GetChild(0).GetComponent<CanvasGroup>().alpha = 0f;
        //     GameObject.Find("Game").GetComponent<Game>().situation = "card_accepting";
        //     GameObject.Find("Game").GetComponent<Game>().showdown();
            
        // }
        // else
        // {
        //    // GameObject.Find("You").transform.Find("Card_" + GameObject.Find("Game").GetComponent<Game>().selected_card + "_canvas").transform.GetChild(0).GetComponent<CanvasGroup>().alpha = 0f;
        //     GameObject.Find("You").transform.Find("Card_" + GameObject.Find("Game").GetComponent<Game>().selected_card + "_canvas").GetComponent<CardCanvas>().StartMoving();
        //     GameObject.Find("Game").GetComponent<Game>().situation = "card_accepting";
        //     GameObject.Find("Game").GetComponent<Game>().showdown();
        //     StartCoroutine(Arpact());
        // }

        // // accept, reject 할 때 Timer 초기화
        // Animator Clock_ani = GameObject.Find("Timer_circle").GetComponent<Animator>();
        // Clock_ani.SetTrigger("Clock_pink_start");
    }


    public void StartMoving()
    {
        StartCoroutine(MoveToTarget());
        StartCoroutine(ScaleToTarget());
        
    }

    public void BackOrigin()
    {
        transform.position = startPosition;
        transform.localScale = startScale;
        transform.GetChild(0).GetComponent<CanvasGroup>().alpha = 0.5f;
    }

    private IEnumerator MoveToTarget()
    {

        startPosition = transform.position;  // 현재 위치 저장
        float elapsedTime = 0f;  // 경과 시간 초기화

        while (elapsedTime < moveDuration)
        {
            // 경과 시간 업데이트
            elapsedTime += Time.deltaTime;

            // 진행 비율 계산
            float progress = Mathf.Clamp01(elapsedTime / moveDuration);

            // 오브젝트 위치를 부드럽게 보간
            transform.position = Vector3.Lerp(startPosition, showdownposition, progress);

            // 한 프레임 대기 (다음 프레임에서 다시 실행)
            yield return null;
        }

        // 목표 위치로 최종 위치 설정 (정확하게 도착하게 하기 위해)
        transform.position = showdownposition;
    }

    private IEnumerator ScaleToTarget()
    {
        startScale = transform.localScale;  // 현재 크기 저장
       
        float elapsedTime = 0f;  // 경과 시간 초기화

        if (!GameObject.Find("Game").GetComponent<Game>().IsMyTurn)
        {
            while (elapsedTime < moveDuration)
            {
            // 경과 시간 업데이트
                elapsedTime += Time.deltaTime;

            // 진행 비율 계산
                float progress = Mathf.Clamp01(elapsedTime / moveDuration);

            // 오브젝트 크기를 부드럽게 보간
                transform.localScale = Vector3.Lerp(startScale, targetScale, progress);

            // 한 프레임 대기 (다음 프레임에서 다시 실행)
                yield return null;
            }

        // 목표 크기로 최종 설정 (정확하게 도착하게 하기 위해)
            transform.localScale = targetScale;
        }
        else
        {
            Vector2 targetScale2 = new Vector2((float)(targetScale.x*0.71),(float)(targetScale.y*0.71));
            while (elapsedTime < moveDuration)
            {
            // 경과 시간 업데이트
                elapsedTime += Time.deltaTime;

            // 진행 비율 계산
                float progress = Mathf.Clamp01(elapsedTime / moveDuration);

            // 오브젝트 크기를 부드럽게 보간
                transform.localScale = Vector3.Lerp(startScale, targetScale2, progress);

            // 한 프레임 대기 (다음 프레임에서 다시 실행)
                yield return null;
            }

        // 목표 크기로 최종 설정 (정확하게 도착하게 하기 위해)
            transform.localScale = targetScale2;
        }
        //GameObject.Find("Me").transform.Find("Card_" + GameObject.Find("Game").GetComponent<Game>().selected_card + "_canvas").transform.GetChild(0).GetComponent<Card>().Front_Back = false;
    }
}

