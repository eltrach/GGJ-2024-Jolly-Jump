using UnityEngine;
using VTemplate.Controller;


public class VehiclesTrigger : MonoBehaviour
{
    protected Collider _trigger;
    public Collider trigger
    {
        get
        {
            _trigger = gameObject.GetComponent<Collider>();

            if (!_trigger) _trigger = gameObject.AddComponent<BoxCollider>();
            return _trigger;
        }
    }
    public BaseVehicle baseVehicle;
    public string playerTag = "Player";

    // ui 
    public string messageOnTriggerEnter = "Press E To Enter";
    public ActionUIPanel actionUIPanel;


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        Debug.Log(other.gameObject.layer);
        if (other.gameObject.CompareTag(playerTag))
        {
            if (other.TryGetComponent<ThirdPersonController>(out var tpsPlayer))
            {
                if (baseVehicle == null)
                {
                    Debug.Log(" Vehicle is null ".Color("red"));
                    return;
                }
                actionUIPanel.Enable(messageOnTriggerEnter);
                if (PlayerRoot.Input.ActionButton)
                {
                    tpsPlayer.EnterVehicle(baseVehicle);
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(playerTag))
        {
            if (other.TryGetComponent<ThirdPersonController>(out var tpsPlayer))
            {
                actionUIPanel.Disable();
            }
        }
    }
    void OnDrawGizmos()
    {

        Color color = Color.green;
        color.a = 0.6f;
        Gizmos.color = color;
        if (!Application.isPlaying && trigger && !trigger.enabled) trigger.enabled = true;
        if (trigger && trigger.enabled)
        {
            if (trigger as BoxCollider)
            {
                BoxCollider box = trigger as BoxCollider;
                Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
                Gizmos.DrawCube(box.center, Vector3.Scale(Vector3.one, box.size));
            }
        }
    }
}
