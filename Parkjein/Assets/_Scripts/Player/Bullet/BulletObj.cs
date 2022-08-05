using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletObj : MonoBehaviour
{
    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public void SetSpawn(Vector2 pos)
    {
        transform.position = pos;
        this.SetActive(true);
    }
}
