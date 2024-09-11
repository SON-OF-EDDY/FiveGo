using UnityEngine;
using System.Collections;

public class ButtonAnimatorManager : MonoBehaviour
{
   
    public Animator animator;

   public void StartAnimation()
    {
        animator.SetTrigger("startAnimation");
    }

    public void StopAnimation()
    {
        animator.SetTrigger("stopAnimation");
    }

}
