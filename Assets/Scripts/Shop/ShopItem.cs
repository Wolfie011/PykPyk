using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "GameObjects/Shop Item", order = 0)]
public class ShopItem : ScriptableObject
{
    //shop item properties
    public string Name = "Default";
    public string Description = "Description";
    public int Level;
    public int Price;
    public CurrencyType Currency;
    public ObjectType Type;
    public Sprite Icon;
    public GameObject Prefab;
    [SerializeField] public Crop crop;
    [SerializeField] public Egg egg;
}

//types of objects - also tabs
public enum ObjectType
{
    AnimalHomes,
    Animals,
    ProductionBuildings,
    Crop,
}
