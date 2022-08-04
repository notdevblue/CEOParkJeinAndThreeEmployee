using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBullet : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rigid;

    private Vector2 startPos = Vector2.zero;
    private Vector2 targetPos = Vector2.zero;

    [SerializeField]
    private float speed = 3f;

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    private void Update()
    {
        
    }

    public void Shoot(Transform curPos, Vector2 targetPos)
    {
        transform.position = startPos = curPos.position;
        this.targetPos = targetPos;
        this.SetActive(true);

        Vector2 dir = targetPos - (Vector2)transform.position;
        //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        rigid.AddForce(dir * speed, ForceMode2D.Impulse);
    }

    private Quaternion LookAt2D(Vector2 forward)
    {
        return Quaternion.Euler(0, 0, Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg);
    }

}
