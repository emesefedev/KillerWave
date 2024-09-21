using UnityEngine;
using UnityEngine.Advertisements;
using System.Collections;

public class PlayerShipBuild : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
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

    [SerializeField] private string androidGameId;
    [SerializeField] private string iosGameId;
    [SerializeField] private bool testMode = true;
    private string adId;

    private void Awake()
    {
        CheckPlatform();
    }

    private void Start()
    {
        purchaseMade = false;

        TurnOffSelectionHighlights();
        
        UpdateBankText();
        
        TurnOffPlayerShipVisuals();
        PreparePlayerShipForUpgrade();

        StartCoroutine(WaitForAd());
    }

    private void Update()
    {
        AttemptSelection();
    }

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
        Advertisement.Show(adId, this);
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
        if (purchaseMade)
        {
            // TODO: Buscar una forma mejor de hacer esto
            if (playerShip.transform.Find("energy +1(Clone)"))
            {
                playerShip.GetComponent<Player>().Health += 1;
            }
            DontDestroyOnLoad(playerShip);
        }

        // TODO: Cambiar esto
        UnityEngine.SceneManagement.SceneManager.LoadScene("TestLevel");
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
                bank += 300;
                UpdateBankText();
                break;
            case UnityAdsShowCompletionState.SKIPPED |  UnityAdsShowCompletionState.UNKNOWN:
                // DON'T REWARD PLAYER
                break;
        }

        Advertisement.Load(adId, this);
        TurnOffSelectionHighlights();
    }
}
