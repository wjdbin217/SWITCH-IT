using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unrev_solo : MonoBehaviour
{
    public int handPositionY;
    public float handPositionX;
    public GameObject card;
    public Vector2 Cardsize;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetHand() //카드 배치
    {
        Vector2 position = new Vector2(0,handPositionY);
        float position_x = -9 * handPositionX;
        foreach (string i in Game_solo.boxcards)
        {
            GameObject temp = Instantiate(card,transform);
            temp.GetComponent<Button>().enabled = false;
            temp.GetComponent<Card_solo>().Front_Back = false;
            temp.GetComponent<RectTransform>().sizeDelta = Cardsize;
            temp.GetComponent<Card_solo>().Cardtype = i;
            position_x +=handPositionX*2;
            position.x = position_x;
            temp.transform.localPosition = position;
            string card_name =  i;
            temp.name =card_name;
            Debug.Log(card_name+" Created at x:" + position_x.ToString());
        }
    }
}

