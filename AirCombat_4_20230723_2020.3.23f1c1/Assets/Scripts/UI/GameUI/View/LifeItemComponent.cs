using QFramework;
using QFramework.AirCombat;

public class LifeItemComponent : ViewBase, QFramework.IController
{
    private int _lifeMin;
    private MessageMgrComponent _messageMgrComponent;
    protected override void InitChild()
    {
        var id = int.Parse(transform.name);
        SetItemPos(id);
        _lifeMin = GetLifeMin(id);
        _messageMgrComponent=transform.GetComponentInParentRecent<MessageMgrComponent>(); 
    }



    #region pri
    private int GetLifeMin(int id)
    {
        var eachLife =   this.GetModel<IAirCombatAppModel>().LifeMax / Const.LIFE_ITEM_NUM;
        return id * eachLife;
    }

    private void SetItemPos(int id)
    {
        var rect = transform.Rect();
        var width = rect.rect.height;
        var pos = rect.anchoredPosition;
        pos.x = width / 2 + width * id;
        rect.anchoredPosition = pos;
    }
    #endregion


    public override void Show()
    {
        base.Show();
        _messageMgrComponent.AddListener(MsgEvent.EVENT_HP, ReceiveMessage);
    }

    public override void Hide()
    {
        base.Hide();
        _messageMgrComponent.RemoveListener(MsgEvent.EVENT_HP, ReceiveMessage);
    }

    public void ReceiveMessage(params object[] args)
    {
        var life = 0;
        life = this.GetModel<IAirCombatAppModel>().Life;
        if (life < _lifeMin) 
            Hide();
    }

    #region 重写
    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }
    #endregion
}