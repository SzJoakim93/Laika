using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class RequiredState
{
    public string UnsuccessfullMessage { 
        get { return unsuccessfullMessage; }
    }

    public string Key { 
        get { return key; }
    }

    [SerializeField]
    string unsuccessfullMessage;
    [SerializeField]
    string key;
}

[System.Serializable]
public class Interaction
{
    public UnityEvent Action { 
        get { return action; }
    }

    public Item RequiredItemAction {
        get { return requiredItemAction; }
    }

    public bool MatchRequirements(ref string message)
    {
        foreach (RequiredState requiredState in requiredStates)
            if (PlayerPrefs.GetInt(requiredState.Key, 0) == 0)
            {
                message = requiredState.UnsuccessfullMessage;
                return false;
            }

        return true;
    }

    [SerializeField]
    RequiredState [] requiredStates;

    [SerializeField]
    Item requiredItemAction;

    [SerializeField]
    UnityEvent action;
}

public class ActionManager : MonoBehaviour
{
    

    [SerializeField]
    Interaction [] interactions;

    [SerializeField]
    PopupText dialog;

    [SerializeField]
    Inventory inventory;

    

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Invoke()
    {
        foreach (Interaction interaction in interactions)
            if (interaction.RequiredItemAction == inventory.SelectedItem)
            {
                string dialogText = null;
                if (interaction.MatchRequirements(ref dialogText))
                    interaction.Action.Invoke();
                else
                {
                    dialog.SetText(dialogText);
                    dialog.ActivateForReadableTime();
                }

                return;
            }

        dialog.SetText("This does not make sense.");
        dialog.ActivateForReadableTime();
    }
}
