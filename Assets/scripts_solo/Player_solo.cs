using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.Collections;
using System.Linq;
public class Player_solo : MonoBehaviour
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



    // Ïπ¥Îìú Î∞∞Ïπò
    public void SetHand()
    {   
        StartCoroutine(SetHandCoroutine());
    }
    public IEnumerator SetHandCoroutine() //Ïπ¥Îìú Î∞∞Ïπò (?Éù?Ñ±?ï† ?ïå ?ï†?ãàÎ©îÏù¥?Öò ?ïåÎß? ÏΩîÎ£®?ã¥?úºÎ°? Î∞îÍøà)
    {
        Vector2 position = new Vector2(0,handPositionY);
        float position_x = -9 * handPositionX;
        foreach (string i in Game_solo.playersCards[view])
        {
            GameObject temp = Instantiate(card,transform);
            temp.transform.Find("Card").GetComponent<Button>().enabled = false;
            if (view == 0)
            {
                temp.GetComponent<CardCanvas_solo>().view = 0;
                temp.transform.Find("Card").GetComponent<Card_solo>().Front_Back = true;
            }
            else if (view == 1)
            {
                temp.GetComponent<CardCanvas_solo>().view = 1;
                temp.transform.Find("Card").GetComponent<Card_solo>().Front_Back = false;
            }
            temp.transform.Find("Card").GetComponent<RectTransform>().sizeDelta = Cardsize;
            temp.transform.Find("Card").GetComponent<Card_solo>().Cardtype = i;

            temp.transform.Find("Card").localScale = new Vector3(0.5f, 0.5f, 0.5f); // Ï≤òÏùå?óê ?ûëÍ≤? ?ãú?ûë?ï®.


            position_x +=handPositionX*2;
            position.x = position_x;
            temp.transform.localPosition = position;

            // ?öå?†Ñ ?ï†?ãàÎ©îÏù¥?Öò
            GameObject Card_obj = temp.transform.Find("Card").gameObject;  // Ï∫îÎ≤Ñ?ä§ ?ïà?óê card ?ò§Î∏åÏ†ù?ä∏
            StartCoroutine(RotateAndScaleAnimation(Card_obj));  // ?ï†?ãàÎ©îÏù¥?Öò ÏΩîÎ£®?ã¥ ?ã§?ñâ
            // temp.transform.Find("Card").GetComponent<RectTransform>().sizeDelta = Cardsize;                       // ?Å¨Í∏? Í≥†Ï†ï 

            string card_name = "Card_" + i;
            temp.transform.Find("Card").name =card_name;
            temp.name = card_name +"_canvas";
            Debug.Log(card_name+" Created at x:" + position_x.ToString());



            // Ïπ¥Îìú Î∞∞Ïπò ?ãú 0.1Ï¥? Ïß??ó∞
            yield return new WaitForSeconds(0.1f);
        }
        
    }


    // ?öå?†Ñ ?ï†?ãàÎ©îÏù¥?Öò ÏΩîÎ£®?ã¥
    private IEnumerator RotateAndScaleAnimation(GameObject card)
    {
        float duration = 0.5f;      // ?ï†?ãàÎ©îÏù¥?Öò Ïß??Üç ?ãúÍ∞?
        float elapsedTime = 0f;     // ?ï†?ãàÎ©îÏù¥?Öò?ù¥ ?ãú?ûë?êú ?õÑ Í≤ΩÍ≥º?êú ?ãúÍ∞ÑÏùÑ Ï∂îÏ†Å

        // Ïπ¥Îìú?ùò Ï¥àÍ∏∞ ?öå?†Ñ ?ÉÅ?Éú ????û•
        Quaternion initialRotation = card.transform.rotation;

        while (elapsedTime < duration)
        {
            // ?öå?†Ñ ?†Å?ö© ani
            float rotationAngle = Mathf.Lerp(0f, 360f, elapsedTime / duration);   // 0 ~ 360?èÑ
            Quaternion targetRotation = Quaternion.Euler(0f, rotationAngle, 0f);
            card.transform.rotation = initialRotation * targetRotation;  // Ï¥àÍ∏∞ Î∞©Ìñ• Í≥†Î†§?ï¥?Ñú ?Éù?Ñ±

            // ?Å¨Í∏? Ï¶ùÍ?? ani
            float scale = Mathf.Lerp(0f, 1f, elapsedTime / duration);   // Ï≤úÏ≤ú?ûà ?Å¨Í∏? Ï¶ùÍ??
            card.transform.localScale = new Vector3(scale, scale, scale);

            elapsedTime += Time.deltaTime;   // Í≤ΩÍ≥º ?ãúÍ∞? ?óÖ?ç∞?ù¥?ä∏
            yield return null;    // ?ã§?ùå ?îÑ?†à?ûÑÍπåÏ?? ???Í∏∞Ìï®.
        }
        card.transform.localScale = Vector3.one; // ÏµúÏ¢Ö ?Å¨Í∏? ?Ñ§?†ï
        card.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);    // ?öå?†Ñ?ùÑ Í≥†Ï†ï
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
                GameObject temp = Instantiate(GameObject.Find("Game").GetComponent<Game_solo>().card,GameObject.Find("Game").GetComponent<Game_solo>().maincanvas.transform);
                if(view == 0)
                {
                    temp.transform.localPosition = new Vector2(0,-220);
                }
                else if (view == 1)
                {
                    temp.transform.localPosition = new Vector2(0,220);
                }
                temp.GetComponent<Card_solo>().Front_Back = true;
                temp.GetComponent<Card_solo>().Cardtype = FairyTargetCard;
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
        float elapsedTime = 0f;  // Í≤ΩÍ≥º ?ãúÍ∞? Ï¥àÍ∏∞?ôî

        while (elapsedTime < slideDuration)
        {
            // Í≤ΩÍ≥º ?ãúÍ∞? ?óÖ?ç∞?ù¥?ä∏
            elapsedTime += Time.deltaTime;

            // ÏßÑÌñâ ÎπÑÏú® Í≥ÑÏÇ∞
            float progress = Mathf.Clamp01(elapsedTime / slideDuration);

            // ?ò§Î∏åÏ†ù?ä∏ ?úÑÏπòÎ?? Î∂??ìú?üΩÍ≤? Î≥¥Í∞Ñ
            a.transform.position = Vector2.Lerp(startPosition, targetPosition, progress);

            // ?ïú ?îÑ?†à?ûÑ ???Í∏? (?ã§?ùå ?îÑ?†à?ûÑ?óê?Ñú ?ã§?ãú ?ã§?ñâ)
            yield return null;
        }

        // Î™©Ìëú ?úÑÏπòÎ°ú ÏµúÏ¢Ö ?úÑÏπ? ?Ñ§?†ï (?†ï?ôï?ïòÍ≤? ?èÑÏ∞©ÌïòÍ≤? ?ïòÍ∏? ?úÑ?ï¥)
        a.transform.position = targetPosition;
        if (view == 0)
        {
            GameObject.Find("Game").GetComponent<Game_solo>().mef.SetActive(true);
            yield return new WaitForSecondsRealtime(1f);
            if (FairyType == "bf")
            {
                GameObject.Find("Game").GetComponent<Game_solo>().mebf.SetActive(true);
            }
            else if (FairyType == "rf")
            {
                GameObject.Find("Game").GetComponent<Game_solo>().merf.SetActive(true);
            }
            StartCoroutine(FairyRotateCard(GameObject.Find("FairyCard_Me"),FairychangeCard));
            yield return new WaitForSecondsRealtime(1f);
            if (FairyType == "bf")
            {
                GameObject.Find("Game").GetComponent<Game_solo>().mebf.SetActive(false);
            }
            else if (FairyType == "rf")
            {
                GameObject.Find("Game").GetComponent<Game_solo>().merf.SetActive(false);
            }
            yield return new WaitForSecondsRealtime(1f);
            GameObject.Find("Game").GetComponent<Game_solo>().mef.SetActive(false);
            for(int i=0; i<int.Parse(FairyTargetCard[2].ToString())*2; i++)
            {
                GameObject.Find("Me_score").GetComponent<Text>().text = (int.Parse(GameObject.Find("Me_score").GetComponent<Text>().text) + 1).ToString();
                GameObject.Find("Game").GetComponent<Game_solo>().Me.score += 1;
                yield return new WaitForSecondsRealtime(0.07f);
            }
        }
        else if (view == 1)
        {
            GameObject.Find("Game").GetComponent<Game_solo>().youf.SetActive(true);
            yield return new WaitForSecondsRealtime(1f);
            if (FairyType == "bf")
            {
                GameObject.Find("Game").GetComponent<Game_solo>().youbf.SetActive(true);
            }
            else if (FairyType == "rf")
            {
                GameObject.Find("Game").GetComponent<Game_solo>().yourf.SetActive(true);
            }
            StartCoroutine(FairyRotateCard(GameObject.Find("FairyCard_You"),FairychangeCard));
            yield return new WaitForSecondsRealtime(1f);
            if (FairyType == "bf")
            {
                GameObject.Find("Game").GetComponent<Game_solo>().youbf.SetActive(false);
            }
            else if (FairyType == "rf")
            {
                GameObject.Find("Game").GetComponent<Game_solo>().yourf.SetActive(false);
            }
            yield return new WaitForSecondsRealtime(1f);
            GameObject.Find("Game").GetComponent<Game_solo>().youf.SetActive(false);
            for(int i=0; i<int.Parse(FairyTargetCard[2].ToString())*2; i++)
            {
                GameObject.Find("You_score").GetComponent<Text>().text = (int.Parse(GameObject.Find("You_score").GetComponent<Text>().text) + 1).ToString();
                GameObject.Find("Game").GetComponent<Game_solo>().You.score += 1;
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
        elapsedTime = 0f;  // Í≤ΩÍ≥º ?ãúÍ∞? Ï¥àÍ∏∞?ôî

        while (elapsedTime < slideDuration)
        {
            // Í≤ΩÍ≥º ?ãúÍ∞? ?óÖ?ç∞?ù¥?ä∏
            elapsedTime += Time.deltaTime;

            // ÏßÑÌñâ ÎπÑÏú® Í≥ÑÏÇ∞
            float progress = Mathf.Clamp01(elapsedTime / slideDuration);

            // ?ò§Î∏åÏ†ù?ä∏ ?úÑÏπòÎ?? Î∂??ìú?üΩÍ≤? Î≥¥Í∞Ñ
            a.transform.position = Vector2.Lerp(targetPosition, startPosition, progress);

            // ?ïú ?îÑ?†à?ûÑ ???Í∏? (?ã§?ùå ?îÑ?†à?ûÑ?óê?Ñú ?ã§?ãú ?ã§?ñâ)
            yield return null;
        }
        a.transform.position = startPosition;
    }

    IEnumerator FairyRotateCard(GameObject a,string b)
    {
        Quaternion startRotation = a.transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0, 90, 0);

        float timeElapsed = 0;
        float duration = 0.15f; // ?öå?†Ñ?ù¥ ?ôÑÎ£åÎê† ?ïåÍπåÏ?? Í±∏Î¶¨?äî ?ãúÍ∞? (Ï¥?)

        while (timeElapsed < duration)
        {
            // ?òÑ?û¨ ?öå?†Ñ ?ÉÅ?Éú?óê?Ñú Î™©Ìëú ?öå?†Ñ ?ÉÅ?ÉúÎ°? Slerp
            a.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed / duration);

            // Í≤ΩÍ≥º ?ãúÍ∞? ?óÖ?ç∞?ù¥?ä∏
            timeElapsed += Time.deltaTime;

            // ?ïú ?îÑ?†à?ûÑ ???Í∏?
            yield return null;
        }
        if(view == 0)
        {
            GameObject.Find("FairyCard_Me").GetComponent<Card_solo>().Cardtype = b;
        }
        else if (view == 1)
        {
            GameObject.Find("FairyCard_You").GetComponent<Card_solo>().Cardtype = b;
        }
        a.transform.rotation = Quaternion.Euler(0, -90,0);
        startRotation = a.transform.rotation;
        targetRotation = Quaternion.Euler(0, 0, 0);
        timeElapsed = 0;
        while (timeElapsed < duration)
        {
            // ?òÑ?û¨ ?öå?†Ñ ?ÉÅ?Éú?óê?Ñú Î™©Ìëú ?öå?†Ñ ?ÉÅ?ÉúÎ°? Slerp
            a.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed / duration);

            // Í≤ΩÍ≥º ?ãúÍ∞? ?óÖ?ç∞?ù¥?ä∏
            timeElapsed += Time.deltaTime;

            // ?ïú ?îÑ?†à?ûÑ ???Í∏?
            yield return null;
        }


        // ?öå?†Ñ ?ôÑÎ£? ?õÑ ?†ï?ôï?ûà Î™©Ìëú ?öå?†Ñ?óê ÎßûÏ∂§
        a.transform.rotation = targetRotation;
    }
}
