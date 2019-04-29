using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NovelController : MonoBehaviour
{
    public static NovelController instance;
    List<string> data = new List<string>();
    int progress = 0;
    //bool _next = false;
    /*void Awake()
    {
        instance = this;    
    }*/

    // Start is called before the first frame update
    void Start() { 
        LoadChapterFile("chapter0_start.txt");
        Debug.Log("Debug start : " + data[progress]);
        Handleline(data[progress]);
        progress++;
        Debug.Log("start :"+progress);
    }

    // Update is called once per frame
    void Update()
    {
        //testing
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Handleline(data[progress]);
            progress++;
            Debug.Log(progress);
        }
    }

    public void LoadChapterFile(string fileName)
    {
        data = FileManager.LoadFile(FileManager.savPath + "Resources/Story/" + fileName);
        
        progress = 0;
        cachedLastSpeaker = "";
        Debug.Log("load masuk = "+data[progress]);
        /*if (handlingChapterFile != null)
        {
            StopCoroutine(handlingChapterFile);
            Debug.Log("handling null");
        }
            
        handlingChapterFile = StartCoroutine(HandlingChapterFile());*/
    }

    
    /*public void Next()
    {
        _next = true;
    }*/

    /*Coroutine handlingChapterFile = null;
    IEnumerator HandlingChapterFile()
    {
        Debug.Log("coroutine jalan");
        Debug.Log("data count : "+data.Count);
        //progress = 0;
        
        while (progress < data.Count)
        {
            //Debug.Log(_next);
            //Debug.Log("Debug blum if : " + progress);
            if (_next)
            {
                Debug.Log("Debug if1 : " + data[progress]);
                Handleline(data[progress]);
                progress++;
                Debug.Log("Debug if2 : "+progress);
                while (isHandlingLine)
                {
                    yield return new WaitForEndOfFrame();
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }*/

    void Handleline(string line)
    {
        Debug.Log("handle masuk1 : "+line); //ada tulisan
        string[] dialogueAndActions = line.Split('"');
        
        //CLM.LINE line = CLM.Interpret(rawLine); // SUMBER ERROR  
        //Debug.Log("line1 " + line);// jadi CLM+LINE - semua tulisan
        //Debug.Log("handle masuk2");
        //StopHandlingLine();
        //Debug.Log("line2 " + line);
        //Debug.Log("handle line "+HandlingLine(line));
        //handlingLine = StartCoroutine(HandlingLine(line));
        if (dialogueAndActions.Length == 3)
        {
            Debug.Log("handle masuk2 : " + dialogueAndActions[0] + " , " + dialogueAndActions[1] + " , " + dialogueAndActions[2]);
            HandleDialogue(dialogueAndActions[0], dialogueAndActions[1]);
            HandleEventsFromLine(dialogueAndActions[2]);
        }
        else
        {
            HandleEventsFromLine(dialogueAndActions[0]);
        }
    }

    /*void StopHandlingLine()
    {
        Debug.Log("ishandlingline3 " + isHandlingLine); // hasilnya false
        if (isHandlingLine)
        {
            Debug.Log("ishandlingline");
            StopCoroutine(handlingLine);
            
        }
            Debug.Log("isHandlingLine fail");
        handlingLine = null;
    }

    public bool isHandlingLine
    {
        get
        {
            Debug.Log("ishandlingline2 " + handlingLine); // hasilnya null
            return handlingLine != null;
        }
    }
    Coroutine handlingLine = null;
    IEnumerator HandlingLine(CLM.LINE line)
    {
        _next = false;
        int lineProgress = 0;
        Debug.Log("line clm : " + line);//CLM+LINE
        Debug.Log("line progress : "+lineProgress);//hasil 0
        Debug.Log("line.segment : "+line.segments.Count);//hasilnya 0
        while (lineProgress < line.segments.Count)//disini yang error out of bound
        {
            _next = false;
            CLM.LINE.SEGMENT segment = line.segments[lineProgress];

            if(lineProgress < 0)
            {
                if (segment.trigger == CLM.LINE.SEGMENT.TRIGGER.autoDelay)
                {
                    for(float timer = segment.autoDelay; timer >= 0; timer -= Time.deltaTime)
                    {
                        yield return new WaitForEndOfFrame();
                        if (_next)
                            break;
                    }
                }
                else
                {
                    while (!_next)
                        yield return new WaitForEndOfFrame();
                }
            }
            _next = false;

            segment.Run();

            while (segment.isRunning)
            {
                yield return new WaitForEndOfFrame();
                if (_next)
                {
                    if (!segment.architect.skip)
                    {
                        segment.architect.skip = true;
                    }
                    else
                    {
                        segment.ForceFinish();
                    }
                }
            }

            yield return new WaitForEndOfFrame();
        } 
        //untuk sekarang keluar disini 
    }*/

    //[HideInInspector]
    public string cachedLastSpeaker = "";

    void HandleDialogue(string dialogueDetails, string dialogue)
    {
        Debug.Log("dialog detail : " + dialogueDetails);
        Debug.Log("dialog  : " + dialogue);
        string speaker = cachedLastSpeaker;
        bool additive = dialogueDetails.Contains("+");

        if (additive)
            dialogueDetails = dialogueDetails.Remove(dialogueDetails.Length - 1);

        if (dialogueDetails.Length > 0)
        {
            Debug.Log("dialog length : "+ dialogueDetails);
            if (dialogueDetails[dialogueDetails.Length - 1] == ' ')//sekarang errornya disini 
            {
                Debug.Log("dialog detail2 : " + dialogueDetails[dialogueDetails.Length -1]);//hasilnya null
                dialogueDetails = dialogueDetails.Remove(dialogueDetails.Length - 1);
            }
                

            speaker = dialogueDetails;
            cachedLastSpeaker = speaker;
        }

        if (speaker != "narrator")
        {
            Debug.Log("dialog jalan : " + dialogue + " , " + additive);
            Character character = CharacterManagement.instance.GetCharacter(speaker);
            character.Say(dialogue, additive);
            
            
        }
        else
        {
            Debug.Log("narrator jalan : " + dialogue+" , "+ speaker+" , "+ additive);

            DialogueSystem.instance.Say(dialogue, speaker, additive);
        }
    }

    void HandleEventsFromLine(string events)
    {
        Debug.Log("events = " + events);
        string[] actions = events.Split(' ');

        foreach (string action in actions)
        {
            Debug.Log("action2 = " + action);
            HandleActions(action);
        }
    }

///ACTIONS

    void HandleActions(string action)
    {
        Debug.Log("action = " + action);
        print("Handle Action[" + action + "]");
        string[] data = action.Split('(', ')');

        if (data[0] == "setBackground")
        {
            Command_SetLayerImage(data[1], BCFC.instance.background);
            return;
        }
        if (data[0] == "setCinematic")
        {
            Command_SetLayerImage(data[1], BCFC.instance.cinematic);
            return;
        }
        if (data[0] == "setForeground")
        {
            Command_SetLayerImage(data[1], BCFC.instance.foreground);
            return;
        }
        if (data[0] == "move")
        {
            Command_MoveCharacter(data[1]);
            return;
        }
        if (data[0] == "setPosition")
        {
            Command_SetPosition(data[1]);
            return;
        }
        if (data[0] == "changeExpression")
        {
            Command_ChangeExpression(data[1]);
            return;
        }
    }



    void Command_SetLayerImage(string data, BCFC.LAYER layer)
     {
            string texName = data.Contains(",") ? data.Split(',')[0] : data;
            Texture2D tex = texName == "null" ? null : Resources.Load("Images/UI/Backdrops/" + texName) as Texture2D;
            float spd = 2f;
            bool smooth = false;

            if (data.Contains(","))
            {
                string[] parameters = data.Split(',');
                foreach (string p in parameters)
                {
                    float fVal = 0;
                    bool bVal = false;
                    if (float.TryParse(p, out fVal))
                    {
                        spd = fVal; continue;
                    }
                    if (bool.TryParse(p, out bVal))
                    {
                        smooth = bVal; continue;
                    }
                }
            }
            layer.TransitionToTexture(tex, spd, smooth);
        }

    void Command_MoveCharacter(string data)
    {
        string[] parameters = data.Split(',');
        string character = parameters[0];
        float locationX = float.Parse(parameters[1]);
        float locationY = float.Parse(parameters[2]);
        float speed = parameters.Length >= 4 ? float.Parse(parameters[3]) : 1f;
        bool smooth = parameters.Length == 5 ? bool.Parse(parameters[4]) : true;

        Character c = CharacterManagement.instance.GetCharacter(character);
        c.MoveTo(new Vector2(locationX, locationY), speed, smooth);
    }

    void Command_SetPosition(string data)
    {
        string[] parameters = data.Split(',');
        string character = parameters[0];
        float locationX = float.Parse(parameters[1]);
        float locationY = float.Parse(parameters[2]);

        Character c = CharacterManagement.instance.GetCharacter(character);
        c.SetPosition(new Vector2(locationX, locationY));
    }

    void Command_ChangeExpression(string data)
    {
        string[] parameters = data.Split(',');
        string character = parameters[0];
        string region = parameters[1];
        string expression = parameters[2];
        float speed = parameters.Length == 4 ? float.Parse(parameters[3]) : 1f;

        Character c = CharacterManagement.instance.GetCharacter(character);
        Sprite sprite = c.GetSprite(expression);
        if (region.ToLower() == "body")
            c.TransitionBody(sprite, speed, false);
        if (region.ToLower() == "face")
            c.TransitionExpression(sprite, speed, false);

    }
}

