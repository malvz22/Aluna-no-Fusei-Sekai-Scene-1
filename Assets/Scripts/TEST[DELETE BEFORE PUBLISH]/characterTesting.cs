using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterTesting : MonoBehaviour
{
    public Character Nella;
    // Start is called before the first frame update
    void Start()
    {
        Nella = CharacterManagement.instance.GetCharacter("Nella", enableCreatedCharacterOnStart: false);
        Nella.GetSprite(2);
    }

    


    public string[] speech;
    int i = 0;

    public Vector2 moveTarget;
    public float moveSpeed;
    public bool smooth;

    public int bodyIndex, expressionIndex = 0;
    public float speed = 5f;
    public bool smoothtransitions = false;





    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Debug.Log(speech.Length);
            if (i < speech.Length)
            {

                Debug.Log("speech length "+speech.Length);
                Debug.Log("i "+i);
                Nella.Say(speech[i]);

            }
            else
            {
                DialogueSystem.instance.Close();
                Debug.Log("i " + i);
            }
                
            i++;
        }

        if (Input.GetKey(KeyCode.M))
        {
            Nella.MoveTo(moveTarget, moveSpeed, smooth);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Nella.StopMoving(true);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            if (Input.GetKey(KeyCode.T))
            {
                Nella.TransitionBody(Nella.GetSprite(bodyIndex), speed, smoothtransitions);
            }
            else
            {
                Nella.SetBody(bodyIndex);
            }
            
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Nella.SetExpression(expressionIndex);
        }


    }
}
