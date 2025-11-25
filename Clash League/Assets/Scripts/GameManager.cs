using UnityEngine;


//Author: Hogan
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public AudioSource audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    /// <summary>
    /// Play BGM
    /// </summary>
    /// <param name="audioClip">Audio Source</param>
    public void PlayMusic(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    public void PlaySound(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }
}
