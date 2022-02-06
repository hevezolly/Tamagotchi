using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuffBallEmotions : MonoBehaviour
{
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private Image image;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private EmotionCanvasPosition position;
    [SerializeField]
    private float ExistTime;
    
    
    [Header("animations")]
    [SerializeField]
    private string FadeInAnimation;
    [SerializeField]
    private string FadeOutAnimation;
    

    private Queue<EmotionRequest> requests = new Queue<EmotionRequest>();
    private bool isShowing = false;
    private bool isApearing = false;
    private bool isHiding = false;
    private float startTime;
    private Vector2 realOffset;

    private void Awake()
    {
        canvas.gameObject.SetActive(true);
    }

    public void RequestEmotion(EmotionRequest request)
    {
        if (!isShowing && requests.Count == 0)
            ShowEmotion(request);
        else
            requests.Enqueue(request);
    }

    private void ShowEmotion(EmotionRequest request)
    {
        isShowing = true;
        image.sprite = request.sprite;
        animator.Play(FadeInAnimation);
        position.StartAdjusting();
        isApearing = true;
    }

    private void HideEmotion()
    {
        isHiding = true;
        animator.Play(FadeOutAnimation);
    }

    private void Update()
    {
        if (isShowing && !isApearing && !isHiding 
            && Time.time >= startTime + ExistTime)
        {
            HideEmotion();
        }
        if (!isShowing && requests.Count > 0)
        {
            ShowEmotion(requests.Dequeue());
        }
    }

    public void InAnimationFinished()
    {
        startTime = Time.time;
        isApearing = false;
    }

    public void OutAnimationFinished()
    {
        isHiding = false;
        isShowing = false;
        image.sprite = null;
        position.StopAdjusting();
    }


}

public class EmotionRequest
{
    public Sprite sprite;
}
