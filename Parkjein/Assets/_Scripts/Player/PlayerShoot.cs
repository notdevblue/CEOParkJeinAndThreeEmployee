using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Core;

public class PlayerShoot : MonoBehaviour
{
    private const KeyCode SHOOT = KeyCode.Mouse0;

    private TetrisBullet[] bulletPrefab;

    [SerializeField]
    private Transform bulletParent;

    [SerializeField]
    private int initCount = 5;

    private Queue<TetrisBullet> bulletQueue = new Queue<TetrisBullet>();

    private Camera mainCam;

    private void Awake()
    {
        bulletPrefab = Resources.LoadAll<TetrisBullet>("Bullets");

        for (int i = 0; i < initCount; i++)
        {
            TetrisBullet bullet = InstantiateBullet();
            bulletQueue.Enqueue(bullet);
            bullet.SetActive(false);
        }
    }

    private void Start()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        if(Input.GetKeyDown(SHOOT))
        {
            Shoot();
        }
    }

    private TetrisBullet InstantiateBullet()
    {
        int idx = Random.Range(0, bulletPrefab.Length);
        TetrisBullet bul = Instantiate(bulletPrefab[idx], bulletParent);

        return bul;
    }

    private void Shoot()
    {
        TetrisBullet bullet = bulletQueue.Count > 0 ? bulletQueue.Dequeue() : InstantiateBullet();

        bullet.Shoot(transform, mainCam.ScreenToWorldPoint(Input.mousePosition));
    }
}
