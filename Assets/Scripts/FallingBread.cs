using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FallingBread : MonoBehaviour
{
    [SerializeField] private RawImage _fallingBreadPrefab;

    [SerializeField] private Texture2D[] _felines;
    [SerializeField] private float _moveSpeed;
    private float _timer;

    public float _destroyYValue; 
    // Start is called before the first frame update
    void Start()
    {
        RandomizeCats();
    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, _moveSpeed * Time.deltaTime, 0); 
        _timer = Time.deltaTime;
        if(transform.position.y < _destroyYValue)
        {
            Destroy(gameObject); 
        }
    }
    private void RandomizeCats()
    {
        //Random the sprite 
        int length = _felines.Length;
        int randomIndex = Random.Range(0, length); 
        Texture2D randonTexture = _felines[randomIndex];
        _fallingBreadPrefab.texture = randonTexture;
    }
}
