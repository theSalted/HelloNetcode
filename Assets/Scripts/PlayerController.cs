using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    public GameObject isSeverIndicator;
    public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            isSeverIndicator.SetActive(true);
        } else {
            isSeverIndicator.SetActive(false);
        }
        if (IsOwner)
        {
            Move();
        }
    }

    public void Move()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            var randomPosition = GetRandomPositionOnPlane();
            transform.position = randomPosition;
            Position.Value = randomPosition;
        }
        else
        {
            SubmitPositionRequestServerRpc();
        }
    }

    [Rpc(SendTo.Server)]
    void SubmitPositionRequestServerRpc(RpcParams rpcParams = default)
    {
        Position.Value = GetRandomPositionOnPlane();
    }

    static Vector3 GetRandomPositionOnPlane()
    {
        return new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));
    }

    void Update()
    {
        transform.position = Position.Value;
    }
}

