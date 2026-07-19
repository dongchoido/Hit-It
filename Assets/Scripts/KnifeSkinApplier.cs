using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class KnifeSkinApplier : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        ApplyCurrentSkin();
        GameEvents.OnKnifeSkinEquipped += HandleSkinEquipped;
    }

    private void OnDisable()
    {
        GameEvents.OnKnifeSkinEquipped -= HandleSkinEquipped;
    }

    private void ApplyCurrentSkin()
    {
        if (KnifeSkinManager.Instance == null) return;
        spriteRenderer.color = KnifeSkinManager.Instance.EquippedColor;
    }

    private void HandleSkinEquipped(Color color)
    {
        spriteRenderer.color = color;
    }
}