using System.Collections.Generic;
using UnityEngine;


//Author: Hogan
public class GameManager : MonoBehaviour
{
        public static GameManager Instance;
        public AudioSource audioSource;
        public List<int> battleCardsList = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8 };
        public List<int> allCardsList = new List<int>() { 9 };
        public AudioClip btnSound;
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
        public void PlayButtonSound()
        {
                audioSource.PlayOneShot(btnSound);
        }
}
