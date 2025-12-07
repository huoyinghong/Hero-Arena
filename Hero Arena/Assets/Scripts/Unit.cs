using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
        public UnitInfo unitInfo;
        public bool isBlue;
        public bool hasTarget; //If the unit has a target then attact, otherwise move to enemy base
        public int currentHP;
        public bool isDead;

        public Unit target;//Target that unit attack to
        public Animator animator;
        public NavMeshAgent meshAgent;
        public Unit defaultTarget;//Default target enemy base;
        public List<Unit> targetsList = new List<Unit>();//List of  units that is avaliable for the unit to attack(within the attack range)
        public List<Unit> attackersList = new List<Unit>();//List of  units that is attacking the unit




        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
    public float damage;
    public bool canCreateAnywhere;
}
