using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Core;
using HanSocket;
using HanSocket.VO.InGame;
using HanSocket.Data;

public class PlayerShoot : MonoBehaviour
{
    private const KeyCode SHOOT = KeyCode.Mouse0;

    private PlayerAnimation anim;
    private PlayerMove move;

    [SerializeField]
    private float bulletSpeed = 3f;

    private Camera mainCam;

    private void Start()
    {
        anim = GetComponent<PlayerAnimation>();
        move = GetComponent<PlayerMove>();

        mainCam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetKeyDown(SHOOT))
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        Vector2 dir = mainCam.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        dir = dir.normalized;

        if ((dir.x > 0 && move.Sr.flipX) || (dir.x < 0 && !move.Sr.flipX))
        {
            return;
        }

        TetrisBullet bullet = BulletPool.Instance.GetBullet();

        //WebSocketClient.Instance.Send("fire",
        //    new FireVO( bullet.bulletIdx, transform.position, dir, bulletSpeed).ToJson());

        anim.Anim.SetTrigger(anim.ANIM_ATTACK);
        bullet.Shoot(transform.position, dir, bulletSpeed);
    }
}
