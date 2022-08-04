 using UnityEngine;

public class Remote : MonoBehaviour
{
    private Vector2 _target;

    [SerializeField]
    private float _t = 0.15f;

    public void SetTarget(Vector2 pos)
    {
        _target = pos;
    }

    private void Update()
    {
        transform.position =
            Vector2.Lerp(transform.position, _target, _t);
    }
}