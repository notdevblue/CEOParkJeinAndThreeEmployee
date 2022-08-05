using HanSocket;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("BULLET") &&
           !other.gameObject.GetComponent<TetrisBullet>().firedByMe)
        {
            Rigidbody2D rigid = other.gameObject.GetComponent<Rigidbody2D>();
            rigid.constraints = RigidbodyConstraints2D.FreezeAll;
            rigid.simulated = false;
            WebSocketClient.Instance.Send("damage", "");
        }
    }
}