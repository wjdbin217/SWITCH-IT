using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class btnToLogin : MonoBehaviour
{
    public void OnclickExit() {
        SceneManager.LoadScene("LoginScene");
    }

    public void OnclickPvE() {
        SceneManager.LoadScene("solo");
    }
}
