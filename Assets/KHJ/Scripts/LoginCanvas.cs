using MySql.Data.MySqlClient;
using Photon.Pun;
using System;
using TMPro;
using UnityEngine;

public class LoginCanvas : MonoBehaviour
{
    [SerializeField] TMP_InputField idInputField;
    [SerializeField] TMP_InputField PasswordInputField;

    private MySqlConnection con;
    private MySqlDataReader reader;

    private void Start()
    {
        ConnectDataBase();
    }

    private void ConnectDataBase()
    {
        try
        {
            string serverInfo = "Server=43.202.3.31; Database=userdata; Uid=root; PWD=1234; Port=3306; CharSet=utf8;";
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

            string sqlCommand = string.Format("SELECT ID, PWD FROM user_info WHERE ID='{0}';", id);
            MySqlCommand cmd = new MySqlCommand(sqlCommand, con);
            reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    string readID = reader["ID"].ToString();
                    string readPass = reader["PWD"].ToString();

                    Debug.Log($"ID : {readID}, Pass : {readPass}");

                    if (pass == readPass)
                    {
                        PhotonNetwork.LocalPlayer.NickName = id;
                        PhotonNetwork.ConnectUsingSettings();
                        if (!reader.IsClosed)
                            reader.Close();
                    }
                    else
                    {
                        Debug.Log("비번틀림");
                    }
                }
            }
            else
            {
                Debug.Log("해당 아이디가 없슴");
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