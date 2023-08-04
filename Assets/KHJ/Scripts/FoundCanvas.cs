using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FoundCanvas : MonoBehaviour
{
    [SerializeField] TMP_InputField answerInputField;
    [SerializeField] LoginCanvas LC;
    [SerializeField] GameObject answerCanvas;
    [SerializeField] TMP_Text answer;

    private MySqlConnection con;
    private MySqlDataReader reader;

    private void OnEnable()
    {
        con = LC.con;
    }

    public void FoundID()
    {
        try
        {
            string id = answerInputField.text;

            string sqlCommand = string.Format("SELECT ID FROM user_info WHERE PWDANSWER='{0}';", id);

            MySqlCommand cmd = new MySqlCommand(sqlCommand, con);
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                string readID = reader["ID"].ToString();
                answerCanvas.SetActive(true);
                answer.text = readID;
            }
            else
            {
                answerCanvas.SetActive(true);
                answer.text = "그런 답변은 없습니다.";
            }
            reader.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public void FoundPW()
    {
        try
        {
            string id = answerInputField.text;

            string sqlCommand = string.Format("SELECT PWD FROM user_info WHERE PWDANSWER='{0}';", id);

            MySqlCommand cmd = new MySqlCommand(sqlCommand, con);
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                string readPass = reader["PWD"].ToString();
                answerCanvas.SetActive(true);
                answer.text = readPass;
            }
            else
            {
                answerCanvas.SetActive(true);
                answer.text = "그런 답변은 없습니다.";
            }
            reader.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public void OkButton()
    {
        answer.text = "";
        answerCanvas.SetActive(false);
    }
}
