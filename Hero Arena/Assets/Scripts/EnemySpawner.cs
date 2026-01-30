using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
                InvokeRepeating("CreateUnits", 5, 15);
        }

        private void CreateUnits()
        {
                GameController.Instance.CreatUnit(Random.Range(1,1),transform.position, false);
        }

}
