using UnityEngine;

public class Building : Unit
{
        public bool isBase;
        private float attackCD = 1.4f;
        private float attackTimer;
        public Transform characterTrans;
        public GameObject[] buildingGOs;
        public AudioClip destoryClip;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected override void Start()
        {
                unitInfo = GameController.Instance.unitInfos[12];
                base.Start();
                if (isBase)
                {
                        SetColliders(false);//Disable the base at start
                        unitInfo.attackRange += 1;
                        unitInfo.damage += 1;
                        unitInfo.hp += 100;
                }
        }

        // Update is called once per frame
        void Update()
        {
                if (isDead)
                {
                        return;
                }
                BuildingAttack();
        }

        private void BuildingAttack()
        {
                if (Time.time-attackTimer >= attackCD)
                {
                        attackTimer = Time.time;
                        if (hasTarget && target != null)
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
                GameManager.Instance.PlaySound(destoryClip);
                if (isBase)
                {
                        //If base has been destoryed then game over
                        UIManager.Instance.GameOver(!isOrange);
                }
                else
                {
                        //Enable the base when one of the building is destoryed
                        GameController.Instance.EnableBase(isOrange);
                }
        }

        public override void AttackAnimationEvent()
        {
        }
}
