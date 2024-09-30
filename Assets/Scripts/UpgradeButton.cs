using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _costText;
    [SerializeField] private float _costPowerScale = 1.5f;
    [SerializeField] private UpgradeType _upgradeType;

    private int Level; 
    private int CurrentCost
    {
        get
        {
            return 5 + Mathf.RoundToInt(Mathf.Pow(Level, _costPowerScale));
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        UpdateUI();
        GetComponent<Button>().onClick.AddListener(OnUpgradeClicked);
        _gameManager.OnTotalBiscuitsChanged.AddListener(TotalBiscuitsChanged);
    }
    public void TotalBiscuitsChanged(int totalBiscuits)
    {
        bool canAfford = totalBiscuits >= CurrentCost;
        _costText.color = canAfford ? Color.green : Color.red;
    }
    public void OnUpgradeClicked()
    {
        int currentCost = CurrentCost;
        bool purchaseUpgrade = _gameManager.TryPurchaseUpgrade(currentCost, Level, _upgradeType);
        if (purchaseUpgrade)
        {
            Level++;
            UpdateUI(); 
        }
    }
    private void UpdateUI()
    {
        _levelText.text = Level.ToString();
        _costText.text = CurrentCost.ToString();
    }
}
