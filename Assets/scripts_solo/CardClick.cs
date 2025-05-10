using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardClick : MonoBehaviour
{
    public Image[] targetImages; // 투명도를 조절할 이미지 배열
    public float transparency = 0.5f; // 설정할 투명도 (0.0 투명 ~ 1.0 불투명)
    private float[] originalAlphas; // 각 이미지의 원래 알파 값
    void Start()
    {
        // 각 이미지의 원래 알파 값을 저장할 배열 초기화
        originalAlphas = new float[targetImages.Length];

        // 각 이미지에 클릭 이벤트 리스너 추가 및 원래 알파 값 저장
        for (int i = 0; i < targetImages.Length; i++)
        {
            originalAlphas[i] = targetImages[i].color.a;
            AddClickListener(targetImages[i], i);
        }
    }
    // 이미지를 클릭한 것을 알 수 있게 함
    void AddClickListener(Image image, int index)
    {
        EventTrigger trigger = image.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((eventData) => { ToggleTransparency(image, index); });
        trigger.triggers.Add(entry);
    }
    
    //다시 클릭했을 때 원래대로 돌아오게 하기
    void ToggleTransparency(Image image, int index)
    {
        // 이미지의 현재 색상을 가져옴
        Color color = image.color;
        // 현재 알파 값이 원래 알파 값인지 체크
        if (Mathf.Approximately(color.a, originalAlphas[index]))
        {
            // 알파 값을 투명도로 설정
            color.a = transparency;
        }
        else
        {
            // 알파 값을 원래 알파 값으로 되돌림
            color.a = originalAlphas[index];
        }
        // 변경된 색상 적용
        image.color = color;
    }
}
