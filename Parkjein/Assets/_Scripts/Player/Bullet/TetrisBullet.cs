using HanSocket;
using HanSocket.Data;
using HanSocket.VO.InGame;
using Objects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBullet : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rigid;

    public int bulletIdx;

    private FireVO fireVO;
    public FireVO @FireVO => fireVO;


    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public void Shoot(FireVO fireVO)
    {
        if (rigid == null)
            rigid = gameObject.AddComponent<Rigidbody2D>();

        transform.position = fireVO.startPos;
        this.fireVO = fireVO;
        this.SetActive(true);
        rigid.AddForce(fireVO.dir * fireVO.bulletSpeed, ForceMode2D.Impulse);
        rigid.AddTorque(fireVO.rotationSpeed);
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

        if (user != null && fireVO.shooterId != user.id)
        {
            if (user.id == UserData.Instance.myId)
            {
                stopBullet = true;
                WebSocketClient.Instance.Send("damage", "");
            }
        }
        else if (col.gameObject.CompareTag("GROUND")
              || col.gameObject.CompareTag("BULLET"))
        {
            stopBullet = true;
        }

        if (stopBullet)
        {
            this.gameObject.tag = "GROUND";
            rigid.velocity = Vector2.zero;
            Destroy(rigid);
        }
    }
}
