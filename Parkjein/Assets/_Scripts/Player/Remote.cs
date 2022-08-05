 using UnityEngine;

public class Remote : MonoBehaviour
{
    private Vector2 _target;

    private PlayerAnimation anim;
    private SpriteRenderer sr;

    [SerializeField]
    private float _t = 0.15f;

    private void Start()
    {
        anim = GetComponent<PlayerAnimation>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void SetTarget(Vector2 pos)
    {
        Vector2 dir = pos - (Vector2)transform.position;

        if(dir != Vector2.zero && dir.x != 0)
        {
            if(sr != null)
                sr.flipX = dir.x < 0;
        }

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