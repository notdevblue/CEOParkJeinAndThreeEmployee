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

    private int bulletId = 0;

    private List<TetrisBullet> bulletList = new List<TetrisBullet>();

    protected override void Awake()
    {
        base.Awake();

        bulletPrefab = Resources.LoadAll<TetrisBullet>("Bullets");

        for (int i = 0; i < initCount; i++)
        {
            TetrisBullet bullet = InstantiateBullet();
            bullet.SetActive(false);
        }
    }

    public TetrisBullet GetActiveBullet(int id)
    {
        return bulletList.Find(x => x.bulletId == id);
    }

    #region Bullet
    private TetrisBullet InstantiateBullet()
    {
        int idx = Random.Range(0, bulletPrefab.Length);
        TetrisBullet bul = Instantiate(bulletPrefab[idx], bulletParent);
        bul.bulletIdx = idx;
        bul.bulletId = ++bulletId;
        bulletList.Add(bul);
        return bul;
    }

    private TetrisBullet InstantiateBullet(int bulletIdx,int bulletId)
    {
        TetrisBullet bul = Instantiate(bulletPrefab[bulletIdx], bulletParent);
        bul.bulletIdx = bulletIdx;
        bul.bulletId = bulletId;
        bulletList.Add(bul);
        return bul;
    }

    public TetrisBullet GetBullet()
    {
        TetrisBullet bullet = bulletList.Find(x => !x.gameObject.activeSelf);
        return bullet != null ? bullet : InstantiateBullet();
    }

    public void InitBullet()
    {
        bulletList.ForEach(x =>
        {
            if (x.gameObject.activeSelf) x.SetActive(false);
        });
    }

    public TetrisBullet GetBullet(int bulletIdx,int bulletId)
    {
        return InstantiateBullet(bulletIdx,bulletId);
    }
    #endregion
}
