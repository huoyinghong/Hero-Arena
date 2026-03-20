using UnityEngine;

public class Death : Character
{
        public AudioClip callSkeletonClip;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected override void Start()
        {
                base.Start();
                InvokeRepeating("CreateSkeletons", 1, 4);
        }

        private void CreateSkeletons()
        {
                GameManager.Instance.PlaySound(callSkeletonClip);
                GameController.Instance.CreatUnit(11, transform.position + new Vector3(0,0,1), isOrange);
                GameController.Instance.CreatUnit(11, transform.position + new Vector3(-1, 0, 1), isOrange);
                GameController.Instance.CreatUnit(11, transform.position + new Vector3(1, 0, 1), isOrange);

        }
}
