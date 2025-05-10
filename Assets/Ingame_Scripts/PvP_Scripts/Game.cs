using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Photon.Pun.UtilityScripts;
using UnityEngine;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using TMPro;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{

    [SerializeField]
    private UserInfo    user;
    [SerializeField]
    private TextMeshProUGUI textNickname;
    [SerializeField]
    private TextMeshProUGUI textNickname_enemy;
    public float slideDuration = 2f;
    float revealcardposition = 2.7f;
    public GameObject card;
    public int turn;
    public Player Me;
    public Player You;
    public Unrev unrev;
    public string selected_card;
    public string situation;
    public GameObject maincanvas;
    public GameObject arpa;
    public GameObject revealcanvas;
    public bool IsMyTurn;
    public GameObject bg1;
    public GameObject unrevbg;
    public GameObject ap;
    public GameObject rp;
    public GameObject win;
    public GameObject lose;
    public GameObject GameOver;
    public GameObject myturn;
    public GameObject youturn;
    public GameObject mef;
    public GameObject youf;
    public GameObject merf;
    public GameObject mebf;
    public GameObject yourf;
    public GameObject youbf;
    public GameObject Play;
    public string MyNickname;
    public string EnemyNickname = "";

    private List<string> OriginCardsList = new List<string>() {
        "rm1","rp1","bm1","bp1",
        "rm2","rp2","rf","bm2","bp2",
        "rm3","rp3","bm3","bp3",
        "rm4","rp4","bm4","bp4",
        "rm5","rp5","bm5","bp5",
        "rm6","rp6","bf","bm6","bp6",
        "rm7","rp7","bm7","bp7",};

    public List<List<string>> playersCards;
    public List<string> boxcards;

    void Start()
    {

        // GameStart();
        Application.targetFrameRate = 30;
        IsMyTurn = true;
        turn = 1;
        situation = "card_selecting";
        StartCoroutine(GameStart());
    }

    public void realstart()
    {
        GameObject.FindWithTag("NetworkManager").GetComponent<NetworkManager>().GameStart();
    }
    public void CardSet() // ?���? 배치
    {
        List<string> Cards = new List<string>(OriginCardsList);
        List<List<string>> playersCards_ = new List<List<string>>();
        boxcards = new List<string>();

        for (int i = 0; i < 2; i++)
        {
            playersCards_.Add(new List<string>());
        }

        playersCards = new List<List<string>>();

        for (int i = 0; i < 2; i++)
        {
            playersCards.Add(new List<string>());
        }

        Shuffle(Cards);

        for (int i = 0; i<2; i ++)
        {
            for (int j =0; j<8;j++)
            {
                playersCards_[i].Add(Cards[i*8 + j]);
            }
        }
        for (int i = 16; i<=29; i++)
        {
            boxcards.Add(Cards[i]);
        }

        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 30; j++)
            {
                if (playersCards_[i].Contains(OriginCardsList[j]))
                {
                    if (!playersCards[i].Contains(OriginCardsList[j]))
                    {
                        playersCards[i].Add(OriginCardsList[j]);
                    }
                }
            }
        }

        for (int i = 0; i < 2; i++)
        {
            Debug.Log($"Player {i + 1} cards: {string.Join(", ", playersCards[i])}");
        }
        Debug.Log($"box cards: {string.Join(", ", boxcards)}");
        // Me.SetHand();
        // You.SetHand();
        // unrev.SetHand();
        myturn.SetActive(true);
    }

    public void allsethand()
    {
        Me.SetHand();
        You.SetHand();
        unrev.SetHand();
    }

    void Shuffle(List<string> list) //카드 ?���?
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(i, list.Count); // i�??�� list.Count ?��?��?��?�� 무작?�� ?��?��?�� ?��?��
            string temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    // public void Card_select()
    // {
    //     Me.transform.Find("Card_" + selected_card + "_canvas").GetComponent<CardCanvas>().Card_select();
    // }

    public void turn_pass()
    {
        // 민�???�� ?��?�� Timer ?��?��메이?��^^
        // ?�� ?���? ?��마다 ?��?��
        Animator Clock_ani = GameObject.Find("Timer_circle").GetComponent<Animator>();
        Clock_ani.SetTrigger("Clock_pink_start");


        string RevealCard;
        turn += 1;
        if (IsMyTurn)
        {
            IsMyTurn = false;
            myturn.SetActive(false);
            youturn.SetActive(true);
        }
        else
        {
            IsMyTurn = true;
            myturn.SetActive(true);
            youturn.SetActive(false);
        }
        if (turn==17)
        {
            StartCoroutine(GameEnd());
        }
        //situation = "card_selecting";
        if (turn%2 == 1 && turn!= 17)
        {
            RevealCard = GameObject.Find("UnrevealedCard").transform.GetChild(GameObject.Find("UnrevealedCard").transform.childCount -1).name;
            Destroy(GameObject.Find("UnrevealedCard").transform.GetChild(GameObject.Find("UnrevealedCard").transform.childCount -1).gameObject);
            GameObject temp1 = Instantiate(card,revealcanvas.transform);
            temp1.transform.localPosition = new Vector2((float)-0.32,revealcardposition);
            temp1.GetComponent<Card>().Front_Back = true;
            temp1.GetComponent<Card>().Cardtype = RevealCard;
            temp1.GetComponent<RectTransform>().sizeDelta = new Vector2((float)0.6222, (float)0.869);
            temp1.name = "revealed_card_";
            RevealCard = GameObject.Find("UnrevealedCard").transform.GetChild(GameObject.Find("UnrevealedCard").transform.childCount -2).name;
            Destroy(GameObject.Find("UnrevealedCard").transform.GetChild(GameObject.Find("UnrevealedCard").transform.childCount -2).gameObject);
            GameObject temp2 = Instantiate(card,revealcanvas.transform);
            temp2.transform.localPosition = new Vector2((float)0.58,revealcardposition);
            temp2.GetComponent<Card>().Front_Back = true;
            temp2.GetComponent<Card>().Cardtype = RevealCard;
            temp2.GetComponent<RectTransform>().sizeDelta = new Vector2((float)0.6222, (float)0.869);
            temp2.name = "revealed_card_";
            revealcardposition -= 0.9f;
        }
    }

    public void showdown()
    {
        // GameObject temp = Instantiate(card,maincanvas.transform);
        // temp.transform.localPosition = new Vector2(-146,35);
        // temp.GetComponent<Card>().Front_Back = false;
        // temp.GetComponent<Card>().Cardtype = selected_card;
        // temp.GetComponent<RectTransform>().sizeDelta = new Vector2(189,264);
        // temp.name = "ShowdownCard";
    }

    public void accept()
    {
        //arpaunactive();
        if (IsMyTurn)
        {
            StartCoroutine(RotateObject(GameObject.Find("Me").transform.Find("Card_" + GameObject.Find("Game").GetComponent<Game>().selected_card + "_canvas").gameObject));
            GameObject.Find("Me").transform.Find("Card_" + GameObject.Find("Game").GetComponent<Game>().selected_card + "_canvas").transform.GetChild(0).GetComponent<Card>().Front_Back = true;
            StartCoroutine(destroyshowdown("Me"));
            ap.SetActive(true);
        }
        else
        {
            StartCoroutine(RotateObject(GameObject.Find("You").transform.Find("Card_" + GameObject.Find("Game").GetComponent<Game>().selected_card + "_canvas").gameObject));
            GameObject.Find("You").transform.Find("Card_" + GameObject.Find("Game").GetComponent<Game>().selected_card + "_canvas").transform.GetChild(0).GetComponent<Card>().Front_Back = true;
            StartCoroutine(destroyshowdown("You"));
        }
        if(!IsMyTurn)
        {
            Me.received_cards.Add(selected_card);
            if (selected_card[1].ToString() == "m")
            {
                // GameObject.Find("Me_score").GetComponent<Text>().text = (int.Parse(GameObject.Find("Me_score").GetComponent<Text>().text) - int.Parse(selected_card[2].ToString())).ToString();
                // Me.score -= int.Parse(selected_card[2].ToString());
                StartCoroutine(ScoreCountMeMinus());
            }
            else if (selected_card[1].ToString() == "p")
            {
                // GameObject.Find("Me_score").GetComponent<Text>().text = (int.Parse(GameObject.Find("Me_score").GetComponent<Text>().text) + int.Parse(selected_card[2].ToString())).ToString();
                // Me.score += int.Parse(selected_card[2].ToString());
                StartCoroutine(ScoreCountMEPlus());
            }
            //bg1.SetActive(true);
            StartCoroutine(SelectPopUpClose());
        }
        else
        {
            You.received_cards.Add(selected_card);
            if (selected_card[1].ToString() == "m")
            {
                // GameObject.Find("You_score").GetComponent<Text>().text = (int.Parse(GameObject.Find("You_score").GetComponent<Text>().text) - int.Parse(selected_card[2].ToString())).ToString();
                // You.score -= int.Parse(selected_card[2].ToString());
                StartCoroutine(ScoreCountYouMinus());
            }
            else if (selected_card[1].ToString() == "p")
            {
                // GameObject.Find("You_score").GetComponent<Text>().text = (int.Parse(GameObject.Find("You_score").GetComponent<Text>().text) + int.Parse(selected_card[2].ToString())).ToString();
                // You.score += int.Parse(selected_card[2].ToString());
                StartCoroutine(ScoreCountYouPlus());
            }
            //bg1.SetActive(false);
            //StartCoroutine(SelectPopUpClose());
        }
        turn_pass();
    }

    public void reject()
    {
        //arpaunactive();
        if (IsMyTurn)
        {
            StartCoroutine(RotateObject(GameObject.Find("Me").transform.Find("Card_" + GameObject.Find("Game").GetComponent<Game>().selected_card + "_canvas").gameObject));
            GameObject.Find("Me").transform.Find("Card_" + GameObject.Find("Game").GetComponent<Game>().selected_card + "_canvas").transform.GetChild(0).GetComponent<Card>().Front_Back = true;
            StartCoroutine(destroyshowdown("Me"));
            rp.SetActive(true);
        }
        else
        {
            StartCoroutine(RotateObject(GameObject.Find("You").transform.Find("Card_" + GameObject.Find("Game").GetComponent<Game>().selected_card + "_canvas").gameObject));
            GameObject.Find("You").transform.Find("Card_" + GameObject.Find("Game").GetComponent<Game>().selected_card + "_canvas").transform.GetChild(0).GetComponent<Card>().Front_Back = true;
            StartCoroutine(destroyshowdown("You"));
        }
        if(IsMyTurn)
        {
            Me.received_cards.Add(selected_card);
            if (selected_card[1].ToString() == "m")
            {
                // GameObject.Find("Me_score").GetComponent<Text>().text = (int.Parse(GameObject.Find("Me_score").GetComponent<Text>().text) - int.Parse(selected_card[2].ToString())).ToString();
                // Me.score -= int.Parse(selected_card[2].ToString());
                StartCoroutine(ScoreCountMeMinus());
            }
            else if (selected_card[1].ToString() == "p")
            {
                // GameObject.Find("Me_score").GetComponent<Text>().text = (int.Parse(GameObject.Find("Me_score").GetComponent<Text>().text) + int.Parse(selected_card[2].ToString())).ToString();
                // Me.score += int.Parse(selected_card[2].ToString());
                StartCoroutine(ScoreCountMEPlus());
            }
            //bg1.SetActive(false);
        }
        else
        {
            You.received_cards.Add(selected_card);
            if (selected_card[1].ToString() == "m")
            {
                // GameObject.Find("You_score").GetComponent<Text>().text = (int.Parse(GameObject.Find("You_score").GetComponent<Text>().text) - int.Parse(selected_card[2].ToString())).ToString();
                // You.score -= int.Parse(selected_card[2].ToString());
                StartCoroutine(ScoreCountYouMinus());
            }
            else if (selected_card[1].ToString() == "p")
            {
                // GameObject.Find("You_score").GetComponent<Text>().text = (int.Parse(GameObject.Find("You_score").GetComponent<Text>().text) + int.Parse(selected_card[2].ToString())).ToString();
                // You.score += int.Parse(selected_card[2].ToString());
                StartCoroutine(ScoreCountYouPlus());
            }
            //bg1.SetActive(true);
            StartCoroutine(SelectPopUpClose());
        }
        turn_pass();
    }

    public IEnumerator SelectPopUpClose()
    {
        if(!IsMyTurn)
        {
            StartCoroutine(RotateObject2(GameObject.Find("ShowdownCard")));
        }
        yield return new WaitForSecondsRealtime(1.5f);
        arpaunactive();
        bg1.SetActive(true);
        Destroy(GameObject.Find("ShowdownCard"));
    }

    public void arpaactive()
    {
        arpa.SetActive(true);
        bg1.SetActive(false);
        unrevbg.SetActive(false);
        ap.SetActive(false);

        rp.SetActive(false);
    }

    public void arpaunactive()
    {
        arpa.SetActive(false);
        unrevbg.SetActive(true);
    }

    public IEnumerator GameEnd()
    {
        yield return new WaitForSecondsRealtime(3);
        GameOver.SetActive(true);
        yield return new WaitForSecondsRealtime(2);
        GameOver.SetActive(false);
        Me.Fairyactive();
        You.Fairyactive();
        if (Me.received_cards.Contains("bf") || Me.received_cards.Contains("rf"))
        {
            for (int j = 0;j<Me.received_cards.Count;j++)
            {
                if(Me.received_cards[j][1].ToString() == "m")
                {
                    yield return new WaitForSecondsRealtime(6.5f);
                    break;
                }
            }
        }
        else if (You.received_cards.Contains("bf") || You.received_cards.Contains("rf"))
        {
            for (int j = 0;j<You.received_cards.Count;j++)
            {
                if(You.received_cards[j][1].ToString() == "m")
                {
                    yield return new WaitForSecondsRealtime(7);
                    break;
                }
            }
        }
        if (You.score > Me.score)
        {
            lose.SetActive(true);
        }
        else
        {
            win.SetActive(true);
        }
        yield return new WaitForSecondsRealtime(4);
        SceneManager.LoadScene("GameChooseScene");
    }

    public void  GameEndhi()
    {
        StartCoroutine(GameEnd());
    }


    IEnumerator destroyshowdown(string a)
    {
        yield return new WaitForSecondsRealtime(1);
        GameObject.Find(a).transform.Find("Card_" + GameObject.Find("Game").GetComponent<Game>().selected_card + "_canvas").GetComponent<CardCanvas>().BackOrigin();
        situation = "card_selecting";
    }

    public IEnumerator GameStartSlide(GameObject a,float b=0)
    {
        // 민�???��?�� ?��?�� ?�� ?��?��립트^^
        Animator animator = a.GetComponent<Animator>();
        if (animator != null)
        {
            animator.enabled = false;    // ?���? ?��?��메이?�� 비활?��?��
        }

        Vector2 targetPosition = a.transform.position;
        Vector2 startPosition = new UnityEngine.Vector2(targetPosition.x+20,targetPosition.y);
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


        // 민�???�� ?��.?��        Animator ?��?�� ?��?��?��?���?   
        if (animator != null)
        {
            animator.enabled = true;
        }
    }

    public IEnumerator GameStart()
    {
        StartCoroutine(GameStartSlide(GameObject.Find("Timer"),0.9f));
        StartCoroutine(GameStartSlide(GameObject.Find("Me_background"),0.9f));
        StartCoroutine(GameStartSlide(GameObject.Find("You_background"),0.9f));
        StartCoroutine(GameStartSlide(GameObject.Find("YourProfile"),(float)1.4));
        StartCoroutine(GameStartSlide(GameObject.Find("MyProfile"),(float)1.4));
        StartCoroutine(GameStartSlide(GameObject.Find("UnrevealedCard_background"),(float)1.4));
        StartCoroutine(GameStartSlide(GameObject.Find("Me_score"),(float)1.6));
        StartCoroutine(GameStartSlide(GameObject.Find("You_score"),(float)1.6));
        StartCoroutine(GameStartSlide(GameObject.Find("Checkbox"),1.9f));
        yield return new WaitForSecondsRealtime(2.6f);
        //GameObject.FindWithTag("NetworkManager").GetComponent<NetworkManager>().GameStart();

        // 민�???�� ?��?�� Timer ?��?��메이?��^^
        Animator Clock_ani = GameObject.Find("Timer_circle").GetComponent<Animator>();
        Clock_ani.SetTrigger("Clock_pink_start");

        
    }

    public void ShowdownCardset()
    {
        GameObject temp = Instantiate(card,maincanvas.transform);
        temp.transform.localPosition = new Vector2(-1,81);
        temp.GetComponent<Card>().Front_Back = false;
        temp.GetComponent<Card>().Cardtype = selected_card;
        temp.GetComponent<RectTransform>().sizeDelta = new Vector2(244,341);
        temp.name = "ShowdownCard";
    }

    IEnumerator ScoreCountMeMinus()
    {
        //?�� ?��?���? - ?��?�� 코루?��

        for(int i=0; i<int.Parse(selected_card[2].ToString()); i++)
        {
            GameObject.Find("Me_score").GetComponent<Text>().text = (int.Parse(GameObject.Find("Me_score").GetComponent<Text>().text) - 1).ToString();
            Me.score -= 1;
            yield return new WaitForSecondsRealtime(0.07f);
        }
    }

    IEnumerator ScoreCountMEPlus()
    {
        //?�� ?��?���? + ?��?�� 코루?��

        for(int i=0; i<int.Parse(selected_card[2].ToString()); i++)
        {
            GameObject.Find("Me_score").GetComponent<Text>().text = (int.Parse(GameObject.Find("Me_score").GetComponent<Text>().text) + 1).ToString();
            Me.score += 1;
            yield return new WaitForSecondsRealtime(0.07f);
        }
    }

    IEnumerator ScoreCountYouPlus()
    {
        //?��??? ?��?���? + ?��?�� 코루?��

        for(int i=0; i<int.Parse(selected_card[2].ToString()); i++)
        {
            GameObject.Find("You_score").GetComponent<Text>().text = (int.Parse(GameObject.Find("You_score").GetComponent<Text>().text) + 1).ToString();
            You.score += 1;
            yield return new WaitForSecondsRealtime(0.07f);
        }
    }

    IEnumerator ScoreCountYouMinus()
    {
        //?��??? ?��?���? - ?��?�� 코루?��

        for(int i=0; i<int.Parse(selected_card[2].ToString()); i++)
        {
            GameObject.Find("You_score").GetComponent<Text>().text = (int.Parse(GameObject.Find("You_score").GetComponent<Text>().text) - 1).ToString();
            You.score -= 1;
            yield return new WaitForSecondsRealtime(0.07f);
        }
    }

    // public IEnumerator Cardopen()
    // {
    //     yield new WaitForSecondsRealtime(1);
    //     GameObject.Find("Me").transform.Find("Card_" + GameObject.Find("Game").GetComponent<Game>().selected_card + "_canvas").transform.GetChild(0).GetComponent<Card>().Front_Back = true;
    // }

    IEnumerator RotateObject(GameObject a)
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
        if(IsMyTurn)
        {
            GameObject.Find("You").transform.Find("Card_" + GameObject.Find("Game").GetComponent<Game>().selected_card + "_canvas").transform.GetChild(0).GetComponent<Card>().Front_Back = true;
        }
        else
        {
            GameObject.Find("Me").transform.Find("Card_" + GameObject.Find("Game").GetComponent<Game>().selected_card + "_canvas").transform.GetChild(0).GetComponent<Card>().Front_Back = true;
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

    IEnumerator RotateObject2(GameObject a)
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
            GameObject.Find("ShowdownCard").GetComponent<Card>().Front_Back = true;
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

    private void Awake()
    {
        user.GetUserInfoFromBackend();
        UpdateNIckname();
    }

    public void UpdateNIckname()
    {
        
        MyNickname = UserInfo.Data.nickname;
    }
    
    public void PlayerName()
    {
        
        UpdateNIckname();
        textNickname.text = UserInfo.Data.nickname;
    }

    public void Card_select()
    {
        if (GameObject.Find("Game").GetComponent<Game>().IsMyTurn)
        {
            GameObject.Find("Me").transform.Find("Card_" + GameObject.Find("Game").GetComponent<Game>().selected_card + "_canvas").transform.GetChild(0).GetComponent<Card>().OnPointer2();
            GameObject.Find("Me").transform.Find("Card_" + GameObject.Find("Game").GetComponent<Game>().selected_card + "_canvas").transform.GetChild(1).gameObject.SetActive(false);
            GameObject.Find("Me").transform.Find("Card_" + GameObject.Find("Game").GetComponent<Game>().selected_card + "_canvas").GetComponent<CardCanvas>().view = 1;
            GameObject.Find("Me").transform.Find("Card_" + GameObject.Find("Game").GetComponent<Game>().selected_card + "_canvas").GetComponent<CardCanvas>().StartMoving();
            GameObject.Find("Me").transform.Find("Card_" + GameObject.Find("Game").GetComponent<Game>().selected_card + "_canvas").transform.GetChild(0).GetComponent<Card>().Front_Back = false;
            //GameObject.Find("Me").transform.Find("Card_" + GameObject.Find("Game").GetComponent<Game>().selected_card + "_canvas").transform.GetChild(0).GetComponent<CanvasGroup>().alpha = 0f;
            GameObject.Find("Game").GetComponent<Game>().situation = "card_accepting";
            GameObject.Find("Game").GetComponent<Game>().showdown();
            
        }
        else
        {
           // GameObject.Find("You").transform.Find("Card_" + GameObject.Find("Game").GetComponent<Game>().selected_card + "_canvas").transform.GetChild(0).GetComponent<CanvasGroup>().alpha = 0f;
            GameObject.Find("You").transform.Find("Card_" + GameObject.Find("Game").GetComponent<Game>().selected_card + "_canvas").GetComponent<CardCanvas>().StartMoving();
            GameObject.Find("Game").GetComponent<Game>().situation = "card_accepting";
            GameObject.Find("Game").GetComponent<Game>().showdown();
            StartCoroutine(Arpact());
        }

        // accept, reject 할 때 Timer 초기화
        Animator Clock_ani = GameObject.Find("Timer_circle").GetComponent<Animator>();
        Clock_ani.SetTrigger("Clock_pink_start");
    }

    public void acceptRPC()
    {
        GameObject.FindWithTag("Player").GetComponent<photonplayer>().accept();
    }
    public void rejectRPC()
    {
        GameObject.FindWithTag("Player").GetComponent<photonplayer>().reject();
    }

    private IEnumerator Arpact()
    {
        yield return new WaitForSecondsRealtime(1);
        GameObject.Find("Game").GetComponent<Game>().arpaactive();
        GameObject.Find("Game").GetComponent<Game>().ShowdownCardset();
        
    }

    public void Update()
    {
        textNickname_enemy.text = EnemyNickname;
    }
    public void playby()
    {
        GameObject.FindWithTag("Player").GetComponent<photonplayer>().playby();
    }
}
