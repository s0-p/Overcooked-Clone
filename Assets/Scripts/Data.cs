using System;

public enum eCOOKERY
{
    None = 0,
    Cutting,
    Boil,
    Gril,
    Blend,
    Bake,
    Count
}
public enum eINGREDIENT
{
    None = -1,
    Fish,
    Shrimp,
    Rice,
    Laver,
    Cucumber
}
[Serializable]
public struct SIngredient
{
    public int bitId;
    public string name;
    public int cookeryId;
}
[Serializable]
public struct SMenu
{
    public int bitId;
    public string name;

    public int ingredientsBit;
}

[Serializable]
public struct SStage
{
    public int id;
    public int chapter;
    public int stage;

    public int limitedTime;

    public int goalProfits1;
    public int goalProfits2;
    public int goalProfits3;

    public int menusBit;
}

[Serializable]
public class JsonWrapper<T>
{
    public T[] items;
}