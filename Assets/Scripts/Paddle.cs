using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    #region Singleton
    private static Paddle _instance;
    public static Paddle Instance => _instance;
    public bool PaddleIsTransforming { get; set; }
    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion

    private Camera mainCamera;
    private float paddleInitialY;
    private float defaultPaddleWidthInPixels = 200;
    private float defautLeftClamp = 180;
    private float defautRightClamp = 1740;
    private SpriteRenderer sr;
    private BoxCollider2D boxCol;
    public float extendShrinkDuration = 5;
    public float paddleWidth = 2f;
    public float paddleHeight = 0.3f;

    private void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
        paddleInitialY = this.transform.position.y;
        this.sr = GetComponent<SpriteRenderer>();
        this.boxCol = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        PaddleMovement();
    }

    private void PaddleMovement()
    {
        float paddleShift = (defaultPaddleWidthInPixels - ((defaultPaddleWidthInPixels / 2) * this.sr.size.x)) / 2;
        float leftClamp = defautLeftClamp - paddleShift;
        float rightClamp = defautRightClamp + paddleShift;
        float mousePositionPixels = Mathf.Clamp(Input.mousePosition.x, leftClamp, rightClamp);
        float mousePositionWorldX = mainCamera.ScreenToWorldPoint(new Vector3(mousePositionPixels, 0, 0)).x;
        this.transform.position = new Vector3(mousePositionWorldX, paddleInitialY, 0);
    }

    public void StartWidthAnimation(float newWidth){
        StartCoroutine(AnimatePaddleWidth(newWidth));
    }
    public IEnumerator AnimatePaddleWidth(float width){
        this.PaddleIsTransforming = true;
        if(width!=paddleWidth){
            this.StartCoroutine(ResetPaddleWidthAfterTime(this.extendShrinkDuration));
        }
        
        float currentWidth = this.sr.size.x;
        if(width > currentWidth){
            while(currentWidth<width){
                currentWidth += Time.deltaTime *2;
                this.sr.size = new Vector2(currentWidth,paddleHeight);
                boxCol.size = new Vector2(currentWidth,paddleHeight);
                yield return null;
            }
        }
        else{
            while(currentWidth > width){
                currentWidth -= Time.deltaTime *2;
                this.sr.size = new Vector2(currentWidth,paddleHeight);
                boxCol.size = new Vector2(currentWidth,paddleHeight);
                yield return null;
            }
        }
        this.PaddleIsTransforming = false;
    }

    private IEnumerator ResetPaddleWidthAfterTime(float duration){
        // Debug.Log("ResetPaddleWidthAfterTime START WAITING");
        yield return new WaitForSeconds(duration);
        // Debug.Log("ResetPaddleWidthAfterTime END ");
        this.StartWidthAnimation(this.paddleWidth);
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Ball")
        {
            Rigidbody2D ballRb = coll.gameObject.GetComponent<Rigidbody2D>();
            Vector3 hitPoint = coll.contacts[0].point;
            Vector3 paddleCenter = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y);

            ballRb.velocity = Vector2.zero;

            float difference = paddleCenter.x - hitPoint.x;
            if (hitPoint.x < paddleCenter.x)
            {
                ballRb.AddForce(new Vector2(-(Mathf.Abs(difference * 200)), BallsManager.Instance.initialBallSpeed));
            }
            else
            {
                ballRb.AddForce(new Vector2((Mathf.Abs(difference * 200)), BallsManager.Instance.initialBallSpeed));
            }
        }
    }
}
