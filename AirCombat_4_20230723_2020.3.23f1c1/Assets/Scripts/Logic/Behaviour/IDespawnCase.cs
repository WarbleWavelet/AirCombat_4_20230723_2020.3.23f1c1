/// <summary>被销毁的情况，销毁的明水明Destroy，故此命名</summary>
public interface IDespawnCase
{
    /// <summary>伤害</summary>
    void Injure(int value);
    void Dead();
}