using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private AudioSource hitSound;
    [SerializeField] private AudioSource quoteSource;
    [SerializeField] private AudioClip[] quotesClips;
    private Dictionary<AudioClip, bool> PlayedClipsDict = new Dictionary<AudioClip, bool>();
    private int playedQuotesCounter = 0;
    void Start()
    {
        foreach(AudioClip clip in quotesClips)
        {
            PlayedClipsDict.Add(clip, false);
        }
    }
    public void PlaySoundJump()
    {
        jumpSound.Play();
    }
    public void PlaySoundHit()
    {
        hitSound.Play();
    }

    public void PlayRandomQuoteSound()
    {
        TakeRandomQuote();
        quoteSource.Play();
    }

    void TakeRandomQuote()
    {
        if (playedQuotesCounter >= 20) ResetDictionary();
        int indexRand = Random.Range(0, quotesClips.Length);
        AudioClip tempClip = quotesClips[indexRand];
        if (PlayedClipsDict[tempClip]) {
            TakeRandomQuote();
        }
        else {
            GetComponent<AudioSource>().clip = tempClip;
            playedQuotesCounter++;
            PlayedClipsDict[tempClip] = true;
        }
    }
    void ResetDictionary()
    {
        foreach (AudioClip clip in quotesClips)
        {
            PlayedClipsDict[clip] = false;
        }
        playedQuotesCounter = 0;
    }
}
