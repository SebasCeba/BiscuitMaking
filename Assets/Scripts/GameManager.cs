using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public UnityEvent<int> OnTotalBiscuitsChanged; 
    public UnityEvent<int> OnBiscuitsPerSecChanged;

    [Range(0f, 1f)]
    [SerializeField] private float _critChance = 0.01f;

    private int _biscuitPerClick = 1;
    private int _totalBiscuits = 0;
    private int _biscuitsPerSec;
    private float _biscuitsPerSecTimer; 

    //To track the current upgraed levels 
    private Dictionary<string, int> _upgradeLevels = new Dictionary<string, int>();
    private int TotalBiscuits
    {
        get { return _totalBiscuits;  }
        set
        {
            _totalBiscuits = value;
            OnTotalBiscuitsChanged.Invoke(_totalBiscuits);
            SaveGame(); //Save when biscuits change 
        }
    }
    private int BiscuitPerSec
    {
        get { return _biscuitsPerSec;  }
        set
        {
            _biscuitsPerSec = value; 
            OnTotalBiscuitsChanged.Invoke(_biscuitsPerSec);
            SaveGame(); //Save when biscuits per second change 
        }
    }
    private void Start()
    {
        LoadGame(); //Load the game data at the start 
    }
    public int AddBiscuits()
    {
        int addedBiscuits = 0; 
        if(Random.value <= _critChance)
        {
            addedBiscuits = _biscuitPerClick * 10;
        }
        else
        {
            //Normal 
            addedBiscuits = _biscuitPerClick;
        }
        TotalBiscuits += addedBiscuits;

        return addedBiscuits;
    }
    public bool TryPurchaseUpgrade(int currentCost, int level, UpgradeType upgradeType)
    {
        if(TotalBiscuits >= currentCost)
        {
            //Purchasing power 
            TotalBiscuits -= currentCost;
            level++; 

            switch(upgradeType)
            {
                case UpgradeType.BiscuitUpgrade:
                    _biscuitPerClick = 1 + level * 2;
                    _upgradeLevels["BiscuitUpgrade"] = level; //Save level 
                    break;

                case UpgradeType.LoafingUpgrade:
                    _biscuitsPerSec = level;
                    _upgradeLevels["LoafingUpgrade"] = level; //Save level 
                    OnBiscuitsPerSecChanged.Invoke(_biscuitsPerSec);
                    break;

                case UpgradeType.ZoomiesUpgrade:
                    _biscuitPerClick = 1 + level * 2;
                    _biscuitsPerSec = level * 2;
                    _upgradeLevels["ZoomiesUpgrade"] = level; //Save level 
                    OnBiscuitsPerSecChanged.Invoke(_biscuitsPerSec);
                    break; 
            }
            SaveGame(); //Save the game after purchasing the upgrade
            return true;
        }
        return false;
    }
    public int GetUpgradeLevel(UpgradeType upgradeType)
    {
        //Default to level 0 if no level is found 
        if (_upgradeLevels.ContainsKey(upgradeType.ToString()))
        {
            return _upgradeLevels[upgradeType.ToString()];
        }
        return 0;
    }
    public void SetUpgradeLevel(UpgradeType upgradeType, int level)
    {
        _upgradeLevels[upgradeType.ToString()] = level;
        SaveGame();
    }
    private void Update()
    {
        _biscuitsPerSecTimer += Time.deltaTime; 
        if(_biscuitsPerSecTimer >= 1) //Check if a second has passed 
        {
            _biscuitsPerSecTimer--; //reset the timer 
            TotalBiscuits += _biscuitsPerSec;
        }
    }
    private void SaveGame()
    {
        //Save total biscuits and biscuits per second 
        PlayerPrefs.SetInt("TotalBiscuits", _totalBiscuits);
        PlayerPrefs.SetInt("BiscuitsPerSecond", _biscuitsPerSec); 

        //Save the upgrade levels 
        foreach(var upgrade in _upgradeLevels)
        {
            PlayerPrefs.SetInt(upgrade.Key + "Level", upgrade.Value);
        }

        PlayerPrefs.Save(); //Save everything to playerprefs 
    }
    private void LoadGame()
    {
        //Load total biscuits and biscuits per second 
        if (PlayerPrefs.HasKey("TotalBiscuits"))
        {
            _totalBiscuits = PlayerPrefs.GetInt("TotalBiscuits"); 
        }
        if (PlayerPrefs.HasKey("BiscuitsPerSecond"))
        {
            _biscuitsPerSec = PlayerPrefs.GetInt("BiscuitsPerSecond");
            OnBiscuitsPerSecChanged.Invoke(_biscuitsPerSec); //Update the UI/Events
        }

        //Load the Upgrade levels 
        if (PlayerPrefs.HasKey("BiscuitUpgradeLevel"))
        {
            int biscuitUpgradeLevel = PlayerPrefs.GetInt("BiscuitUpgradeLevel");
            _biscuitPerClick = 1 + biscuitUpgradeLevel * 2;
            _upgradeLevels["BiscuitUpgrade"] = biscuitUpgradeLevel; 
        }
        if (PlayerPrefs.HasKey("LoafingUpgradeLevel"))
        {
            int loafingUpgradeLevel = PlayerPrefs.GetInt("LoafingUpgradeLevel");
            _biscuitsPerSec = loafingUpgradeLevel;
            _upgradeLevels["LoafingUpgrade"] = loafingUpgradeLevel;
            OnBiscuitsPerSecChanged.Invoke(_biscuitsPerSec); 
        }
        if (PlayerPrefs.HasKey("ZoomiesUpgradeLevel"))
        {
            int zoomiesUpgradeLevel = PlayerPrefs.GetInt("ZoomiesUpgradeLevel");
            _biscuitPerClick = 1 + zoomiesUpgradeLevel * 2;
            _biscuitsPerSec = zoomiesUpgradeLevel * 2;
            _upgradeLevels["ZoomiesUpgrade"] = zoomiesUpgradeLevel; 
            OnBiscuitsPerSecChanged.Invoke(_biscuitsPerSec);
        }
        OnTotalBiscuitsChanged.Invoke(_totalBiscuits);
    }
}
