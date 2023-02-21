using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.InputSystem;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 1.5f;
    public float movespeed = 18.0f;
    public float originalspeed = 1.0f;

    //invincible time after collision
    public float invincible = 0;
    
    //public GameObject winTextObject;

    private Rigidbody rb;
    private float movementX;
    private float movementY;

    //counting score
    private int count;
    public TextMeshProUGUI countText;

    //counting life
    public Image[] lives;
    public int life = 5;

    //Joystick movement
    public FixedJoystick Fjs;

    //win condition
    public GameObject winTextObject;
    public GameObject failTextObject;
    public bool isWin = false;
    public int trashLeft = 20;

    //Trash instantiate
    public GameObject SpaceTrash;
    public GameObject MoonTrash;

    //touch collection
    public Material Notify;


    public float FlashTime = 1;
    public float frozenTime = 0.5f;
    //public List<touchLocation> touches = new List<touchLocation>(); 

    public Button boost;
    public Button backCamera;

    public Camera camera1;
    public Camera camera2;
    private Camera currentCam;

    //Shaking
    public float duration = 1f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        currentCam = camera1;
        SetCountText();
        winTextObject.SetActive(false);
        failTextObject.SetActive(false);
    }

    //counting scores
    void SetCountText()
    {
        countText.text = "Value collected Trash: $" + count.ToString();
        if (GameObject.FindGameObjectsWithTag("LunarTrash").Length == 0 && GameObject.FindGameObjectsWithTag("Trash").Length == 0)
        {
            winTextObject.SetActive(true);
            Time.timeScale = 0;
        }
    }


    //Screen shaking
    IEnumerator Shaking()
    {
        Vector3 startPosition = currentCam.transform.position;
        float elapsedTime = 0f;

        while(elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            transform.position = startPosition + Random.insideUnitSphere;
            yield return null;
        }
        transform.position = startPosition;
    }

    public void LoseLife()
    {
        //Decrease life
        //Hide one image
        life--;
        lives[life].enabled = false;
        count = count - 50;
        StartCoroutine(Shaking());
        SetCountText();
        //If no life, game lose
        if (life <= 0)
        {
            life = 0;
            failTextObject.SetActive(true);
            Time.timeScale = 0;
        }
        //SetCountText();
    }

    //Collect Trash
    private void TestRay()
    {
        Rect bottomLeft = new Rect(0, 0, Screen.width / 3, Screen.height / 2);
        Rect bottomRight = new Rect(3*Screen.width / 4, 0, Screen.width / 4, Screen.height / 2);
        Vector2 deltaPos = Fjs.Direction;
        
        if (backCamera.GetComponent<BackCamera>().isPressed == true)
        {
            currentCam = camera2;
            Debug.Log("Back Camera Enabled");
        }
        else
        {
            currentCam = camera1;
        }

        if (Mathf.Abs(deltaPos.x) > 0.1 || Mathf.Abs(deltaPos.y) > 0.1 || boost.GetComponent<BoostControll>().isPressed == true)
        {
            frozenTime = 0.5f;
        }
        else if (Input.touchCount > 0 && frozenTime <= 0)
        {
            for (int i = 0; i < Input.touchCount; ++i)
            {
                Touch touch = Input.GetTouch(i);
                //avoid mis touhing
                if (touch.phase == TouchPhase.Began && !(bottomLeft.Contains(touch.position) || bottomRight.Contains(touch.position)))
                {
                    Debug.Log("touched began");
                    if (bottomLeft.Contains(touch.position) || bottomRight.Contains(touch.position))
                    {
                        Debug.Log("Bottom touched");
                        return;
                    }
                    Debug.Log(currentCam);
                    Ray ray = currentCam.ScreenPointToRay(touch.position);
                    RaycastHit hitInfo;
                    //detecting ray hit object
                    if (Physics.Raycast(ray, out hitInfo))
                    {
                        Debug.Log("Succeed");

                        if (hitInfo.collider.gameObject.CompareTag("Trash"))
                        {
                            //avoid multiple touch
                            hitInfo.collider.gameObject.tag = "Untagged";
                            hitInfo.collider.gameObject.GetComponent<MeshRenderer>().material = Notify;
                            Object.Destroy(hitInfo.collider.gameObject, 0.6f);
                            //hitInfo.collider.gameObject.SetActive(false);
                            count += 50;
                            trashLeft--;
                            SetCountText();
                        }
                        else if (hitInfo.collider.gameObject.CompareTag("LunarTrash"))
                        {
                            hitInfo.collider.gameObject.tag = "Untagged";
                            hitInfo.collider.gameObject.GetComponent<MeshRenderer>().material = Notify;
                            Object.Destroy(hitInfo.collider.gameObject, 0.6f);
                            count += 100;
                            trashLeft--;
                            SetCountText();
                        }
                        else if (hitInfo.collider.gameObject.CompareTag("Planets") && invincible <= 0)
                        {
                            LoseLife();
                            invincible = 2;
                            Vector3 moonPos = hitInfo.collider.gameObject.transform.position;
                            Vector3 newPos = new Vector3(UnityEngine.Random.Range(2, 3), UnityEngine.Random.Range(2, 3), UnityEngine.Random.Range(2, 3));
                            GameObject newTrash = Instantiate(MoonTrash, moonPos + newPos, Random.rotation, hitInfo.collider.gameObject.transform.parent.transform);
                            newTrash.GetComponent<RandomRotateAround>().target = hitInfo.collider.gameObject;
                            trashLeft++;

                            SetCountText();
                        }
                        else if (hitInfo.collider.gameObject.CompareTag("CenterPlanet") && invincible <= 0)
                        {
                            LoseLife();
                            invincible = 2;
                            Vector3 newPos = new Vector3(UnityEngine.Random.Range(-39, -31), 4, UnityEngine.Random.Range(-49, -26));
                            Instantiate(SpaceTrash, newPos, Random.rotation);
                            trashLeft++;
                            SetCountText();
                        }
                        else if (hitInfo.collider.gameObject.CompareTag("Satellites") && invincible <= 0)
                        {
                            LoseLife();
                            invincible = 2;
                            SetCountText();
                        }

                    }
                }
            }

            
        }
        frozenTime = frozenTime - Time.deltaTime;
    }

    
    //check collision
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Planets" && invincible <= 0)
        {
            LoseLife();
            invincible = 2;
            Vector3 moonPos = collision.gameObject.transform.position;
            Vector3 newPos = new Vector3(UnityEngine.Random.Range(2, 3), UnityEngine.Random.Range(2, 3), UnityEngine.Random.Range(2, 3));
            GameObject newTrash = Instantiate(MoonTrash, moonPos+newPos, Random.rotation, collision.gameObject.transform.parent.transform);
            newTrash.GetComponent<RandomRotateAround>().target = collision.gameObject;
            trashLeft++;

            SetCountText();
        }

        if(collision.gameObject.tag == "CenterPlanet" && invincible <= 0)
        {
            LoseLife();
            invincible = 2;
            Vector3 newPos = new Vector3(UnityEngine.Random.Range(-39, -31), 4, UnityEngine.Random.Range(-49, -26));
            Instantiate(SpaceTrash, newPos, Random.rotation);
            trashLeft++;
            SetCountText();
        }
        if(collision.gameObject.tag == "Satellites")
        {
            LoseLife();
            invincible = 2;
            SetCountText();
        }

        
    }

    void FixedUpdate()
    {
        Vector2 deltaPos = Fjs.Direction;
        if(Mathf.Abs(deltaPos.x) > 0.1)
        {
            transform.Rotate(Vector3.up * deltaPos.x * speed, Space.Self);
        }
        if(Mathf.Abs(deltaPos.y) > 0.1)
        {
            transform.Rotate(Vector3.left * deltaPos.y * speed, Space.Self);
        }

        
    }

    void Update()
    {
        if (backCamera.GetComponent<BackCamera>().isPressed == true)
        {
            currentCam = camera2;
            Debug.Log("Back Camera Enabled");
        }
        else
        {
            currentCam = camera1;
        }




        if(invincible > 0)
        {
            invincible = invincible - Time.deltaTime;
        }
        else
        {
            invincible = 0;
        }
        SetCountText();
        TestRay();
        Debug.Log(trashLeft);
    }

    //Collect Trash
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Trash"))
        {
            other.gameObject.tag = "Untagged";
            other.gameObject.GetComponent<MeshRenderer>().material = Notify;
            Object.Destroy(other.gameObject, 0.6f);
            count +=50;
            trashLeft--;
            SetCountText();
        }
        else if (other.gameObject.CompareTag("LunarTrash"))
        {
            other.gameObject.tag = "Untagged";
            other.gameObject.GetComponent<MeshRenderer>().material = Notify;
            Object.Destroy(other.gameObject, 0.6f);
            count += 100;
            trashLeft--;
            SetCountText();
        }

    }


}
