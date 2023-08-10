using Photon.Pun;
using UnityEngine;

public class FootTrigger : MonoBehaviourPun
{
    [SerializeField] LayerMask platformLayer;

    RoundManager round;

    private void Awake()
    {
        round = GameObject.Find("RoundManager").GetComponent<RoundManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (photonView.IsMine)
        {
            if (collision.gameObject.name == "EndPoint")
            {
                photonView.RPC("StepEndPoint", RpcTarget.AllBuffered);
            }
        }
    }

    [PunRPC]
    public void StepEndPoint()
    {
        round.NextRound();
    }
}
