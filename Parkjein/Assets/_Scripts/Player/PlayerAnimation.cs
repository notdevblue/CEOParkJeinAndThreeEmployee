using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public readonly string ANIM_MOVE = "isMoving";

    public readonly string ANIM_ATTACK_SPEED = "attackSpeed";
    public readonly string ANIM_ATTACK = "attack";
    public readonly string ANIM_DIE = "die";
    public readonly string ANIM_HURT = "hurt";
    public readonly string ANIM_JUMP = "jump";

    private const string ANIM_THROW = "Throw";

    private Animator anim;
    public Animator Anim => anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    //public bool IsThrow()
    //{
    //    AnimatorStateInfo a = anim.GetCurrentAnimatorStateInfo(0);
    //    return a.IsName(ANIM_THROW) && a.normalizedTime <= 0.6f;
    //}
}
