using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class btnToRegister : MonoBehaviour
{
    public void OnclickExit() {
        SceneManager.LoadScene("RegisterScene");
    }
}
