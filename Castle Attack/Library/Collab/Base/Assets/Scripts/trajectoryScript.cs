using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class trajectoryScript : MonoBehaviour
{

    public GameObject CurrentCharacterPlayer;
    public SpriteRenderer ThisObject;
    public Sprite dotSprite;
    public bool changeSpriteAfterStart;
    public bool isTrack;
    public bool isThrown;
    public float initialDotSize;
    public int numberOfDots;
    public float dotSeparation;
    public float dotShift;
    public float idleTime;
    private GameObject trajectoryDots;
    private GameObject ball;
    private Rigidbody2D ballRB;
    public Vector3 ballPos;
    private Vector3 fingerPos;
    private Vector3 ballFingerDiff;
    public Vector2 shotForce;
    public float x1, y1;
    public float DotDiff;
    public bool ballIsClicked = false;
    public bool ballIsClicked2 = false;
    public bool HitGround = false;
    public GameObject ballClick;
    public float shootingPowerX;
    public float shootingPowerY;
    public bool usingHelpGesture;
    public bool explodeEnabled;
    public bool grabWhileMoving;
    public float MinDragDistance;
    public float draggedDistance;
    public float SHOTX;
    public float SHOTY;
    public bool mask;
    private BoxCollider2D[] dotColliders;
    public BoxCollider2D MachineryCollider;
    public Animator animator;
    public AudioSource CatapultSFXsource;
    public AudioClip[] CatapultSFXclips;
    [Header("[NEW]")]
    public Vector3 ResetSpawnPos;
    public GameObject NewArrow;
    public enum WeaponType { Rock, FireRock, Arrow, FireArrow, Cannon, M777A2 };
    public WeaponType weaponType;
    public enum AngleDistortion { Arrow = 36,  M777A2 = 90};
    public AngleDistortion angleDistortion;
    public bool isFire;
    private bool spawnNew = false;
    void Start()
    {
        ball = gameObject;

        // Turn on //For machinery
        if (weaponType == WeaponType.FireRock || weaponType == WeaponType.Rock)
        {
            MachineryCollider = GameObject.Find("Necessity").GetComponent<BoxCollider2D>();
            animator = GameObject.Find("Catapult_Rock_Animator").GetComponent<Animator>();
            animator.SetInteger("Catapult", 3);
        }
        else if (weaponType == WeaponType.FireArrow || weaponType == WeaponType.Arrow)
        {
            MachineryCollider = GameObject.Find("Necessity").GetComponent<BoxCollider2D>();
            animator = GameObject.Find("Catapult_Arrow_Animator").GetComponent<Animator>();
        }
        else if (weaponType == WeaponType.Cannon)
        {
            MachineryCollider = GameObject.Find("Necessity").GetComponent<BoxCollider2D>();
            animator = GameObject.Find("Catapult_Cannon_Animator").GetComponent<Animator>();
        }
        else if (weaponType == WeaponType.M777A2)
        {
            MachineryCollider = GameObject.Find("Necessity").GetComponent<BoxCollider2D>();
            animator = GameObject.Find("Modern_Era_Ammunition_Fire_S1").transform.parent.GetComponent<Animator>();
        }
        //ballClick = GameObject.Find("Ball Click Area");

        GameManager.instance.TrajectoryDots.SetActive(true);
        //CurrentCharacterPlayer = GameManager.instance.CharacterPlayer;
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
        if (weaponType == WeaponType.Arrow || weaponType == WeaponType.FireArrow)
        {
            angleDistortion = AngleDistortion.Arrow;
            if (!HitGround && isTrack)
                TrackMovement((int)angleDistortion);
        }
        else if (weaponType == WeaponType.M777A2)
        {
            angleDistortion = AngleDistortion.M777A2;
            if (!HitGround && isTrack)
                TrackMovement((int)angleDistortion);
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

                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                draggedDistance = Vector2.Distance(mousePos, ballRB.position);

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

                /*DotDiff = GameManager.instance.dots[1].transform.position.x - GameManager.instance.dots[0].transform.position.x;

                if (DotDiff < 2)
                {

                    for (int k = 0; k < numberOfDots; k++)
                    {
                        x1 = ballPos.x + shotForce.x * Time.fixedDeltaTime * (dotSeparation * k + dotShift * 0.1f);
                        y1 = ballPos.y + shotForce.y * Time.fixedDeltaTime * (dotSeparation * k + dotShift * 0.1f) - (-Physics2D.gravity.y / 2f * Time.fixedDeltaTime * Time.fixedDeltaTime * (dotSeparation * k + dotShift) * (dotSeparation * k + dotShift)); //Y position for each point is found


                        GameManager.instance.dots[k].transform.position = new Vector3(x1, y1, GameManager.instance.dots[k].transform.position.z);
                    }
                }
                else
                {
                    x1 = 170f;
                    y1 = -180f;
                    for (int k = 0; k < numberOfDots; k++)
                    {
                        GameManager.instance.dots[k].transform.position = new Vector3(x1, y1, GameManager.instance.dots[k].transform.position.z);
                    }
                }*/

                for (int k = 0; k < numberOfDots; k++)
                {
                    x1 = ballPos.x + shotForce.x * Time.fixedDeltaTime * (dotSeparation * k + dotShift * 0.1f);
                    y1 = ballPos.y + shotForce.y * Time.fixedDeltaTime * (dotSeparation * k + dotShift * 0.1f) - (-Physics2D.gravity.y / 2f * Time.fixedDeltaTime * Time.fixedDeltaTime * (dotSeparation * k + dotShift) * (dotSeparation * k + dotShift)); //Y position for each point is found
                    GameManager.instance.dots[k].transform.position = new Vector3(x1, y1, GameManager.instance.dots[k].transform.position.z);
                }

                SHOTX = shotForce.x;
                SHOTY = shotForce.y;

                if (SHOTX > 16.5f)//16.5
                {
                    shotForce.x = 16.5f;//16.5
                    SHOTX = shotForce.x;
                }
                if (SHOTY > 16.5f)//16.5
                {
                    shotForce.y = 16.5f;
                    SHOTY = shotForce.y;
                }

                if (x1 > 177)
                {
                x1 = 170f;
                y1 = -180f;
                }
            }
            if (this.shotForce.x <= 1)
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
       /* if (MachineryCollider != null)
            MachineryCollider.enabled = false;*/
        if (weaponType == WeaponType.FireRock || weaponType == WeaponType.Rock)
        {
            StartCoroutine(ThrowBall(0.15f,2));
        }
        else if (weaponType == WeaponType.FireArrow || weaponType == WeaponType.Arrow)
        {
            StartCoroutine(ThrowBall(.5f,0));
        }
        else if (weaponType == WeaponType.Cannon || weaponType == WeaponType.M777A2)
        {
            StartCoroutine(ThrowBall(0.5f,0));
        }
    }
    
    void InitiateTrack()
    {
        isTrack = true;

        //if (MachineryCollider != null)
        //    MachineryCollider.enabled = false;
    }

    void TrackMovement(int AngleDisturbance)
    {
        // All Arrows = (36) , M777A2 = (90)

        Vector2 direction = ballRB.velocity;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - AngleDisturbance, Vector3.forward);
    }

    public IEnumerator ThrowBall(float delay,int value)
    {
        isThrown = true;
        trajectoryDots.SetActive(false);
        animator.SetInteger("Catapult", 1);
        CatapultSFXsource.clip = CatapultSFXclips[0];
        CatapultSFXsource.Play();

        if (weaponType == WeaponType.FireRock || weaponType == WeaponType.Rock)//Rock
        {
            GameManager.instance.IsDustParticles = true;
            yield return new WaitForSeconds(delay);
        }

        CharacterAttack.instance.GamePlayThrowAnimation();
        ThisObject.enabled = true;
        if (isFire)
        {
            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(2).gameObject.SetActive(true);
        }
        Invoke("InitiateTrack", 0f);

        if (weaponType == WeaponType.Cannon)//Canon
        {
            yield return new WaitForSeconds(delay/10);

            Instantiate(EnemyManager.insance.DustSpawn,
                GameManager.instance.ThrowableSpawnPosition[(int)MachineryManager.instance._WeaponType].position,
                GameManager.instance.ThrowableSpawnPosition[(int)MachineryManager.instance._WeaponType].rotation);

            yield return new WaitForSeconds(delay/5);
        }

        ballRB.velocity = new Vector2(shotForce.x, shotForce.y);

        if (weaponType == WeaponType.FireArrow || weaponType == WeaponType.Arrow)//Arrow
        {
            yield return new WaitForSeconds(delay);
        }

        if (ballRB.isKinematic == true)
        {
            ballRB.isKinematic = false;
        }

        animator.SetInteger("Catapult", value);

        if (weaponType == WeaponType.FireRock || weaponType == WeaponType.Rock)//Rock
        {
            yield return new WaitForSeconds(0.15f);
        }

        /*if (MachineryCollider != null)      
            MachineryCollider.enabled = true;*/
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
        if (collision.gameObject.tag == "Ground")// || (collision.gameObject.CompareTag("Catapult") && isThrown))
        {
            Blast(collision);
            HitGround = true;
            DestroyPreviousArrow();  
        }
        else if (collision.gameObject.tag == "Castle")
        {
            HitGround = true;
            GameManager.instance.currentCastleHP -= MachineryManager.instance.machineryDamage;
            DestroyPreviousArrow();

             if (GameManager.instance.currentCastleHP<=0 && !GameManager.instance.GameOver)
            {
                Blast(collision);
                GameManager.instance.PlayerScore = GameManager.instance.PlayerScore + 50;

                GameManager.instance.GameOver = true;
                for (int i = 0; i < GameManager.instance.Flame.Length - 1; i++)
                {
                    GameManager.instance.Flame[i].transform.localScale = new Vector3(6.2f, 6.2f, 6.2f);
                }
                GameManager.instance.Flame[6].SetActive(true);
                GameManager.instance.Victory(0);

                EnemyManager.insance.StopEnemyShooting();

                GameManager.instance.CallBonusRound();
                
                 if (GameManager.instance.Player != null)
                {
                    if (weaponType == WeaponType.M777A2)
                    {
                        print("Hello");
                        Destroy(GameManager.instance.Player.transform.parent.gameObject);
                    }
                    else
                    {
                        Destroy(GameManager.instance.Player);
                    }
                }
                trajectoryDots.SetActive(false);//new
                return;
            }
             else if (GameManager.instance.currentCastleHP > 0 && !GameManager.instance.GameOver && !GameManager.instance.CHK_GAME_WIN)
            {
                float temp = ((float)MachineryManager.instance.machineryDamage / GameManager.instance.levelCastleHP);
                GameManager.instance.CastleHealthFillbar.fillAmount -= ((float)MachineryManager.instance.machineryDamage / GameManager.instance.levelCastleHP);
                GameManager.instance.PlayerScore = GameManager.instance.PlayerScore + 50;

                Blast(collision);
            }
            if (GameManager.instance.CastleHealthFillbar.fillAmount < 0.5f && GameManager.instance.CastleHealthFillbar.fillAmount > 0.35f)
            {
                for (int i = 0; i < GameManager.instance.Flame.Length - 1; i++)
                {
                    GameManager.instance.Flame[i].SetActive(true);
                }
            }
            else if (GameManager.instance.CastleHealthFillbar.fillAmount < 0.35f && GameManager.instance.CastleHealthFillbar.fillAmount > 0.25f)
            {
                for (int i = 0; i < GameManager.instance.Flame.Length - 1; i++)
                {
                    GameManager.instance.Flame[i].transform.localScale = new Vector3(4.5f, 4.5f, 4.5f);
                }
            }
            else if (GameManager.instance.CastleHealthFillbar.fillAmount < 0.25f)
            {
                for (int i = 0; i < GameManager.instance.Flame.Length - 1; i++)
                {
                    GameManager.instance.Flame[i].transform.localScale = new Vector3(6.2f, 6.2f, 6.2f);
                }
            }
        }
    }

    public void Blast(Collision2D collision)
    {
        //Debug.Log(collision.gameObject.name + "COLLISION");
        ContactPoint2D contact = collision.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 pos = contact.point;

        if (collision.gameObject.CompareTag("Castle") || collision.gameObject.CompareTag("Ground"))
            Instantiate(GameManager.instance.BlastPrefab, pos, rot);
    }
    void DestroyPreviousArrow()
    {
        GameManager.instance.IsDustParticles = false;

        if(GameManager.instance.TotalAmmo==0)
        {
            print("Ammo is 0");
            EnemyManager.insance.StopEnemyShooting();

            if (GameManager.instance.currentCastleHP <= 0 && !GameManager.instance.CHK_GAME_WIN)
            {
                print("Victory");
                GameManager.instance.Victory(0);
            }
            else if (!GameManager.instance.CHK_GAME_WIN)
            {
                GameManager.instance.GameOver = true;
                Debug.Log("GAMMOVOER");
                GameManager.instance.Defeat();
            }
            return;
        }
        this.enabled = false;
        GameManager.instance.CheckForPlayerNull();
        
        if (weaponType == WeaponType.M777A2)
        {
            print(1);
            this.gameObject.transform.parent.gameObject.SetActive(false);
        }
        else 
        {
            Destroy(this.gameObject);
        }
       
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
                        //Debug.Log("nigggssss");
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

