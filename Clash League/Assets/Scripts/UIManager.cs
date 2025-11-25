using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public Text currentGold;
    public Slider goldSlider;  
    private void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetGoldAmount()
    {
        currentGold.text = (GameController.Instance.goldAmount).ToString();
    }
}
