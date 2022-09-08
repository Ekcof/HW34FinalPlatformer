using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateRandomizer : StateMachineBehaviour
{
    [SerializeField] private int stateNum;
    [SerializeField] private string stateNumName;


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateNum <= 0)
        {
            animator.SetInteger(stateNumName, 0);
        }
        else
        {
            int index = Random.Range(0, stateNum);
            animator.SetInteger(stateNumName, index);
        }
        animator.SetBool("Attack", false);
    }
}
