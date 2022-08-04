 using UnityEngine;

public class Remote : MonoBehaviour
{
    private Vector2 _target;

    private PlayerAnimation anim;

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

        if (Vector2.Distance(_target, transform.position) <= 0.03f)
        {
            anim.Anim.SetBool(anim.ANIM_MOVE, false);
        }
        else
        {
            anim.Anim.SetBool(anim.ANIM_MOVE, true);
        }
    }
}