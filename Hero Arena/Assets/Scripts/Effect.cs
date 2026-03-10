using UnityEngine;

public class Effect : MonoBehaviour
{
        public float destoryTime = 2;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
                Destroy(gameObject, destoryTime);
        }

}
