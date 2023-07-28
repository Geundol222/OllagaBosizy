using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public class ChainFunction : UnityEvent<int> { }

public class Platform : MonoBehaviourPun
{
    public static ChainFunction OnStart = new ChainFunction();

    private int objID;

    private void Awake()
    {
        objID = gameObject.GetComponent<PhotonView>().ViewID;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (PhotonNetwork.IsConnected)
            OnStart?.Invoke(objID);
    }
}
