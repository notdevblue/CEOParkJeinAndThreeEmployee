using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public readonly string ANIM_MOVE = "isMoving";

    public readonly string ANIM_ATTACK = "attack";
    public readonly string ANIM_DIE = "die";
    public readonly string ANIM_HURT = "hurt";

    private Animator anim;
    public Animator Anim => anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
}
