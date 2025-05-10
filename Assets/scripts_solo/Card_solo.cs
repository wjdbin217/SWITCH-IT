using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class Card_solo : MonoBehaviour
{
    public int upsize;
    private RectTransform rectTransform;
    public string Cardtype;
    public bool Front_Back;
    public Sprite rm1,rm2,rm3,rm4,rm5,rm6,rm7,bm1,bm2,bm3,bm4,bm5,bm6,bm7,rp1,rp2,rp3,rp4,rp5,rp6,rp7,bp1,bp2,bp3,bp4,bp5,bp6,bp7,rf,bf,back_Blue,back_Red;
    private Dictionary<string, Sprite> spriteDict_Front;
    void Start()
    {
        spriteDict_Front = new Dictionary<string, Sprite>
        {
            {"rm1", rm1}, {"rm2", rm2}, {"rm3", rm3}, {"rm4", rm4}, {"rm5", rm5}, {"rm6", rm6}, {"rm7", rm7},
            {"bm1", bm1}, {"bm2", bm2}, {"bm3", bm3}, {"bm4", bm4}, {"bm5", bm5}, {"bm6", bm6}, {"bm7", bm7},
            {"rp1", rp1}, {"rp2", rp2}, {"rp3", rp3}, {"rp4", rp4}, {"rp5", rp5}, {"rp6", rp6}, {"rp7", rp7},
            {"bp1", bp1}, {"bp2", bp2}, {"bp3", bp3}, {"bp4", bp4}, {"bp5", bp5}, {"bp6", bp6}, {"bp7", bp7},{"bf",bf},{"rf",rf}
        };

        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        card_image_update();
    }

    public void card_image_update()
    {
        if (Front_Back)
        {
            GetComponent<Image>().sprite = spriteDict_Front[Cardtype];
        }
        else
        {
            if (Cardtype[0].ToString() == "b")
            {
                GetComponent<Image>().sprite = back_Blue;
            }
            else if (Cardtype[0].ToString() == "r")
            {
                GetComponent<Image>().sprite = back_Red;
            }
        }
    }

    public void OnPointer1()
    {
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x,rectTransform.anchoredPosition.y +upsize);
        GameObject.Find("Game").GetComponent<Game_solo>().selected_card = Cardtype;
    }

    // 마우스가 UI에서 나갔을 때 호출
    public void OnPointer2()
    {
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x,0);
    }
}

