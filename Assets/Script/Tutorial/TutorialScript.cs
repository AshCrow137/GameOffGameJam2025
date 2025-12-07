using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class TutorialScript : MonoBehaviour
{
    private int currentPage = 0;
    [SerializeField]
    private List<TutorialStruct> tutorialPages = new List<TutorialStruct>();
    [SerializeField]
    private Button nextButton;
    [SerializeField]
    private Button prevButton;
    [SerializeField]
    private TMP_Text currentPageText;
    private void Start()
    {
        prevButton.interactable = false;
        currentPageText.text = tutorialPages[currentPage].TutorialText;
    }
    public void OnTutorialOpen()
    {
        tutorialPages[currentPage].PageOpenEvent.Invoke();
    }
    public void OnTutorialClosed()
    {
        tutorialPages[currentPage].PageCloseEvent.Invoke();
    }
    public void NextPage()
    {
        tutorialPages[currentPage].PageCloseEvent.Invoke();
        currentPage++;
        if(currentPage >= tutorialPages.Count-1)
        {
            currentPage = tutorialPages.Count-1;
            nextButton.interactable = false;
        }
        prevButton.interactable = true;
        currentPageText.text = tutorialPages[currentPage].TutorialText;
        tutorialPages[currentPage].PageOpenEvent.Invoke();
    }
    public void PrevPage()
    {
        tutorialPages[currentPage].PageCloseEvent.Invoke();
        currentPage--;
        if(currentPage<=0)
        {
            currentPage = 0;
            prevButton.interactable = false;
        }
        nextButton.interactable = true;
        currentPageText.text = tutorialPages[currentPage].TutorialText;
        tutorialPages[currentPage].PageOpenEvent.Invoke();
    }
}

[Serializable]
public struct TutorialStruct
{
    public UnityEvent PageOpenEvent;
    public UnityEvent PageCloseEvent;
    [TextArea(15,20)]
    public string TutorialText;
}
