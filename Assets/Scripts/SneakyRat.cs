using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class SneakyRat : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager; // Reference to your GameManager
    [SerializeField] private GameObject SneakyRatButton; // The button 
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip clickSound; 

    public float _speed;
    public float _distance;

    private Vector2 _originalPosition; // Starting position of the button  
    private bool _movingUp = true; 

    // Start is called before the first frame update
    void Start()
    {
        _originalPosition = SneakyRatButton.transform.localPosition;

        SneakyRatButton.GetComponent<Button>().onClick.AddListener(OnButtonClicked); 
    }
    private void Update()
    {
        MoveButton();
    }
    private void MoveButton()
    {
        // Calculate the new position based on the direction of movement
        float newY = SneakyRatButton.transform.localPosition.y + (_speed * Time.deltaTime * (_movingUp ? 1 : -1));

        // Check if the button has reached the target distance
        if(_movingUp && newY >= _originalPosition.y + _distance)
        {
            newY = _originalPosition.y + _distance; // Clamp to max distance
            _movingUp = false; // Change direction
        }
        else if(!_movingUp && newY <= _originalPosition.y)
        {
            newY = _originalPosition.y; // Clamp to original position
            _movingUp = true; // Change direction
        }

        // Set the new position
        SneakyRatButton.transform.localPosition = new Vector2(_originalPosition.x, newY);
    }
    public void OnButtonClicked()
    {
        PlayClickedSound(); 
    }
    // Reset the button to its orginal position 
    private void PlayClickedSound()
    {
        if(_audioSource && clickSound)
        {
            _audioSource.PlayOneShot(clickSound); 
        }
    }
}
