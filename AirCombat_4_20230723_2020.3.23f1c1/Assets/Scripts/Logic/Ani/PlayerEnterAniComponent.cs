using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using QFramework;
using QFramework.AirCombat;
using UnityEngine;


/// <summary>结束后PlayerCanControl</summary>
public class PlayerEnterAniComponent : MonoBehaviour,QFramework.IController ,IInitComponent<PlayerEnterAniComponent>
{
    public IArchitecture GetArchitecture()
    {
		return AirCombatApp.Interface;
    }
    private readonly float _startPosY = -2.0f;

    public bool Arrived = false;



    private void Start()
    {

    }
    // Use this for initialization
    public PlayerEnterAniComponent InitComponent()
	{
		
		Camera camera = transform.MainOrOtherCamera();
		Vector2 minSize = camera.CameraSizeMin();
        float yMin = minSize.y;
		float boundsSizeY = GetComponent<SpriteRenderer>().BoundsSizeY();
        float fromY = yMin - boundsSizeY * 0.5f;
         float toY	= yMin + boundsSizeY; //0.5刚刚贴着，再加0.5多一小段距离,也就是1
		//
        Vector3 pos = transform.position;
        Vector3 toPos = pos.SetY(toY);

        //
        transform.SetPosY(fromY); //开始在下面刚刚看不到的地方
		float cameraSpeed = this.GetModel<IAirCombatAppStateModel>().CameraSpeed;
		float time = 1;
		transform
			.DOMove(toPos + Vector3.up * cameraSpeed * time, time)   //玩家、相机相对静止
			.OnComplete(()=>
			{
				this.SendCommand<PlayerCanControlCommand>();
                Arrived=true;
                GameObject.Destroy(this);
            });

		return this;
	}

}
