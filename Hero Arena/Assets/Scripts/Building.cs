using UnityEngine;

public class Building : Unit
{
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected override void Start()
        {
                base.Start();
        }

        // Update is called once per frame
        void Update()
    {
                if (isDead) return;

                BuildingAttack();
    }

        private void BuildingAttack()
        {

        }

        protected override void Die(Unit attacker)
        {
                base.Die(attacker);
        }
}
