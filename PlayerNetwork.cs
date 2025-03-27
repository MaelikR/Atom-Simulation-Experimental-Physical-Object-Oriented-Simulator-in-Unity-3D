using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PlayerNetwork : NetworkBehaviour
{
    public float speed = 5f; // Vitesse du joueur
    private CharacterController controller;
    
    public override void Spawned()
    {
        if (Object.HasInputAuthority) 
        {
            Camera.main.transform.SetParent(transform);
            Camera.main.transform.localPosition = new Vector3(0, 1.5f, -3);
            Camera.main.transform.localRotation = Quaternion.identity;
        }
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            Vector3 move = transform.forward * data.direction.y + transform.right * data.direction.x;
            controller.Move(move * speed * Runner.DeltaTime);
        }
    }

    public override void Render()
    {
        // Affichage fluide côté client
    }
}

// Structure de stockage des inputs
public struct NetworkInputData : INetworkInput
{
    public Vector2 direction;
}
