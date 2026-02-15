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
        protected Collider[] colliders; 




        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected virtual void Start()
        {
                animator = GetComponentInChildren<Animator>();
                meshAgent = GetComponent<NavMeshAgent>();
                GetComponentInChildren<SphereCollider>().radius = unitInfo.attackRange;
                colliders = GetComponentsInChildren<Collider>();
                currentHP = unitInfo.hp;
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
                                ResetTarget(target);
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

        private void ResetTarget(Unit unit)
        {
                targetsList.Remove(unit);
                ClearDeadUnitInList();
                if (targetsList.Count > 0)
                {
                        SetTarget();
                }
                else//No unit within attack range.  attack buildings
                {
                        hasTarget = false;
                        target = null;
                        GameController.Instance.UnitGetTargetPosi(this, this.isOrange);
                }
        }


        protected virtual void UnitBehaviour()
        {
                if (meshAgent.enabled)
                {
                        meshAgent.isStopped = false;
                }
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
                currentHP -= dmgValue;
                Mathf.Clamp(currentHP, 0, unitInfo.hp);
                if (currentHP <= 0)
                {
                        Die(attacker);
                }
        }



        protected virtual void Die(Unit attacker)
        {
                isDead = true;
                animator.SetTrigger("Die");
                SetColliders(false);
                attacker.ResetTarget(this);
                RemoveSelfFromOtherAttacker();
                if (meshAgent.enabled)
                {
                        meshAgent.isStopped = true;
                }
                Invoke("DestoryUnit", 4);

        }

        /// <summary>
        /// Destory the object after it's dead
        /// </summary>
        private void DestoryUnit()
        {
                Destroy(gameObject);
        }

        /// <summary>
        /// Make other attackers remove this unit from their target list
        /// (otherwise the attacker will keep attacking this unit even if it's dead)
        /// </summary>
        private void RemoveSelfFromOtherAttacker()
        {
                for (int i = 0; i < attackersList.Count; i++)
                {
                        attackersList[i].targetsList.Remove(this);
                }
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

        /// <summary>
        /// Detect unit within attack range and target it
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
                if (other.CompareTag("Unit"))
                {
                        Unit unit = other.GetComponentInParent<Unit>();
                        if (isOrange != unit.isOrange)
                        {
                                targetsList.Add(unit);//Add target to unit's target list
                                unit.AddAttackerToList(this);//Add unit itself to target's attacker list 
                                ClearDeadUnitInList();
                                SetTarget();
                        }
                }
        }


        /// <summary>
        /// Detect unit within attack range, reset target and search for new target
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerExit(Collider other)
        {
                if (other.CompareTag("Unit"))
                {
                        Unit unit = other.GetComponentInParent<Unit>();
                        if (isOrange != unit.isOrange && target == unit)//If current target is the unit then reset target. if not ignore
                        {
                                ResetTarget(unit);
                        }
                }
        }

        /// <summary>
        /// When current unit is attacking to another unit, the other unit needs to add 
        /// current unit into its attackersList. So this function will called by the other unit
        /// </summary>
        /// <param name="unit"></param>
        public void AddAttackerToList(Unit unit)
        {
               attackersList.Add(unit);
        }

        /// <summary>
        /// Remove the dead units in the target list
        /// </summary>
        private void ClearDeadUnitInList()
        {
                List<int> temp = new List<int>();//this array holds the index of the units that need to be removed
                for (int i = 0; i < targetsList.Count; i++)
                {
                        if (targetsList[i] == null)
                        {
                                temp.Add(i);
                        }
                }

                for (int i = 0;i < temp.Count; i++)
                {
                        targetsList.RemoveAt(temp[i]);
                }
        }

        /// <summary>
        /// Set attack target, with priority given to the nearest target
        /// </summary>
        private void SetTarget()
        {
                float closestDistance = 9999;
                Unit u = null;
                //Go through all the units within attack range and find the nearest one
                for (int i = 0; i < targetsList.Count;i++)
                {
                        float distance = Vector3.Distance(transform.position, targetsList[i].transform.position);
                        if (distance < closestDistance)
                        {
                                closestDistance = distance;
                                u = targetsList[i];
                        }
                }
                target = u;
                hasTarget = true;
        }

        /// <summary>
        /// Enable or disable unit's collider
        /// </summary>
        /// <param name="state"></param>
        protected void SetColliders(bool state)
        {
                for (int i = 0; i < colliders.Length; i++)
                {
                        colliders[i].enabled = state;
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
