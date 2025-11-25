using System.Runtime.CompilerServices;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public float goldAmount;

    private void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (goldAmount < 10)
        {
            goldAmount += Time.deltaTime;
            UIManager.Instance.SetGoldAmount();
        }
        
    }
}
