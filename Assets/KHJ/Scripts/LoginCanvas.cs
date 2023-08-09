using MySql.Data.MySqlClient;
using Photon.Pun;
using System;
using TMPro;
using UnityEngine;

public class LoginCanvas : MonoBehaviour
{
    [SerializeField] TMP_InputField idInputField;
    [SerializeField] TMP_InputField PasswordInputField;
    [SerializeField] LogImage logImage;
    public MySqlConnection con;
    public MySqlDataReader reader;

    private void Start()
    {
        ConnectDataBase();
        logImage.gameObject.SetActive(false);
    }

    private void ConnectDataBase()
    {
        try
        {
            string serverInfo = "Server=127.0.0.1; Database=userdata; Uid=root; PWD=shdP23gh; Port=3306; CharSet=utf8mb4_general_ci;";
            con = new MySqlConnection(serverInfo);
            con.Open();

            Debug.Log("데이터베이스 접속 성공");
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public void Login()
    {
        try
        {
            string id = idInputField.text;
            string pass = PasswordInputField.text;

            string sqlCommand = string.Format("SELECT ID, PWD, NICKNAME, PWDANSWER FROM user_info WHERE ID='{0}';", id);
            MySqlCommand cmd = new MySqlCommand(sqlCommand, con);
            reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    string readID = reader["ID"].ToString();
                    string readPass = reader["PWD"].ToString();
                    string readnick = reader["NICKNAME"].ToString();
                    string readPassANS = reader["PWDANSWER"].ToString();

                    Debug.Log($"ID : {readID}, Pass : {readPass}, PassANS : {readPassANS}");

                    if (pass == readPass)
                    {
                        PhotonNetwork.LocalPlayer.NickName = readnick;
                        PhotonNetwork.ConnectUsingSettings();
                    }
                    else
                    {
                        logImage.gameObject.SetActive(true);
                        logImage.SetText("비밀번호가 틀렸습니다.");
                    }
                }
            }
            else
            {
                logImage.gameObject.SetActive(true);
                logImage.SetText("해당 아이디가 없습니다.");
            }
            if (!reader.IsClosed)
                reader.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }
}