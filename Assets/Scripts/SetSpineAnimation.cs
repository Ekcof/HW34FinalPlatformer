using UnityEngine;
using Spine.Unity;

public class SetSpineAnimation : StateMachineBehaviour
{
    public string animationName;
    public float speed = 1f;
    public InitialAnimatorBehaviour initialAnimatorBehaviour;
    private SkeletonAnimation anim;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        anim = initialAnimatorBehaviour.skeletonAnimation;
        //Debug.Log("trying " + anim);
        if (anim != null)
        {
            anim.state.SetAnimation(0, animationName, true).TimeScale = speed;
            Debug.Log("playing " + animationName);
        }
    }

}
