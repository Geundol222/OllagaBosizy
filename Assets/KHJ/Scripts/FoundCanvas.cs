using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FoundCanvas : MonoBehaviour
{
    [SerializeField] TMP_InputField answerInputField;               //유저의 답변이 적힐 inputfield
    [SerializeField] LoginCanvas LC;                                //LoginCanvas의 데이터베이스를 받아오기 위함
    [SerializeField] LogImage answer;                               //유저에게 이것저것 알려 줄 것이 담길 image
    [SerializeField] Animator anim;                                 //FoundCanvas의 animator

    private MySqlConnection con;
    private MySqlDataReader reader;

    private void OnEnable()
    {
        //시작할 때 로그인켄버스에서 서버를 받아옴
        con = LC.con;
        anim.SetTrigger("IsOpen");
        answer.gameObject.SetActive(false);
    }

    //받아온 서버에서 유저가 입력한 PWDANSWER를 채크한 후 있으면 ID를 반환해주는 함수
    public void FoundID()
    {
        try
        {
            if (!LC.isConnected)
                LC.ConnectDataBase();
            string id = answerInputField.text;

            string sqlCommand = string.Format("SELECT ID FROM user_info WHERE PWDANSWER='{0}';", id);
            MySqlCommand cmd = new MySqlCommand(sqlCommand, con);
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    string readID = reader["ID"].ToString();
                    answer.gameObject.SetActive(true);
                    string printText = "당신의 아이디는 " + readID + "입니다.";
                    string text = (printText).ToString();
                    answer.SetText(text);
                    if (!reader.IsClosed)
                        reader.Close();
                }
            }
            else
            {
                answer.gameObject.SetActive(true);
                answer.SetText("그런 답변은 없습니다.");
            }
            if (!reader.IsClosed)
                reader.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return;
        }
    }

    //받아온 서버에서 유저가 입력한 PWDANSWER를 채크한 후 있으면 PASSWORD를 반환해주는 함수
    public void FoundPW()
    {
        try
        {
            if (!LC.isConnected)
                LC.ConnectDataBase();
            string id = answerInputField.text;

            string sqlCommand = string.Format("SELECT PWD FROM user_info WHERE PWDANSWER='{0}';", id);
            MySqlCommand cmd = new MySqlCommand(sqlCommand, con);
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    string readPass = reader["PWD"].ToString();
                    answer.gameObject.SetActive(true);
                    string printText = "당신의 비밀번호는 " + readPass + "입니다.";
                    string text = (printText).ToString();
                    answer.SetText(text);
                    if (!reader.IsClosed)
                        reader.Close();
                }
            }
            else
            {
                answer.gameObject.SetActive(true);
                answer.SetText("그런 답변은 없습니다.");
            }
            if (!reader.IsClosed)
                reader.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return;
        }
    }

    public void CloseCanvas()
    {
        StartCoroutine(CloseCanvasRoutine());
    }

    IEnumerator CloseCanvasRoutine()
    {
        answerInputField.text = "";
        anim.SetTrigger("IsClose");
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
        yield return null;
    }
}
