using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scroller : MonoBehaviour
{
    [SerializeField] RawImage _img;
    [SerializeField] float xposition, yposition;
    public bool shouldMove = true;

    

    void Update()
    {
        // If movement is allowed and the stop event is not being handled, move the image
        if (shouldMove is true)
        {
            _img.uvRect = new Rect(_img.uvRect.position + new Vector2(xposition, yposition) * Time.deltaTime, _img.uvRect.size);
        }
        
    }

    // Method to stop the movement
    public void StopMoving()
    {
        shouldMove = false;
    }
    public void StartMoving()
    {
        shouldMove = true;
        
    }

    
}




