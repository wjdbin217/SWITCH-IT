using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickBtn : MonoBehaviour
{
    public Animator animator1;
    public Animator animator2;

    public void Clickaccept()
    {
        animator1.SetTrigger("Clickaccept");
    }
    public void Clickreject()
    {
        animator2.SetTrigger("Clickreject");
    }
}
