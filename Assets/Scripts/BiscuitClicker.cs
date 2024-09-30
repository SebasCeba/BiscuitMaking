using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BiscuitClicker : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private TextMeshProUGUI _textRewardPrefab;
    [SerializeField] private RawImage _fallingBreadPrefab;
    [SerializeField] private float _minXpos, _maxXpos, _minYpos, _maxYpos;
    [SerializeField] private float _minX, _maxX, _maxY;

    [SerializeField] private Animator _animator;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _meowClips; //Audio Clips for meowing/purring. 

    //Spawning intervals 
    public float spawnInterval = 5f;
    public float spawnChance = 0.3f;

    private float lastSpawnTime; 
    // Start is called before the first frame update
    void Start()
    {
        lastSpawnTime = Time.time;
    }
    // Update is called once per frame
    void Update()
    {
        if(Time.time - lastSpawnTime > spawnInterval)
        {
            //Randomly decide whether to spawn the fallBreadPrefab
            if (Random.value < spawnChance)
            {
                //Instantiate the prefab
                CreateFallTreatPrefab();

                //UPdate the last spawn time
                lastSpawnTime = Time.time; 
            }
        }
    }
    public void OnBiscuitClicked()
    {
        //Add Biscuits 
        int addedBiscuits = _gameManager.AddBiscuits(); 

        CreateTextRewardPrefab(addedBiscuits); 
        if(Time.time - lastSpawnTime > spawnInterval)
        {
            //Randfomly decide whether to spawn 
            if(Random.value < spawnChance)
            {
                //Instantiate the prefab 
                RawImage fallingBreadClone = Instantiate(_fallingBreadPrefab, transform.position, Quaternion.identity);

                //Update the last spawn time 
                lastSpawnTime = Time.time; 
            }
        }
    }
    public void OnCatHeadClicked()
    {
        if (_meowClips.Length > 0)
        {
            int randomIndex = Random.Range(0, _meowClips.Length);
            _audioSource.PlayOneShot(_meowClips[randomIndex]);
        }
    }
    private void CreateTextRewardPrefab(int addedBiscuits)
    {
        //This will clone the textReward prefab
        TextMeshProUGUI textRewardClone = Instantiate(_textRewardPrefab, transform); 
        //Random Position
        Vector2 randomVector = MyToolbox.GetRandomVector2(_minXpos, _maxXpos, _minYpos, _maxYpos);
        textRewardClone.transform.localPosition = randomVector; 
        textRewardClone.text = $"+{addedBiscuits}";
    }
    public void CreateFallTreatPrefab()
    {
        //This will just clone the bread
        RawImage fallingTreatsClone = Instantiate(_fallingBreadPrefab, transform);
        //Random Position
        Vector2 randomVector = MyToolbox.FallingRandomVector2(_minX, _maxX, _maxY); 
        fallingTreatsClone.transform.localPosition = randomVector;
    }
}
