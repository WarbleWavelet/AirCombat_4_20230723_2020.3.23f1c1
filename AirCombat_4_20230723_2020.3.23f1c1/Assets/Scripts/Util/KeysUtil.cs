using QFramework.AirCombat;
using QFramework;
using System.Linq;


public interface IKeysUtil : QFramework.IUtility
{
	public string GetPropertyKeysWithoutPlaneID(string name);
	/// <summary>包括了等级 0level,attack,fireRate,life</summary>
	public string GetPropertyKeys(string name);
	/// <summary>包括了等级 0level,attack,fireRate,life</summary>
	public string GetPropertyKeys(int id, string name);
	public string GetNewKey(PropertyItem.ItemKey key, string propertyName);

}
public  class KeysUtil: IKeysUtil ,ICanGetModel
{

	#region pub


	public string GetPropertyKeysWithoutPlaneID(string name)//level 
	{
	  return  this.GetModel<IAirCombatAppStateModel>().SelectedPlaneID + name;//0level
	}

	public  string GetPropertyKeys(string key)//level 
	{
		char c = key.FirstOrDefault();
		int i;
		if (!int.TryParse(c.ToString(), out i))	//
		{

			throw new System.Exception("异常:类,类似于:0level,0attack,0fireRate,0life");
		}
		return  key; //0level
	}

	public  string GetPropertyKeys(int id, string name)
	{
		return id + name; //0+level
	}

	public  string GetNewKey(PropertyItem.ItemKey key   , string propertyName)
	{
		
		return GetPropertyKeysWithoutPlaneID(propertyName + key);
	}
	#endregion  



	#region 重写
	public IArchitecture GetArchitecture()
	{
		return AirCombatApp.Interface;
	}
	#endregion
}