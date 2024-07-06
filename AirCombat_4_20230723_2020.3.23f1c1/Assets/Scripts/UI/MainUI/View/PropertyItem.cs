using QFramework;
using QFramework.AirCombat;
using UnityEngine;
using UnityEngine.UI;


/// <summary>升级界面的属性条</summary>
public class PropertyItem : MonoBehaviour, IViewUpdate, IViewShow  ,QFramework.IController
{
    public enum ItemKey
    {
        /// <summary>目前有攻击，攻速，生命</summary>
        name,
        value,
        cost,
        /// <summary>前面属性的长度</summary>
        grouth,
        /// <summary>关联着json，要改一起改</summary>   
        maxVaue
    }

    private static int _itemId = -1;
    private string _key;

    public void Show()
    {
        UpdatePlaneId(this.GetModel<IAirCombatAppStateModel>().SelectedPlaneID);
    }

    public int Framing { get; set; }

    public int Frame { get; }

    public void FrameUpdate()
    {
        UpdatePlaneId(this.GetModel<IAirCombatAppStateModel>().SelectedPlaneID);
    }

    public void Init(string key)
    {
        _key = key;
        _itemId++;
        GetComponent<RectTransform>().VerticalGroup(_itemId);
    }



    private void UpdatePlaneId(int planeId)
    {
        UpdateData(planeId);
        UpdateSlider();
    }

    private void UpdateData(int planeId)
    {

        #region 说明
        /**
        {
            "planes": [
              {
                "planeId": 0,
                "level": 0,
                "attackTime":1,
                "attack": { "name":"攻击","value":5,"cost":200,"costUnit":"star","grouth":10,"maxVaue": 500},
                "fireRate": { "name":"攻速","value":80,"cost":200,"costUnit":"star","grouth":1,"maxVaue": 100},
                "life": { "name":"生命","value":100,"cost":200,"costUnit":"star","grouth":50,"maxVaue": 1000},
                "upgrades": { "name":"升级","coefficient": 2,"max":4,"0": 100,"1": 200,"2": 300,"3": 400,"costUnit":"diamond"}
                    },
        ......
            ],
            "planeSpeed": 1.2
        }
        **/
        #endregion
        for (ItemKey i = 0; i < ItemKey.grouth; i++)
        {
            var trans = transform.Find(UpperFirstLetter(i));
            if (trans != null)
            {
                var key = this.GetUtility<IKeysUtil>().GetPropertyKeysWithoutPlaneID(_key + i);
                Debug.Log("***" + key);
                var value=   this.GetUtility<IStorageUtil>().GetObject(key).ToString();
                Debug.Log("***" + key + "," + value);
                trans.SetText(value) ;
            }
            else
            {
                Debug.LogError("当前预制名称错误，正确名称：" + UpperFirstLetter(i));
            }
        }
    }

    private void UpdateSlider()
    {
        var slider = transform.Find(GameObjectName.Slider).GetComponent<Slider>();
        slider.minValue = 0;
        slider.maxValue = this.GetUtility<IStorageUtil>().Get<int>(this.GetUtility<IKeysUtil>().GetNewKey(ItemKey.maxVaue, _key));
        slider.value = this.GetUtility<IStorageUtil>().Get<int>(this.GetUtility<IKeysUtil>().GetNewKey(ItemKey.value, _key));
    }


    /// <summary>头一个字母大写</summary>
    public static string UpperFirstLetter(ItemKey key)
    {
        return key.ToString().UpperFirstLetter();
    }

    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }
}