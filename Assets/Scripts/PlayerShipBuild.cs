using UnityEngine;
using UnityEngine.UI;

public class PlayerShipBuild : MonoBehaviour
{
    [SerializeField] private GameObject[] shopButtons;
    [SerializeField] private TextMesh textBoxName;
    [SerializeField] private TextMesh textBoxDescription;

    [SerializeField] private SOActorModel defaultPlayerShip;
    [SerializeField] private GameObject[] visualWeapons;
    [SerializeField] private GameObject[] weaponsPrefabs;
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
        foreach (GameObject visualWeapon in visualWeapons)
        {
            visualWeapon.SetActive(false);
        }
    }

    private void PreparePlayerShipForUpgrade()
    {
        playerShip = Instantiate(defaultPlayerShip.actor);

        playerShip.GetComponent<Player>().enabled = false;
        playerShip.transform.position = new Vector3(0, 10000, 0);
        playerShip.GetComponent<IActorTemplate>().ActorStats(defaultPlayerShip);
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
                // TODO: Mejorar la forma de identificar los botones
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
                else if (target.name.Equals("Watch Ad"))
                {
                    WatchAd();
                }
                else if (target.name.Equals("Start"))
                {
                    StartGame();
                }
                else if (target.name.Equals("Buy Button"))
                {
                    BuyUpgrade();
                }
            }
        }
    }

    private void WatchAd()
    {

    }

    private void BuyUpgrade()
    {
        Debug.Log("Purchase made");
        purchaseMade = true;
        buyButton.SetActive(false);
        currentSelection.SetActive(false);

        //TODO: Mejorar esto para que no dependa de los nombres (strings) de los SO
        ShopPiece currentShopPiece = currentSelection.GetComponentInParent<ShopPiece>();
        for (int i = 0; i < visualWeapons.Length; i++)
        {   
            if (visualWeapons[i].name.Equals(currentShopPiece.ShopSelection.iconName))
            {
                visualWeapons[i].SetActive(true);
            }
        }

        UpgradeShip(currentShopPiece);

        bank -= currentShopPiece.ShopSelection.cost;
        UpdateBankText();

        currentSelection.transform.parent.transform.GetComponentInChildren<TextMesh>().text = "SOLD";
    }

    //TODO: Mejorar la forma en que se relacionan los prefabs con la mejora elegida
    private void UpgradeShip(ShopPiece shopPiece)
    {
        foreach (GameObject weapon in weaponsPrefabs)
        {
            GameObject shipUgrade;
            if (weapon.name.Equals(shopPiece.ShopSelection.iconName))
            {
                shipUgrade = Instantiate(weapon);
                shipUgrade.transform.SetParent(playerShip.transform);
                shipUgrade.transform.localPosition = Vector3.zero;
            }
        }
    }

    private void StartGame()
    {

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
