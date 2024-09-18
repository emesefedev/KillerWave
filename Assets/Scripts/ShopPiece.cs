using UnityEngine;

public class ShopPiece : MonoBehaviour
{
    [SerializeField] private SOShopSelection shopSelection;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private TextMesh costText;

    public SOShopSelection ShopSelection {
        get { return shopSelection; }
        set { shopSelection = value; }
    }

    private void Awake()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = shopSelection.icon;
        }

        if (costText != null)
        {
            costText.text = shopSelection.cost.ToString();
        }
    }
}
