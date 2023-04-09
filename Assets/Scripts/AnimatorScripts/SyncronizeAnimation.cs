using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncronizeAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Animator followingAnimator;
    [SerializeField] private int layer;
    [SerializeField] private int followingLayer = 0;
    private AnimationClip clip;
    private AnimationClip followingClip;
    private bool syncronizing;
    private float normalizedTime;

    private void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    private void Update()
    {
        if (syncronizing)
        {
            SyncronizeBoth();
        }
    }

    public bool Syncronizing { get { return syncronizing; } set => syncronizing = value; }

    private void SyncronizeBoth()
    {
        AnimatorStateInfo animState = animator.GetCurrentAnimatorStateInfo(layer);
        normalizedTime = animState.normalizedTime;
        followingAnimator.Play(followingAnimator.GetCurrentAnimatorStateInfo(followingLayer).nameHash, followingLayer, normalizedTime);
    }
}
