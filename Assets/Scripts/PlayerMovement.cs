using UnityEditor;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Pun;
public class PlayerMovement : MonoBehaviourPunCallbacks
{
    Hashtable hash;
    public Vector3 cam_rot;
    public GameObject bullet;
    public GameObject laserBullet;
    bool automaticGunBool = false;
    float automaticGunTimer;
    bool laserBool = false;
    //public GameObject aim;
    float playerSpeed = 75.0f;
    public float mouseSensitivity;
    Vector3 fwd;
    //bool speedupBool = false;
    //bool reFuelBool = false;
   // float speedUpTempTimer;
    //float speedUpRefuelTimer;
    public GameObject shotgunBullet;
    public static bool shotgunBool = false;
    bool bulletBool = false;
    int shotGunAmmo = 1;
    public static bool fastShotGunBool = false;
    public GameObject cam;
    public Transform bulletSpawn;
    public GameObject UI;
    public static int gunnum;
    //  public ParticleSystem bulletPartical;
    //public GameObject cam_ctrl;
    // Start is called before the first frame update

    private void Awake()
    {
        //laserBullet = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //shotgunBullet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //bullet = GameObject.CreatePrimitive(PrimitiveType.Capsule);
    }


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        if (!photonView.IsMine)
        {
            cam.SetActive(false);
            UI.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(photonView.IsMine)
        {
            Player();
     
            BulletSpawn();
           
            //  Laser();
           // AutomaticGun();
           // Shotgun();
           // FastShotgun();
            //SpeedUpButton();
            Gun();
        }
        else if(!photonView.IsMine)
        {
            //Debug.Log("Controlling the other guy");
        }


        /*if (speedupBool == true)
        {
            SpeedUp();
        }

        if (speedupBool == false)
        {
            playerSpeed = default;
            //Debug.Log(enemySpeed);
        }*/

    }

    void Player() 
    {
        
        cam_rot.x += Input.GetAxis("Mouse X") * mouseSensitivity*Time.deltaTime;
        cam_rot.y += Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        // cam_rot.x = Mathf.Repeat(cam_rot.x, 360);
       
        float clampRot = Mathf.Clamp(cam_rot.x, 90, -90);
        transform.rotation = Quaternion.Euler(-cam_rot.y, cam_rot.x, 0);
        
        if (Input.GetKey(KeyCode.W)) 
        {

            fwd = transform.TransformDirection(Vector3.forward);
            fwd.y = 0;
            transform.position += fwd * playerSpeed * Time.deltaTime;
            

        }

        if (Input.GetKey(KeyCode.S))
        {

            fwd = transform.TransformDirection(Vector3.back);
            fwd.y = 0;
            transform.position += fwd*playerSpeed*Time.deltaTime;

        }

        if (Input.GetKey(KeyCode.A))
        {

            fwd = transform.TransformDirection(Vector3.left);
            fwd.y = 0;
            transform.position += fwd * playerSpeed * Time.deltaTime;

        }

        if (Input.GetKey(KeyCode.D))
        {

            fwd = transform.TransformDirection(Vector3.right);
            fwd.y = 0;
            transform.position += fwd * playerSpeed * Time.deltaTime;

        }
    }

    void Gun()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            
            gunnum = 1;
            // shotgunBool = false;
            // Debug.Log("You Have Pressed No.1");
            hash = new Hashtable();
            hash.Add("gun", gunnum);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
    }
    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, Hashtable changedProps)
    {
       if(!photonView.IsMine )
        BulletSpawn();
    }
   
  public  void BulletSpawn() 
    {
      //  gunno = gunnum;
        //if (Input.GetMouseButtonDown(0) && bulletBool == true)
        // {
        //     Instantiate(bullet, transform.position, transform.rotation);
        //    //PhotonNetwork.Instantiate(bullet.name, bulletSpawn.position, bulletSpawn.rotation);
        // }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Physics.Raycast(transform.position,transform.forward, out hit, 1000000);
            
            if (Physics.Raycast(transform.position, transform.forward, out hit, 1000000))
            {
                //Debug.Log(hit.collider);
                if(hit.collider.tag == "Player")
                {
                    hit.collider.GetComponent<PlayerHealth>().TakeDamage();
                }
            }
            if (gunnum == 1)
            {
                GameObject bulletobj = Instantiate(bullet, transform.position, Quaternion.identity);
                //   bulletobj.GetComponent<Rigidbody>().AddForce(transform.forward, ForceMode.Impulse);
            }
            /* bulletPartical.Play();
             bulletPartical.transform.position = aim.transform.position;
             bulletPartical.transform.position += transform.forward;*/
        }
    }
    
    void Laser()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            laserBool = true;
            bulletBool = false;
            Debug.Log("Laser Activated.");
        }

        if(Input.GetMouseButtonDown(0) && laserBool == true)
        {
            Instantiate(laserBullet, transform.position, transform.rotation);
            laserBool = false;
            bulletBool = true;
        }
    }

    void AutomaticGun()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            automaticGunBool = true;
            bulletBool = false;
        }

        if(automaticGunBool == true)
        {
            Instantiate(bullet, transform.position, transform.rotation);
            automaticGunTimer += Time.deltaTime;
        }

        if(automaticGunTimer >= 5)
        {
            automaticGunBool = false;
            automaticGunTimer = default;
            bulletBool = true;
        }
    }

    void Shotgun()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            shotgunBool = true;
            bulletBool = false;
            fastShotGunBool = false;
            Debug.Log("ShotGun Activated.");
        }

        if(Input.GetMouseButtonDown(0) && shotgunBool == true && shotGunAmmo == 1)
        {
            Instantiate(shotgunBullet, transform.position, transform.rotation);
            shotGunAmmo--;
            Debug.Log("Reload chey ra jaffa");
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            shotGunAmmo = 1;
            Debug.Log("Reloaded");
        }
    }

    void FastShotgun()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            fastShotGunBool = true;
            bulletBool = false;
            shotGunAmmo = 1;
            shotgunBool = false;
            Debug.Log("Fast ShotGun Activated.");
        }

        if (Input.GetMouseButtonDown(0) && fastShotGunBool == true && shotGunAmmo == 1)
        {
            Instantiate(shotgunBullet, transform.position, transform.rotation);
            shotGunAmmo--;
            Debug.Log("Reload chey ra jaffa");
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            shotGunAmmo = 1;
            Debug.Log("Reloaded");
        }
    }

   /* void SpeedUpButton()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speedupBool = true;
            playerSpeed = playerSpeed * 2;
            Debug.Log(playerSpeed);
        }
        
        //speedUpButtonGO.SetActive(false);
    }

    void SpeedUp()
    {
        speedUpTempTimer += 1 * Time.deltaTime;
        Debug.Log(speedUpTempTimer);

        if (speedUpTempTimer >= 5)
        {
            speedupBool = false;
            speedUpTempTimer = default;
            reFuelBool = true;

        }

        if(speedupBool == false)
        {
            playerSpeed = 75.0f;
        }
    }

    void SpeedUpRefueling()
    {
        if (reFuelBool == true)
        {
            speedUpRefuelTimer += 1 * Time.deltaTime;
        }

        if (speedUpRefuelTimer >= 5)
        {
            //speedUpButtonGO.SetActive(true);
            reFuelBool = false;
            speedUpRefuelTimer = default;
        }
    }*/
}