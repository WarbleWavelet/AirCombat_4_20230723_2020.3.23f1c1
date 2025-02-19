﻿using System;
using UnityEngine;

namespace QFramework.Example
{
    public class CustomPanelLoaderExample : MonoBehaviour
    {
        void Start()
        {
            // 游戏启动时，设置一次
            UIKit.Config.PanelLoaderPool = new ResourcesPanelLoaderPool();
        }


        #region 内部类


        public class ResourcesPanelLoaderPool : AbstractPanelLoaderPool
        {
            /// <summary>
            /// LoadTime Panel from Resources
            /// </summary>
            public class ResourcesPanelLoader : IPanelLoader
            {
                private GameObject mPanelPrefab;

                public GameObject LoadPanelPrefab(PanelSearchKeys panelSearchKeys)
                {
                    mPanelPrefab = Resources.Load<GameObject>(panelSearchKeys.GameObjName);
                    return mPanelPrefab;
                }

                public void LoadPanelPrefabAsync(PanelSearchKeys panelSearchKeys
                    , Action<GameObject> onPanelLoad)
                {
                    var request = Resources.LoadAsync<GameObject>(panelSearchKeys.GameObjName);

                    request.completed += operation => { onPanelLoad(request.asset as GameObject); };
                }

                public void Unload()
                {
                    mPanelPrefab = null;
                }
            }

            protected override IPanelLoader CreatePanelLoader()
            {
                return new ResourcesPanelLoader();
            }
        }
        #endregion
    }
}
