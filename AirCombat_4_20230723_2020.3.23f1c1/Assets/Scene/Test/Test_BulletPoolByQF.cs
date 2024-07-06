/****************************************************
    文件：Test_BulletPoolByQF.cs
	作者：lenovo
    邮箱: 
    日期：2023/8/19 15:9:45
	功能： 使用QF的PoolKit
*****************************************************/

using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Random = UnityEngine.Random;
 
namespace Demo00_00
{
    public class Test_BulletPoolByQF : MonoBehaviour
    {
        #region 属性
        SimpleObjectPool<GameObject> _pool;
        public GameObject prefab;
         GameObject _curGo;
         float _fireSpanTime=0.2f;
        #endregion

        #region 生命

        /// <summary>首次载入</summary>
        void Awake()
        {
            
        }
        

        /// <summary>Go激活</summary>                       s
        void OnEnable ()
        {
            
        }

        /// <summary>首次载入且Go激活</summary>
        void Start()
        {
            _pool = new SimpleObjectPool<GameObject>(() =>
            {
                GameObject go = GameObject.Instantiate(prefab, Vector2.zero,Quaternion.identity);
                go.Identity();
                _curGo = go;
                go.UpdateAsObservable()
                    .Subscribe(_ =>
                    {
                        if (go.ActiveSelf() == true)
                        {
                            go.transform.Translate(Vector2.up * Time.deltaTime, Space.World);
                        }
                    } )
                    .AddTo(go);
                go.Hide();
                go.SetParent(gameObject);

                return go;
            }, go => 
            {
                go.Identity();
                go.Hide(); 
            },5);
        }

         /// <summary>固定更新</summary>
        void FixedUpdate()
        {
            
        }






        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q)) 
            {
                GameObject go = new GameObject();
                go.Hide();
                go.DestroySelfAfterDelay(1f);
                //一个反复跑，加上前前的。就是一个会延时销毁的反复跑的
                gameObject
                    .UpdateAsObservable()
                    .Sample(TimeSpan.FromSeconds(_fireSpanTime))
                    .Subscribe(_ =>
                    {
                        _curGo = _pool.Allocate();
                        _curGo.Show();
                    })
                    .AddTo(go);
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                _pool.Recycle(_curGo); 
                foreach (Transform t in transform)
                {
                    if (t.ActiveSelf() == true)
                    {
                        _curGo = t.gameObject;
                        break;
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.E)) 
            { 
                _pool.Clear(go =>
                {
                    Destroy(go);
                }); 
            }
        }

         /// <summary>延迟更新。适用于跟随逻辑</summary>
        void LateUpdae()
        {
            
        }

        /// <summary> 组件重设为默认值时（只用于编辑状态）</summary>
        void Reset()
        {
            
        }
      

        /// <summary>当对象设置为不可用时</summary>
        void OnDisable()
        {
            
        }


        /// <summary>组件销毁时调用</summary>
        void OnDestroy()
        {
            
        }
        #endregion

        #region 系统

        #endregion

        #region 辅助

        #endregion

    }
}



