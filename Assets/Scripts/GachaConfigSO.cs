using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GachaConfigSO", menuName = "KnifeHit/Gacha Config")]
public class GachaConfigSO : ScriptableObject
{
    public List<GachaEntry> entries = new List<GachaEntry>();
}