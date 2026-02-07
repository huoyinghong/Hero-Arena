using UnityEngine;

public class Building : Unit
{
        public bool isBase;
        private float attackSpeed = 1.4f;
        private float attackTimer;
        public Transform characterTrans;
        public GameObject[] buildingGOs;


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected override void Start()
        {
                base.Start();
                if (isBase)//Not active the base at start
                {
                        SetColliders(false);
                }
        }

        // Update is called once per frame
        void Update()
    {
                if (isDead) return;

                BuildingAttack();
    }

        private void BuildingAttack()
        {
                if (Time.time - attackTimer >= attackSpeed)
                {
                        attackTimer = Time.time;
                        if (hasTarget && target != null )
                        {
                                animator.SetBool("IsAttacking", true);
                                characterTrans.LookAt(new Vector3(target.transform.position.x, 
                                        characterTrans.position.y, target.transform.position.z));
                                target.TakeDamage(unitInfo.damage, this);
                        }
                        else
                        {
                                animator.SetBool("IsAttacking", false);
                        }
                }
        }

        protected override void Die(Unit attacker)
        {
                base.Die(attacker);
                buildingGOs[0].SetActive(false);//Hide normal building image
                buildingGOs[1].SetActive(true);// Display destoryed building image
                if (isBase)
                {
                        //If base has been destoryed then game over
                }
                else
                {
                        //Enable the base when one of the building is destoryed
                        GameController.Instance.EnableBase(isOrange);
                }
        }
}
