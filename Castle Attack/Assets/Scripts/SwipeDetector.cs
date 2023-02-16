using System;
using UnityEngine;
using UnityEngine.UI;
public class SwipeDetector : MonoBehaviour
{



    public static SwipeDetector instance;
    private Vector2 fingerDownPosition;
    private Vector2 fingerUpPosition;
    public int currentEqId = 0;
    bool AllowSwipe = true;
    //public GameObject Swipemsg;
    //public Text DirectionCheck;
    //public Text currentEqIdText;
    [SerializeField]
    private bool detectSwipeOnlyAfterRelease = false;

    [SerializeField]
    private float minDistanceForSwipe = 35f;

    public static event Action<SwipeData> OnSwipe = delegate { };

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Update()
    {
        if (UIScript.instance.StoreUI.activeInHierarchy)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    fingerUpPosition = touch.position;
                    fingerDownPosition = touch.position;

                }

                if (!detectSwipeOnlyAfterRelease && touch.phase == TouchPhase.Moved)
                {
                    fingerDownPosition = touch.position;
                    DetectSwipe();

                }

                if (touch.phase == TouchPhase.Ended)
                {
                    fingerDownPosition = touch.position;
                    DetectSwipe();
                    AllowSwipe = true;

                }
            }
        }
    }

    private void DetectSwipe()
    {
        if (SwipeDistanceCheckMet() && PlayerPrefs.GetInt("GameStarted") != 1)
        {
            if (IsVerticalSwipe())
            {
                var direction = fingerDownPosition.y - fingerUpPosition.y > 0 ? SwipeDirection.Up : SwipeDirection.Down;
                SendSwipe(direction);
                //DirectionCheck.text = "Vertical";
            }
            else
            {
                var direction = fingerDownPosition.x - fingerUpPosition.x > 0 ? SwipeDirection.Right : SwipeDirection.Left;
                SendSwipe(direction);

                if (AllowSwipe)
                {
                if (direction.ToString() == SwipeDirection.Right.ToString())
                {
                    currentEqId++;
                        if (currentEqId > 15)
                        {
                            currentEqId = 0;
                            if (UIScript.instance.MachineryStoreUI.activeInHierarchy)
                            {
                                // use currentEqId as array index for displaying
                                // Machinery Related
                                UIScript.instance.OnStoreProductChange(0,currentEqId);
                            }
                            else if (UIScript.instance.CharacterStoreUI.activeInHierarchy)
                            {
                                // use currentEqId as array index for displaying
                                // Character Related
                                UIScript.instance.OnStoreProductChange(1, currentEqId);
                            }
                            // Change Machinery or Character to Right Starting from Selected Product(When Array out of Index)

                        }
                        else
                        {
                            // Change Machinery or Character to Right Starting from Selected Product
                            if (UIScript.instance.MachineryStoreUI.activeInHierarchy)
                            {
                                // use currentEqId as array index for displaying
                                // Machinery Related
                                UIScript.instance.OnStoreProductChange(0, currentEqId);
                            }
                            else if (UIScript.instance.CharacterStoreUI.activeInHierarchy)
                            {
                                // use currentEqId as array index for displaying
                                // Character Related
                                UIScript.instance.OnStoreProductChange(1, currentEqId);
                            }
                        }
                       
                        
                }
                else if (direction.ToString() == SwipeDirection.Left.ToString())
                {
                    
                    if (currentEqId > 0)
                    {
                           
                            currentEqId--;
                            // Change Machinery or Character to Left Starting from Selected Product
                            if (UIScript.instance.MachineryStoreUI.activeInHierarchy)
                            {
                                // use currentEqId as array index for displaying
                                // Machinery Related
                                UIScript.instance.OnStoreProductChange(0, currentEqId);
                            }
                            else if (UIScript.instance.CharacterStoreUI.activeInHierarchy)
                            {
                                // use currentEqId as array index for displaying
                                // Character Related
                                UIScript.instance.OnStoreProductChange(1, currentEqId);
                            }
                        }
                        if (currentEqId == 0)
                        {
                            
                            currentEqId = 15;
                            // Change Machinery or Character to Left Starting from Selected Product(When Array out of Index)
                            if (UIScript.instance.MachineryStoreUI.activeInHierarchy)
                            {
                                // use currentEqId as array index for displaying
                                // Machinery Related
                                UIScript.instance.OnStoreProductChange(0, currentEqId);
                            }
                            else if (UIScript.instance.CharacterStoreUI.activeInHierarchy)
                            {
                                // use currentEqId as array index for displaying
                                // Character Related
                                UIScript.instance.OnStoreProductChange(1, currentEqId);
                            }

                        }
                }

                    AllowSwipe = false;
                }

            }
            fingerUpPosition = fingerDownPosition;
        }
    }



    public void Equip()
    {
        //UnEquipLast();

        PlayerPrefs.SetInt("Filter", 2);
        

        currentEqId = 0;

        //CheckWeapon(scrollSnap._currentPage);

        //TankManager.ActiveWeaponById();
    }
    private bool IsVerticalSwipe()
    {
        return VerticalMovementDistance() > HorizontalMovementDistance();
    }

    private bool SwipeDistanceCheckMet()
    {
        return VerticalMovementDistance() > minDistanceForSwipe || HorizontalMovementDistance() > minDistanceForSwipe;
    }

    private float VerticalMovementDistance()
    {
        return Mathf.Abs(fingerDownPosition.y - fingerUpPosition.y);
    }

    private float HorizontalMovementDistance()
    {
        return Mathf.Abs(fingerDownPosition.x - fingerUpPosition.x);
    }

    private void SendSwipe(SwipeDirection direction)
    {
        SwipeData swipeData = new SwipeData()
        {
            Direction = direction,
            StartPosition = fingerDownPosition,
            EndPosition = fingerUpPosition
        };
        OnSwipe(swipeData);
    }
}

public struct SwipeData
{
    public Vector2 StartPosition;
    public Vector2 EndPosition;
    public SwipeDirection Direction;
}

public enum SwipeDirection
{
    Up,
    Down,
    Left,
    Right
}