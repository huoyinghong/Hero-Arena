using UnityEngine;
using UnityEngine.SceneManagement;

public class Gamestater : MonoBehaviour
{
    public AudioClip audioClip;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.PlayMusic(audioClip);
        Invoke("LoadCardsScene", 1.5f);

        
    }

    // Update is called once per frame
    private void LoadCardsScene()
    {
        SceneManager.LoadScene(1);
    }


}
