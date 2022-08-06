using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HanSocket;

public class PlayerMove : MonoBehaviour
{
    private const KeyCode LEFT = KeyCode.A;
    private const KeyCode RIGHT = KeyCode.D;
    private const KeyCode JUMP = KeyCode.Space;

    private SpriteRenderer sr;
    public SpriteRenderer Sr => sr;

    private Rigidbody2D rigid;

    private PlayerAnimation anim;

    [SerializeField]
    private PlayerData data;

    private const int maxJumpCount = 2;
    [SerializeField]
    private int jumpCount = 2;

    private bool isGround = false;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        anim = GetComponent<PlayerAnimation>();
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Move();
        Jump();
    }

    private void Move()
    {
        if (!data.CanMove) return;

        if (Input.GetKey(LEFT))
        {
            Move(Vector3.left);
        }
        else if (Input.GetKey(RIGHT))
        {
            Move(Vector3.right);
        }else {
            anim.Anim.SetBool(anim.ANIM_MOVE, false);
        }
    }

    private void Jump()
    {
        isGround = rigid.velocity.y == 0;

        if (isGround)
        {
            jumpCount = maxJumpCount;
        }
        else if (jumpCount == 2) jumpCount = 1;

        if (Input.GetKeyDown(JUMP))
        {
            if ((!isGround && jumpCount <= 0)) return;

            jumpCount--;
            anim.Anim.SetTrigger(anim.ANIM_JUMP);
            rigid.velocity = new Vector2(rigid.velocity.x, data.JumpSpeed);
        }
    }

    private void Move(Vector3 dir)
    {
        if(dir != Vector3.zero)
        {
            if(dir.x != 0)
            {
                sr.flipX = dir.x < 0;
            }

            anim.Anim.SetBool(anim.ANIM_MOVE, true);
        }
        else
        {
            anim.Anim.SetBool(anim.ANIM_MOVE, false);
        }

        transform.position += dir * data.MoveSpeed * Time.deltaTime;
    }
}
