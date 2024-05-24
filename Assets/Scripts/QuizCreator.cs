using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;
using System;
using System.IO;

[Serializable]
public class Question
{
    public int questionIndex;
    public string questionText;
    public List<string> answers = new List<string>();
    public int correctAnswerIndex = -1;
}

[Serializable]
public class Quiz
{
    public List<Question> questions = new List<Question>();
}

public class QuizCreator : MonoBehaviour
{
    public TMP_InputField questionInputField;
    public List<TMP_InputField> answerInputFields;
    public List<GameObject> answerInputFieldsObject;
    public List<Toggle> answerToggles;
    public Button saveButton;
    public Button deleteButton;
    public TextMeshProUGUI QuestionIndicator;
    public Button addQuestionButton;
    public GameObject buttonPrefab;
    public Transform content;
    public GameObject questionPanel;
    public TMP_InputField questionText;

    private Quiz quiz = new Quiz();
    private int currentQuestionIndex = -1;

    private void Start()
    {
        LoadQuizData();
        if (saveButton != null)
        {
            saveButton.onClick.AddListener(SaveQuestion);
        }

        if (deleteButton != null)
        {
            deleteButton.onClick.AddListener(DeleteCurrentQuestion);
        }

        if (addQuestionButton != null)
        {
            addQuestionButton.onClick.AddListener(AddNewQuestion);
        }
        
        
    }

    void SaveQuestion()
    {
        if (string.IsNullOrEmpty(questionInputField.text))
        {
            Debug.LogError("Question text cannot be empty.");
            return;
        }

        // Check if editing an existing question
        Question question;
        if (currentQuestionIndex >= 0 && currentQuestionIndex < quiz.questions.Count)
        {
            question = quiz.questions[currentQuestionIndex];
        }
        else
        {
            question = new Question
            {
                questionIndex = quiz.questions.Count
            };
            quiz.questions.Add(question);
        }

        question.questionText = questionInputField.text;

        // Add all non-empty answers to the question
        question.answers.Clear();
        for (int i = 0; i < answerInputFields.Count; i++)
        {
            if (!string.IsNullOrEmpty(answerInputFields[i].text))
            {
                question.answers.Add(answerInputFields[i].text);
                if (answerToggles[i].isOn)
                {
                    question.correctAnswerIndex = i;
                }
            }
        }

        // Make sure there's at least one answer
        if (question.answers.Count == 0)
        {
            Debug.LogError("At least one answer must be provided.");
            return;
        }

        SaveQuizData();
        ClearInputFields();
        RefreshQuestionButtons();
    }

    void ClearInputFields()
    {
        questionInputField.text = "";
        foreach (var inputField in answerInputFields)
        {
            inputField.text = "";
        }
        foreach (var toggle in answerToggles)
        {
            toggle.isOn = false;
        }
    }

    void DeleteCurrentQuestion()
    {
        if (currentQuestionIndex >= 0 && currentQuestionIndex < quiz.questions.Count)
        {
            quiz.questions.RemoveAt(currentQuestionIndex);
            SaveQuizData();
            RefreshQuestionButtons();
            ClearInputFields();
        }
        else
        {
            Debug.LogWarning("Invalid question index.");
        }
    }

    void AddNewQuestion()
    {
        Question newQuestion = new Question
        {
            questionIndex = quiz.questions.Count,
            questionText = "",
            answers = new List<string> { "", "", "", "" } // Assuming you want 4 empty answers by default
        };

        // Add the new question to the quiz
        quiz.questions.Add(newQuestion);

        // Save the quiz data to ensure persistence
        SaveQuizData();

        // Refresh the UI to include the new question button
        AddQuestionButtonToUI(newQuestion);

        // Clear input fields and display the new question for editing
        ClearInputFields();
        DisplayQuestionContent(newQuestion.questionIndex);
    }

    private void LoadQuizData()
    {
        string filePath = Application.persistentDataPath + "/quiz.json";
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            quiz = JsonConvert.DeserializeObject<Quiz>(json);
            int indicatorindex = quiz.questions.Count;
            QuestionIndicator.text = "Q" + (indicatorindex + 1);
            RefreshQuestionButtons();
        }
        else
        {
            Debug.LogWarning("Quiz file not found. Starting with an empty quiz.");
        }
    }

    private void SaveQuizData()
    {
        string json = JsonConvert.SerializeObject(quiz, Formatting.Indented);
        string filePath = Application.persistentDataPath + "/quiz.json";
        File.WriteAllText(filePath, json);
    }

    private void RefreshQuestionButtons()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < quiz.questions.Count; i++)
        {
            AddQuestionButtonToUI(quiz.questions[i]);
        }
        
    }

    private void AddQuestionButtonToUI(Question question)
    {
        GameObject newButton = Instantiate(buttonPrefab, content);
        TextMeshProUGUI buttonText = newButton.GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText != null)
        {
            buttonText.text = "Q" + (question.questionIndex + 1);
        }

        Button button = newButton.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(() => DisplayQuestionContent(question.questionIndex));
        }
    }

    private void DisplayQuestionContent(int questionIndex)
    {
        if (questionIndex < 0 || questionIndex >= quiz.questions.Count)
        {
            Debug.LogWarning("Question index is out of range.");
            return;
        }

        currentQuestionIndex = questionIndex;
        Question question = quiz.questions[questionIndex];

        ClearInputFields();
        questionInputField.text = question.questionText;
        for (int i = 0; i < answerInputFields.Count; i++)
        {
            if (i < question.answers.Count)
            {
                answerInputFieldsObject[i].SetActive(true);
                answerInputFields[i].text = question.answers[i];
                answerToggles[i].isOn = (i == question.correctAnswerIndex);
            }
            else
            {
                answerInputFieldsObject[i].SetActive(false);
            }
        }
        QuestionIndicator.text = "Q" + (questionIndex + 1);
        questionPanel.SetActive(true);
    }
}
