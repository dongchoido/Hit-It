using UnityEngine;

[CreateAssetMenu(fileName = "KnifeSkinSO", menuName = "KnifeHit/Knife Skin")]
public class KnifeSkinSO : ScriptableObject
{
    public string skinId;
    public string displayName;
    public Color tintColor = Color.white;
    public int cost;
}