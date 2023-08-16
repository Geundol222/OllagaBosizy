using MySql.Data.MySqlClient;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class LoginCanvas : MonoBehaviour
{
    [SerializeField] TMP_InputField idInputField;               //유저가 적은 id inputfield
    [SerializeField] TMP_InputField PasswordInputField;         //유저가 적은 pwd를 받아오는 inputfield
    [SerializeField] LogImage logImage;                         //유저에게 이것저것 알려주기 위한 오브잭트
    public GameObject SignUpCanvas;                             //회원가입 창
    public GameObject FoundCanvas;                              //id&pwd찾는 창
    public MySqlConnection con;                                 //회원가입창이나 id&pwd찾는 창에서 데이터베이스를 받아가게 하기 위해 제작
    public MySqlDataReader reader;                              //
    Animator anim;
    public bool isConnected = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    //데이터베이스에 연결되어있지 않으면 연결하고 logImage나 SignUpCanvas, FoundCanvas가 켜져있다면 꺼주는 함수
    private void OnEnable()
    {
        if (!isConnected)
            ConnectDataBase();
        logImage.gameObject.SetActive(false);
        SignUpCanvas.SetActive(false);
        FoundCanvas.SetActive(false);
    }

    //데이터 베이스에 연결해주는 함수
    public void ConnectDataBase()
    {
        try
        {
            string serverInfo = "Server=3.34.182.2; Database=user_data; Uid=root; PWD=dhffkrkwh; Port=3306; CharSet=utf8;";
            con = new MySqlConnection(serverInfo);
            con.Open();
            isConnected = true;
        }
        catch (Exception e)
        {
            return; ;
        }
    }

    //유저가 입력한 id와 pwd를 받아 데이터베이스에서 조회하여 있으면 해당 아이디의 닉네임을 받아 서버에 들어가게 해주는 함수
    public void Login()
    {
        if(!isConnected)
            ConnectDataBase();
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

                    if (pass == readPass)
                    {
                        PhotonNetwork.LocalPlayer.NickName = readnick;
                        StartCoroutine(CloseLoginCanvasRoutine());
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
            return;
        }
    }
    
    IEnumerator CloseLoginCanvasRoutine()
    {
        anim.SetTrigger("IsClose");
        yield return new WaitForSeconds(1.0f);
        PhotonNetwork.ConnectUsingSettings();
        yield break;
    }
}