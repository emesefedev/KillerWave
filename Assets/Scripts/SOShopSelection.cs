using UnityEngine;

[CreateAssetMenu(fileName = "New Shop Piece", menuName = "Shop Piece")]
public class SOShopSelection : ScriptableObject
{
    public Sprite icon;
    public string iconName;
    public string description;
    public int cost;
}
