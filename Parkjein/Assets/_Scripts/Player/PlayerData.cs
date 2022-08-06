using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [SerializeField]
    private PlayerUI myUI;
    public PlayerUI MyUI { get => myUI; set => myUI = value; }

    [Header("Move")]
    [SerializeField]
    private float moveSpeed = 5f;
    public float MoveSpeed => moveSpeed;
    [SerializeField]
    private float jumpSpeed = 3f;
    public float JumpSpeed => jumpSpeed;

    [Header("Attack")]
    [SerializeField]
    private float bulletSpeed = 3f;
    public float BulletSpeed => bulletSpeed;
    [SerializeField]
    private float attackSpeed = 1f;
    public float AttackSpeed => attackSpeed;
    [SerializeField]
    private float rotationSpeed = 0.0f;
    public float RotationSpeed => rotationSpeed;

    [SerializeField]
    private PlayerShoot shoot;
    [SerializeField]
    private PlayerAnimation anim;

    private bool _canMove = true;
    public bool CanMove => _canMove;

    public void InitValue(float jump, float speed, float bulletSpeed, float atkspeed, float rotationSpeed)
    {
        this.jumpSpeed = jump;
        this.moveSpeed = speed;
        this.attackSpeed = atkspeed;
        this.bulletSpeed = bulletSpeed;
        this.rotationSpeed = rotationSpeed;

        SetAttackSpeed(attackSpeed);
    }

    public void SetAttackSpeed(float attackSpeed)
    {
        shoot.SetAttackSpeed(attackSpeed);
        anim.SetAttackSpeed(attackSpeed > 1 ? attackSpeed : 1);
    }

    public void Knockout(float restoreTime)
    {
        _canMove = false;
        Invoke("Restore", restoreTime);
    }

    private void Restore()
    {
        _canMove = true;
    }
}
