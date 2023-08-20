using Photon.Pun;
using UnityEngine;

public class FootTrigger : MonoBehaviourPun
{
    [SerializeField] LayerMask platformLayer;

    GameSceneManager gameSceneManager;

    private void Awake()
    {
        gameSceneManager = GameObject.Find("GameSceneManager").GetComponent<GameSceneManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (photonView.IsMine)
        {
            if (collision.gameObject.name == "EndPoint")
            {
                gameSceneManager.PlayerStepEndPoint();
            }
        }
    }

    
}
