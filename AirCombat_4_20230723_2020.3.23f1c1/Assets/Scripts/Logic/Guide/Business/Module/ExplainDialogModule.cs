using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ExplainDialogModule : MonoBehaviour
{

	public EDir Dir= EDir.LEFT;
	//
	public RectTransform ImgRect;		//一层的兄弟
	private RectTransform _selfRect;	//一层
    private Text _textText;				//二层
	private LayoutElement _textLayout;	//二层
	private RectTransform _bgRect;		//三层
	private float _offset = 20;
	
	// Use this for initialization
	async void Start ()
	{
		Init(ImgRect);
		_textText.BaseBy(placeParent: _selfRect
			, baseRect: ImgRect
			, dir: Dir
			, offset: _offset);
	}



    private void Update()
    {
      
        //Debug.LogFormat("", ImgRect.rect.xMin, ImgRect.rect.xMax);
        //Debug.LogFormat("", ImgRect.rect.yMin, ImgRect.rect.yMax);


    }


	#region 系统


	private void OnGUI1()
    {

       // ImgRect.ToString_Common();
    }
	#endregion	


	#region 辅助



	public async void Init(RectTransform imgRect)
	{
		if (imgRect == null)
		{
			Debug.LogError("目标ui不能为空");
			return;
		}
		
		InitComponent();
		gameObject.Hide();
		await Task.Delay(100);
		gameObject.Show();

	}




    private void InitComponent()
    {
        if (_textText == null)
			_textText = transform.Find("Text").GetComponent<Text>();

		if (_textText != null)
		{
			if (_bgRect == null)
				_bgRect = _textText.transform.Find("Bg").GetComponent<RectTransform>();

			if (_textLayout == null)
				_textLayout = _textText.GetComponent<LayoutElement>();
		}

		if (_selfRect == null)
			_selfRect = GetComponent<RectTransform>();
		
	}
	#endregion


}
