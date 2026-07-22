using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KnifeSkinLibrarySO", menuName = "KnifeHit/Knife Skin Library")]
public class KnifeSkinLibrarySO : ScriptableObject
{
    public List<KnifeSkinSO> skins = new List<KnifeSkinSO>();
}