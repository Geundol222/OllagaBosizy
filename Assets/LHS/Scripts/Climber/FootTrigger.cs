using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootTrigger : MonoBehaviour
{
    [SerializeField] LayerMask platformLayer;

    DebuffManager debuffManager;

    private void Awake()
    {
        debuffManager = GameObject.Find("DebuffManager").GetComponent<DebuffManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (platformLayer.IsContain(collision.gameObject.layer))
        {
            //Platform platform = collision.gameObject.GetComponent<Platform>();
            //
            //debuffManager.ClimberStepOnPlatform(platform);
            Debug.Log(collision.gameObject.name);
        }

        if (collision.gameObject.name == "EndPoint")
        {
            if (GameManager.Round.GetRound() == Round.ROUND1)
            {
                GameManager.Round.SetRound(Round.ROUND2);
            }
            else
            {
                GameManager.Round.SetRound(Round.END);
            }
        }
    }
}
