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
    private Rigidbody2D rigid;
    private PlayerAnimation anim;

    [SerializeField]
    private float moveSpeed = 5f;
    [SerializeField]
    private float jumpSpeed = 3f;

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

        if (isGround) jumpCount = maxJumpCount;

        if (Input.GetKeyDown(JUMP))
        {
            if ((!isGround && jumpCount <= 0)) return;

            jumpCount--;
            rigid.velocity = new Vector2(rigid.velocity.x, jumpSpeed);
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

        transform.position += dir * moveSpeed * Time.deltaTime;
    }
}
