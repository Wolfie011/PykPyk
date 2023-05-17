using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Egg", menuName = "GameObjects/Egg Item", order = 2)]

public class EggItem : ScriptableObject
{
    public string EggName = "Default";
    public string Description = "Description";
    public int Level;
    public int Price;
    public CurrencyType Currency;
    public EggType Type;
    public Sprite Icon;
    public GameObject Prefab;
}
public enum EggType
{
    Vanilla,
    Color,
    Material
}