//Script written by Matthew Rukas - Volumetric Games || volumetricgames@gmail.com || www.volumetric-games.com

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CanvasInteract : MonoBehaviour
{
    [SerializeField] private Canvas keyPadCanvas;

    public void CanvasOn()
    {
        keyPadCanvas.enabled = true;
    }

    public void CanvasOff()
    {
        keyPadCanvas.enabled = false;
    }
}
