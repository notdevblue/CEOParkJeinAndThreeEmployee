using HanSocket;
using HanSocket.VO.InGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBullet : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rigid;

    public int bulletIdx;

    private FireVO fireVO;

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public void Shoot(Vector3 curPos,Vector2 dir,float speed)
    {
        transform.position = curPos;
        this.SetActive(true);
        
        //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        rigid.AddForce(dir * speed, ForceMode2D.Impulse);
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

    public void CollisionEnter(GameObject obj)
    {
        if(obj.CompareTag("PLAYER") || obj.CompareTag("GROUND"))
        {
            WebSocketClient.Instance.Send("collision", null);
        }
    }
}
