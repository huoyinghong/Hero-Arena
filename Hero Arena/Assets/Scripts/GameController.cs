using NUnit.Framework;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

using static Unit;

public class GameController : MonoBehaviour
{
        public static GameController Instance;

        public float goldAmount;
        public float remainingTime;
        public List<UnitInfo> unitInfos;
        public GameObject[] unitGOs; //All unit prefads
        public Building[] purpleBuildings;
        public Building[] orangeBuildings;

        private void Awake()
        {
                Instance = this;
                remainingTime = 180;
                unitInfos = new List<UnitInfo>()
                {
                    new UnitInfo(){ ID = 1, name = "Elf Archer", cost = 3, attackRange = 5, hp = 10, damage = 2, speed = 1},
                    new UnitInfo(){ ID = 2, name = "Healing Angel", cost = 4, attackRange = 3, hp = 10, damage = 1, speed = 1},
                    new UnitInfo(){ ID = 3, name = "Three Headed Wolf", cost = 6, attackRange = 3, hp = 30, damage = 5, speed = 1},
                    new UnitInfo(){ ID = 4, name = "Fallen Angel", cost = 6, attackRange = 4, hp = 10, damage = 4, speed = 2},
                    new UnitInfo(){ ID = 5, name = "Lava Monster", cost = 8, attackRange = 3, hp = 30, damage = 3, speed = 1},
                    new UnitInfo(){ ID = 6, name = "Archer Brothers", cost = 5, attackRange = 5, hp = 10, damage = 2, speed = 1},
                    new UnitInfo(){ ID = 7, name = "Bear", cost = 7, attackRange = 2, hp = 10, damage = 8, speed = 2},
                    new UnitInfo(){ ID = 8, name = "Death", cost = 6, attackRange = 3, hp = 10, damage = 7, speed = 1},
                    new UnitInfo(){ ID = 9, name = "Plaguebringer", cost = 4, attackRange = 3, damage = 1, speed = 1, canCreateAnywhere = true},
                    new UnitInfo(){ ID = 10, name = "Fireball", cost = 4, attackRange = 4, damage = 3, speed = 18, canCreateAnywhere = true},
                    new UnitInfo(){ ID = 11, name = "Skeletons", cost = 0, attackRange = 2, hp = 2, damage = 1, speed = 1},
                    new UnitInfo(){ ID = 12, name = "Healing Aura", cost = 0, attackRange = 2f, damage = -2, speed = 18},
                    new UnitInfo(){ ID = 13, name = "Building", cost = 0, attackRange = 6, hp = 200, damage = 4, speed = 0},

                };
        }

        // Update is called once per frame
        void Update()
        {
                if (goldAmount < 10)
                {
                        goldAmount += Time.deltaTime;
                        UIManager.Instance.SetGoldAmount();
                        UIManager.Instance.SetGoldSlider();
                }
                DecreaseTime();
        }

        public void DecreaseTime()
        {
                remainingTime -= Time.deltaTime;
                int min = (int)(remainingTime / 60);
                int sec = (int)(remainingTime % 60);
                UIManager.Instance.SetTime(min, sec);
        }

        /// <summary>
        /// Check current gold is enough to use the card
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CanUseCard(int id)
        {
                return unitInfos[id - 1].cost <= goldAmount;
        }

        /// <summary>
        /// Spend gold
        /// </summary>
        /// <param name="value"></param>
        public void DecreaseGoldValue(int id)
        {
                int value = unitInfos[id - 1].cost;
                goldAmount -= value;
        }

        /// <summary>
        /// Create unit
        /// </summary>
        /// <param name="id">unit id</param>
        /// <param name="pos">creation position</param>
        /// <param name="isOrange">If unit belongs to orange side</param>
        public void CreatUnit(int id, Vector3 pos, bool isOrange = true)
        {
                GameObject go = Instantiate(unitGOs[id - 1]);
                go.transform.position = pos;
                switch (id)
                {
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 8:
                        case 9:
                        case 11:
                        case 12:
                                Unit u1 = go.GetComponent<Unit>();
                                u1.isOrange = isOrange;
                                u1.unitInfo = unitInfos[id - 1];
                                break;
                        case 6:
                        case 7:
                                for (int i = 0; i < go.transform.childCount; i++)
                                {
                                        Unit u2 = go.transform.GetChild(i).GetComponent<Unit>();
                                        u2.isOrange = isOrange;
                                        u2.unitInfo = unitInfos[id - 1];
                                }
                                break;
                        case 10:
                                MagicFire fireball = go.GetComponent<MagicFire>();
                                fireball.targetPos = pos;
                                fireball.isOrange = isOrange;
                                fireball.unitInfo = unitInfos[id - 1];
                                break;

                        default:
                                break;

                }
        }

        /// <summary>
        /// A unit get its default target(left tower or right tower)
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="isOrange"></param>
        public void UnitGetTargetPosi(Unit unit, bool isOrange)
        {
                Building[] buildings = isOrange ? purpleBuildings : orangeBuildings;
                if (buildings[0] == null)//The base has destroyed
                {
                        return;
                }
                //The base is still alive
                int index = unit.transform.position.x //If the unit x position is less than base x position(which is on the left side)
                        <= buildings[0].transform.position.x ? 1 : 2;//then take index 1(buildings[1] which is left building)
                                                                     //Otherwise, use buildings[2] (right building).

                if (buildings[index].isDead)
                {
                        //if the index building is dead set default target to the base
                        unit.defaultTarget = buildings[0];
                }
                else
                {
                        //if the index building is not dead set default target to the building
                        unit.defaultTarget = buildings[index];
                }
        }

        public void EnableBase(bool isOrange)
        {

        }
}
