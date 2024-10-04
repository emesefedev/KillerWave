using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;
using TMPro;

public class PlayerShipBuild : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] private GameObject[] shopButtons;
    private GameObject target;
    private GameObject currentSelection;

    [SerializeField] private TextMeshProUGUI infoPanelName;
    [SerializeField] private TextMeshProUGUI infoPanelDescription;

    [SerializeField] GameObject[] visualUpgrades;
    [SerializeField] private GameObject[] upgradePrefabs;
    [SerializeField] SOActorModel defaultPlayerShip;
    private GameObject playerShip;

    [SerializeField] private GameObject buyButton;
    [SerializeField] private TextMeshProUGUI bankText;
    private int bank = 2000; //TODO: En un futuro, cambiar a 0
    private bool purchaseMade = false;

    [SerializeField] private string androidGameId;
    [SerializeField] private string iOSGameId;
    [SerializeField] private bool testMode = true;
    private string adId = null;
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

        buyButton.SetActive(false);        
    }

    #region ADVERTISEMENT
    private void CheckPlatform()
    {
        string gameId = null;

        #if UNITY_IOS
        {
            gameId = iOSGameId;
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

        Advertisement.Load(placementId, this);
        TurnOffSelectionHighlights();
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
    
    #region SHOP BUTTONS
    private void TurnOffSelectionHighlights()
    {
        // TODO: Comprobar el funcionamiento de esta función, porque no sé si es lo que se busca (código original en el txt PlayerShipBuild)
        foreach (GameObject shopButton in shopButtons)
        {
            ShopPiece shopPiece = shopButton.GetComponent<ShopPiece>();
            if (shopPiece)
            {
                if (shopPiece.Sold)
                {
                    shopButton.SetActive(false);
                }
            }
        }
    }

    //Antiguo Attempt Selection. No sé si esto funcionará con el parámetro. Está por ver
    private void SelectUpgrade(ShopPiece shopPiece)
    {
        TurnOffSelectionHighlights(); 
        UpdateInfoPanel(shopPiece);

        if (!shopPiece.Sold) 
        {
            CheckUpgradeAffordable(shopPiece);
        }
        else 
        {
            UpgradeSoldOut();
        }
    }

    private void ClearInfoPanel()
    {
        infoPanelName.text = "";
        infoPanelDescription.text = "";
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

        ClearInfoPanel();
        
        //TODO: Mejorar esto para que no dependa de los nombres (strings) de los SO
        ShopPiece currentShopPiece = currentSelection.GetComponentInParent<ShopPiece>();
        foreach (GameObject visualUpgrade in visualUpgrades)
        {
            if (visualUpgrade.name.Equals(currentShopPiece.ShopSelection.upgradeName))
            {
                visualUpgrade.SetActive(true);
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
            playerShip.name = "Upgraded Ship";

            // TODO: Buscar una forma mejor de hacer esto
            if (playerShip.transform.Find("Energy +1(Clone)"))
            {
                playerShip.GetComponent<Player>().Health += 1;
            }

            DontDestroyOnLoad(playerShip);
        }

        GameManager.Instance.GetScenesManager().BeginGame((int)ScenesManager.Scenes.Level1);
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
        playerShip.GetComponent<Player>().enabled = false;
        playerShip.transform.position = new Vector3(0, 10000, 0); //TODO: Avoid magic numbers
        playerShip.GetComponent<IActorTemplate>().ActorStats(defaultPlayerShip);
    }

    private void UpgradeShip(ShopPiece shopPiece)
    {
        //TODO: Hacer un for con break porque esto es una búsqueda. Mejorar esto para que no dependa del nombre
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
        currentSelection.transform.parent.transform.GetComponentInChildren<TextMeshProUGUI>().text = "SOLD";
    }

    #endregion

    
}
