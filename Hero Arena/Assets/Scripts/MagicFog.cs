using UnityEngine;

public class Magic : Unit
{
        public float timeVal;
        public float destoryTime;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected override void Start()
        {
                base.Start();
                Destroy(gameObject, destoryTime);
        }

        // Update is called once per frame
        void Update()
        {
                if (Time.time - timeVal >= 1)//Damage units every one second
                {
                        for (int i = 0; i < targetsList.Count; i++)
                        {
                                targetsList[i].TakeDamage(unitInfo.damage, this);
                        }
                        timeVal = Time.time;
                }
        }
}
