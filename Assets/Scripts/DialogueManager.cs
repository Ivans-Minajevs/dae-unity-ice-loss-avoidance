using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueManager : MonoBehaviour
{
    private VisualElement _dialoguePanel;
    private Label _answerText;
    private VisualElement _questionList;
    private Button _backButton;
    private Label _footer;

    // Dictionary to hold questions and their corresponding answers
    private Dictionary<string, string> questionsAndAnswers = new Dictionary<string, string>
    {
        { "What is your purpose?", "I am here to assist you!" },
        { "What is your favorite color?", "I like blue, it's very calming." },
        { "Do you require assistance?", "I am always ready to help!" }
    };

    private void OnEnable()
    {
       

        var root = GetComponent<UIDocument>().rootVisualElement;

        _dialoguePanel = root.Q<VisualElement>("DialoguePanel");
        _answerText = root.Q<Label>("AnswerText");
        _questionList = root.Q<ScrollView>("QuestionList");
        _backButton = root.Q<Button>("BackButton");
        _footer = root.Q<Label>("Footer");
        
        _dialoguePanel.style.display = DisplayStyle.None;

        foreach (var question in questionsAndAnswers.Keys)
        {
            Button questionButton = new Button
            {
                text = question,
                name = "QuestionButton"
            };
            questionButton.AddToClassList("question-button");
            questionButton.clicked += () => ShowAnswer(question);
            _questionList.Add(questionButton);
        }

        _backButton.clicked += GoBack;
    }

    private void ShowAnswer(string question)
    {
        if (questionsAndAnswers.TryGetValue(question, out string answer))
        {
            _answerText.text = answer;
            _footer.text = "";  
            _questionList.style.display = DisplayStyle.None; // Hide the question list
            _backButton.style.visibility = Visibility.Visible;
        }
    }

    private void GoBack()
    {
        // Hide the answer and show the question list again
        _answerText.text = "Select a question to see the answer.";
        _questionList.style.display = DisplayStyle.Flex; // Show the question list
        _backButton.style.visibility = Visibility.Hidden; // Hide the back button
    }

    // New method to show dialogue options
    public void ShowDialogue(List<string> options)
    {
        Debug.Log("ShowDialogue called with options: " + string.Join(", ", options));
        
        // Show the dialogue panel
        _dialoguePanel.style.display = DisplayStyle.Flex;
        
        // Clear previous options
        _questionList.Clear();

        // Add each option to the question list
        foreach (var option in options)
        {
            Button optionButton = new Button
            {
                text = option,
                name = "OptionButton"
            };
            optionButton.AddToClassList("option-button");
            optionButton.clicked += () => HandleOptionSelected(option);
            _questionList.Add(optionButton);
        }
        
        // Hide the answer text and show the footer
        _answerText.text = "Select an option to get a response.";
        _footer.text = "Make your choice.";
    }

    private void HandleOptionSelected(string option)
    {
        // Handle what happens when an option is selected
        switch (option)
        {
            case "Build arm (requires resources)":
                // Trigger the build arm logic
                Debug.Log("Attempting to build the arm...");
                break;
            case "Inspect mechanism":
                // Logic for inspecting the mechanism
                _answerText.text = "This mechanism is capable of various tasks.";
                break;
            case "Ask about functionality":
                // Logic for asking about functionality
                _answerText.text = "I can help you build and repair components.";
                break;
        }

        // Hide options after selection
        _questionList.style.display = DisplayStyle.None; // Hide the options
        _backButton.style.visibility = Visibility.Visible; // Ensure back button is visible
    }
}
