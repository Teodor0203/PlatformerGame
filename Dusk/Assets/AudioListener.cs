using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioListener : MonoBehaviour
{
    public static AudioListener instance { get; private set; }
    public Transform player;
    [SerializeField] Transform thisTransform;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {       
        player = Player.instance.GetComponent<Transform>();
    }

    private void Update()
    {
        if(player != null && this.transform != null)
            thisTransform.position = player.position;
    }
}
