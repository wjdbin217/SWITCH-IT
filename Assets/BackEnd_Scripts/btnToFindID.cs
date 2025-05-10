using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class btnToFindID : MonoBehaviour
{
    public void OnclickExit() {
        SceneManager.LoadScene("FindIDScene");
    }
}
