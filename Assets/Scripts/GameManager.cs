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

    private GameSaveManager _gameSaveManager; 

    //To track the current upgraed levels 
    private Dictionary<string, int> _upgradeLevels = new Dictionary<string, int>();

    private int TotalBiscuits
    {
        get
        {
            return _totalBiscuits;
        }
        set
        {
            _totalBiscuits = value;
            OnTotalBiscuitsChanged.Invoke(_totalBiscuits);
            SaveGame(); //Save when biscuits change 
        }
    }
    private int BiscuitPerSec
    {
        get
        {
            return _biscuitsPerSec;
        }
        set
        {
            _biscuitsPerSec = value; 
            OnTotalBiscuitsChanged.Invoke(_biscuitsPerSec);
            SaveGame(); //Save when biscuits per second change 
        }
    }
    private void Start()
    {
        _gameSaveManager = FindObjectOfType<GameSaveManager>();
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
                    _upgradeLevels["BiscuitUpgrade"] = level; 
                    break;

                case UpgradeType.LoafingUpgrade:
                    _biscuitsPerSec = level;
                    _upgradeLevels["LoafingUpgrade"] = level;
                    OnBiscuitsPerSecChanged.Invoke(_biscuitsPerSec);
                    break;

                case UpgradeType.ZoomiesUpgrade:
                    _biscuitPerClick = 1 + level * 2;
                    _biscuitsPerSec = level * 2;
                    _upgradeLevels["ZoomiesUpgrade"] = level; 
                    OnBiscuitsPerSecChanged.Invoke(_biscuitsPerSec);
                    break; 
            }
            SaveGame(); //Save the game after purchasing the upgrade
            return true;
        }
        return false;
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
    //private void Start()
    //{
    //    TotalBiscuits = 0;
    //    BiscuitPerSec = 0;
    //}
    private void SaveGame()
    {
        GameData gameData = new GameData
        {
            totalBiscuits = _totalBiscuits, //Save total biscuits from _totalBiscuits 
            biscuitsPerSecond = _biscuitsPerSec,    //Save biscuits per second from _biscuitsPerSec; 
            upgradeLevels = _upgradeLevels     //Save the upgrade levels dictionary 
        };

        _gameSaveManager.SaveGame(gameData);
    }
    private void LoadGame()
    {
        GameData loadedData = _gameSaveManager.LoadGame();
        if(loadedData != null )
        {
            _totalBiscuits = loadedData.totalBiscuits;
            _biscuitsPerSec = loadedData.biscuitsPerSecond;
            _upgradeLevels = loadedData.upgradeLevels;

            if (_upgradeLevels.ContainsKey("BiscuitUpgrade"))
            {
                int level = _upgradeLevels["BiscuitUpgrade"];
                _biscuitPerClick = 1 + level * 2; 
            }
            if (_upgradeLevels.ContainsKey("LoafingUpgrade"))
            {
                int level = _upgradeLevels["LoafingUpgrade"];
                _biscuitsPerSec = level;
                OnBiscuitsPerSecChanged.Invoke(_biscuitsPerSec); 
            }
            if (_upgradeLevels.ContainsKey("ZoomiesUpgrade"))
            {
                int level = _upgradeLevels["ZoomiesUpgrade"];
                _biscuitPerClick = 1 + level * 2;
                _biscuitsPerSec = level * 2;
                OnBiscuitsPerSecChanged.Invoke(_biscuitsPerSec); 
            }
            OnTotalBiscuitsChanged.Invoke(_totalBiscuits);
        }
    }
}
