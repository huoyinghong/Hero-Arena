using UnityEngine;
using UnityEngine.UI;

public class HPSlider : MonoBehaviour
{
        private Camera mainCamera;
        public Slider slider;
        private void Awake()
        {
                mainCamera = Camera.main;
        }

        // Update is called once per frame
        void Update()
        {
                //Make hp slider always facing camera so it wont rotate with units
                transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
                        mainCamera.transform.rotation * Vector3.up);
        }

        public void SetHPColor(bool isOrange)
        {
                if (isOrange)
                {
                        slider = transform.Find("Slider_Red").GetComponent<Slider>();
                }
                else
                {
                        slider = transform.Find("Slider_Blue").GetComponent<Slider>();
                }
                for (int i=0;i<transform.childCount ;i++)
                {
                        transform.GetChild(i).gameObject.SetActive(false);//Hide slider until unit take damage
                }
        }

        public void SetHPSliderValue(float value)
        {
                if (!slider.gameObject.activeSelf)
                {
                        slider.gameObject.SetActive(true);
                }
                slider.value = value;
        }
}
