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

    [SerializeField]
    private float bulletSpeed = 3f;

    private Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        if(Input.GetKeyDown(SHOOT))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        TetrisBullet bullet = BulletPool.Instance.GetBullet();
        Vector2 dir = mainCam.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        WebSocketClient.Instance.Send("fire",
            new FireVO( bullet.bulletIdx, transform.position, dir, bulletSpeed).ToJson());
        bullet.Shoot(transform.position, dir,bulletSpeed);
    }
}
