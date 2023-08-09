using Photon.Pun;
using UnityEngine;

public class FootTrigger : MonoBehaviourPun
{
    [SerializeField] LayerMask platformLayer;

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
        GameManager.Round.NextRound();
    }
}
