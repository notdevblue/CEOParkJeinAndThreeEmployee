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

    public void RemoteShoot(FireVO vo)
    {
        transform.position = vo.startPos;
        
        this.SetActive(true);
        
        //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        rigid.AddForce(vo.dir * vo.bulletSpeed, ForceMode2D.Impulse);
    }

    public void Shoot(FireVO fireVO)
    {
        this.fireVO = fireVO;

        transform.position = fireVO.startPos;
        this.SetActive(true);

        rigid.AddForce(fireVO.dir * fireVO.bulletSpeed, ForceMode2D.Impulse);
    }

    private Quaternion LookAt2D(Vector2 forward)
    {
        return Quaternion.Euler(0, 0, Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg);
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
    }
}
