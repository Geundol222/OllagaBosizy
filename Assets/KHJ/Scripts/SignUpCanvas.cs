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
    [SerializeField] LogImage logImage;
    [SerializeField] Animator anim;
    private MySqlConnection con;
    private MySqlDataReader reader;
    private bool isCheckID;
    private bool isCheckNickName;

    //enable되엇을 때 로그인canvas에서 접속한 데이터베이스를 들고와 사용하게 함.
    private void OnEnable()
    {
        con = LC.con;
        isCheckID = false;
        isCheckNickName = false;
        logImage.gameObject.SetActive(false);
        answer.SetActive(false);
        anim.SetTrigger("IsOpen");
    }

    //중복된 아이디로 만들어 지는 것을 막기 위한 함수
    public void CheckID()
    {
        if (!LC.isConnected)
            LC.ConnectDataBase();
        try
        {
            string id = idInputField.text;

            string sqlCommand = string.Format("SELECT ID FROM user_info WHERE ID='{0}';", id);

            MySqlCommand cmd = new MySqlCommand(sqlCommand, con);
            reader = cmd.ExecuteReader();
            //받은 아이디 값이 데이터베이스에 존재 한다면
            if (reader.HasRows)
            {
                logImage.gameObject.SetActive(true);
                logImage.SetText("중복 아이디가 있습니다.");
                isCheckID = false;
            }
            else
            {
                logImage.gameObject.SetActive(true);
                logImage.SetText("아이디를 만들 수 있습니다.");
                isCheckID = true;
            }
            reader.Close();
        }
        catch (Exception e)
        {
            return;
        }
    }

    //중복된 닉네임으로 만들어 지는 것을 방지하기 위한 함수
    public void CheckNickName()
    {
        try
        {
            if (!LC.isConnected)
                LC.ConnectDataBase();
            string name = NickNameInputField.text;

            string sqlCommand = string.Format("SELECT NICKNAME FROM user_info WHERE NICKNAME='{0}';", name);

            MySqlCommand cmd = new MySqlCommand(sqlCommand, con);
            reader = cmd.ExecuteReader();
            //받은 닉네임 값이 데이터베이스에 존재 한다면
            if (reader.HasRows)
            {
                logImage.gameObject.SetActive(true);
                logImage.SetText("중복 유저이름이 있습니다.");
                isCheckNickName = false;
            }
            else
            {
                logImage.gameObject.SetActive(true);
                logImage.SetText("유저이름을 만들 수 있습니다.");
                isCheckNickName = true;
            }
            reader.Close();
        }
        catch (Exception e)
        {
            return;
        }
    }

    //중복여부 검사를 진행하였는지 확인하고 아이디를 만드는 창을 띄워주는 함수
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
                        logImage.gameObject.SetActive(true);
                        logImage.SetText("유저이름 중복을 확인해주세요.");
                    }
                }
                else
                {
                    logImage.gameObject.SetActive(true);
                    logImage.SetText("아이디 중복을 확인해주세요.");
                }
            }
            else
            {
                logImage.gameObject.SetActive(true);
                logImage.SetText("비밀번호와 확인비밀번호가 다릅니다.");
            }
        }
        catch (Exception e)
        {
            return;
        }
    }

    //유저가 아이디, 비번을 잊어먹을 경우를 생각하여 PWDANSWER를 따로 받아 저장하면서 아이디를 만드는 함수
    public void CreateID()
    {
        if (!LC.isConnected)
            LC.ConnectDataBase();
        string id = idInputField.text;
        string pwd = PasswordInputField.text;
        string name = NickNameInputField.text;
        string answerstring = answerInputField.text;

        string sqlCommand = string.Format("INSERT INTO `user_data`.`user_info` (`ID`, `PWD`, `NICKNAME`, `PWDANSWER`) VALUES ('{0}','{1}','{2}','{3}');", id, pwd, name, answerstring);
        Debug.Log(sqlCommand);
        MySqlCommand cmd = new MySqlCommand(sqlCommand, con);
        reader = cmd.ExecuteReader();
        reader.Close();
        answer.SetActive(false);
    }

    public void CloseCanvas()
    {
        StartCoroutine(CloseCanvasRoutine());
    }

    //전에 입력하였던 값들을 지워주는 것을 실행
    IEnumerator CloseCanvasRoutine()
    {
        idInputField.text = "";
        PasswordInputField.text = "";
        PasswordAgainInputField.text = "";
        NickNameInputField.text = "";
        answerInputField.text = "";
        anim.SetTrigger("IsClose");
        yield return new WaitForSeconds(0.4f);
        gameObject.SetActive(false);
        yield return null;
    }
}
