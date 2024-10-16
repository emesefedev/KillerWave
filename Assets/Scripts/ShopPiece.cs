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
            if (shopSelection.upgradeName != "Sold Out")
            {
                 costText.text = shopSelection.cost.ToString(); 
            }
            else
            {
                costText.text = "SOLD OUT";
                sold = true;
            }
        }
    }
}
