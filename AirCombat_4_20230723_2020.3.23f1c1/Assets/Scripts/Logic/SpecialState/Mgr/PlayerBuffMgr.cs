using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuffMgr : SpecialStateMgrBase 
{
    
    protected override HashSet<BuffType> GetCanBuffHsh()
    {
        HashSet<BuffType> buffHs = new HashSet<BuffType>();
        buffHs.Add(BuffType.LEVEL_UP);
        return buffHs;
    }

    protected override HashSet<DebuffType> GetCanDebuffHsh()
    {
        return null;
    }
}
