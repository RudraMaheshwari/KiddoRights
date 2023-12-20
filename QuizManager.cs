using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    public List<QuestionAndAnswers> QnA;
    public GameObject[] options;
    public Text QuestionTxt;
    public Text TimerText;
    public GameObject QuizPanel;
    public GameObject GoPanel;
    public Text ScoreText;
    public float timePerQuestion = 15.0f;

    private int totalQuestions;
    private int currentQuestion = 0;
    private int score = 0;
    private float timer;
    private bool isTimerRunning = false;

    public void Start()
    {
        totalQuestions = QnA.Count;
        GoPanel.SetActive(false);
        GenerateQuestion();
    }

    public void Update()
    {
        if (isTimerRunning)
        {
            timer -= Time.deltaTime;
            TimerText.text = Mathf.Ceil(timer).ToString();
            if (timer <= 0)
            {
                Wrong();
            }
        }
    }

    public void Retry()
    {
        currentQuestion = 0;
        score = 0;
        GoPanel.SetActive(false);
        QuizPanel.SetActive(true);
        timer = timePerQuestion;
        GenerateQuestion();
    }

    void GameOver()
    {
        QuizPanel.SetActive(false);
        GoPanel.SetActive(true);
        ScoreText.text = score + "/" + totalQuestions;
        timer = 0;
    }

    public void Correct()
    {
        score += 1;
        GenerateQuestion();
    }

    public void Wrong()
    {
        GenerateQuestion();
    }

    void SetAnswer()
    {
        for (int i = 0; i < options.Length; i++)
        {
            options[i].GetComponent<AnswerScript>().isCorrect = false;
            options[i].transform.GetChild(0).GetComponent<Text>().text = QnA[currentQuestion].Answers[i];

            if (QnA[currentQuestion].CorrectAnswer == i)
            {
                options[i].GetComponent<AnswerScript>().isCorrect = true;
            }
        }
    }

    void GenerateQuestion()
    {
        if (currentQuestion < totalQuestions)
        {
            QuestionTxt.text = QnA[currentQuestion].Question;
            SetAnswer();
            timer = timePerQuestion;
            isTimerRunning = true;
            currentQuestion++;
        }
        else
        {
            Debug.Log("Out of Questions");
            GameOver();
        }
    }
}