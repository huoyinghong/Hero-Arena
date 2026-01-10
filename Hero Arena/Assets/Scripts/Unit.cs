using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
        public UnitInfo unitInfo;
        public bool isOrange;
        public bool hasTarget; //If the unit has a target then attact, otherwise move to enemy base
        public int currentHP;
        public bool isDead;
        public Unit target;//Target that unit attack to
        public Animator animator;
        public NavMeshAgent meshAgent;
        public Unit defaultTarget;//Default target enemy base;
        public List<Unit> targetsList = new List<Unit>();//List of  units that is avaliable for this unit to attack(within the attack range)
        public List<Unit> attackersList = new List<Unit>();//List of  units that is attacking this unit




        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected virtual void Start()
        {
                animator = GetComponentInChildren<Animator>();
                meshAgent = GetComponent<NavMeshAgent>();
        }

        // Update is called once per frame
        protected virtual void UnitMove()
        {
                //if the unit has target
                if (hasTarget)
                {
                        if (target != null && target.isDead == false)
                        {
                                meshAgent.SetDestination(target.transform.position);
                                ReachTarget(transform.position, target.transform.position);
                        }
                        else
                        {
                                ResetTarget();
                        }
                }
                else//if the unit doesnt have target
                {
                        GameController.Instance.UnitGetTargetPosi(this, isOrange);
                        if (defaultTarget != null)
                        {
                                //Move to default target
                                meshAgent.SetDestination(defaultTarget.transform.position);
                                ReachTarget(transform.position, defaultTarget.transform.position);

                        }
                }
        }


        /// <summary>
        /// Determine whether the unit reached its attack range (if not reached target execute movement animation)
        /// </summary>
        /// <param name="currentPosi"></param>
        /// <param name="targetPosi"></param>
        public void ReachTarget(Vector3 currentPosi, Vector3 targetPosi)
        {
                if (Vector3.Distance(currentPosi, targetPosi) >= unitInfo.attackRange)//if the target is not in the attackRange
                {
                        UnitBehaviour();
                }
                else//if the target is in the attackRange
                {
                        meshAgent.isStopped = true;
                        animator.SetBool("IsAttacking", true);
                        animator.SetBool("IsMoving", false);
                }

        }

        private void ResetTarget()
        {


        }

        protected virtual void UnitBehaviour()
        {
                animator.SetBool("IsMoving", true);
                animator.SetBool("IsAttacking", false);

        }

        /// <summary>
        /// Taking damage
        /// </summary>
        /// <param name="dmgValue"></param>
        /// <param name="attacker">The unit that is attacking this unit</param>
        public void TakeDamage(int dmgValue, Unit attacker)
        {

        }

        protected virtual void Die(Unit attacker)
        {

        }

        public virtual void AttackAnimationEvent()
        {
                if (hasTarget)//make the unit face to its target when attacking
                {
                        transform.LookAt(target.transform);
                }
                else
                {
                        transform.LookAt(defaultTarget.transform);
                }
                if (target != null)
                {
                        target.TakeDamage(unitInfo.damage, this);//target loses hp

                }
        }

        public struct UnitInfo
        {
                public int ID;
                public string name;
                public int cost;
                public int hp;
                public float attackRange;
                public float speed;
                public int damage;
                public bool canCreateAnywhere;
        }
}
