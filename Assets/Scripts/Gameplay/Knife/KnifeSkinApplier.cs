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
        if (KnifeSkinManager.Instance.EquippedSprite == null) return;
        spriteRenderer.sprite = KnifeSkinManager.Instance.EquippedSprite;
    }

    private void HandleSkinEquipped(Sprite sprite)
    {
        if (sprite == null) return;
        spriteRenderer.sprite = sprite;
    }
}