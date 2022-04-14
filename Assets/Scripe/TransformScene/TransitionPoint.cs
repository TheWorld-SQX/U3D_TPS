using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionPoint : MonoBehaviour
{
    public enum TransitionType { SameScene,DifferentScene}
    [Header("Transition")]
    public string sceneName;
    public TransitionType transitionType;
    public TransitionDestination.DestinationTag destinationTag;
    //private bool canTransform;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //canTransform = true;
            SceneController.Instance.TransitionToDestination(this);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") == false)
        {
            //canTransform = false;
        }
    }

}
