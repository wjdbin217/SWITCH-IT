using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class btnToFindPW : MonoBehaviour
{
    public void OnclickExit() {
        SceneManager.LoadScene("FindPWScene");
    }
}
