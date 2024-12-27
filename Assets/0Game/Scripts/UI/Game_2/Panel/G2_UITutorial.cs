using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class G2_UITutorial : UIPanel
{
    [SerializeField] int currentPage;
    public List<GameObject> pages = new List<GameObject>();
    //[SerializeField] private Vector2 startDragPosition;
    //[SerializeField] private Vector2 endDragPosition;
    //[SerializeField] private float swipeThreshold = 50f;
    [SerializeField] ButtonEffectLogic nextButton;
    [SerializeField] ButtonEffectLogic backButton;
    public Image[] circleSymbols;
    [SerializeField] Sprite circleWhite;
    [SerializeField] Sprite circleBlack;
    protected override void Awake()
    {
        base.Awake();
        nextButton.onClick.AddListener(Next);
        backButton.onClick.AddListener(Back);
        Init();
    }
    public override void Init()
    {
        base.Init();
        ScrollToPage();
    }
    //public void FixedUpdate()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        OnPointerDown();
    //    }
    //    else if (Input.GetMouseButtonUp(0))
    //    {
    //        OnPointerUp();
    //    }
    //}
    //void OnPointerDown()
    //{
    //    startDragPosition = Input.mousePosition;
    //}

    //void OnPointerUp()
    //{
    //    endDragPosition = Input.mousePosition;
    //    DetectSwipeDirection();
    //}
    //private void DetectSwipeDirection()
    //{

    //    if ((endDragPosition - startDragPosition).magnitude >= swipeThreshold)
    //    {
    //        Vector2 swipeDirection = (endDragPosition - startDragPosition).normalized;

    //        if (swipeDirection.x < 0)
    //        {
    //            Next();
    //        }
    //        else if (swipeDirection.x > 0)
    //        {
    //            Back();
    //        }
    //    }
    //}
    public void Next()
    {
        currentPage++;
        if (currentPage > pages.Count-1)
        {
            currentPage = 0;
        }

        ScrollToPage();
    }
    public void Back()
    {
        currentPage--;
        if (currentPage < 0)
        {
            currentPage = pages.Count-1;
        }
        ScrollToPage();
    }
    private void ScrollToPage()
    {
        for (int i = 0; i < pages.Count; i++)
        {
            pages[i].SetActive(false);
        }
        pages[currentPage].SetActive(true);
        UpdateCircleSymbol();
    }
    public void UpdateCircleSymbol()
    {
        if (currentPage == 0)
        {
            ActiveCircle(0);
        }
        else if(currentPage == pages.Count-1)
        {
            ActiveCircle(2);
        }
        else
        {
            ActiveCircle(1);
        }

    }
    public void ActiveCircle(int id)
    {
        for(int i = 0;i<circleSymbols.Length;i++)
        {
            if (i == id)
            {
                circleSymbols[i].sprite = circleWhite;
                circleSymbols[i].GetComponent<RectTransform>().localScale = Vector3.one;
            }
            else
            {
                circleSymbols[i].GetComponent<RectTransform>().localScale = Vector3.one * 0.7f;
                circleSymbols[i].sprite = circleBlack;
            }
        }
    }
}
