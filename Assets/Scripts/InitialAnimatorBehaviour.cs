
using UnityEngine;
using Spine.Unity;


public class InitialAnimatorBehaviour : StateMachineBehaviour
{
    public SkeletonAnimation skeletonAnimation;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        skeletonAnimation = animator.gameObject.GetComponent<SkeletonAnimation>();
        Debug.Log(skeletonAnimation.name);
    }

}
