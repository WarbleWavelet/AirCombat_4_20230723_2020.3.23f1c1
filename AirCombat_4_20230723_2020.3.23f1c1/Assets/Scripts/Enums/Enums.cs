

/// <summary>OrderInLayerҲ�����˳��</summary>
public enum EnemyType
{
	NORMAL,
	ELITES,
	BOSS,
	ITEM,
	COUNT 
}


/// <summary>��������ά��ԭ���ĸ�ʽ</summary>
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




#region ˵��
/// <summary>ԭ��ItemType,����������Star(����ͳһ����),��������,��ΪRewardType</summary>
public enum RewardType
{
	ADD_BULLET,
	ADD_EXP,
	SHIELD,
	POWER
}

/// <summary>ɱ�����˺��Item����.��������ItemType,����</summary>
public enum EItemType
{
	STAR,
    /// <summary>ʵ����һ�б���������</summary>
  	ADD_BULLET,
    ADD_EXP,
    SHIELD,
    /// <summary>ԭ����POWER(�Ĺ�Bomb),ͼƬ·����Assets / Resources / Picture / Item / Power.png</summary>
    POWER,
    //TODO 
    BOSSBULLET1

}
#endregion
