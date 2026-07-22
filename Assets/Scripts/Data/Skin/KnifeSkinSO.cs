using UnityEngine;

public enum SkinRarity { Common, Rare, Epic, Legendary }

[CreateAssetMenu(fileName = "KnifeSkinSO", menuName = "KnifeHit/Knife Skin")]
public class KnifeSkinSO : ScriptableObject
{
    public string skinId;
    public string displayName;
    public Sprite skinSprite;
    public SkinRarity rarity = SkinRarity.Common;
    public int shopCost;
}