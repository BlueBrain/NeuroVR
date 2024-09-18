using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Assertions;
using UnityEngine.Serialization;
using UnityEngine.UI;

using Oculus.Interaction;



public class ImageViewerController : MonoBehaviour
{
    private IInteractableView interactableView;
    public ImageViewer imageViewer;
    public bool updateColor = true;
    public bool forward = true;

    private RawImage image;
    private InteractableState prevState = InteractableState.Normal;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<RawImage>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (interactableView == null)
            interactableView = (IInteractableView)gameObject.GetComponent(typeof(IInteractableView));
            
        switch (interactableView.State)
        {
            case InteractableState.Normal:
                if (updateColor)
                    image.color = Color.grey;
                break;
            case InteractableState.Hover:     
                if (updateColor)
                    image.color = Color.white;
                break;
            case InteractableState.Select:
                if (updateColor)
                    image.color = Color.white;
                if (prevState != InteractableState.Select)
                    if (forward)
                        imageViewer.nextImage();
                    else    
                        imageViewer.previousImage();
                break;
        }
        prevState = interactableView.State;
    }

  
}
