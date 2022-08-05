using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoSingleton<BulletPool>
{
    private TetrisBullet[] bulletPrefab;
    private BulletObj[] bulletObjPrefab;

    [SerializeField]
    private Transform bulletParent;

    [SerializeField]
    private int initCount = 5;
    [SerializeField]
    private int bulletObjInitcount = 10;

    private List<TetrisBullet> bulletList = new List<TetrisBullet>();
    private Dictionary<int, List<BulletObj>> bulletObjDic = new Dictionary<int, List<BulletObj>>();

    protected override void Awake()
    {
        base.Awake();

        bulletPrefab = Resources.LoadAll<TetrisBullet>("Bullets");
        bulletObjPrefab = Resources.LoadAll<BulletObj>("BulletObjs");

        for (int i = 0; i < initCount; i++)
        {
            TetrisBullet bullet = InstantiateBullet();
            bullet.SetActive(false);
        }

        //for (int i = 0; i < bulletObjPrefab.Length; i++)
        //{
        //    for (int j = 0; j < bulletObjInitcount; j++)
        //    {
        //        AddObj(i, InstantiateObj(i));
        //    }
        //}
    }

    #region Bullet
    private TetrisBullet InstantiateBullet()
    {
        int idx = Random.Range(0, bulletPrefab.Length);
        TetrisBullet bul = Instantiate(bulletPrefab[idx], bulletParent);
        bul.bulletIdx = idx;
        bulletList.Add(bul);
        return bul;
    }

    private TetrisBullet InstantiateBullet(int bulletIdx)
    {
        TetrisBullet bul = Instantiate(bulletPrefab[bulletIdx], bulletParent);
        bul.bulletIdx = bulletIdx;
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

    public TetrisBullet GetBullet(int bulletIdx)
    {
        return InstantiateBullet(bulletIdx);
    }
    #endregion

    #region BulletObj

    private void AddObj(int key, BulletObj obj)
    {
        if(!bulletObjDic.ContainsKey(key))
        {
            bulletObjDic.Add(key, new List<BulletObj>());
        }

        bulletObjDic[key].Add(obj);
        obj.SetActive(false);
    }

    private BulletObj InstantiateObj(int key)
    {
        return Instantiate(bulletObjPrefab[key],bulletParent);
    }

    public BulletObj GetObj(int key)
    {
        if (!bulletObjDic.ContainsKey(key)) return null;

        BulletObj obj = bulletObjDic[key].Find(x => !x.gameObject.activeSelf);

        if(obj == null)
        {
            obj = InstantiateObj(key);
        }

        return obj;
    }

    #endregion
}
