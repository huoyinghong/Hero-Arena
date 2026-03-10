using UnityEngine;

public class Death : Character
{
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected override void Start()
        {
                base.Start();
                InvokeRepeating("CreateSkeletons", 1, 4);
        }

        private void CreateSkeletons()
        {
                GameController.Instance.CreatUnit(11, transform.position + new Vector3(0,0,1), isOrange);
                GameController.Instance.CreatUnit(11, transform.position + new Vector3(-1, 0, 1), isOrange);
                GameController.Instance.CreatUnit(11, transform.position + new Vector3(1, 0, 1), isOrange);

        }
}
