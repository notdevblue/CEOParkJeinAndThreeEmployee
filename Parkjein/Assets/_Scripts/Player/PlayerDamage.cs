using HanSocket;
using HanSocket.Data;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("BULLET") &&

           other.gameObject.GetComponent<TetrisBullet>()
            .FireVO.shooterId != GetComponent<User>().id)
        {
            other.gameObject.tag = "GROUND";
            WebSocketClient.Instance.Send("damage", "");
        }
    }
}