using Photon.Pun;
using UnityEngine;

public class FootTrigger : MonoBehaviourPun
{
    [SerializeField] LayerMask platformLayer;

    DebuffManager debuffManager;

    private void Awake()
    {
        debuffManager = GameObject.Find("DebuffManager").GetComponent<DebuffManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (photonView.IsMine)
        {
            if (platformLayer.IsContain(collision.gameObject.layer))
            {
                //Platform platform = collision.gameObject.GetComponent<Platform>();
                //
                //debuffManager.ClimberStepOnPlatform(platform);
                Debug.Log(collision.gameObject.name);
            }
        }

        if (collision.gameObject.name == "EndPoint")
        {
            GameManager.Round.NextRound();
        }
    }
}
