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
    public float phaseLength = 1.0f;
    
    #endregion
    
    #region Private Properties
    
    bool isExecutingState = false;
    
    #endregion

	#region Unity Methods
	
	void Update () {
        Oscillate ();
        
	    if (!isExecutingState) {
            StartCoroutine (PerformState (nextState, phaseLength));
        }
	}
    
    #endregion
    
    #region Private Methods
    
    void Oscillate () {
        transform.localPosition = new Vector2 (transform.localPosition.x, Mathf.Sin (Time.time * verticalFloatingSpeed) * verticalFloatingMagnitide);
    }
    
    IEnumerator PerformState (UFOState phase, float lengthInSeconds) {
        isExecutingState = true;
        currentState = phase;
        float phaseRemaining = 0.0f;
        
        while (phaseRemaining < lengthInSeconds) {
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
            
            phaseRemaining += Time.deltaTime;
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
    
    #endregion
}
