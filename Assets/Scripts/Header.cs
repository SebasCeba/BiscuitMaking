using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Header : MonoBehaviour
{
    //Displays the amount of muffins 
    [SerializeField] TextMeshProUGUI _totalBiscuitsText;
    //Displays the amount of muffins gain over one second 
    [SerializeField] TextMeshProUGUI _biscuitsPerSecText; 

    public void UpdateTotalBiscuits(int counter)
    {
        _totalBiscuitsText.text = counter == 1 ? "1 Biscuit" : $"{counter} Biscuits"; 
    }
    public void UpdateBiscuitPerSec(int counter)
    {
        _biscuitsPerSecText.text = counter == 1 ? "1 Biscuit / second" : $"{counter} Biscuits / second";
    }
}
