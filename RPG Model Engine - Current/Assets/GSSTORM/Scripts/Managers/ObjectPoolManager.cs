using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

namespace  GSStorm.RPG.Engine{

    /// <summary>
    /// 对象池管理器，分普通类对象池+资源游戏对象池
    /// </summary>
	public class ObjectPoolManager : Singleton<ObjectPoolManager>, IManager {
        private Transform _poolRootObject = null;
		private Dictionary<string, GameObjectPool> _gameObjectPools = new Dictionary<string, GameObjectPool>();

		#region IManager Interfaces
		public ManagerStatus Status { get; private set;}

		public IEnumerator PreLaunch(){
			Status = ManagerStatus.PreLaunching;
			yield return null;
		}

		public IEnumerator StartLaunch(){
			Status = ManagerStatus.StarLaunching;

			//Creater pool root object
			var objectPool = new GameObject("ObjectPool");
			objectPool.transform.localScale = Vector3.one;
			objectPool.transform.localPosition = Vector3.zero;
			_poolRootObject = objectPool.transform;

			//TODO: load the prefabs of the game, we use hardcode, may need to make this more flexible later

			CreatePool (PrefabConst.BAG_BUTTON_ITEM, 1, 10, Resources.Load<GameObject> ("Prefabs/UI/Item/UIItem/BtnItem"));
			yield return null;

			CreatePool (PrefabConst.COMMON_MAP_ITEM, 1, 10, Resources.Load<GameObject> ("Prefabs/UI/Item/MapItem/item"));
			yield return null;

			CreatePool (PrefabConst.SKILL_CRUSH_LAND, 1, 10, Resources.Load<GameObject> ("Prefabs/Skills/SkillCrushLand"));
			yield return null;

			CreatePool (PrefabConst.SKILL_PROJECTILE, 1, 10, Resources.Load<GameObject> ("Prefabs/Skills/SkillProjectile"));
			yield return null;

            CreatePool(PrefabConst.BUFF_STATE_ICON, 1, 10, Resources.Load<GameObject>("Prefabs/UI/Item/UIItem/BuffStateIcon"));
            yield return null;

            CreatePool(PrefabConst.SKILL_BLADE_FURY, 1, 10, Resources.Load<GameObject>("Prefabs/Skills/SkillBladeFury"));
            yield return null;

        }

        public IEnumerator PostLaunch(){
			Status = ManagerStatus.Started;
			yield return null;
		}

		#endregion



        private GameObjectPool CreatePool(string poolName, int initSize, int maxSize, GameObject prefab) {
			var pool = new GameObjectPool(poolName, prefab, initSize, maxSize, _poolRootObject);
            _gameObjectPools[poolName] = pool;
            return pool;
        }

		/// <summary>
		/// Gets the pool.
		/// </summary>
		/// <returns>The pool.</returns>
		/// <param name="poolName">Pool name.</param>
        public GameObjectPool GetPool(string poolName) {
            if (_gameObjectPools.ContainsKey(poolName)) {
                return _gameObjectPools[poolName];
            }
            return null;
        }

		/// <summary>
		/// Get the pooled Object by name.
		/// </summary>
		/// <param name="poolName">Pool name.</param>
        public GameObject GetObject(string poolName) {
            GameObject result = null;
            if (_gameObjectPools.ContainsKey(poolName)) {
                GameObjectPool pool = _gameObjectPools[poolName];
                result = pool.NextAvailableObject();
                if (result == null) {
                    Debug.LogWarning("No object available in pool. Consider setting fixedSize to false.: " + poolName);
                }
            } else {
                Debug.LogError("Invalid pool name specified: " + poolName);
            }
            return result;
        }

		/// <summary>
		/// Releases to pool.
		/// </summary>
		/// <param name="poolName">Pool name.</param>
		/// <param name="go">Go.</param>
        public void ReleaseToPool(string poolName, GameObject go) {
            if (_gameObjectPools.ContainsKey(poolName)) {
                GameObjectPool pool = _gameObjectPools[poolName];
                pool.ReturnObjectToPool(poolName, go);
            } else {
                Debug.LogWarning("No pool available with name: " + poolName);
            }
        }




       
    }
}