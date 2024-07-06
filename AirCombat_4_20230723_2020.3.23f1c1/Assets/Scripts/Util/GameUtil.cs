using QFramework;
using QFramework.AirCombat;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using Object = UnityEngine.Object;

public interface IGameUtil : QFramework.IUtility
{
    float CameraSizeHeight();
    float CameraSizeWidth();
    Vector2 CameraMinPoint();
    Vector2 CameraMaxPoint();

    //public SubMsgMgr GetSubMsgMgr(Transform _trans);
    void ShowWarnning();
    MessageMgrComponent GetSubMsgMgr(Transform trans);

}


/// <summary>GameUtil中的方法不想pub</summary>
public abstract class GameUtilBase
{
    protected virtual Vector2 GetCameraOrthographicSize()
    {
        return Vector2.zero;    

    }

    protected virtual Camera GetCamera()
    {
        return Camera.main;
    }
}


public class GameUtil  : GameUtilBase, IGameUtil,ICanGetUtility ,ICanGetSystem
{

    private static Vector2 _cameraSize = Vector2.zero;
    private static Camera _camera;
    //
    //缓存一下,避免性能
    float _sizeHeight = 0.0f;
    float _sizeWidth = 0.0f;
    Vector2 _size = Vector2.zero;
    Vector2 _minPoint = Vector2.zero;
    Vector2 _maxPoint = Vector2.zero;
    //
    private IGameUtil mGameUtil//防止打点时老是跳到底层,不好调试 
    { 
        get 
        { 
            return this.GetUtility<IGameUtil>(); 
        } 
    } 


    #region pub IGameUtil


    /// <summary><左下角/summary>
    public Vector2 CameraMinPoint()
    {
        if (GetCamera().IsNotNull() && _minPoint ==Vector2.zero)
        {
            var cameraMinPoint = _camera.OrthographicMinPoint();
            return cameraMinPoint;
        }

        return _minPoint;
    }



    /// <summary>右上角</summary>
     public Vector2 CameraMaxPoint()                          
    {
        if (GetCamera().IsNotNull() && _maxPoint == Vector2.zero)
        {
            var cameraMaxPoint = _camera.OrthographicMaxPoint();
            return cameraMaxPoint;
        }

        return _maxPoint;
    }


        public float CameraSizeHeight()
    {
        if (_sizeHeight == 0)
        {
            _sizeHeight =   mGameUtil.CameraMaxPoint().y - mGameUtil.CameraMinPoint().y;
        }
        return _sizeHeight;
    }

    public float CameraSizeWidth()
    {
        if (_sizeWidth == 0)
        {
            _sizeWidth =  mGameUtil.CameraMaxPoint().x - mGameUtil.CameraMinPoint().x;
        }
        return _sizeWidth;
    }

    public void ShowWarnning()
    {
        var go = this.GetSystem<IGameObjectSystem>().Instantiate(ResourcesPath.PREFAB_WARNING, LoadType.RESOURCES);
        go.SetParent(this.GetSystem<IUISystem>().Common);
        go.Identity();
        go.GetComponent<RectTransform>().Stretch();
        go.GetOrAddComponent<Warning>().Show();
    }

    public MessageMgrComponent GetSubMsgMgr(Transform trans)
    {
        var msg = trans.GetComponentInParent<MessageMgrComponent>();
        if (msg == null)
        {
            var root = trans.GetComponentInParent<IGameRoot>();
            msg = root.Transform.AddComponent<MessageMgrComponent>();
        }
        return msg;
    }
    #endregion


    #region pro GameUtilBase
    protected override  Camera GetCamera()
    {
        if (_camera.IsNull())
        {
            // _camera = Object.FindObjectOfType<Camera>();
            //_camera = GameObject.Find(GameObjectName.UIRoot).GetComponentInChildren<Camera>() ;
            _camera = Camera.main;
            if (_camera.IsNull())
            { 
                Debug.LogError("当前场景中没有相机");
            }

            return _camera;
        }
        else
        {
            return _camera;
        }
    }

    protected override Vector2 GetCameraOrthographicSize()
    {
        if (_cameraSize == Vector2.zero)
        {
            //  var camera = Object.FindObjectOfType<Camera>();
            Camera camera = GetCamera();
            _cameraSize = camera.OrthographicSize();
        }

        return _cameraSize;
    }


    #endregion




    #region 重写
    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }


    #endregion
}