using QFramework;
using QFramework.AirCombat;
using UnityEngine;

public class LevelItem : ViewBase ,QFramework.ICanGetUtility
{
    private int _id;
    private readonly int _leftOffet = 50;
    private readonly int _lineSpacing = 10;
    private readonly int _offset = 10;

    protected override void InitChild()
    {
        _id = transform.GetSiblingIndex();
        SetLevelId();
        var isOpen = JudgeOpenState();
        SetMaskState(isOpen);
        InitPos();
    }


    #region pri

    private void SetLevelId()
    {
        UiUtil.Get(GameObjectPath.Enter_Text).SetText(_id + 1);
    }


    #region 解锁
    private bool JudgeOpenState()
    {
        var passed = -1;
        if (this.GetUtility<IStorageUtil>().ContainsKey(DataKeys.LEVEL_PASSED))
            passed = this.GetUtility<IStorageUtil>().Get<int>(DataKeys.LEVEL_PASSED);

        return _id <= passed + 1;
    }

    private void SetMaskState(bool isOpen)
    {
        UiUtil.Get(GameObjectName.Mask).GameObject.SetActive(!isOpen);
    }
    #endregion



    #region 排列
    private void InitPos()
    {
        var reader = this.GetUtility<IReaderUtil>().GetReader(ResourcesPath.CONFIG_LEVEL_CONFIG);
        reader[ReaderKey.eachRow].Get<int>(data =>
        {
            var grid = GetGrid(data);
            SetPos(grid);
        });
    }

    private Vector2 GetGrid(int eachRow)
    {
        var y = _id / eachRow;
        var x = _id % eachRow;
        return new Vector2(x, y);
    }

    private void SetPos(Vector2 gridId)
    {
        var width = transform.Rect().rect.width * transform.localScale.x;
        var height = transform.Rect().rect.height * transform.localScale.y;

        var indention = gridId.y % 2 == 0 ? _leftOffet : 0;

        var x = indention + width * 0.5f + (_offset + width) * gridId.x;
        var y = height * 0.5f + (_lineSpacing + height) * gridId.y;
        transform.Rect().anchoredPosition = new Vector2(x, -y);
    }

    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }
    #endregion


    #endregion



}