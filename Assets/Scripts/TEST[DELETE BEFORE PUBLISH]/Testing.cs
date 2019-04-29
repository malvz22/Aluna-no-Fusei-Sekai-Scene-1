using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{/*
    DialogueSystem dialogue;
    // Start is called before the first frame update
    void Start()
    {
        dialogue = DialogueSystem.instance;
    }

    public string[] s = new string[] {
        "Hi, how are you?:Neela",
        "Are you listening?",
    };

    int index = 0;

    void Update()
    {
       if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Index1 : " + index);
            Debug.Log("speak : "+ dialogue.isSpeaking);
            Debug.Log("wait user input : "+ dialogue.isWaitingForUserInput);
            if (!dialogue.isSpeaking || dialogue.isWaitingForUserInput) {
                Debug.Log("dialog masuk");
                if (index >= s.Length) { // index tidak masuk
                    Debug.Log("if dialog masuk");
                    return;
                }
                Say(s[index]);
                index++;
                Debug.Log("Index2 : "+index);
                //Debug.Log( Say(s[index]));
            }
        }    
    }

    /*void Say(string s) {
        string[] parts = s.Split(':');
        string speech = parts[0];
        string speaker = (parts.Length >= 2) ? parts[1] : "";


        dialogue.Say(speech, true, speaker);
    }*/
}
