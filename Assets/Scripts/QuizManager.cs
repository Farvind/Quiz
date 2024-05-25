using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    public TextMeshProUGUI QuestionText;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI QuestionIndexText;
    public TextMeshProUGUI[] AnswerTexts;
    public Button[] AnswerButtons;
    public Button RestartButton;

    private Quiz quiz;
    private int currentQuestionIndex = 0;

    public string filePath = "quiz.json"; // Relative path to the JSON file
    public int score;

    public GameObject resultPannel;

    void Start()
    {
        LoadQuiz();
       

        // Add listeners for answer buttons
        for (int i = 0; i < AnswerButtons.Length; i++)
        {
            int index = i; // Capture the current value of i
            AnswerButtons[i].onClick.AddListener(() => OnAnswerSelected(index));
        }
        if (RestartButton != null)
        {
            RestartButton.onClick.AddListener(RestartQuiz);
           
        }
    }

    void LoadQuiz()
    {
        string fullPath = Path.Combine(Application.persistentDataPath, filePath);

        if (File.Exists(fullPath))
        {
            string json = File.ReadAllText(fullPath);

            quiz = JsonUtility.FromJson<Quiz>(json);

            Debug.Log("Quiz loaded successfully.");

            // Display the first question
            DisplayQuestion(currentQuestionIndex);
        }
        else
        {
            Debug.LogWarning("Quiz file not found at path: " + fullPath);
        }
    }

    void DisplayQuestion(int questionIndex)
    {
        if (quiz == null || questionIndex < 0 || questionIndex >= quiz.questions.Count)
        {
            Debug.LogWarning("Invalid question index or quiz data.");
            return;
        }

        QuestionText.text = quiz.questions[questionIndex].questionText;
        QuestionIndexText.text = "Q " + (questionIndex + 1);

        for (int i = 0; i < AnswerTexts.Length; i++)
        {
            if (i < quiz.questions[questionIndex].answers.Count)
            {
                AnswerTexts[i].gameObject.SetActive(true);
                AnswerTexts[i].text = quiz.questions[questionIndex].answers[i];
                AnswerButtons[i].gameObject.SetActive(true);
                AnswerButtons[i].interactable = true; // Enable answer buttons
            }
            else
            {
                AnswerTexts[i].gameObject.SetActive(false);
                AnswerButtons[i].gameObject.SetActive(false);
            }
        }

    }

    public void OnAnswerSelected(int answerIndex)
    {
        if (quiz == null || currentQuestionIndex < 0 || currentQuestionIndex >= quiz.questions.Count)
        {
            Debug.LogWarning("Invalid quiz data or question index.");
            return;
        }

        // Check if the selected answer is correct
        if (answerIndex == quiz.questions[currentQuestionIndex].correctAnswerIndex)
        {
            Debug.Log("Correct answer!");
            score++; // Increase the score
        }
        else
        {
            Debug.Log("Wrong answer!");
        }

        // Disable answer buttons after an answer is selected
        foreach (Button button in AnswerButtons)
        {
            button.interactable = false;
        }

        OnNextButtonClicked();
    }

    void OnNextButtonClicked()
    {
        currentQuestionIndex++;
        foreach (Button button in AnswerButtons)
        {
            button.interactable = true;
        }
        if (currentQuestionIndex < quiz.questions.Count)
        {
            // Display the next question
            DisplayQuestion(currentQuestionIndex);
        }
        else
        {
            // Quiz ends, display score or do something else
            resultPannel.SetActive(true);
            RestartButton.gameObject.SetActive(true);

            resultText.text = $"You have {score} correct answers."; // Set the result text
            Debug.Log("Quiz ends. Total score: " + score);
        }
    }

    void RestartQuiz()
    {
        currentQuestionIndex = 0;
        score = 0;

        // Hide the Restart button
        RestartButton.gameObject.SetActive(false);
        resultPannel.SetActive(false); // Hide the result panel

        // Display the first question again
        DisplayQuestion(currentQuestionIndex);
    }
}
