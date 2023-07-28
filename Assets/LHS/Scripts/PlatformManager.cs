using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviourPun
{
    [SerializeField] LayerMask platformMask;

    private List<GameObject> groundList;

    private void Awake()
    {
        groundList = new List<GameObject>();
    }

    private void Start()
    {
        StartCoroutine(ConnectDelay());
    }

    private void Init()
    {
        Platform.OnStart.AddListener(StepPlatform);
    }

    public void StepPlatform(int id)
    {
        photonView.RPC("PlayerStepPlatform", RpcTarget.AllViaServer, id);
    }

    IEnumerator ConnectDelay()
    {
        yield return new WaitUntil(() => { return PhotonNetwork.IsConnected; });
        Init();
    }
    
    [PunRPC]
    public void PlayerStepPlatform(int id)
    {
        PhotonView view = PhotonView.Find(id);
        Vector2 range = new Vector2(15, 10);
    
        if (groundList.Count > 0)
            groundList.Clear();
    
        Collider2D[] colliders = Physics2D.OverlapBoxAll(view.gameObject.transform.position, range, 0, platformMask);
    
        if (platformMask.IsContain(view.gameObject.layer))
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                groundList.Add(colliders[i].gameObject);
            }
        }
    }
}
