using Cinemachine;
using Fusion;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }
    public void SetFollowObject(Transform player)
    {
        virtualCamera.Follow = player;
    }

    private Vector3 pos;


    [Rpc]
    public Vector3 GetMousePositionRpc(Vector3 mousePosition)
    {
        Ray ray = cam.ScreenPointToRay(mousePosition);
        if(Physics.Raycast(ray, out RaycastHit target))
        {
            pos = target.point;
        }

        return pos;
    }
}
