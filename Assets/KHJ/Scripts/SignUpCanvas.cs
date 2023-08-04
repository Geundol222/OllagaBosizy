using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class SignUpCanvas : MonoBehaviour
{
    [SerializeField] TMP_InputField idInputField;
    [SerializeField] TMP_InputField PasswordInputField;
    [SerializeField] TMP_InputField PasswordAgainInputField;
    [SerializeField] TMP_InputField NickNameInputField;
    [SerializeField] TMP_InputField answerInputField;
    [SerializeField] LoginCanvas LC;
    [SerializeField] GameObject answer;
    [SerializeField] GameObject logCanvas;
    [SerializeField] TMP_Text log;
    private MySqlConnection con;
    private MySqlDataReader reader;
    private bool isCheckID;
    private bool isCheckNickName;

    private void OnEnable()
    {
        con = LC.con;
        isCheckID = false;
        isCheckNickName = false;
    }

    public void CheckID()
    {
        try
        {
            string id = idInputField.text;

            string sqlCommand = string.Format("SELECT ID FROM user_info WHERE ID='{0}';", id);

            MySqlCommand cmd = new MySqlCommand(sqlCommand, con);
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                logCanvas.SetActive(true);
                log.text = "중복 아이디가 있습니다.";
            }
            else
            {
                logCanvas.SetActive(true);
                log.text = "아이디를 만들 수 있습니다.";
                isCheckID = true;
            }
            reader.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public void CheckNickName()
    {
        try
        {
            string name = NickNameInputField.text;

            string sqlCommand = string.Format("SELECT NICKNAME FROM user_info WHERE NICKNAME='{0}';", name);

            MySqlCommand cmd = new MySqlCommand(sqlCommand, con);
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                logCanvas.SetActive(true);
                log.text = "중복 유저이름이 있습니다.";
            }
            else
            {
                logCanvas.SetActive(true);
                log.text = "유저이름을 만들 수 있습니다.";
                isCheckNickName = true;
            }
            reader.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public void OKButton()
    {
        try
        {
            string id = idInputField.text;
            string pwd = PasswordInputField.text;
            string pwdck = PasswordAgainInputField.text;
            string name = NickNameInputField.text;

            if (pwd == pwdck)
            {
                if (isCheckID)
                {
                    if (isCheckNickName)
                    {
                        answer.SetActive(true);
                    }
                    else
                    {
                        logCanvas.SetActive(true);
                        log.text = "유저이름 중복을 확인해주세요.";
                    }
                }
                else
                {
                    logCanvas.SetActive(true);
                    log.text = "아이디 중복을 확인해주세요.";
                }
            }
            else
            {
                logCanvas.SetActive(true);
                log.text = "비밀번호와 확인비밀번호가 다릅니다.";
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public void CreateID()
    {
        string id = idInputField.text;
        string pwd = PasswordInputField.text;
        string name = NickNameInputField.text;
        string answerstring = answerInputField.text;

        string sqlCommand = string.Format("INSERT INTO `userdata`.`user_info` (`ID`, `PWD`, `NICKNAME`, `PWDANSWER`) VALUES ('{0}','{1}','{2}','{3}');", id, pwd, name, answerstring);
        Debug.Log(sqlCommand);
        MySqlCommand cmd = new MySqlCommand(sqlCommand, con);
        reader = cmd.ExecuteReader();
        reader.Close();
        answer.SetActive(false);
    }

    public void CloseCanvas()
    {
        idInputField.text = "";
        PasswordInputField.text = "";
        PasswordAgainInputField.text = "";
        NickNameInputField.text = "";
        answerInputField.text = "";
    }

    public void CloseLogCanvas()
    {
        log.text = "";
        logCanvas.SetActive(false);
    }
}
