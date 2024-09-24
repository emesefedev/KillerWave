using UnityEngine;
using UnityEngine.Advertisements;
using System.Collections;
using UnityEditor.SearchService;

public class PlayerShipBuild : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] private GameObject[] shopButtons;
    private GameObject target;
    private GameObject currentSelection;
    private int rayDistance = 100;
    
    [SerializeField] private TextMesh infoPanelName;
    [SerializeField] private TextMesh infoPanelDescription;

    [SerializeField] private SOActorModel defaultPlayerShip;
    [SerializeField] private GameObject[] visualUpgrades;
    [SerializeField] private GameObject[] upgradePrefabs;
    private GameObject playerShip;
    
    [SerializeField] private GameObject buyButton;
    [SerializeField] private TextMesh bankText;
    private int bank = 2000;
    private bool purchaseMade;

    [SerializeField] private string androidGameId;
    [SerializeField] private string iosGameId;
    [SerializeField] private bool testMode = true;
    private string adId;
    private int watchedAdAward = 300;

    private void Awake()
    {
        CheckPlatform();
    }

    private void Start()
    {
        purchaseMade = false;

        TurnOffSelectionHighlights();
        
        UpdateBankText();
        
        TurnOffPlayerShipVisualUpgrades();
        PreparePlayerShipForUpgrade();

        StartCoroutine(WaitForAd());
    }

    private void Update()
    {
        AttemptSelection();
    }

    #region ADVERTISEMENT
    private void CheckPlatform()
    {
        string gameId = null;

        #if UNITY_IOS
        {
            gameId = iosGameId;
            adId = "Rewarded_iOS";
        }
        #elif UNITY_ANDROID
        {
            gameId = androidGameId;
            adId = "Rewarded_Android";
        }
        #endif

        Advertisement.Initialize(gameId, testMode, this);
    }

    private IEnumerator WaitForAd()
    {
        while (!Advertisement.isInitialized)
        {
            yield return null;
        }

        LoadAd();
    }

    private void LoadAd()
    {
        Advertisement.Load(adId, this);
    }

    private void WatchAd()
    {
        Advertisement.Show(adId, this);
    }

    public void OnInitializationComplete()
    {
       Debug.Log("Unity Ads initialization complete"); 
    }  

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads initialization failed: {error} - {message}"); 
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Unity Ads show failed: {error} - {message}"); 
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        switch (showCompletionState)
        {
            case UnityAdsShowCompletionState.COMPLETED:
                Debug.Log("Unity Ads reward completed");
                UpdateBank(watchedAdAward);
                break;
            case UnityAdsShowCompletionState.SKIPPED |  UnityAdsShowCompletionState.UNKNOWN:
                // DON'T REWARD PLAYER
                break;
        }

        Advertisement.Load(adId, this);
        TurnOffSelectionHighlights();
    }

    #endregion 

    #region SHOP BUTTONS
    private void TurnOffSelectionHighlights()
    {
        foreach (GameObject shopButton in shopButtons)
        {
            shopButton.SetActive(false);
        }
    }

    private void AttemptSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            target = ReturnClickedObject(out hit); // TODO: ¿El out RaycastHit se utiliza para algo? Comprobar tras finalizar

            if (target != null)
            {
                // TODO: Mejorar la forma de identificar los botones
                ShopPiece shopPiece = target.GetComponent<ShopPiece>();
                if (shopPiece)
                {
                    SelectUpgrade(shopPiece);
                }
                else if (target.name.Equals("Buy Button"))
                {
                    BuyUpgrade();
                }
                else if (target.name.Equals("Start"))
                {
                    StartGame();
                } 
                else if (target.name.Equals("Watch Ad"))
                {
                    WatchAd();
                }  
            }
        }
    }

    private GameObject ReturnClickedObject(out RaycastHit hit)
    {
        GameObject clickedObject = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray.origin, ray.direction * rayDistance, out hit))
        {
            clickedObject = hit.collider.gameObject;
        }

        return clickedObject;
    }

    private void SelectUpgrade(ShopPiece shopPiece)
    {
        TurnOffSelectionHighlights();
        Select(); 
        UpdateInfoPanel(shopPiece);

        //string costText = target.transform.GetChild(1).GetComponent<TextMesh>().text;
        if (!shopPiece.Sold)//(costText != "SOLD") 
        {
            CheckUpgradeAffordable(shopPiece);
        }
        else 
        {
            UpgradeSoldOut();
        }
    }

    private void Select()
    {
        currentSelection = target.transform.GetChild(2).gameObject;
        currentSelection.SetActive(true);
    }

    private void UpdateInfoPanel(ShopPiece shopPiece)
    {
        infoPanelName.text = shopPiece.ShopSelection.upgradeName;
        infoPanelDescription.text = shopPiece.ShopSelection.description;
    }

    private void CheckUpgradeAffordable(ShopPiece shopPiece)
    {
        int targetCost = shopPiece.ShopSelection.cost;

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

    private void UpgradeSoldOut()
    {
        Debug.Log("SOLD OUT");
    }

    private void BuyUpgrade()
    {
        Debug.Log("Purchase made");
        purchaseMade = true;

        buyButton.SetActive(false);
        currentSelection.SetActive(false);

        //TODO: Mejorar esto para que no dependa de los nombres (strings) de los SO
        ShopPiece currentShopPiece = currentSelection.GetComponentInParent<ShopPiece>();
        for (int i = 0; i < visualUpgrades.Length; i++)
        {   
            if (visualUpgrades[i].name.Equals(currentShopPiece.ShopSelection.upgradeName))
            {
                visualUpgrades[i].SetActive(true);
            }
        }

        UpgradeShip(currentShopPiece);

        UpdateBank(-currentShopPiece.ShopSelection.cost);

        SoldUpgrade(currentShopPiece);
    }

    private void StartGame()
    {
        if (purchaseMade)
        {
            // TODO: Buscar una forma mejor de hacer esto
            if (playerShip.transform.Find("Energy +1(Clone)"))
            {
                playerShip.GetComponent<Player>().Health += 1;
            }
            DontDestroyOnLoad(playerShip);
        }

        // TODO: Cambiar esto
        GameManager.Instance.GetScenesManager().BeginGame((int)ScenesManager.Scenes.Level1);
    }

    #endregion

    #region BANK

    private void UpdateBank(int quantity)
    {
        bank += quantity;
        UpdateBankText();
    }

    private void UpdateBankText()
    {
        bankText.text = bank.ToString();
    }

    #endregion

    #region PLAYER UPGRADES

    private void TurnOffPlayerShipVisualUpgrades()
    {
        foreach (GameObject visualUpgrade in visualUpgrades)
        {
            visualUpgrade.SetActive(false);
        }
    }

    private void PreparePlayerShipForUpgrade()
    {
        playerShip = Instantiate(defaultPlayerShip.actor);
        playerShip.name = "Upgraded Ship";

        playerShip.GetComponent<Player>().enabled = false;
        playerShip.transform.position = new Vector3(0, 10000, 0);
        playerShip.GetComponent<IActorTemplate>().ActorStats(defaultPlayerShip);
    }

    //TODO: Mejorar la forma en que se relacionan los prefabs con la mejora elegida
    private void UpgradeShip(ShopPiece shopPiece)
    {
        foreach (GameObject weapon in upgradePrefabs)
        {
            GameObject shipUgrade;
            if (weapon.name.Equals(shopPiece.ShopSelection.upgradeName))
            {
                shipUgrade = Instantiate(weapon);
                shipUgrade.transform.SetParent(playerShip.transform);
                shipUgrade.transform.localPosition = Vector3.zero;
            }
        }
    }

    private void SoldUpgrade(ShopPiece shopPiece)
    {
        shopPiece.Sold = true;
        currentSelection.transform.parent.transform.GetComponentInChildren<TextMesh>().text = "SOLD";
    }

    #endregion

}
