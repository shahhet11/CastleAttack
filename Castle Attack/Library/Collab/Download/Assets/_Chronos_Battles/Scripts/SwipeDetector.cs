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
   
    [SerializeField]
    private bool detectSwipeOnlyAfterRelease = false;

    [SerializeField]
    private float minDistanceForSwipe = 35f;

    public static event Action<SwipeData> OnSwipe = delegate { };
    int medievalCharMax = 2, modCharMax = 7, futCharMax = 11, mediLvlMax = 5, modLvlMax = 10, futLvlMax = 15, startIndex = 0, endIndex = 0;
    int medievalMacMax = 4, modMacMax = 9, futMacMax = 14;

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

    public void ShowCharacterAsPerEra(int lvlNo)
    {
        //Medilevel
        if (lvlNo <= mediLvlMax)
        {
            startIndex = 0;
            endIndex = medievalCharMax;
        }
        //Modern
        else if (lvlNo <= modLvlMax)
        {
            startIndex = medievalCharMax + 1;
            endIndex = modCharMax;
        }
        //Future
        else if (lvlNo <= futLvlMax)
        {
            startIndex = modCharMax + 1;
            endIndex = futCharMax;
        }

        for (int i = startIndex; i <= endIndex; i++)
        {
            if (PlayerPrefs.GetInt("Character" + i) == 1)
                currentEqId = i;
        }
        currentEqId++;
        if (currentEqId > endIndex)
            currentEqId = endIndex;

        UIScript.instance.OnStoreProductChange(currentEqId);
    }

    public void ShowMachineAsPerEra(int lvlNo,int productNo)
    {
        //Medilevel
        if (lvlNo <= mediLvlMax)
        {
            startIndex = 0;
            endIndex = medievalMacMax;
        }
        //Modern
        else if (lvlNo <= modLvlMax)
        {
            startIndex = medievalMacMax + 1;
            endIndex = modMacMax;
        }
        //Future
        else if (lvlNo <= futLvlMax)
        {
            startIndex = modMacMax + 1;
            endIndex = futMacMax;
        }

        for (int i = startIndex; i <= endIndex; i++)
        {
            if (PlayerPrefs.GetInt("Machinery" + i) == 1)
                currentEqId = i;
        }
        if (productNo == 0)
        {
            currentEqId++;
            if (currentEqId > endIndex)
                currentEqId = endIndex;
        }

        UIScript.instance.OnStoreProductChange(currentEqId);
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

                if (AllowSwipe)
                {
                    if (direction.ToString() == SwipeDirection.Up.ToString())
                    {
                        currentEqId++;
                        if (currentEqId > endIndex)
                            currentEqId = startIndex;

                        UIScript.instance.OnStoreProductChange(currentEqId);
                    }
                    else if (direction.ToString() == SwipeDirection.Down.ToString())
                    {
                        currentEqId--;
                        if (currentEqId < startIndex)
                            currentEqId = endIndex;

                        UIScript.instance.OnStoreProductChange(currentEqId);
                    }
                    AllowSwipe = false;
                }
            }
            else
            {
                var direction = fingerDownPosition.x - fingerUpPosition.x > 0 ? SwipeDirection.Right : SwipeDirection.Left;
                SendSwipe(direction);
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