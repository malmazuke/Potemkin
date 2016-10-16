using UnityEngine;
using System.Collections;

public enum UFOState {
    Stopped,
    Moving,
    Observing
}

public class UFOController : MonoBehaviour {

    #region Public Properties
    
    public UFOState currentState = UFOState.Stopped;
    public UFOState nextState = UFOState.Moving;
    public float verticalFloatingMagnitide = 0.03f;
    public float verticalFloatingSpeed = 5.0f;
    public float movementSpeed = 1.0f;
    public float stateLength = 1.0f;
    
    #endregion
    
    #region Private Properties
    
    bool isExecutingState = false;
    bool isCurrentlyDisgusted = false;
    
    #endregion

	#region Unity Methods
	
	void Update () {
        Oscillate ();
        
	    if (!isExecutingState) {
            StartCoroutine (PerformState (nextState, stateLength));
        }
	}
    
    void OnTriggerEnter2D (Collider2D other) {
        ObjectController oc = other.GetComponent<ObjectController> ();
        
        if (oc == null) {
            return;
        }
        
        CheckIfDisgustedWithObject (oc);
    }
    
    void OnTriggerStay2D (Collider2D other) {
        ObjectController oc = other.GetComponent<ObjectController> ();
        
        if (oc == null) {
            return;
        }
        
        CheckIfDisgustedWithObject (oc);
    }
    
    #endregion
    
    #region Private Methods
    
    void Oscillate () {
        transform.localPosition = new Vector2 (transform.localPosition.x, Mathf.Sin (Time.time * verticalFloatingSpeed) * verticalFloatingMagnitide);
    }
    
    IEnumerator PerformState (UFOState state, float lengthInSeconds) {
        isExecutingState = true;
        currentState = state;
        float stateRemaining = 0.0f;
        
        while (stateRemaining < lengthInSeconds) {
            switch (currentState) {
                case UFOState.Stopped:
                    break;
                case UFOState.Observing:
                    Observe ();
                    break;
                case UFOState.Moving:
                    Move ();
                    break;
            }
            
            stateRemaining += Time.deltaTime;
            yield return null;
        }
        
        currentState = nextState;
        isExecutingState = false;
    }
    
    void Observe () {
        // TODO: Observe what's around
    }
    
    void Move () {
        transform.position = Vector2.MoveTowards (transform.position, (Vector2)transform.position + new Vector2 (1.0f, 0.0f), movementSpeed * Time.deltaTime);
    }
    
    void CheckIfDisgustedWithObject (ObjectController objectController) {
        if (objectController.isABadThing && !isCurrentlyDisgusted) {
            StartCoroutine (GetDisgusted (objectController, stateLength));
        }
    }
    
    IEnumerator GetDisgusted (ObjectController objectController, float lengthInSeconds) {
        isCurrentlyDisgusted = true;
        
        Debug.Log ("Is currently disgusted with " + objectController.name);
        yield return new WaitForSeconds ((int)lengthInSeconds);
        
        isCurrentlyDisgusted = false;
    }
    
    #endregion
}
