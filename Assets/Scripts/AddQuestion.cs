using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AddQuestion : MonoBehaviour
{
    public Button addButton;
    public Button saveJsonButton; // New button to save JSON data
    public GameObject buttonPrefab;
    public Transform content; // The content object in the ScrollView
    public GameObject questionPanel;
    public TMP_InputField questionText;
    public TMP_InputField[] answerTexts;

    private List<Question> savedQuestions = new List<Question>();
    QuizCreator quizCreator;
    public TextMeshProUGUI QuestionIndicator;

    public int qsnIndex;

    private void Start()
    {
        quizCreator = FindObjectOfType<QuizCreator>();
        //if (addButton != null)
        //{
        //    addButton.onClick.AddListener(AddNewQuestionButton);
        //}

    }

    //public void AddNewQuestionButton()
    //{
    //    // Check if question text and at least one answer text are not empty
    //    if (!string.IsNullOrEmpty(questionText.text) && AtLeastOneAnswerNotEmpty())
    //    {
    //        // Proceed with adding the question
    //        GameObject newButton = Instantiate(buttonPrefab, content);
    //        int questionIndex = content.childCount - 1;

    //        Question question = quizCreator.GetQuestion(questionIndex);
    //        if (question != null)
    //        {
    //            savedQuestions.Add(question);

    //            TextMeshProUGUI buttonText = newButton.GetComponentInChildren<TextMeshProUGUI>();
    //            if (buttonText != null)
    //            {
    //                buttonText.text = "Q" + (questionIndex + 1);
    //            }

    //            Button button = newButton.GetComponent<Button>();
    //            if (button != null)
    //            {
    //                button.onClick.AddListener(() => DisplayQuestionContent(questionIndex));
    //            }
    //        }
    //        else
    //        {
    //            Debug.LogWarning("Failed to fetch question data from JSON.");
    //        }
    //    }
    //    else
    //    {
    //        Debug.LogWarning("Question text or at least one answer text is empty.");
    //    }
    //}

    //// Helper method to check if at least one answer text is not empty
    //private bool AtLeastOneAnswerNotEmpty()
    //{
    //    foreach (TMP_InputField answerInputField in answerTexts)
    //    {
    //        if (!string.IsNullOrEmpty(answerInputField.text))
    //        {
    //            return true;
    //        }
    //    }
    //    return false;
    //}
    //public void DisplayQuestionContent(int questionIndex)
    //{
    //    if (questionIndex < savedQuestions.Count)
    //    {
    //        Question question = savedQuestions[questionIndex];
    //        if (question != null)
    //        {
    //            questionPanel.SetActive(true);
    //            qsnIndex = questionIndex;
    //            questionText.text = question.questionText;

    //            for (int i = 0; i < answerTexts.Length; i++)
    //            {
    //                if (i < question.answers.Count)
    //                {
    //                    answerTexts[i].text = question.answers[i];
    //                    answerTexts[i].gameObject.SetActive(!string.IsNullOrEmpty(question.answers[i]));

    //                    if (quizCreator != null)
    //                    {
    //                        QuestionIndicator.text = "Q" + (questionIndex + 1);
    //                    }
    //                }
    //                else
    //                {
    //                    answerTexts[i].text = "";
    //                }
    //            }
    //        }
    //        else
    //        {
    //            Debug.LogWarning("Question data is null.");
    //        }
    //    }
    //    else
    //    {
    //        Debug.LogWarning("Question index out of range.");
    //    }
    //}

  
}
