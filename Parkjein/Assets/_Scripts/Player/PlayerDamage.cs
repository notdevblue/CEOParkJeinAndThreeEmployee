using HanSocket;
using HanSocket.Data;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("BULLET") &&

           other.gameObject.GetComponent<TetrisBullet>()
            .FireVO.shooterId != GetComponent<User>().id)
        {
            Rigidbody2D rigid = other.gameObject.GetComponent<Rigidbody2D>();
            Destroy(rigid);
            
            other.gameObject.tag = "GROUND";

            WebSocketClient.Instance.Send("damage", "");
        }
    }
}