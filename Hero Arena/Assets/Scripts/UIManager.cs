using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;

public class UIManager : MonoBehaviour
{
        public static UIManager Instance;
        public Text currentGold;
        public Slider goldSlider;
        public Text timeText;
        private List<int> cardIDList = new List<int>();
        public GameObject cardGO;
        public Sprite[] cardSprites;
        public Sprite[] cardDisableSprites;
        private int maxCardNum = 4;
        private int currentCardNum = 0;
        public Transform nextCardTransform;
        public Transform[] boardCardsTransform;
        public Transform boardTransform;


        private void Awake()
        {
                Instance = this;
                CreateNewCard();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetGoldAmount()
        {
                currentGold.text = ((int)GameController.Instance.goldAmount).ToString();
        }

        public void SetGoldSlider()
        {
                goldSlider.value = GameController.Instance.goldAmount;
        }

        public void SetTime(int min, int sec)
        {
                timeText.text = min.ToString() + ":" + sec.ToString();
        }

        private void CreateNewCard()
        {
                if (currentCardNum > maxCardNum)
                {
                        return;
                }
                GameObject go = Instantiate(cardGO, nextCardTransform);
                go.transform.localPosition = Vector3.zero;

               
                int randomNum = Random.Range(2,2);
                //Avoid generating the same card consecutively
                //while (cardIDList.Contains(randomNum))
                //{
                //        randomNum = Random.Range(1, 11);
                //}
                //cardIDList.Add(randomNum);


                //Set card style
                Image image = go.transform.GetChild(0).GetComponent<Image>();
                image.sprite = cardSprites[randomNum - 1];
                //Set card style when disabled
                Button button = go.transform.GetChild(0).GetComponent<Button>();
                SpriteState ss = button.spriteState;
                ss.disabledSprite = cardDisableSprites[randomNum - 1];
                button.spriteState = ss;

                go.GetComponent<Card>().ID = randomNum;
                if (currentCardNum < maxCardNum)
                {
                        MoveCardToBoard(currentCardNum);
                }

        }
        /// <summary>
        /// Move the card to the board
        /// </summary>
        /// <param name="posID">Position ID</param>
        private void MoveCardToBoard(int posID)
        {
                Transform t = nextCardTransform.GetChild(0);
                t.SetParent(boardTransform);
                t.DOScale(Vector3.one * 1.1f, 0.2f);
                t.GetComponent<Card>().posiID = posID;

                t.DOLocalMove(boardCardsTransform[posID].localPosition, 0.3f).OnComplete
            (() => { CompleteMoveTween(t); });
        }
        /// <summary>
        /// The function called after a card complete its move animation to the board
        /// </summary>
        private void CompleteMoveTween(Transform t)
        {
                currentCardNum++;
                CreateNewCard();
                t.GetComponent<Card>().SetInitPosi();
        }

        /// <summary>
        /// Refill the card after use the card
        /// </summary>
        /// <param name="posID">Which card on the board is used</param>
        public void UseCard(int posID)
        {
                currentCardNum -= 1;
                MoveCardToBoard(posID);
        }

        public void RemoveCardIDFromList(int ID)//Remove used card from the list(line 60)
        {
                cardIDList.Remove(ID);
        }
}
