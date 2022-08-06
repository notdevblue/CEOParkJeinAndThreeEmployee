using HanSocket;
using HanSocket.Data;
using HanSocket.VO.InGame;
using Objects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;

public class TetrisBullet : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rigid;

    public int bulletIdx = -1;
    public int bulletId = -1;
    public int shooterId = -1;

    private FireVO fireVO;
    public FireVO @FireVO => fireVO;

    public bool damaged = false;

    private Vector2 _pos
        = new Vector2();
    private Quaternion _rot
        = new Quaternion();

    private WaitForSecondsRealtime wait
        = new WaitForSecondsRealtime(1.0f / 15.0f);


    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public void Shoot(FireVO fireVO)
    {
        if (rigid == null)
            rigid = gameObject.AddComponent<Rigidbody2D>();

        damaged = false;
        transform.position = fireVO.startPos;
        this.fireVO = fireVO;
        this.shooterId = this.fireVO.shooterId;
        this.SetActive(true);
        _pos = transform.position;
        _rot = Quaternion.identity;
        SoundManager.Instance.PlaySfxSound(SoundManager.Instance.throwSfx);
        rigid.AddForce(fireVO.dir * fireVO.bulletSpeed, ForceMode2D.Impulse);
        rigid.AddTorque(fireVO.rotationSpeed);

        // StartCoroutine(Sync());
    }

    public void SetTarget(Vector2 pos, Quaternion rot)
    {
        this.transform.position = pos;
        this.transform.rotation = rot;
    }

    IEnumerator Sync()
    {
        while (gameObject.activeSelf)
        {
            yield return wait;
            WebSocketClient.Instance.Send("bulletstop",
                    new BulletStopVO(bulletId, shooterId, transform.position, transform.rotation).ToJson());
        }
    }

    private Quaternion LookAt2D(Vector2 forward)
    {
        return Quaternion.Euler(0, 0, Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg);
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (rigid == null)
            return;

        User user = col.gameObject.GetComponent<User>();

        bool stopBullet = false;

        if (!damaged && user != null && fireVO.shooterId != user.id)
        {
            if (user.id == WebSocketClient.Instance.id)
            {
                damaged = true;
                WebSocketClient.Instance.Send("damage",
                    new DamageVO(col.contacts[0].point).ToJson());
            }
        }


        if (col.gameObject.CompareTag("GROUND")
         || col.gameObject.CompareTag("BULLET"))
        {
            stopBullet = true;
            damaged = true;
            if (shooterId == WebSocketClient.Instance.id)
            {
                WebSocketClient.Instance.Send("bulletstop",
                    new BulletStopVO(bulletId, shooterId, transform.position, transform.rotation).ToJson());
            }
        }

        if (stopBullet)
        {
            damaged = true;
            this.gameObject.tag = "GROUND";
            rigid.velocity = Vector2.zero;
        }
    }
}
