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
            WebSocketClient.Instance.Send("damage", "");
        }
    }
}