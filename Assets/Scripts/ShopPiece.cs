using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopPiece : MonoBehaviour
{
    [SerializeField] private SOShopSelection shopSelection;
    [SerializeField] private Image powerupIconImage;
    [SerializeField] private TextMeshProUGUI costText;

    private bool sold;

    public SOShopSelection ShopSelection {
        get { return shopSelection; }
        set { shopSelection = value; }
    }

    public bool Sold {
        get { return sold; }
        set { sold = value; }
    }

    private void Awake()
    {
        if (powerupIconImage != null)
        {
            powerupIconImage.sprite = shopSelection.icon;
        }

        if (costText != null)
        {
            costText.text = shopSelection.upgradeName != "Sold Out" 
                ? shopSelection.cost.ToString()
                : "SOLD OUT";
        }
    }
}
