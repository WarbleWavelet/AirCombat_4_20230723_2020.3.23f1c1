using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLifeComponent : MonoBehaviour
{
	private LifeComponent _lifeComponent;
	private int LifeItemCount { get { return transform.childCount; } }

	public EnemyLifeComponent Init(LifeComponent lifeComponent, SpriteRenderer sr)
	{
		_lifeComponent = lifeComponent;
		float tarDRealRatio = GetRatio(sr);
		transform.localScale *= tarDRealRatio;//缩放10个血块
		transform.position=	InitPos(sr); //血块Pos

        InitLifeItems();
		//gameObject.GetOrAddComponent<PRSInspector>();
		return this;
	}
    private void OnDisable()
    {
        foreach (Transform trans in transform) //预制体自带了10个LifeItem
        {
            trans.Show();//这样也行,不用特意地Init,(基类操作了),Show加上去自动Show
        }
    }

    #region pri
    private void InitLifeItems()
	{
		int eachLife = _lifeComponent.LifeMax / LifeItemCount;//摘出来,防重复计算
		foreach (Transform trans in transform) //预制体自带了10个LifeItem
		{	
			trans.GetOrAddComponent<EnemyLifeItem>().Init(eachLife);//这样也行,不用特意地Init,(基类操作了),Show加上去自动Show
		}
	}

	private float GetRatio(SpriteRenderer sr)
	{
		if (sr == null)
			return 0;
		float planeHeight = sr.BoundsSizeY();//用高就很疑惑
		float tarWidth = planeHeight * Const.ENEMY_LIFE_BAR_WIDTH; //  希望的宽度
		int count = transform.childCount;
		float xMin = transform.GetChild(0).GetComponent<SpriteRenderer>().BoundsMinX();
		float xMax = transform.GetChild(count - 1).GetComponent<SpriteRenderer>().BoundsMaxX();
		float realWidth = xMax - xMin;//总体长度
		float ratio = tarWidth / realWidth;
		return ratio;
	}

	private Vector3 InitPos(SpriteRenderer sr)
	{
		if (sr == null)
			throw new System.Exception("异常");
		float yMin = sr.BoundsMinY();
		float xCenter = sr.BoundsCenterX();
		float itemHeight = 0;
		if (transform.childCount > 0)
		{
			itemHeight = transform.GetChild(0).GetComponent<SpriteRenderer>().BoundsSizeY();
		}

		return new Vector3(xCenter, yMin - itemHeight / 2, transform.position.z); //中间靠下,z不变
		//transform.SetLocalPosZ(0);
	}
	#endregion  

	
}