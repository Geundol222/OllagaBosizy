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

            string sqlCommand = string.Format("SELECT PWDANSWER FROM user_info WHERE ID='{0}';", id);

            MySqlCommand cmd = new MySqlCommand(sqlCommand, con);
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                string readID = reader["ID"].ToString();

                Debug.Log($"ID : {readID}");
            }
            else
            {
                Debug.Log("그런 답변은 없슴!");
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

            string sqlCommand = string.Format("SELECT PWDANSWER FROM user_info WHERE ID='{0}';", id);

            MySqlCommand cmd = new MySqlCommand(sqlCommand, con);
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                string readPass = reader["PWD"].ToString();

                Debug.Log($"Password : {readPass}");
            }
            else
            {
                Debug.Log("그런 답변은 없슴!");
            }
            reader.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }
}
