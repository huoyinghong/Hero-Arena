using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
        public int ID;
        public Button button;
        private Vector3 cardInitPosi;
        private Tween tween;
        private bool isDragging;
        private bool showModel;
        public GameObject charModel;
        private Camera cam;
        public Text cardNameText;
        public GameObject spellCastingRange;
        public Transform imgGold;
        public int posiID;//Card position id on on the board
        public GameObject[] modelGOs;//All unit models
        public bool canCreateAnywhere;

        public AudioClip useCardClip;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
                cam = Camera.main;
                if (modelGOs == null || modelGOs.Length == 0)
                {
                        return;
                }
                for (int i = 0; i < modelGOs.Length; i++)//Hide all  units models
                {
                        modelGOs[i].SetActive(false);
                }
                
                if (ID <= 8)
                {
                        modelGOs[ID - 1].SetActive(true);//Show the model corresponding to the selected card ID. 
                }
                else
                {
                        spellCastingRange.SetActive(false);
                        canCreateAnywhere = GameController.Instance.unitInfos[ID - 1].canCreateAnywhere;
                }

        }

        // Update is called once per frame
        void Update()
        {
                if (ID <= 0) return;
                button.interactable = GameController.Instance.CanUseCard(ID);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
                if (!button.interactable)
                {
                        return;
                }
                if (!isDragging)
                {
                        tween = transform.DOLocalMove(cardInitPosi + new Vector3(0, 30, 0), 0.1f);
                }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
                if (!button.interactable)
                {
                        return;
                }
                if (!isDragging)
                {
                        tween.Pause();
                        transform.localPosition = cardInitPosi;
                }

        }

        public void SetInitPosi()
        {
                cardInitPosi = transform.localPosition;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
                if (!button.interactable)
                {
                        return;
                }
                tween.Pause();
                isDragging = true;

        }

        public void OnDrag(PointerEventData eventData)
        {
                if (!button.interactable)
                {
                        return;
                }
                Vector2 cardPosi;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), Input.mousePosition, null, out cardPosi);
                transform.localPosition = cardPosi;
                if (showModel)//Show Model
                {
                        float scale = Mathf.Clamp(((transform.localPosition.y - cardInitPosi.y) - 150) / 150, 0, 1);
                        charModel.transform.position = ScreenPointToWoldPoint(transform.position, 14.46f);
                        charModel.transform.localScale = Vector3.one * scale;
                        if (charModel.transform.localScale.x <= 0)
                        {//Back to board(show card)
                                showModel = false;
                                button.gameObject.SetActive(true);
                                charModel.SetActive(false);
                                if (ID > 8)
                                {
                                        spellCastingRange.SetActive(false);
                                }
                                cardNameText.gameObject.SetActive(false);
                        }
                }
                else//Show card
                {
                        float scale = Mathf.Clamp((150 - (transform.localPosition.y - cardInitPosi.y)) / 150, 0, 1);
                        button.transform.localScale = Vector3.one * scale;
                        if (button.transform.localScale.x <= 0)
                        {//back to field(show model)
                                showModel = true;
                                button.gameObject.SetActive(false);
                                charModel.SetActive(true);
                                if (ID > 8)
                                {
                                        spellCastingRange.SetActive(true);
                                }
                                cardNameText.gameObject.SetActive(true);
                        }
                }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
                if (!button.interactable)
                {
                        return;
                }

                if (showModel) //Model state
                {
                        imgGold.gameObject.SetActive(true);
                        //Check if the card in use is within deployment range
                        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                        RaycastHit[] hits = Physics.RaycastAll(ray);


                        if(!canCreateAnywhere && hits[0].collider.tag == "CantClick")
                        {//if the card is a magic card skip this
                                RetunToInitPosi();
                                UIManager.Instance.ShowMuskPanel();//if the card is the unit show undeployment area
                                return;
                        }
                        GameManager.Instance.PlaySound(useCardClip);

                        imgGold.DOLocalMove(imgGold.localPosition + new Vector3(0, 50, 0), 0.5f)
                                .OnComplete(() => { UseCurrentCard(hits); });
                }
                else//Card state
                {
                        RetunToInitPosi();
                }

        }

        /// <summary>
        /// Transfer screen coordinate to world coordinate
        /// </summary>
        /// <param name="screenpoint">The point on screen coordinate</param>
        /// <param name="planeZ">Distance from the camera's Z plane</param>
        /// <returns></returns>
        private Vector3 ScreenPointToWoldPoint(Vector2 screenpoint, float planeZ)
        {
                return cam.ScreenToWorldPoint(new Vector3(screenpoint.x, screenpoint.y, planeZ));
        }


        /// <summary>
        /// Return the card to its initial position on UI board when not used.
        /// </summary>
        private void RetunToInitPosi()
        {
                charModel.SetActive(false);
                button.gameObject.SetActive(true);
                cardNameText.gameObject.SetActive(false);
                imgGold.gameObject.SetActive(false);
                transform.DOLocalMove(cardInitPosi, 0.1f).OnComplete(() => { isDragging = false; });
        }

        /// <summary>
        ///Use the currently selected card.
        /// </summary>
        private void UseCurrentCard(RaycastHit[] hits)
        {
                //Spend gold
                GameController.Instance.DecreaseGoldValue(ID);
                //Raycast: check whether any of the detected colliders is plane
                for (int i = 0; i < hits.Length; i++)
                {
                        RaycastHit hit = hits[i];
                        if (hit.collider != null && hit.collider.tag == "Plane")
                        {
                                //If its on the plane then create the unit
                                GameController.Instance.CreatUnit(ID, hit.point);
                                //Empty selected card position on the board, then refill
                                UIManager.Instance.UseCard(posiID);

                                UIManager.Instance.RemoveCardIDFromList(ID);
                                Destroy(gameObject);//Destory currently using card
                        }
                }




        }


}
