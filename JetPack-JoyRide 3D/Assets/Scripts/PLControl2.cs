using UnityEngine;


public class PLControl2 : MonoBehaviour
{
    public Rigidbody rb;
    public float force;
    public float forceOfJump;


    public Transform objCamera;
    public GameObject gameOver;
    public CanvasScript canvas;
    public ParticleSystem particleSystem;
    public Canvas menuCanvas;
    public Canvas playCanvas;
    public UnityEngine.UI.Image pauseImage;
    public UnityEngine.UI.Button pauseButton;
    public GameObject[] ObjectsToHide;

    private Vector3 mPos;
    private float dif;
    private bool loose = false;
    private bool isPause = false;
    private bool isStarted = false;
    private bool OnVehicle = false;
    public bool CanVehicle = false;
    [SerializeField] private BuyMenu menu;
    public int vehicleActive = -1;
    public int vehicleChecker = 0;
    [SerializeField] private GameObject Veh1;
    [SerializeField] private GameObject Veh2;
    [SerializeField] private GameObject platformContainer;
    private GameObject currentVehicle;

    void Start()
    {
        mPos = objCamera.position;
        dif = transform.position.x - mPos.x;

        UnityEngine.UI.Button btn = pauseButton.GetComponent<UnityEngine.UI.Button>();
        btn.onClick.AddListener(TaskOnClick);
    }


    void Update()
    {
        if(isStarted)
        { 
            if (!isPause)
            {
                objCamera.position = new Vector3(transform.position.x - dif, mPos.y, mPos.z);

                if (rb.position.y < 0.5)
                {
                    particleSystem.Stop();
                }
                else
                {
                    particleSystem.Play();
                }

                if (!loose)
                {
                    transform.position += transform.forward * Time.deltaTime * -force;

                    //transform.eulerAngles = new Vector3(0, 90, 0);

                    if (Input.GetKey(KeyCode.Space) && !OnVehicle)
                    {
                        rb.AddForce(Vector3.up * forceOfJump);
                    }
                    else if (Input.GetKeyDown(KeyCode.Space) && OnVehicle)
                    {
                        rb.AddForce(Vector3.up * forceOfJump * 100);
                    }
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        playCanvas.gameObject.SetActive(false);
                        menuCanvas.gameObject.SetActive(true);
                        pauseImage.gameObject.SetActive(true);
                        rb.isKinematic = true;
                        isPause = true;
                        foreach(GameObject obj in ObjectsToHide)
                        {
                            obj.SetActive(false);
                        }
                    }
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    playCanvas.gameObject.SetActive(true);
                    pauseImage.gameObject.SetActive(false);
                    menuCanvas.gameObject.SetActive(false);
                    rb.isKinematic = false;
                    isPause = false;
                }
            }
        }
        else
        {
            playCanvas.gameObject.SetActive(false);
            pauseImage.gameObject.SetActive(false);
            menuCanvas.gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                playCanvas.gameObject.SetActive(true);
                pauseImage.gameObject.SetActive(false);
                menuCanvas.gameObject.SetActive(false);
                rb.isKinematic = false;
                isPause = false;
                isStarted = true;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Laser")
        {
            if (!OnVehicle)
            {
                LooseGame();
            }
            else
            {
                collision.gameObject.SetActive(false);
                Vehicle();
            }
        }
        else
        {
        }
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Laser")
        {
            if (!OnVehicle)
            {
                LooseGame();
            }
            else
            {
                Vehicle();
            }
        }
        else if(collider.gameObject.tag == "Pickup")
        {
            collider.gameObject.SetActive(false);
            canvas.PlusCoin();
        }
        else if (collider.gameObject.tag == "Pickup2")
        {
            collider.gameObject.SetActive(false);
            Vehicle();
        }
        else
        {

        }
    }
    void TaskOnClick()
    {
        playCanvas.gameObject.SetActive(false);
        menuCanvas.gameObject.SetActive(true);
        pauseImage.gameObject.SetActive(true);
        rb.isKinematic = true;
        isPause = true;
        foreach (GameObject obj in ObjectsToHide)
        {
            obj.SetActive(false);
        }
    }
    void Vehicle()
    {
        if (!OnVehicle)
        {
            if (vehicleActive == 0)
            {
                currentVehicle = Instantiate(Veh1, rb.transform);
                particleSystem.gameObject.SetActive(false);
                force = 30;
                OnVehicle = true;
                vehicleActive = UnityEngine.Random.Range(0, vehicleChecker);
            }
            else if (vehicleActive == 1)
            {
                currentVehicle = Instantiate(Veh2, rb.transform);
                particleSystem.gameObject.SetActive(false);
                force = 18;
                OnVehicle = true;
                vehicleActive = UnityEngine.Random.Range(0, vehicleChecker);
            }

        }
        else
        {
            Destroy(currentVehicle);
            particleSystem.gameObject.SetActive(true);
            OnVehicle = false;
            force = 12;
        }
    }
    void LooseGame()
    {
        loose = true;
        gameOver.SetActive(true);
        canvas.FillText();
        canvas.SumCoin();
    }
    public void Veh()
    {
        if (menu.vehicles[0] == 1) CanVehicle = true;
    }
}