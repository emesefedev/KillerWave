using UnityEngine;

public class PlayerShipBuild : MonoBehaviour
{
    [SerializeField] private GameObject[] shopButtons;
    [SerializeField] private TextMesh textBoxName;
    [SerializeField] private TextMesh textBoxDescription;


    private GameObject target;
    private GameObject currentSelection;

    private void Start()
    {
        TurnOffSelectionHighlights();
    }

    private void Update()
    {
        AttemptSelection();
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
                if (target.gameObject.name.Contains("Upgrade"))
                {
                    TurnOffSelectionHighlights();
                    Select();
                    UpdateTextBoxPanel();
                }
            }
        }
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
