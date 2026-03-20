using UnityEngine;

public class Magic : Unit
{
        public float timeVal;
        public float destoryTime;

        public AudioClip damageClip;
        public AudioClip healingClip;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected override void Start()
        {
                base.Start();
                //since magic fog card and healing angel's healing effect using the same script. 
                // Distinguish them by its damage. if the damage is negative then its healing.
                if (unitInfo.damage >= 0)
                {
                        GameManager.Instance.PlaySound(damageClip);
                }
                else
                {
                        GameManager.Instance.PlaySound(healingClip);
                }
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
