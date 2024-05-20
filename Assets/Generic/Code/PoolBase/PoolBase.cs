using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Generic.Code.PoolBase
{
    [DefaultExecutionOrder(-50)]
    public class PoolBase:MonoBehaviour,IPool
    {
        public GameObject itemPrefab; // Bus prefabı
        public int initialPoolSize = 20; // Havuz boyutu

        private List<GameObject> pool= new List<GameObject>();
        void Awake()
        {
            // Mermi havuzunu başlat
            InitializeBulletPool();
        }

        [Button]
        public void ClearPool()
        {
            pool.Clear();
        }
        public void InitializeBulletPool()
        {
            // Belirlenen boyutta mermi nesneleri oluştur ve havuza ekle
            for (int i = 0; i < initialPoolSize; i++)
            {
                GameObject bullet = Instantiate(itemPrefab);
                bullet.SetActive(false);
                pool.Add(bullet);
            }
        }

        public GameObject GetGameobjectFromPool()
        {
            // Havuzda aktif olmayan bir mermi bul ve onu döndür
            if (pool==null)
            {
                pool = new List<GameObject>();
            }
#if !UNITY_EDITOR
            
            for (int i = 0; i < pool.Count; i++)
            {
                if (!pool[i].activeInHierarchy)
                {
                    return pool[i];
                }
            }
#endif

            // Havuzda uygun mermi bulunamazsa yeni bir mermi oluştur ve havuza ekle
#if UNITY_EDITOR
            GameObject newBullet = PrefabUtility.InstantiatePrefab(itemPrefab) as GameObject;

#else 
GameObject newBullet = Instantiate(itemPrefab);
#endif
            newBullet.SetActive(false);
            pool.Add(newBullet);

            return newBullet;
        }

        public GameObject GetGameobjectFromPool(Vector3 position, Quaternion rotation)
        {
            // Havuzda aktif olmayan bir mermi bul ve onu döndür
            for (int i = 0; i < pool.Count; i++)
            {
                if (!pool[i].activeInHierarchy)
                {
                    pool[i].transform.position = position;
                    pool[i].transform.rotation = rotation;
                    pool[i].transform.DOKill(true);
                    
                    pool[i].SetActive(true);
                    return pool[i];
                }
            }

            // Havuzda uygun mermi bulunamazsa yeni bir mermi oluştur ve havuza ekle
            GameObject newBullet = Instantiate(itemPrefab);
            newBullet.SetActive(false);
            pool.Add(newBullet);

            newBullet.transform.position = position;
            newBullet.transform.rotation = rotation;

            return newBullet;
        }
    }
}