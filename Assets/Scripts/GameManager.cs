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
        }
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
                    break;

                case UpgradeType.LoafingUpgrade:
                    _biscuitsPerSec = level; 
                    OnBiscuitsPerSecChanged.Invoke(_biscuitsPerSec);
                    break;

                case UpgradeType.ZoomiesUpgrade:
                    _biscuitPerClick = 1 + level * 2;
                    _biscuitsPerSec = level * 2; 
                    OnBiscuitsPerSecChanged.Invoke(_biscuitsPerSec);
                    break; 
            }
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
    private void Start()
    {
        TotalBiscuits = 0;
        BiscuitPerSec = 0;
    }
}
