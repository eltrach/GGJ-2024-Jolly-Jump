using UnityEngine;
using Cinemachine;

public class CameraCullingMaskController : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public LayerMask cullingMaskOn;
    public LayerMask cullingMaskOff;

    private bool isActive = false;

    private void Update()
    {
        if (virtualCamera != null)
        {
            CinemachineBrain cinemachineBrain = CinemachineCore.Instance.FindPotentialTargetBrain(virtualCamera);
            if (cinemachineBrain != null && cinemachineBrain.IsLive(virtualCamera))
            {
                if (!isActive)
                {
                    Camera.main.cullingMask = cullingMaskOn;
                    isActive = true;
                }
            }
            else
            {
                if (isActive)
                {
                    Camera.main.cullingMask = cullingMaskOff;
                    isActive = false;
                }
            }
        }
    }
}
