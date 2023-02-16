using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class trajectoryScript : MonoBehaviour
{
    public SpriteRenderer ThisObject;
   /* public AudioSource CatapultSFXsource;
    public AudioClip[] CatapultSFXclips;*/

    public enum WeaponType { Rock, FireRock, Arrow, FireArrow, Cannon, M777A2, Panhard, Rocket, Tank, Drone,TwinTank,Robot,Helicopter,SpaceShip14,SpaceShip15};
    public WeaponType weaponType;
    public int angleRotation=0;
    public bool isFire;
    public GameObject otherAmmo;

    [HideInInspector] public bool DotsLimit = true, isThrown;
    [HideInInspector] public float SHOTX = 0;
    [HideInInspector]public Collision2D collisionObj;
   // public Animator animator;

    bool spawnNew = false;
    Sprite dotSprite;
    bool changeSpriteAfterStart;
    bool isTrack;
    float initialDotSize = 1;
    int numberOfDots = 50;
    float dotSeparation = 5;
    float dotShift = 3;
    [HideInInspector]public GameObject trajectoryDots;
    GameObject ball;
    Rigidbody2D ballRB;
    Vector3 ballPos;
    Vector3 fingerPos;
    Vector3 ballFingerDiff;
    Vector2 shotForce;
    float x1, y1;
    bool ballIsClicked = false;
    bool ballIsClicked2 = false;
    bool HitGround = false;
    float shootingPowerY = 5;
    bool explodeEnabled;
    bool grabWhileMoving;
    float SHOTY = 0;

    void Start()
    {
        ball = gameObject;

        GameManager.instance.TrajectoryDots.SetActive(true);
        trajectoryDots = GameObject.Find("Trajectory Dots");
        ballRB = GetComponent<Rigidbody2D>();

        trajectoryDots.transform.localScale = new Vector3(initialDotSize, initialDotSize, trajectoryDots.transform.localScale.z);

        for (int k = 0; k < 40; k++)
        {
            GameManager.instance.dots[k] = GameObject.Find("Dot (" + k + ")");
            if (dotSprite != null)
            {
                GameManager.instance.dots[k].GetComponent<SpriteRenderer>().sprite = dotSprite;
            }
        }
        for (int k = numberOfDots; k < 40; k++)
        {
            GameObject.Find("Dot (" + k + ")").SetActive(false);
        }
        trajectoryDots.SetActive(false);
    }

    void Update()
    {
        ProjectileAngleFixer();
        PredictedTrajectoryPathTravel();
    }

    private void ProjectileAngleFixer()
    {
        if(angleRotation!=0)
        {
            if (!HitGround && isTrack)
                TrackMovement(angleRotation);
        }
    }

    private void PredictedTrajectoryPathTravel()
    {
        if (!isThrown)
        {
            numberOfDots = GameManager.instance.dots.Length;

            PointerEventData pointer = new PointerEventData(EventSystem.current);
            pointer.position = Input.mousePosition;

            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointer, raycastResults);

            if (raycastResults.Count > 0)
            {
                for (int i = 0; i < raycastResults.Count; i++)
                {

                    if (raycastResults[i].gameObject.name.Equals("Ball Click Area"))
                    {
                        ballIsClicked = true;
                    }
                }
            }

            if (ballIsClicked2 == true)
            {
                ballIsClicked = true;
            }

            if ((ballRB.velocity.x * ballRB.velocity.x) + (ballRB.velocity.y * ballRB.velocity.y) <= 0.0085f)
            {
                ballRB.velocity = new Vector2(0f, 0f);
            }
            else
            {
                trajectoryDots.SetActive(false);
            }
            ballPos = ball.transform.position;

            if (changeSpriteAfterStart == true)
            {
                for (int k = 0; k < numberOfDots; k++)
                {
                    if (dotSprite != null)
                    {
                        GameManager.instance.dots[k].GetComponent<SpriteRenderer>().sprite = dotSprite;
                    }
                }
            }

            if ((Input.GetKey(KeyCode.Mouse0) && ballIsClicked == true) && ((ballRB.velocity.x == 0f && ballRB.velocity.y == 0f) || (grabWhileMoving == true)))
            {
                //Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                ballIsClicked2 = true;
                HitGround = false;
                GameManager.instance.fingertouch = true;
                fingerPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                fingerPos.z = 0;

                if (grabWhileMoving == true)
                {
                    ballRB.velocity = new Vector2(0f, 0f);
                    ballRB.isKinematic = true;
                }
                ballFingerDiff = ballPos - fingerPos;

                //==== New
                ballFingerDiff.y -= 5;

                shotForce = new Vector2(ballFingerDiff.y * 1.5f * shootingPowerY * -1, ballFingerDiff.y * 1.5f * shootingPowerY * -1);

                if (!GameManager.instance.isPaused)
                {
                    if ((Mathf.Sqrt((ballFingerDiff.x * ballFingerDiff.x) + (ballFingerDiff.y * ballFingerDiff.y)) > (0.4f)))
                    {
                        trajectoryDots.SetActive(true);
                    }
                    else
                    {
                        trajectoryDots.SetActive(false);
                        if (ballRB.isKinematic == true)
                        {
                            ballRB.isKinematic = false;
                        }
                    }
                    if (DotsLimit)
                    {
                        for (int k = 0; k < numberOfDots; k++)
                        {
                            x1 = ballPos.x + shotForce.x * Time.fixedDeltaTime * (dotSeparation * k + dotShift * 0.1f);
                            y1 = ballPos.y + shotForce.y * Time.fixedDeltaTime * (dotSeparation * k + dotShift * 0.1f) - (-Physics2D.gravity.y / 2f * Time.fixedDeltaTime * Time.fixedDeltaTime * (dotSeparation * k + dotShift) * (dotSeparation * k + dotShift)); //Y position for each point is found
                            GameManager.instance.dots[k].transform.position = new Vector3(x1, y1, GameManager.instance.dots[k].transform.position.z);
                        }
                    }
                }

                SHOTX = shotForce.x;
                SHOTY = shotForce.y;

                if (SHOTX > 18f)//16.5
                {
                    shotForce.x = 18f;//16.5
                    SHOTX = shotForce.x;
                }
                if (SHOTY > 18f)//16.5
                {
                    shotForce.y = 18f;
                    SHOTY = shotForce.y;
                }

                if (x1 > 177)
                {
                    x1 = 170f;
                    y1 = -180f;
                }
            }
           
            if (this.shotForce.x <= 1 )
            {
                trajectoryDots.SetActive(false);
            }

            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                if (this.shotForce.x <= 1)
                {
                    ballIsClicked2 = false;
                    trajectoryDots.SetActive(false);
                    GameManager.instance.fingertouch = false;
                    return;
                }
                GameManager.instance.fingertouch = false;
                ballIsClicked2 = false;

                if (GameManager.instance.TrajectoryDots.activeInHierarchy)
                {
                    if (explodeEnabled == true)
                    {
                        StartCoroutine(explode());
                    }
                }
                /* All Ammos Projectile Shoot */
                if(!spawnNew)
                {
                    ShootProjectile();
                    spawnNew = true;
                }
            }
        }
    }

    void ShootProjectile()
    {
        if (GameManager.instance.playerMachine)
            GameManager.instance.playerMachine.GetComponent<Player_Machine>().ShootProjectile();
        StartCoroutine(ThrowBall(0.3f));
    }
    
    void InitiateTrack()
    {
        isTrack = true;
    }

    void TrackMovement(int AngleDisturbance)
    {
        // All Arrows = (36) , M777A2 = (90)
        Vector2 direction = ballRB.velocity;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - AngleDisturbance, Vector3.forward);
    }

    public IEnumerator ThrowBall(float delay)//,int value)
    {
        isThrown = true;
        trajectoryDots.SetActive(false);
        CharacterAttack.instance.playerVoiceSource.Play();
        if (Sound_Manager.instance)
            Sound_Manager.instance.playerFire.Play();

        if (weaponType == WeaponType.FireRock || weaponType == WeaponType.Rock || weaponType==WeaponType.Rocket)// || weaponType==WeaponType.)//Rock
        {
            GameManager.instance.IsDustParticles = true;
            yield return new WaitForSeconds(delay);
        }

        ThisObject.enabled = true;
        if (PlayerPrefs.GetInt("Machinery" + (int)weaponType + "_Upgraded")==1 && PlayerPrefs.GetInt("Machinery" + (int)weaponType + "_Exhausted")!=1)
        {
            otherAmmo.SetActive(true);
        }
        if (isFire)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(true);
        }
        Invoke("InitiateTrack", 0f);

        ballRB.velocity = new Vector2(shotForce.x, shotForce.y);

        if ((int)weaponType>1) //Except Rock and Fire Rock
        {
            yield return new WaitForSeconds(delay);
        }

        if (ballRB.isKinematic)
        {
            ballRB.isKinematic = false;
        }

        if (weaponType == WeaponType.FireRock || weaponType == WeaponType.Rock )//Rock
        {
            yield return new WaitForSeconds(delay);
        }

        if(spawnNew)
        {
            GameManager.instance.SpawnNew = true;
            spawnNew = false;
        }
    }

    public IEnumerator explode()
    {
        yield return new WaitForSeconds(Time.fixedDeltaTime * (dotSeparation * (numberOfDots - 1f)));
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        collisionObj = collision;
        if (collision.gameObject.tag == "Ground" || (collision.gameObject.CompareTag("Catapult") && isThrown))
        {
            Blast();
            HitGround = true;
            DestroyPreviousAmmo();  
        }
        else if (collision.gameObject.tag == "Castle")
        {
            HitGround = true;
            GameManager.instance.decreaseHealthOfCastle((int)weaponType,this);
            DestroyPreviousAmmo();
            CharacterAttack.instance.hitCastelSource.Play();
        }
    }

    public void Blast()
    {
        ContactPoint2D contact = collisionObj.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 pos = contact.point;

        if (collisionObj.gameObject.CompareTag("Castle") || collisionObj.gameObject.CompareTag("Ground"))
            Instantiate(GameManager.instance.BlastPrefab[Random.Range(0, GameManager.instance.BlastPrefab.Length)], pos, rot);
    }

    void DestroyPreviousAmmo()
    {
        if (Sound_Manager.instance)
            Sound_Manager.instance.playerHit.Play();

        if (weaponType == WeaponType.M777A2)
            GameManager.instance.CheckAmmo(this.gameObject.transform.parent.gameObject,this, 0.5f);
        else if (weaponType == WeaponType.Panhard)
            GameManager.instance.CheckAmmo(this.gameObject,this, 0.5f);
        else
            GameManager.instance.CheckAmmo(this.gameObject,this, 2);
    }

    public void collided(GameObject dot)
    {
        for (int k = 0; k < numberOfDots; k++)
        {
            if (dot.name == "Dot (" + k + ")")
            {
                for (int i = k + 1; i < numberOfDots; i++)
                {

                    GameManager.instance.dots[i].gameObject.GetComponent<SpriteRenderer>().enabled = false;
                }
            }
        }
    }

    public void uncollided(GameObject dot)
    {
        for (int k = 0; k < numberOfDots; k++)
        {
            if (dot.name == "Dot (" + k + ")")
            {
                for (int i = k - 1; i > 0; i--)
                {
                    if (GameManager.instance.dots[i].gameObject.GetComponent<SpriteRenderer>().enabled == false)
                    {
                        return;
                    }
                }
                if (GameManager.instance.dots[k].gameObject.GetComponent<SpriteRenderer>().enabled == false)
                {
                    for (int i = k; i > 0; i--)
                    {
                        GameManager.instance.dots[i].gameObject.GetComponent<SpriteRenderer>().enabled = true;
                    }

                }
            }
        }
    }
}

