using System;
using UnityEngine;

namespace QFramework.Example
{
    public class MultiPanelExample : MonoBehaviour
    {

        private UIMultiPanel mUIMultiPanel;

        private int mPageIndex = 0; 



        void Start()
        {
            ResKit.Init();
        }


#if UNITY_EDITOR
        private void OnGUI()
        {
            if (GUILayout.Button("打开(此时才会更新计数)"))
            {
                mUIMultiPanel = UIKit.OpenPanel<UIMultiPanel>(
                    new UIMultiPanelData() { PageIndex = mPageIndex }, 
                    PanelOpenType.Single
                );

                mPageIndex++;
            }
            
            if (GUILayout.Button("关闭"))
            {
                UIKit.ClosePanel<UIMultiPanel>();
            }
            
            if (mUIMultiPanel && GUILayout.Button("关闭当前"))
            {
                UIKit.ClosePanel(mUIMultiPanel);
                mUIMultiPanel = null;
            }
        }
#endif


    }
}