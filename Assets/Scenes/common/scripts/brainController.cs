using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Assertions;
using UnityEngine.Serialization;
using Oculus.Interaction;



public class brainController : MonoBehaviour
{
    private IInteractableView interactableView;

    public float rotationSpeed = 1.0f;
    public int sceneIndex = 0;
    public float baseAlpha = 0.2f;
    public float overAlpha = 0.8f;


    public Material mat;
    public Material selectMat;

    public Material noSelectMat;

    public bool windowsOnly = false;
    private bool selectable = true;

    private Material _mat;



    // Start is called before the first frame update
    void Start()
    {
        #if !UNITY_STANDALONE_WIN
        Debug.Log("not windows");
        if (windowsOnly)
            selectable = false;
        #endif

        foreach ( Transform child in transform)
        {
            if (!selectable)
                child.gameObject.GetComponent<Renderer>().material = noSelectMat;
            else 
            {
                child.gameObject.GetComponent<Renderer>().material = mat;
                // Material mat = child.gameObject.GetComponent<Renderer>().material;
                // Color color = _mat.GetColor("_BaseColor");
                // color.a = baseAlpha;
                // mat.SetColor("_BaseColor", color);
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, Time.deltaTime * rotationSpeed, 0, Space.Self);
       
        if (selectable)
        {   
        
            if (interactableView == null)
                interactableView = (IInteractableView)gameObject.GetComponent(typeof(IInteractableView));
                
            foreach (Transform child in transform)
            {
                // Material mat = child.gameObject.GetComponent<Renderer>().material;
                // Color color = mat.GetColor("_BaseColor");
                switch (interactableView.State)
                {
                    case InteractableState.Normal:
                        child.gameObject.GetComponent<Renderer>().material = mat;
                        break;
                    case InteractableState.Hover:                
                        child.gameObject.GetComponent<Renderer>().material = selectMat;
                        break;
                    case InteractableState.Select:
                        SceneManager.LoadScene(sceneIndex);
                        break;
                }
            }
        }
    }

  
}
