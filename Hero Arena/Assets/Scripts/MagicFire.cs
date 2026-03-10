using UnityEngine;

public class MagicFire : Unit
{
        private Vector3 moveSpeed;
        public GameObject explosion;
        public Vector3 targetPos;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected override void Start()
    {
                base.Start();
                Vector3 startPos = GameController.Instance.orangeBuildings[0].transform.position;
                transform.position = startPos;
                moveSpeed = (targetPos - startPos).normalized * unitInfo.speed;
                SetColliders(false);
    }

    // Update is called once per frame
    void Update()
    {
                if (Vector3.Distance(transform.position, targetPos) <= 0.3f)
                {
                        SetColliders(true);
                        Invoke("DamageAllUnits", 0.2f);
                }
                else {
                        transform.position += moveSpeed * Time.deltaTime;
                }

    }
        private void DamageAllUnits()
        {
                for (int i = 0; i < targetsList.Count; i++)
                {
                        targetsList[i].TakeDamage(unitInfo.damage, this);
                }
                Instantiate(explosion,transform.position, Quaternion.identity);
                Destroy(gameObject);
        }
}
