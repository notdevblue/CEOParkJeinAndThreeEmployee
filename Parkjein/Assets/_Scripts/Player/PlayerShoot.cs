using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Core;
using HanSocket;
using HanSocket.VO.InGame;
using HanSocket.Data;
using UnityEngine.EventSystems;

public class PlayerShoot : MonoBehaviour
{
    private const KeyCode SHOOT = KeyCode.Mouse0;

    private PlayerAnimation anim;
    private PlayerMove move;

    [SerializeField]
    private PlayerData data;

    [SerializeField]
    private Transform leftBulletPos;
    [SerializeField]
    private Transform rightBulletPos;

    [SerializeField]
    private float attackCoolTime = 1f;
    private float curAttackCoolTime = 1f;

    private bool isAttackAble = false;

    private Camera mainCam;

    private void Start()
    {
        anim = GetComponent<PlayerAnimation>();
        move = GetComponent<PlayerMove>();

        mainCam = Camera.main;

    }

    private void Update()
    {
        if(!isAttackAble)
        {
            curAttackCoolTime += Time.deltaTime;

            if (curAttackCoolTime >= attackCoolTime)
            {
                curAttackCoolTime = 0f;
                isAttackAble = true;
            }
        }

        if (Input.GetKeyDown(SHOOT) && isAttackAble)
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            Shoot();
        }
    }

    public void SetAttackSpeed(float attackSpeed)
    {
        this.attackCoolTime = 1f / attackSpeed;

        this.curAttackCoolTime = this.attackCoolTime;
    }

    public void Shoot()
    {
        Vector2 dir = mainCam.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        Vector2 startPos = move.Sr.flipX ? leftBulletPos.position : rightBulletPos.position;
        dir = dir.normalized;

        if ((dir.x > 0 && move.Sr.flipX) || (dir.x < 0 && !move.Sr.flipX))
        {
            return;
        }

        TetrisBullet bullet = BulletPool.Instance.GetBullet();
        FireVO vo = new FireVO(UserData.Instance.myId,bullet.bulletIdx, startPos, dir,data.BulletSpeed, data.RotationSpeed);

        WebSocketClient.Instance.Send("fire",
            vo.ToJson());

        anim.Anim.SetTrigger(anim.ANIM_ATTACK);
        isAttackAble = false;
        bullet.Shoot(vo);
    }
}
