using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpForce = 3f;
    [SerializeField] private Vector3 jumpVector = new Vector3(0.0f, 2.0f, 0.0f);
    [SerializeField] private Vector3 MoveLeftVector = new Vector3(-5.0f, 0.0f, 0.0f);
    [SerializeField] private Vector3 MoveRightVector = new Vector3(5.0f, 0.0f, 0.0f);
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject deathPanel;
    [SerializeField] private GameObject startText;
    [SerializeField] private AudioController audioController;
    [SerializeField] private WorldGenerator worldController;

    private Vector2 touchUpPosition;
    private Vector2 touchDownPosition;
    private Vector3 curPos;
    private bool IsGrounded = true;
    private bool IsPlayed = false;
    private bool IsLive = true;
    const double yGroundDif = 3.05;
    const float xBorder = 35f;
    const float minDistanceForSwipe = 20f;
    void Update()
    {
        if (IsPlayed)
        {
            if (IsLive)
            {
                curPos = transform.position;
                IsGrounded = curPos.y <= yGroundDif ? true : false;
                transform.position += transform.forward * Time.deltaTime * speed;

                foreach (Touch touch in Input.touches)
                {
                    if(touch.phase == TouchPhase.Began)
                    {
                        touchUpPosition = touch.position;
                        touchDownPosition = touch.position;
                    }
                    if (touch.phase == TouchPhase.Ended)
                    {
                        touchUpPosition = touch.position;
                        DetectSwipe();
                    }
                }

            }
        }
        else
        {
            if (Input.anyKeyDown)
            {
                IsPlayed = true;
                startText.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Obstacle")
        {
            IsPlayed = false;
            IsLive = false;
            audioController.PlaySoundHit();
            deathPanel.SetActive(true);
            Invoke(nameof(RestartGame), 8.0f);
        }
        if (collider.gameObject.tag == "PickUp")
        {
            audioController.PlayRandomQuoteSound();
            Invoke(nameof(SpawnBaobab), 2.0f);
        }
    }
    void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    void SpawnBaobab()
    {
        worldController.SpawnBaobabTree();
    }

    void DetectSwipe()
    {
        if (SwipeDistanceCheck())
        {
            if (IsVerticalSwipe())
            {
                if ((VerticalDirection() < minDistanceForSwipe) && IsGrounded)
                {
                    rb.AddForce(jumpVector * jumpForce, ForceMode.Impulse);
                    audioController.PlaySoundJump();
                }
            }
            else
            {
                if (HorizontalDirection()<0)
                {
                    rb.AddForce(MoveRightVector, ForceMode.Impulse);
                }
                else
                {
                    rb.AddForce(MoveLeftVector, ForceMode.Impulse);
                }
            }
        }
    }
    float VerticalDirection()
    {
        return touchDownPosition.y - touchUpPosition.y;
    }
    float VerticalMovementDistance()
    {
        return Mathf.Abs(VerticalDirection());
    }
    float HorizontalDirection()
    {
        return touchDownPosition.x - touchUpPosition.x;
    }
    float HorizontalMovementDistance()
    {
        return Mathf.Abs(HorizontalDirection());
    }
    bool IsVerticalSwipe()
    {
        return VerticalMovementDistance() > HorizontalMovementDistance();
    }
    bool SwipeDistanceCheck()
    {
        return VerticalMovementDistance() > minDistanceForSwipe || HorizontalMovementDistance() > minDistanceForSwipe;
    }
}
