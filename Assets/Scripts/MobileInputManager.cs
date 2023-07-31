using UnityEngine;
using UnityEngine.EventSystems;

public class MobileInputManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public PlayerController playerController;
    public bool isLeftButton;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        if (isLeftButton)
            playerController.OnLeftButtonDown();
        else
            playerController.OnRightButtonDown();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isLeftButton)
            playerController.OnLeftButtonUp();
        else
            playerController.OnRightButtonUp();
    }
}
