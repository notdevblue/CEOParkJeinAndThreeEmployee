using HanSocket;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("BULLET"))
        {
            WebSocketClient.Instance.Send("damage", "");
        }
    }
}