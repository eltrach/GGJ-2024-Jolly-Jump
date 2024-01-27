using UnityEngine;

public class CollisionTest : MonoBehaviour
{
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log(hit.controller);
        //hit.controller.detectCollisions = false;
    }



}
