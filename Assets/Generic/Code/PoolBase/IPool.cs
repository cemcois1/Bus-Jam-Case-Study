using UnityEngine;

namespace Generic.Code.PoolBase
{
    public interface IPool
    {
        public void InitializeBulletPool();
        public GameObject GetGameobjectFromPool(Vector3 position, Quaternion rotation);
        public GameObject GetGameobjectFromPool();

    }
}