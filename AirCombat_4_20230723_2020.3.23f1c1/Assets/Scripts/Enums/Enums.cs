

/// <summary>OrderInLayer也是这个顺序</summary>
public enum EnemyType
{
	NORMAL,
	ELITES,
	BOSS,
	ITEM,
	COUNT 
}


/// <summary>场景名，维持原来的格式</summary>
public enum ESceneName
{
	NULL,
	Main,
	Game,
	COUNT
}


public enum HandMode
{
	RIGHT,
	LEFT
}


public enum LevelState
{ 
	 NULL,
	 START,
	 END
}
public enum GameState
{
	NULL,
	START,
	PAUSE,
	CONTINUE,
	END
}




#region 说明
/// <summary>原名ItemType,但它不处理Star(不好统一处理),所以让名,改为RewardType</summary>
public enum RewardType
{
	ADD_BULLET,
	ADD_EXP,
	SHIELD,
	POWER
}

/// <summary>杀死敌人后的Item奖励.本来就用ItemType,弃用</summary>
public enum EItemType
{
	STAR,
    /// <summary>实际是一列变两列三列</summary>
  	ADD_BULLET,
    ADD_EXP,
    SHIELD,
    /// <summary>原来叫POWER(改过Bomb),图片路径是Assets / Resources / Picture / Item / Power.png</summary>
    POWER,
    //TODO 
    BOSSBULLET1

}
#endregion
