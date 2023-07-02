using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region Singleton
    private static AudioManager _instance;
    public static AudioManager Instance => _instance;
    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion

    public AudioSource audioS;
    public AudioClip victoryClip;
    public AudioClip gameOverClip;
    public AudioClip hitClip;
    public AudioClip fallClip;
    public AudioClip levelUpClip;
    public AudioClip btnClip;

    public void VictoryAudioPlay()
    {
        audioS.clip = victoryClip;
        audioS.volume = 1.0f;
        audioS.Play();
    }
    public void GameOverAudioPlay()
    {
        audioS.clip = gameOverClip;
        audioS.volume = 1.0f;
        audioS.Play();
    }
    public void LostLifeAudioPlay()
    {
        audioS.clip = fallClip;
        audioS.volume = 1.0f;
        audioS.Play();
    }
    public void LevelUpAudioPlay()
    {
        audioS.clip = levelUpClip;
        audioS.volume = 1.0f;
        audioS.Play();
    }
    public void HitAudioPlay()
    {
        audioS.clip = hitClip;
        audioS.volume = 1.0f;
        audioS.Play();
    }
    public void BtnAudioPlay()
    {
        audioS.clip = btnClip;
        audioS.volume = 1.0f;
        audioS.Play();
    }
}
