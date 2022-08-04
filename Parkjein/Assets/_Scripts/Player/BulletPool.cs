using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoSingleton<BulletPool>
{
    private TetrisBullet[] bulletPrefab;

    [SerializeField]
    private Transform bulletParent;

    [SerializeField]
    private int initCount = 5;

    private Queue<TetrisBullet> bulletQueue = new Queue<TetrisBullet>();


    protected override void Awake()
    {
        base.Awake();

        bulletPrefab = Resources.LoadAll<TetrisBullet>("Bullets");

        for (int i = 0; i < initCount; i++)
        {
            TetrisBullet bullet = InstantiateBullet();
            bulletQueue.Enqueue(bullet);
            bullet.SetActive(false);
        }
    }

    private TetrisBullet InstantiateBullet()
    {
        int idx = Random.Range(0, bulletPrefab.Length);
        TetrisBullet bul = Instantiate(bulletPrefab[idx], bulletParent);
        bul.bulletIdx = idx;
        return bul;
    }

    private TetrisBullet InstantiateBullet(int bulletIdx)
    {
        TetrisBullet bul = Instantiate(bulletPrefab[bulletIdx], bulletParent);
        bul.bulletIdx = bulletIdx;
        return bul;
    }

    public TetrisBullet GetBullet()
    {
        return bulletQueue.Count > 0 ? bulletQueue.Dequeue() : InstantiateBullet();
    }

    public TetrisBullet GetBullet(int bulletIdx)
    {
        return InstantiateBullet(bulletIdx);
    }
}
