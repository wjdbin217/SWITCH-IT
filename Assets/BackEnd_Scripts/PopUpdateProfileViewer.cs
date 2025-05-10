using UnityEngine;
using TMPro;

public class PopUpdateProfileViewer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textNickname;
    [SerializeField]
    private TextMeshProUGUI textGamerID;

    public void UpdateNIckname()
    {
        // �г����� ������ gamer_id�� ����ϰ�, �г����� ������ �г��� ���
        textNickname.text = UserInfo.Data.nickname == null?
                            UserInfo.Data.gamerId : UserInfo.Data.nickname;

        // gamer_id ���
        textGamerID.text = UserInfo.Data.gamerId;
    }
}
