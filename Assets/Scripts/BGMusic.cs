using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMusic : MonoBehaviour
{
    [Header("--- Audio Source ---")]
    [SerializeField] AudioSource musicSource;
    [Header("--- Background Music ---")]
    public AudioClip[] Tracks;

    private int currentTrackIndex = 0; 
    // Start is called before the first frame update
    void Start()
    {
        PlayTrack(currentTrackIndex); 
    }
    private void PlayTrack(int trackIndex)
    {
        if(Tracks.Length == 0)
        {
            return; 
        }
        musicSource.clip = Tracks[currentTrackIndex];
        musicSource.Play();

        StartCoroutine(WaitForTrackEnd()); 
    }
    private IEnumerator WaitForTrackEnd()
    {
        yield return new WaitForSeconds(musicSource.clip.length);
        currentTrackIndex = (currentTrackIndex + 1) % Tracks.Length;
        PlayTrack(currentTrackIndex);
    }
}
