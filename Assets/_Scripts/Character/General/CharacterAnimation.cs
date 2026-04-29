using Spine;
using Spine.Unity;
using System;
using UnityEngine;

public enum CharacterState
{
    Idle,
    Run,
    Attack,
    Skill,
    TakeDamage,
    Die,
    Win,
    Lose,
    Buff
}

public class CharacterAnimation : MonoBehaviour
{
    private SkeletonAnimation skeletonAnimation;
    public event Action<string> OnAnimationEvent; 
    public event Action<string> OnAnimationComplete;
    private CharacterState currentState = CharacterState.Idle;
    private string currentAnimation;

    private void Awake()
    {
        skeletonAnimation = GetComponentInChildren<SkeletonAnimation>();
    }

    private void OnEnable()
    {
        if (skeletonAnimation != null)
        {
            skeletonAnimation.AnimationState.Event += HandleSpineEvent;
            skeletonAnimation.AnimationState.Complete += HandleAnimationComplete;
        }
        SetState(currentState);
    }


    private void OnDisable()
    {
        if (skeletonAnimation != null)
        {
            skeletonAnimation.AnimationState.Event -= HandleSpineEvent;
            skeletonAnimation.AnimationState.Complete -= HandleAnimationComplete;
        }
    }

    private void HandleSpineEvent(TrackEntry trackEntry, Spine.Event e)
    {
        OnAnimationEvent?.Invoke(e.Data.Name);
    }

    private void HandleAnimationComplete(TrackEntry trackEntry)
    {
        OnAnimationComplete?.Invoke(trackEntry.Animation.Name);
    }

    public void SetState(CharacterState newState)
    {
        if (currentState == newState) return;

        currentState = newState;

        switch (newState)
        {
            case CharacterState.Idle:
                Play(CONSTANT.idleAnimation, true);
                break;

            case CharacterState.Run:
                Play(CONSTANT.runAnimation, true);
                break;

            case CharacterState.Attack:
                Play(CONSTANT.attackAnimation, false, CONSTANT.idleAnimation);
                break;

            case CharacterState.Skill:
                Play(CONSTANT.skill1Animation, false, CONSTANT.idleAnimation);
                break;

            case CharacterState.TakeDamage:
                Play(CONSTANT.takeDamageAnimation, false, CONSTANT.idleAnimation);
                break;

            case CharacterState.Die:
                Play(CONSTANT.dieAnimation, false);
                break;

            case CharacterState.Win:
                Play(CONSTANT.winAnimation, false);
                break;

            case CharacterState.Lose:
                Play(CONSTANT.loseAnimation, false);
                break;

            case CharacterState.Buff:
                Play(CONSTANT.buffAnimation, false, CONSTANT.idleAnimation);
                break;
        }
    }

    private void Play(string animationName, bool loop, string nextAnimation = null)
    {
        if (string.IsNullOrEmpty(animationName)) return;

        var state = skeletonAnimation.AnimationState;
        var current = state.GetCurrent(0);

        if (current != null && current.Animation.Name == animationName)
            return;

        state.SetAnimation(0, animationName, loop);
        currentAnimation = animationName;

        if (!loop && !string.IsNullOrEmpty(nextAnimation))
        {
            var nextEntry = state.AddAnimation(0, nextAnimation, true, 0f);

            nextEntry.Start += (t) =>
            {
                currentState = GetStateFromAnimation(nextAnimation);
            };
        }
    }

    private CharacterState GetStateFromAnimation(string anim)
    {
        if (anim == CONSTANT.idleAnimation) return CharacterState.Idle;
        if (anim == CONSTANT.runAnimation) return CharacterState.Run;
        return CharacterState.Idle;
    }

    public CharacterState GetCurrentState()
    {
        return currentState;
    }
}
