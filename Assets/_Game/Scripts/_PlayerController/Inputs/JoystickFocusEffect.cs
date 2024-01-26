using UnityEngine;
using UnityEngine.UI;

public class JoystickFocusEffect : MonoBehaviour
{
    public Image focus_1;
    public Image focus_2;
    public Image focus_3;
    public Image focus_4;

    public ThirdPersonInputs inputs;

    private void Update()
    {
        if (inputs) UpdateFocus(inputs.Move);
    }

    public void UpdateFocus(Vector2 direction)
    {
        if (direction.x > 0 && direction.y > 0)
        {
            EnableSpecificFocusObj(focus_1);
        }
        else if (direction.x > 0 && direction.y < 0)
        {
            EnableSpecificFocusObj(focus_2);
        }
        else if (direction.x < 0 && direction.y > 0)
        {
            EnableSpecificFocusObj(focus_3);
        }
        else if (direction.x < 0 && direction.y < 0)
        {
            EnableSpecificFocusObj(focus_4);
        }
        else
        {
            DisableAllFocusObjects();
        }
    }

    public void EnableSpecificFocusObj(Image focusImageObj)
    {
        DisableAllFocusObjects();

        focusImageObj.enabled = true;
    }

    private void DisableAllFocusObjects()
    {
        focus_1.enabled = false;
        focus_2.enabled = false;
        focus_3.enabled = false;
        focus_4.enabled = false;
    }
}
