using UnityEngine;
using UnityEngine.UI;

public class PlayerShipBuild : MonoBehaviour
{
    [SerializeField] private GameObject[] shopButtons;
    [SerializeField] private TextMesh textBoxName;
    [SerializeField] private TextMesh textBoxDescription;

    [SerializeField] private SOActorModel defaultPlayerShip;
    [SerializeField] private GameObject[] visualWeapons;
    private GameObject playerShip;
    
    [SerializeField] private GameObject buyButton;
    [SerializeField] private TextMesh bankText;
    private int bank = 600;
    private bool purchaseMade;


    private GameObject target;
    private GameObject currentSelection;

    private void Start()
    {
        purchaseMade = false;

        TurnOffSelectionHighlights();
        
        UpdateBankText();
        
        TurnOffPlayerShipVisuals();
        PreparePlayerShipForUpgrade();
    }

    private void Update()
    {
        AttemptSelection();
    }

    private void TurnOffPlayerShipVisuals()
    {

    }

    private void PreparePlayerShipForUpgrade()
    {

    }

    private void UpdateBankText()
    {
        bankText.text = bank.ToString();
    }

    private GameObject ReturnClickedObject(out RaycastHit hit)
    {
        GameObject clickedObject = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray.origin, ray.direction * 100, out hit))
        {
            clickedObject = hit.collider.gameObject;
        }
        return clickedObject;
    }

    private void AttemptSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            target = ReturnClickedObject(out hit);
            Debug.Log(target);

            if (target != null)
            {
                if (target.name.Contains("Upgrade"))
                {
                    TurnOffSelectionHighlights();
                    Select();
                    UpdateTextBoxPanel();

                    string costText = target.transform.GetChild(1).GetComponent<TextMesh>().text;
                    if (costText != "SOLD") 
                    {
                        CheckTargetAffordable();
                    }
                    else 
                    {
                        SoldOut();
                    }
                }
            }
        }
    }

    private void CheckTargetAffordable()
    {
        ShopPiece targetShopPiece = target.GetComponent<ShopPiece>();
        int targetCost = targetShopPiece.ShopSelection.cost;

        if (bank >= targetCost)
        {
            Debug.Log("Can Buy");
            buyButton.SetActive(true);
        } 
        else 
        {
            Debug.Log("Can't Buy");
        }
    }

    private void SoldOut()
    {
        Debug.Log("SOLD OUT");
    }

    private void Select()
    {
        currentSelection = target.transform.GetChild(2).gameObject;
        currentSelection.SetActive(true);
    }

    private void UpdateTextBoxPanel()
    {
        ShopPiece currentShopPiece = currentSelection.GetComponentInParent<ShopPiece>();
        textBoxName.text = currentShopPiece.ShopSelection.iconName;
        textBoxDescription.text = currentShopPiece.ShopSelection.description;
    }

    private void TurnOffSelectionHighlights()
    {
        foreach (GameObject shopButton in shopButtons)
        {
            shopButton.SetActive(false);
        }
    }
}
