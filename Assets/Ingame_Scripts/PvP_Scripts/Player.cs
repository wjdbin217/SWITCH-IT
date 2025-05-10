using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.Collections;
using System.Linq;
public class Player : MonoBehaviour
{
    public string FairyTargetCard;
    public string FairychangeCard;
    public string FairyType;
    public int handPositionY;
    public float handPositionX;
    public GameObject card;
    public int view;
    public Vector2 Cardsize;
    public Vector2 Canvassize;
    public int score;
    public List<string> received_cards;

    public string playername;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   

    }



    // 카드 배치
    public void SetHand()
    {   
        StartCoroutine(SetHandCoroutine());
    }
    public IEnumerator SetHandCoroutine() //카드 배치 (?��?��?�� ?�� ?��?��메이?�� ?���? 코루?��?���? 바꿈)
    {
        Vector2 position = new Vector2(0,handPositionY);
        float position_x = -9 * handPositionX;
        foreach (string i in GameObject.Find("Game").GetComponent<Game>().playersCards[view])
        {
            GameObject temp = Instantiate(card,transform);
            temp.transform.Find("Card").GetComponent<Button>().enabled = false;
            if (view == 0)
            {
                temp.GetComponent<CardCanvas>().view = 0;
                temp.transform.Find("Card").GetComponent<Card>().Front_Back = true;
            }
            else if (view == 1)
            {
                temp.GetComponent<CardCanvas>().view = 1;
                temp.transform.Find("Card").GetComponent<Card>().Front_Back = false;
            }
            temp.transform.Find("Card").GetComponent<RectTransform>().sizeDelta = Cardsize;
            temp.transform.Find("Card").GetComponent<Card>().Cardtype = i;

            temp.transform.Find("Card").localScale = new Vector3(0.5f, 0.5f, 0.5f); // 처음?�� ?���? ?��?��?��.


            position_x +=handPositionX*2;
            position.x = position_x;
            temp.transform.localPosition = position;

            // ?��?�� ?��?��메이?��
            GameObject Card_obj = temp.transform.Find("Card").gameObject;  // 캔버?�� ?��?�� card ?��브젝?��
            StartCoroutine(RotateAndScaleAnimation(Card_obj));  // ?��?��메이?�� 코루?�� ?��?��
            // temp.transform.Find("Card").GetComponent<RectTransform>().sizeDelta = Cardsize;                       // ?���? 고정 

            string card_name = "Card_" + i;
            temp.transform.Find("Card").name =card_name;
            temp.name = card_name +"_canvas";
            Debug.Log(card_name+" Created at x:" + position_x.ToString());



            // 카드 배치 ?�� 0.1�? �??��
            yield return new WaitForSeconds(0.1f);
        }
        
    }


    // ?��?�� ?��?��메이?�� 코루?��
    private IEnumerator RotateAndScaleAnimation(GameObject card)
    {
        float duration = 0.5f;      // ?��?��메이?�� �??�� ?���?
        float elapsedTime = 0f;     // ?��?��메이?��?�� ?��?��?�� ?�� 경과?�� ?��간을 추적

        // 카드?�� 초기 ?��?�� ?��?�� ????��
        Quaternion initialRotation = card.transform.rotation;

        while (elapsedTime < duration)
        {
            // ?��?�� ?��?�� ani
            float rotationAngle = Mathf.Lerp(0f, 360f, elapsedTime / duration);   // 0 ~ 360?��
            Quaternion targetRotation = Quaternion.Euler(0f, rotationAngle, 0f);
            card.transform.rotation = initialRotation * targetRotation;  // 초기 방향 고려?��?�� ?��?��

            // ?���? 증�?? ani
            float scale = Mathf.Lerp(0f, 1f, elapsedTime / duration);   // 천천?�� ?���? 증�??
            card.transform.localScale = new Vector3(scale, scale, scale);

            elapsedTime += Time.deltaTime;   // 경과 ?���? ?��?��?��?��
            yield return null;    // ?��?�� ?��?��?��까�?? ???기함.
        }
        card.transform.localScale = Vector3.one; // 최종 ?���? ?��?��
        card.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);    // ?��?��?�� 고정
    }



    public void Fairyactive()
    {
        int sibal = 0;
        List<string> L = new List<string>();
        if (received_cards.Contains("bf") || received_cards.Contains("rf"))
        {
            if(received_cards.Contains("bf"))
            {
                FairyType = "bf";
            }
            else
            {
                FairyType = "rf";
            }
            for (int j = 0;j<received_cards.Count;j++)
            {
                if(received_cards[j][1].ToString() == "m")
                {
                    L.Add(received_cards[j].ToString());
                }
            }
            if(L != null && L.Any())
            {
                for (int j = 0; j<L.Count;j++)
                {
                    if (int.Parse(L[j][2].ToString()) > sibal)
                    {
                        sibal = int.Parse(L[j][2].ToString());
                        FairyTargetCard = L[j].ToString();
                    }
                }
                FairychangeCard = FairyTargetCard[0] + "p"+FairyTargetCard[2];
                
                // if (view == 0)
                // {
                //     GameObject.Find("Me_score").GetComponent<Text>().text =  score.ToString();
                // }
                // else if (view == 1)
                // {
                //     GameObject.Find("You_score").GetComponent<Text>().text = score.ToString();
                // }
                GameObject temp = Instantiate(GameObject.Find("Game").GetComponent<Game>().card,GameObject.Find("Game").GetComponent<Game>().maincanvas.transform);
                if(view == 0)
                {
                    temp.transform.localPosition = new Vector2(0,-220);
                }
                else if (view == 1)
                {
                    temp.transform.localPosition = new Vector2(0,220);
                }
                temp.GetComponent<Card>().Front_Back = true;
                temp.GetComponent<Card>().Cardtype = FairyTargetCard;
                temp.GetComponent<RectTransform>().sizeDelta = new Vector2(283.45f,396.14f);
                if(view == 0)
                {
                    temp.name = "FairyCard_Me";
                }
                else if (view == 1)
                {
                    temp.name = "FairyCard_You";
                }
                if(view == 0)
                {
                    StartCoroutine(FairySlide(GameObject.Find("FairyCard_Me"),-20));
                }
                else if (view == 1)
                {
                    StartCoroutine(FairySlide(GameObject.Find("FairyCard_You"),20));
                }
            }
        }
    }

    // public IEnumerator Fairyactive2()
    // {

    // }

    public IEnumerator FairySlide(GameObject a,float c,float b=0)
    {
        
        float slideDuration = 0.5f;
        Vector2 targetPosition = a.transform.position;
        Vector2 startPosition = new UnityEngine.Vector2(targetPosition.x,targetPosition.y+c);
        a.transform.position = startPosition;
        yield return new WaitForSecondsRealtime(b);
        float elapsedTime = 0f;  // 경과 ?���? 초기?��

        while (elapsedTime < slideDuration)
        {
            // 경과 ?���? ?��?��?��?��
            elapsedTime += Time.deltaTime;

            // 진행 비율 계산
            float progress = Mathf.Clamp01(elapsedTime / slideDuration);

            // ?��브젝?�� ?��치�?? �??��?���? 보간
            a.transform.position = Vector2.Lerp(startPosition, targetPosition, progress);

            // ?�� ?��?��?�� ???�? (?��?�� ?��?��?��?��?�� ?��?�� ?��?��)
            yield return null;
        }

        // 목표 ?��치로 최종 ?���? ?��?�� (?��?��?���? ?��착하�? ?���? ?��?��)
        a.transform.position = targetPosition;
        if (view == 0)
        {
            GameObject.Find("Game").GetComponent<Game>().mef.SetActive(true);
            yield return new WaitForSecondsRealtime(1f);
            if (FairyType == "bf")
            {
                GameObject.Find("Game").GetComponent<Game>().mebf.SetActive(true);
            }
            else if (FairyType == "rf")
            {
                GameObject.Find("Game").GetComponent<Game>().merf.SetActive(true);
            }
            StartCoroutine(FairyRotateCard(GameObject.Find("FairyCard_Me"),FairychangeCard));
            yield return new WaitForSecondsRealtime(1f);
            if (FairyType == "bf")
            {
                GameObject.Find("Game").GetComponent<Game>().mebf.SetActive(false);
            }
            else if (FairyType == "rf")
            {
                GameObject.Find("Game").GetComponent<Game>().merf.SetActive(false);
            }
            yield return new WaitForSecondsRealtime(1f);
            GameObject.Find("Game").GetComponent<Game>().mef.SetActive(false);
            for(int i=0; i<int.Parse(FairyTargetCard[2].ToString())*2; i++)
            {
                GameObject.Find("Me_score").GetComponent<Text>().text = (int.Parse(GameObject.Find("Me_score").GetComponent<Text>().text) + 1).ToString();
                GameObject.Find("Game").GetComponent<Game>().Me.score += 1;
                yield return new WaitForSecondsRealtime(0.07f);
            }
        }
        else if (view == 1)
        {
            GameObject.Find("Game").GetComponent<Game>().youf.SetActive(true);
            yield return new WaitForSecondsRealtime(1f);
            if (FairyType == "bf")
            {
                GameObject.Find("Game").GetComponent<Game>().youbf.SetActive(true);
            }
            else if (FairyType == "rf")
            {
                GameObject.Find("Game").GetComponent<Game>().yourf.SetActive(true);
            }
            StartCoroutine(FairyRotateCard(GameObject.Find("FairyCard_You"),FairychangeCard));
            yield return new WaitForSecondsRealtime(1f);
            if (FairyType == "bf")
            {
                GameObject.Find("Game").GetComponent<Game>().youbf.SetActive(false);
            }
            else if (FairyType == "rf")
            {
                GameObject.Find("Game").GetComponent<Game>().yourf.SetActive(false);
            }
            yield return new WaitForSecondsRealtime(1f);
            GameObject.Find("Game").GetComponent<Game>().youf.SetActive(false);
            for(int i=0; i<int.Parse(FairyTargetCard[2].ToString())*2; i++)
            {
                GameObject.Find("You_score").GetComponent<Text>().text = (int.Parse(GameObject.Find("You_score").GetComponent<Text>().text) + 1).ToString();
                GameObject.Find("Game").GetComponent<Game>().You.score += 1;
                yield return new WaitForSecondsRealtime(0.07f);
            }
        }
        yield return new WaitForSecondsRealtime(2);
        // if(view == 0)
        // {
        //     StartCoroutine(FairyRotateCard(GameObject.Find("FairyCard_Me"),FairychangeCard));
        // }
        // else if (view == 1)
        // {
        //     StartCoroutine(FairyRotateCard(GameObject.Find("FairyCard_You"),FairychangeCard));
        // }
        elapsedTime = 0f;  // 경과 ?���? 초기?��

        while (elapsedTime < slideDuration)
        {
            // 경과 ?���? ?��?��?��?��
            elapsedTime += Time.deltaTime;

            // 진행 비율 계산
            float progress = Mathf.Clamp01(elapsedTime / slideDuration);

            // ?��브젝?�� ?��치�?? �??��?���? 보간
            a.transform.position = Vector2.Lerp(targetPosition, startPosition, progress);

            // ?�� ?��?��?�� ???�? (?��?�� ?��?��?��?��?�� ?��?�� ?��?��)
            yield return null;
        }
        a.transform.position = startPosition;
    }

    IEnumerator FairyRotateCard(GameObject a,string b)
    {
        Quaternion startRotation = a.transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0, 90, 0);

        float timeElapsed = 0;
        float duration = 0.15f; // ?��?��?�� ?��료될 ?��까�?? 걸리?�� ?���? (�?)

        while (timeElapsed < duration)
        {
            // ?��?�� ?��?�� ?��?��?��?�� 목표 ?��?�� ?��?���? Slerp
            a.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed / duration);

            // 경과 ?���? ?��?��?��?��
            timeElapsed += Time.deltaTime;

            // ?�� ?��?��?�� ???�?
            yield return null;
        }
        if(view == 0)
        {
            GameObject.Find("FairyCard_Me").GetComponent<Card>().Cardtype = b;
        }
        else if (view == 1)
        {
            GameObject.Find("FairyCard_You").GetComponent<Card>().Cardtype = b;
        }
        a.transform.rotation = Quaternion.Euler(0, -90,0);
        startRotation = a.transform.rotation;
        targetRotation = Quaternion.Euler(0, 0, 0);
        timeElapsed = 0;
        while (timeElapsed < duration)
        {
            // ?��?�� ?��?�� ?��?��?��?�� 목표 ?��?�� ?��?���? Slerp
            a.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed / duration);

            // 경과 ?���? ?��?��?��?��
            timeElapsed += Time.deltaTime;

            // ?�� ?��?��?�� ???�?
            yield return null;
        }


        // ?��?�� ?���? ?�� ?��?��?�� 목표 ?��?��?�� 맞춤
        a.transform.rotation = targetRotation;
    }
}
