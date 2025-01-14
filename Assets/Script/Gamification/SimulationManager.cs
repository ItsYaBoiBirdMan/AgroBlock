using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SimulationManager : MonoBehaviour
{
   [Header("UI References")]
    [SerializeField] private Button[] optionButtons;
    [SerializeField] private TextMeshProUGUI botText;
    [SerializeField] private TextMeshProUGUI alertText;
    [SerializeField] private GameObject agroBotPanel;
    
    [Header("Simulation Settings")]
    [SerializeField] private int correctAnswer;
    [SerializeField] private string[] tipMessages;
    [SerializeField] private float returnDelay = 2f;
    
    private int attempts = 0;
    
    void Start()
    {
        SetupButtons();
        ShowInitialPrompt();
    }
    
    private void SetupButtons()
    {
        for(int i = 0; i < optionButtons.Length; i++)
        {
            int optionIndex = i;
            optionButtons[i].onClick.AddListener(() => HandleAnswer(optionIndex));
        }
    }
    
    private void ShowInitialPrompt()
    {
        botText.text = "Select an option";
        //AnimateBot();
    }
    
    private void HandleAnswer(int selectedOption)
    {
        DisableAllButtons();
        
        // Change the text color of the selected option to white
        optionButtons[selectedOption].GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        
        if(selectedOption == correctAnswer)
        {
            HandleCorrectAnswer();
            EventManager.GiveUserPointsAfterTutorial.Invoke(50);
        }
        else
        {
            HandleWrongAnswer(selectedOption);
        }
    }
    
    private void HandleCorrectAnswer()
    {
        optionButtons[correctAnswer].GetComponent<Image>().color = Color.green;
        string tip = tipMessages[Random.Range(0, tipMessages.Length)];
        botText.text = $"You're correct!\nTip: {tip}";
        
        StartCoroutine(ReturnToMainMenu(returnDelay));
    }
    
    private void HandleWrongAnswer(int selectedOption)
    {
        attempts++;
        optionButtons[selectedOption].GetComponent<Image>().color = Color.red;
        
        if(attempts >= 2)
        {
            RevealCorrectAnswer();
        }
        else
        {
            botText.text = "Try another option!";
            StartCoroutine(ResetButton(selectedOption));
            EnableAllButtons();
        }
    }
    
    private void RevealCorrectAnswer()
    {
        optionButtons[correctAnswer].GetComponent<Image>().color = Color.green;
        botText.text = $"The correct answer was option {correctAnswer + 1}. This is because it enables consistent water distribution";
        StartCoroutine(ReturnToMainMenu(returnDelay));
    }
    
    private void DisableAllButtons()
    {
        foreach(Button button in optionButtons)
        {
            button.interactable = false;
        }
    }
    
    private void EnableAllButtons()
    {
        foreach(Button button in optionButtons)
        {
            button.interactable = true;
        }
    }
    
    private IEnumerator ResetButton(int buttonIndex)
    {
        yield return new WaitForSeconds(1f);
        optionButtons[buttonIndex].GetComponent<Image>().color = Color.white;
        optionButtons[buttonIndex].GetComponentInChildren<TextMeshProUGUI>().color = Color.black; // Reset text color
    }
    
    private IEnumerator ReturnToMainMenu(float delay)
    {
        yield return new WaitForSeconds(delay);
       
        EventManager.CloseAgroSimEvent.Invoke();
        
        Destroy(gameObject);
    }
    
    /*private void AnimateBot()
    {
        LeanTween.scale(agroBotPanel, new Vector3(1.1f, 1.1f, 1f), 0.3f)
            .setEase(LeanTweenType.easeOutBack)
            .setLoopPingPong(1);
    }*/
}
