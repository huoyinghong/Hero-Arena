using UnityEngine;

public class UnitAnimationEvent : MonoBehaviour
{
        private Unit unit;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
                unit = GetComponentInParent<Unit>();
        }

        private void Attack()
        {
                unit.AttackAnimationEvent();
        }
}
