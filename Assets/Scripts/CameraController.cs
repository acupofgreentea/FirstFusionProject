using Cinemachine;
using Fusion;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    public void SetFollowObject(Transform player)
    {
        virtualCamera.Follow = player;
    }

}
