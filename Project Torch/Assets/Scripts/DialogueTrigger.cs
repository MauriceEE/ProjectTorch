using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {
#region Private Fields
    protected GameObject player;
    #endregion
#region Public Fields
    public string dialogueLinesName;
    public float range;
    public bool destroyAfterActivation;
    #endregion
#region Unity Defaults
    void Awake () {
        player = GameObject.Find("Player");
	}
	void Update () {
        if ((this.transform.position - player.transform.position).sqrMagnitude < range * range)
        {
            GameObject.Find("FlagManagerGO").GetComponent<FlagManager>().ActivateDialogueLines(dialogueLinesName);
            if (destroyAfterActivation)
                Destroy(this.gameObject);
            else
                this.gameObject.SetActive(false);
        }
	}
#endregion
}
