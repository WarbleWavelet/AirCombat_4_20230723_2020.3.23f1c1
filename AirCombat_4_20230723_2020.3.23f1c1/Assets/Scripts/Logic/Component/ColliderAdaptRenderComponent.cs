using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ColliderAdaptRenderComponent : MonoBehaviour 
{
	
	public SpriteRenderer SpriteRenderer { get;private set;}
	private CapsuleCollider2D _capsuleCollider;

	public ColliderAdaptRenderComponent InitComponent(SpriteRenderer sr,CapsuleCollider2D capsuleCollider2D)
	{

		if (ExtendJudge.IsOnceNull(SpriteRenderer, _capsuleCollider))
		{
			SpriteRenderer = sr;
			_capsuleCollider = capsuleCollider2D;

        }


		return this;
	}

	public bool SetSprite(Sprite sprite)
	{
		if (SpriteRenderer != null)
		{
			bool success = false;
			success = SpriteRenderer.sprite != sprite;


			Vector2 ratio = SpriteRenderer.Ratio(sprite);
			_capsuleCollider.Adapt(ratio);

            return success;
		}
		else
		{
			return false;
			Debug.LogError("当前物体SpriteRenderer为空，物体名:"+gameObject.name);
		}
	}


}
