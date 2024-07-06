using QFramework;
using QFramework.AirCombat;
using UnityEngine;

public class PlayerController : MonoBehaviour   ,ICanGetUtility  ,ICanGetSystem   ,IInit,IDestroy
{
    private MoveSelfComponent _move;

    //飞机中心点到边界的差值
    private Vector2 _offset;
    private SpriteRenderer _renderer;


    // Use this for initialization
    private void Start()
    {
        this.GetSystem<ILifeCycleSystem>().Add(LifeName.INIT,this);
        this.GetSystem<ILifeCycleSystem>().Add(LifeName.DESTROY,this);
    }


    #region IInit,IDestroy

    public void Init()
    {
        _move = GetComponent<MoveSelfComponent>();
        _renderer = GetComponent<SpriteRenderer>();
        this.GetSystem<IInputSystem>().AddListener(KeyCode.W);
        this.GetSystem<IInputSystem>().AddListener(KeyCode.A);
        this.GetSystem<IInputSystem>().AddListener(KeyCode.S);
        this.GetSystem<IInputSystem>().AddListener(KeyCode.D);

        this.GetSystem<IInputSystem>().AddListener(KeyCode.W, KeyState.PREE, ReveiveW);
        this.GetSystem<IInputSystem>().AddListener(KeyCode.A, KeyState.PREE, ReveiveA);
        this.GetSystem<IInputSystem>().AddListener(KeyCode.S, KeyState.PREE, ReveiveS);
        this.GetSystem<IInputSystem>().AddListener(KeyCode.D, KeyState.PREE, ReveiveD);
        InitData();
    }

    public void DestroyFunc()
    {
        this.GetSystem<IInputSystem>().RemoveListener(KeyCode.W);
        this.GetSystem<IInputSystem>().RemoveListener(KeyCode.A);
        this.GetSystem<IInputSystem>().RemoveListener(KeyCode.S);
        this.GetSystem<IInputSystem>().RemoveListener(KeyCode.D);

        this.GetSystem<IInputSystem>().RemoveListener(KeyCode.W, KeyState.PREE, ReveiveW);
        this.GetSystem<IInputSystem>().RemoveListener(KeyCode.A, KeyState.PREE, ReveiveA);
        this.GetSystem<IInputSystem>().RemoveListener(KeyCode.S, KeyState.PREE, ReveiveS);
        this.GetSystem<IInputSystem>().RemoveListener(KeyCode.D, KeyState.PREE, ReveiveD);
    }
    #endregion



    #region pri
    private void InitData()
    {
        _offset = transform.position - _renderer.bounds.min;
    }

    void ReveiveW(params object[] args)
    {
        if (!JudgeUpBorder()) _move.Move(Vector2.up);
    }

     void ReveiveA(params object[] args)
    {
        if (!JudgeLeftBorder()) _move.Move(Vector2.left);
    }

     void ReveiveS(params object[] args)
    {
        if (!JudgeDownBorder()) _move.Move(Vector2.down);
    }

     void ReveiveD(params object[] args)
    {
        if (!JudgeRightBorder()) _move.Move(Vector2.right);
    }

    private bool JudgeUpBorder()
    {
        return _renderer.bounds.max.y >= this.GetUtility<IGameUtil>().CameraMaxPoint().y;
    }

    private bool JudgeDownBorder()
    {
        return _renderer.bounds.min.y <= this.GetUtility<IGameUtil>().CameraMinPoint().y;
    }

    private bool JudgeLeftBorder()
    {
        return _renderer.bounds.min.x <= this.GetUtility<IGameUtil>().CameraMinPoint().x;
    }

    private bool JudgeRightBorder()
    {
        return _renderer.bounds.max.x >= this.GetUtility<IGameUtil>().CameraMaxPoint().x;
    }

    private void ResetPosX(Vector2 border, Vector2 direction)
    {
        var pos = transform.localPosition;
        pos.z = 0;
        pos.x = border.x - Vector2.Dot(_offset, direction);
        transform.localPosition = pos;
    }

    private void ResetPosY(Vector2 border, Vector2 direction)
    {
        var pos = transform.localPosition;
        pos.z = 0;
        pos.y = border.y - Vector2.Dot(_offset, direction);
        transform.localPosition = pos;
    }

    private void Drag(Vector3 screenPos)
    {
        var pos = Camera.main.ScreenToWorldPoint(screenPos);
        pos.z = 0;
        transform.localPosition = pos;
    }
    #endregion



    #region 系统
    private void OnMouseDrag()
    {
#if UNITY_EDITOR
        Drag(Input.mousePosition);
#else
		if (Input.touches.Length > 0)
		{
			Drag(Input.touches[0].position);
		}
#endif

        if (JudgeUpBorder())
        {
            ResetPosY(this.GetUtility<IGameUtil>().CameraMaxPoint(), Vector2.up);
        }
        else if (JudgeDownBorder())
        { 
            ResetPosY(this.GetUtility<IGameUtil>().CameraMinPoint(), Vector2.down);
        }

        if (JudgeLeftBorder())
        {
            ResetPosX(this.GetUtility<IGameUtil>().CameraMinPoint(), Vector2.left);
        }
        else if (JudgeRightBorder())
        { 
            ResetPosX(this.GetUtility<IGameUtil>().CameraMaxPoint(), Vector2.right);
        }
    }


    #endregion


    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }


}