using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class BtnCard : MonoBehaviour
//cards in the main scene
{
        public bool isBattleCard;
        public int posIndex;
        public MainUIManager mainUIManager;
        private Button button;

        private void Start()
        {
                button = GetComponent<Button>();
                button.onClick.AddListener(MoveToTargetPos);
        }

        public void MoveToTargetPos()
        {
                button.interactable = false;
                if (mainUIManager.freeStorePosIndexList.Count > 0)
                {
                        if (isBattleCard)
                        {
                                if (mainUIManager.freeStorePosIndexList.Count <= 0)
                                {
                                        button.interactable = true;
                                        return;
                                }
                                transform.DOLocalMove(mainUIManager.GetFreeStorePos(posIndex), 0.3f).
                                    OnComplete(() => { button.interactable = true; isBattleCard = false; }
                                    );

                        }
                        else
                        {
                                if (mainUIManager.freeBattlePosIndexList.Count <= 0)
                                {
                                        button.interactable = true;
                                        return;
                                }
                                transform.DOLocalMove(mainUIManager.GetFreeBattlePos(posIndex), 0.3f).
                                    OnComplete(() => { button.interactable = true; isBattleCard = true; }
                                    );
                        }
                }
        }
}
