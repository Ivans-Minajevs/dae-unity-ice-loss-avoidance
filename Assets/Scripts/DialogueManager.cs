using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueManager : MonoBehaviour
{
    
    [SerializeField] GameObject fireworks;
    [SerializeField] Transform spawnPoint;  
    
    private VisualElement _dialoguePanel;
    private Label _answerText;
    private VisualElement _questionList;
    private Button _backButton;
    private Label _footer;
    private Father _father;
    
    private List<string> _options = new()
    {
        "Build arm (1 Metal, 5 Plastic, 1 Wood)",
        "Build heart (6 Metal, 2 Plastic, 0 Wood)",
        "Ask for direction",
        "Ask about surviving",
        "Ask about zombies"
    };

    private bool _isArmBuilt = false;
    private bool _isHeartBuilt = false;
    
    void Awake()
    {
        _father = FindObjectOfType<Father>();
    }

    private void OnEnable()
    {
       

        var root = GetComponent<UIDocument>().rootVisualElement;

        _dialoguePanel = root.Q<VisualElement>("DialoguePanel");
        _answerText = root.Q<Label>("AnswerText");
        _questionList = root.Q<ScrollView>("QuestionList");
        _backButton = root.Q<Button>("BackButton");
        _footer = root.Q<Label>("Footer");
        
        _dialoguePanel.style.display = DisplayStyle.None;
        _backButton.clicked += GoBack;
    }
    

    private void GoBack()
    {
        //_answerText.text = "Select a question to see the answer.";
        _dialoguePanel.style.display = DisplayStyle.None;
        _questionList.style.display = DisplayStyle.Flex; 
        _backButton.style.visibility = Visibility.Hidden; 
        
    }

    // New method to show dialogue options
    public void ShowDialogue()
    {
        Debug.Log("ShowDialogue called with options: " + string.Join(", ", _options));
        
        // Show the dialogue panel
        _dialoguePanel.style.display = DisplayStyle.Flex;
        
        // Clear previous options
        _questionList.Clear();

        // Add each option to the question list
        foreach (var option in _options)
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
            case "Build arm (1 Metal, 5 Plastic, 1 Wood)":
                if (!_isArmBuilt)
                {
                    if (_father != null)
                    {
                        _isArmBuilt = _father.TryBuildRobotPart("Arm", 1, 5, 1);
                        if (_isArmBuilt)
                        {
                            _answerText.text = "Arm was successfully built";
                            if (_isHeartBuilt)
                            {
                                Instantiate(fireworks, spawnPoint.position, spawnPoint.rotation);
                            }
                        }
                        else
                        {
                            _answerText.text = "Not enough resources to built an arm";
                        }
                    }
                }
                else
                {
                    _answerText.text = "Arm is already built";
                }

                break;
            case "Build heart (6 Metal, 2 Plastic, 0 Wood)":
                if (!_isHeartBuilt)
                {
                    if (_father != null)
                    {
                        _isHeartBuilt = _father.TryBuildRobotPart("Heart", 6, 2, 0);
                        if (_isHeartBuilt)
                        {
                            _answerText.text = "Heart was successfully built";
                            if (_isArmBuilt)
                            {
                                Instantiate(fireworks, spawnPoint.position, spawnPoint.rotation);
                            }
                        }
                        else
                        {
                            _answerText.text = "Not enough resources to built a heart";
                        }
                    }
                }
                else
                {
                    _answerText.text = "Heart is already built";
                }

                break;
            case "Ask for direction":
                // Logic for inspecting the mechanism
                if (!_isHeartBuilt && !_isArmBuilt)
                {
                    _answerText.text = "I don't have an arm. Building it requires plastic, \n" +
                                       "it is mostly located on the WEST";
                }
                else if (_isArmBuilt && !_isHeartBuilt)
                {
                    _answerText.text =
                        "Last component is my hear. I cant function without it. \n" +
                        "Heart requires metal. Deposits were discovered NORTH of here.";
                }
                else if (_isArmBuilt && _isHeartBuilt)
                {
                    _answerText.text = "We did it, Father. I can... I can... Thank you. (crying) \n" +
                                       "GAME IS COMPLETE";
                }

                break;
            case "Ask about surviving":
                // Logic for asking about functionality
                _answerText.text = "Its really cold outside, so always keep in mind your frostbite level. \n" +
                                   "Make sure you light bonfires located on the map. \n" +
                                   "House is your place to restore health and frostbite."

        ;
                break;
            case "Ask about zombies":
                _answerText.text =
                    "Zombies are very dangerous creatures, mainly spotted near sources. \n" +
                    "Be careful and dont rush into the fight, otherwise you will get killed.";
                break;
        }

        // Hide options after selection
        _questionList.style.display = DisplayStyle.None;
        _backButton.style.visibility = Visibility.Visible;
    }
}
