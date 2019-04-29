using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    public static DialogueSystem instance;
    public ELEMENTS elements;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update


    public void Say(string speech, string speaker = "", bool additive = false) {
        StopSpeaking();
        Debug.Log("speech = " + speech);
        Debug.Log("speaker = " + speaker);
        Debug.Log("additive = " + additive);
        
        Debug.Log("speech2 = " + speech);
        speaking = StartCoroutine(Speaking(speech, additive, speaker));
        speechText.text = targetSpeech;
        if (additive)
        {
            Debug.Log("masuk");
            speechText.text = targetSpeech;// masi null
        }
    }

    

    public void StopSpeaking() {
        if (isSpeaking)
        {
            StopCoroutine(speaking);
        }
        /*if(textArchitect != null && textArchitect.isConstructing)
        {
            textArchitect.Stop();
        }*/
        speaking = null;
    }

    
    public bool isSpeaking {
        get {
            return speaking != null;
        }
    }

    [HideInInspector] public bool isWaitingForUserInput = false;

    string targetSpeech = "";
    Coroutine speaking = null;
    //TextArchitect textArchitect = null;

    IEnumerator Speaking(string speech, bool additive, string speaker = "")
    {
        Debug.Log("speech3 = " + speech);
        speechPanel.SetActive(true);
        string additiveSpeech = additive ? speechText.text : "";
        Debug.Log("additivespeech = " + additiveSpeech);
        targetSpeech = additiveSpeech + speech;
        Debug.Log("targetspeech = " + targetSpeech);
        //textArchitect = new TextArchitect(speechText, speech, additiveSpeech);
        
        speakerNameText.text = DetermineSpeaker(speaker);//temp yang manggil speaker
        isWaitingForUserInput = false;

        /*while (textArchitect.isConstructing) {
            if (Input.GetKey(KeyCode.Space))
                textArchitect.skip = true;
            
            yield return new WaitForEndOfFrame();
        }*/
        isWaitingForUserInput = true;
        while (isWaitingForUserInput)
            yield return new WaitForEndOfFrame();

        StopSpeaking();
    }

    string DetermineSpeaker(string s) {
        string retVal = speakerNameText.text;
        if (s != speakerNameText.text && s != "")
            retVal = (s.ToLower().Contains("narrator")) ? "" : s;

        return retVal;
    }

    public void Close() {
        Debug.Log("close  ");
        StopSpeaking();
        speechPanel.SetActive(false);
    }

    /*internal void Say(string dialogue, bool v, bool additive)
    {
        throw new NotImplementedException();
    }*/

    [System.Serializable]
    public class ELEMENTS
    {
        /// <Summary>
        /// The Main panel containing all dialogues
        public GameObject speechPanel;
        public Text speakerNameText;
        public Text speechText;
    }
    public GameObject speechPanel {
        get {
            return elements.speechPanel;
        }
    }
    public Text speakerNameText {
        get {
            return elements.speakerNameText;
        }
    }
    public Text speechText {
        get {
            return elements.speechText;
        }
    }
    /*public TextArchitect currentArchitect
    {
        get
        {
            return textArchitect;
        }
    }*/
}
