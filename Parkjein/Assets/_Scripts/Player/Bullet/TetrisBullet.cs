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
        User user = col.gameObject.GetComponent<User>();
        if (user != null && user.id == fireVO.shooterId) return;

        this.gameObject.tag = "GROUND";
        Destroy(this.GetComponent<Rigidbody2D>());
    }
}
