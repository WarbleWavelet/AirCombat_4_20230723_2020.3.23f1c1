using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework
{
    public class SimpleObjectPoolExample : MonoBehaviour
    {
        private SimpleObjectPool<GameObject> mObjectPool;
        GameObject _curGo;

        void Start()
        {
            mObjectPool = new SimpleObjectPool<GameObject>(factoryMethod:() =>
            {
                var gameObj = new GameObject();                                              
                gameObj.Hide();
                gameObj.transform.SetParent(transform);
                return gameObj;
            }
            ,resetMethod: gameObj => 
            { 
                gameObj.Hide(); 
            }
            ,initCount: 5);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                _curGo = mObjectPool.Allocate();
                _curGo.Show();
                _curGo.transform.SetParent(transform);
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                mObjectPool.Recycle(_curGo);
                foreach (Transform t in transform)
                {
                    if (t.gameObject.activeInHierarchy == true)
                    {
                        _curGo = t.gameObject;
                        break;
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                mObjectPool.Clear(go => 
                { 
                    Destroy(go); 
                });
            }

        }
    }
}