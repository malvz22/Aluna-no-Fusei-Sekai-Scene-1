using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Character
{
    public string characterName;
    [HideInInspector] public RectTransform root;

    public bool isMultiLayerCharacter {
        get {
            return renderers.renderer == null;
        }
    }

    public bool enabled {
        get {
            return root.gameObject.activeInHierarchy;
        }
        set
        {
            root.gameObject.SetActive(value);
        }
    }

    public Vector2 anchorPadding {
        get{
            return root.anchorMax - root.anchorMin;
        }
    }

    DialogueSystem dialogue;

    public void Say(string speech, bool add = false) {
        if (!enabled)
            enabled = true;

        Debug.Log("speech = "+speech);
        Debug.Log("char = "+characterName);
        dialogue.Say(speech, characterName, add);

    }

    Vector2 targetPosition;
    Coroutine moving;
    bool isMoving {
        get {
            return moving != null;
        }
    }
    public void MoveTo (Vector2 Target, float speed, bool smooth = true)
    {
        StopMoving();
        moving = CharacterManagement.instance.StartCoroutine(Moving(Target, speed, smooth));
    }

    public void StopMoving(bool arriveAtTargetPositionImmediately = false) {
        if (isMoving)
        {
            CharacterManagement.instance.StopCoroutine(moving);
            if (arriveAtTargetPositionImmediately)
                SetPosition(targetPosition);
        }
        moving = null;
    }

    public void SetPosition(Vector3 target)
    {
        
        Vector2 padding = anchorPadding;
        float maxX = 1f - padding.x;
        float maxY = 1f - padding.y;

        Vector2 minAnchorTarget = new Vector2(maxX * targetPosition.x, maxY * targetPosition.y);
        root.anchorMin = minAnchorTarget;
        root.anchorMax = root.anchorMin + padding;
    }

    IEnumerator Moving(Vector2 target, float speed, bool smooth)
    {
        targetPosition = target;
        Vector2 padding = anchorPadding;
        float maxX = 1f - padding.x;
        float maxY = 1f - padding.y;

        Vector2 minAnchorTarget = new Vector2(maxX * targetPosition.x, maxY * targetPosition.y);
        speed *= Time.deltaTime;

        while(root.anchorMin != minAnchorTarget)
        {
            root.anchorMin = (!smooth) ? Vector2.MoveTowards(root.anchorMin, minAnchorTarget, speed) : Vector2.Lerp(root.anchorMin, minAnchorTarget, speed);
            root.anchorMax = root.anchorMin + padding;
            yield return new WaitForEndOfFrame();
        }
        StopMoving();
    }

    //Begin Transition
    public Sprite GetSprite(int index = 0)
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Images/Characters/" + characterName);
        return sprites[index];
    }
    public Sprite GetSprite(string spriteName = "")
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Images/Characters/" + characterName);
        for(int i = 0; i < sprites.Length; i++) {
            if (sprites[i].name == spriteName)
                return sprites[i];
        }
        return sprites.Length > 0 ? sprites[0] : null;
    }

    public void SetBody(int index)
    {
        renderers.bodyRenderer.sprite = GetSprite(index);
    }
    public void SetBody(Sprite sprite)
    {
        renderers.bodyRenderer.sprite = sprite;
    }
    public void SetBody(string spriteName)
    {
        renderers.bodyRenderer.sprite = GetSprite(spriteName);
    }
    public void SetExpression(int index)
    {
        renderers.expressionRenderer.sprite = GetSprite(index);
    }
    public void SetExpression(Sprite sprite)
    {
        renderers.expressionRenderer.sprite = sprite;
    }
    public void SetExpression(string spriteName)
    {
        renderers.expressionRenderer.sprite = GetSprite(spriteName);
    }


    // Transntion Body
    bool isTransitioningBody {
        get
        {
            return transitioningBody != null;
        }
    }

    Coroutine transitioningBody = null;

    public void TransitionBody(Sprite Sprite, float speed, bool smooth)
    {
        if (renderers.bodyRenderer.sprite == Sprite)
            return;
        StopTransitioningBody();
        transitioningBody = CharacterManagement.instance.StartCoroutine(TransitioningBody(Sprite, speed, smooth));
    }

    void StopTransitioningBody()
    {
        if (isTransitioningBody)
        {
            CharacterManagement.instance.StopCoroutine(transitioningBody);
        }
        transitioningBody = null;
    }

    public IEnumerator TransitioningBody(Sprite sprite, float speed, bool smooth)
    {
        for(int i = 0; i < renderers.allBodyRenderers.Count; i++)
        {
            Image image = renderers.allBodyRenderers[i];
            if(image.sprite == sprite)
            {
                renderers.bodyRenderer = image;
                break;
            }
        }
        if(renderers.bodyRenderer.sprite != sprite)
        {
            Image image = GameObject.Instantiate(renderers.bodyRenderer.gameObject, renderers.bodyRenderer.transform.parent).GetComponent<Image>();
            renderers.allBodyRenderers.Add(image);
            renderers.bodyRenderer = image;
            image.color = GlobalF.SetAlpha(image.color, 0f);
            image.sprite = sprite;
        }
        while (GlobalF.TransitionImages(ref renderers.bodyRenderer, ref renderers.allBodyRenderers, speed, smooth))
            yield return new WaitForEndOfFrame();

        StopTransitioningBody();
    }

    //TransitionExpression

    bool isTransitioningExpression
    {
        get
        {
            return transitioningExpression != null;
        }
    }

    Coroutine transitioningExpression = null;

    public void TransitionExpression(Sprite Sprite, float speed, bool smooth)
    {
        if (renderers.expressionRenderer.sprite == Sprite)
            return;
        StopTransitioningExpression();
        transitioningExpression = CharacterManagement.instance.StartCoroutine(TransitioningExpression(Sprite, speed, smooth));
    }

    void StopTransitioningExpression()
    {
        if (isTransitioningExpression)
        {
            CharacterManagement.instance.StopCoroutine(transitioningExpression);
        }
        transitioningExpression = null;
    }

    public IEnumerator TransitioningExpression(Sprite sprite, float speed, bool smooth)
    {
        for (int i = 0; i < renderers.allExpressionRenderers.Count; i++)
        {
            Image image = renderers.allExpressionRenderers[i];
            if (image.sprite == sprite)
            {
                renderers.expressionRenderer = image;
                break;
            }
        }
        if (renderers.expressionRenderer.sprite != sprite)
        {
            Image image = GameObject.Instantiate(renderers.expressionRenderer.gameObject, renderers.expressionRenderer.transform.parent).GetComponent<Image>();
            renderers.allExpressionRenderers.Add(image);
            renderers.expressionRenderer = image;
            image.color = GlobalF.SetAlpha(image.color, 0f);
            image.sprite = sprite;
        }
        while (GlobalF.TransitionImages(ref renderers.expressionRenderer, ref renderers.allExpressionRenderers, speed, smooth))
            yield return new WaitForEndOfFrame();

        StopTransitioningExpression();
    }

    //Stop transition

    public Character(string _name, bool enableOnStart = true) {
        CharacterManagement cm = CharacterManagement.instance;
        GameObject prefab = Resources.Load<GameObject>("Characters/Character[" + _name + "]");
        GameObject ob = GameObject.Instantiate(prefab, cm.characterPanel);

        root = ob.GetComponent<RectTransform>();
        characterName = _name;

        renderers.renderer = ob.GetComponentInChildren<RawImage>();
        if (isMultiLayerCharacter) {
            renderers.bodyRenderer = ob.transform.Find("BodyLayer").GetComponentInChildren<Image>();
            renderers.expressionRenderer = ob.transform.Find("ExpressionLayer").GetComponentInChildren<Image>();
            renderers.allBodyRenderers.Add(renderers.bodyRenderer);
            renderers.allBodyRenderers.Add(renderers.expressionRenderer);
        }
        dialogue = DialogueSystem.instance;

        enabled = enableOnStart;
    }

    [System.Serializable]
    public class Renderers {
        public RawImage renderer;

        public Image bodyRenderer;
        public Image expressionRenderer;

        public List<Image> allBodyRenderers = new List<Image>();
        public List<Image> allExpressionRenderers = new List<Image>();

    }

   public Renderers renderers = new Renderers();
}
