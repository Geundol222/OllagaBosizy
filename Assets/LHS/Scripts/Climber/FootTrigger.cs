using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootTrigger : MonoBehaviour
{
    [SerializeField] DebuffManager debuffManager;
    [SerializeField] LayerMask platformLayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (platformLayer.IsContain(collision.gameObject.layer))
        {
            //Platform platform = collision.gameObject.GetComponent<Platform>();

            //debuffManager.ClimberStepOnPlatform(platform);

            Debug.Log(collision.gameObject.name);
        }
    }
}
