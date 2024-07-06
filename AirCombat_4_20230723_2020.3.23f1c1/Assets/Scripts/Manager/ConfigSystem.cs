using LitJson;
using QFramework;
using QFramework.AirCombat;



public interface IConfigSystem : QFramework.ISystem
{
  public void InitPlaneConfig();
}

public class ConfigSystem : AbstractSystem, IConfigSystem
{
    protected override void OnInit()
    {
        InitPlaneConfig();
    }

    public void InitPlaneConfig()
    {

        #region 说明
        /**
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
        **/
        #endregion
        IReader reader = this.GetUtility<IReaderUtil>().GetReader(ResourcesPath.CONFIG_INIT_PLANE_CONFIG);
        reader[ReaderKey.planes].Get<JsonData>(jsons =>
        {
            foreach (JsonData json in jsons) //每个id的飞机
            {
                foreach (string key in json.Keys)//planeId level attackName等
                {
                    if (key == JsonKey.planeId)
                    {
                        continue;
                    }

                    int planeId = int.Parse(json[JsonKey.planeId].ToJson()); //0

                    string planeId_Key = this.GetUtility<IKeysUtil>().GetPropertyKeys(planeId, key); // 0level
                    JsonData value = json[key];

                    //发生一次只跑了level那一层的
                    if (!this.GetUtility<IStorageUtil>().ContainsKey(planeId_Key)) //初始一般没有
                    {
                        this.GetUtility<IStorageUtil>().SetJsonData(planeId_Key, value); //0level,0
                    }
                }
            }

        });
    }

}

