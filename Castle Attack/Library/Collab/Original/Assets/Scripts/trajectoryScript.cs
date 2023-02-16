using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class trajectoryScript : MonoBehaviour {

    public GameObject CurrentCharacterPlayer;
    public SpriteRenderer ThisObject;
    public Sprite dotSprite;
    public bool changeSpriteAfterStart;
    public bool isTrack;
    public  bool isThrown;
    public float initialDotSize;
    public int numberOfDots;
    public float dotSeparation;
    public float dotShift;
    public float idleTime;
    private GameObject trajectoryDots;
    private GameObject ball;
    private Rigidbody2D ballRB;
    private Vector3 ballPos;
    private Vector3 fingerPos;
    private Vector3 ballFingerDiff;
    private Vector2 shotForce;
    private float x1, y1;
    private GameObject helpGesture;
    private float idleTimer = 7f;
    private bool ballIsClicked = false;
    private bool ballIsClicked2 = false;
    public bool HitGround = false;
    public GameObject ballClick;
    public float shootingPowerX;
    public float shootingPowerY;
    public bool usingHelpGesture;
    public bool explodeEnabled;
    public bool grabWhileMoving;
    public float MinDragDistance;
    public float draggedDistance;
    public bool mask;
    private BoxCollider2D[] dotColliders;
    public BoxCollider2D MachineryCollider;
    public Animator animator;
    public AudioSource CatapultSFXsource;
    public AudioClip[] CatapultSFXclips;
    [Header("[NEW]")]
    public Vector3 ResetSpawnPos;
    public GameObject NewArrow;
    string xx;
    public enum WeaponType { Rock, FireRock, Arrow, FireArrow, Cannon };
    public WeaponType weaponType;
    public bool isFire;

    void Start() {

        //gameObject.layer = LayerMask.NameToLayer("YourLayerName");
        ball = gameObject;

        // Turn on
        if (weaponType == WeaponType.FireRock || weaponType == WeaponType.Rock)
        {
            MachineryCollider = GameObject.Find("Necessity").GetComponent<BoxCollider2D>();
            animator = GameObject.Find("Catapult_Rock_Animator").GetComponent<Animator>();
        }
        else if (weaponType == WeaponType.FireArrow || weaponType == WeaponType.Arrow)
        {
            MachineryCollider = GameObject.Find("Necessity").GetComponent<BoxCollider2D>();
            animator = GameObject.Find("Catapult_Arrow_Animator").GetComponent<Animator>();
        }
        else if (weaponType == WeaponType.Cannon)
        {
            animator = GameObject.Find("Catapult_Cannon_Animator").GetComponent<Animator>();
        }
        //ballClick = GameObject.Find("Ball Click Area");
        GameManager.instance.TrajectoryDots.SetActive(true);
        //CurrentCharacterPlayer = GameManager.instance.CharacterPlayer;
        trajectoryDots = GameObject.Find("Trajectory Dots");
        ballRB = GetComponent<Rigidbody2D>();

        trajectoryDots.transform.localScale = new Vector3(initialDotSize, initialDotSize, trajectoryDots.transform.localScale.z);

        for (int k = 0; k < 40; k++) {
            GameManager.instance.dots[k] = GameObject.Find("Dot (" + k + ")");
            if (dotSprite != null) {
                GameManager.instance.dots[k].GetComponent<SpriteRenderer>().sprite = dotSprite;
            }
        }
        for (int k = numberOfDots; k < 40; k++) {
            GameObject.Find("Dot (" + k + ")").SetActive(false);
        }
        trajectoryDots.SetActive(false);
    }

    void Update() {

        if (weaponType == WeaponType.Arrow || weaponType == WeaponType.FireArrow)
        {
            if (!HitGround && isTrack)
                TrackMovement();
        }
        if (!isThrown)
        {


            //if (numberOfDots > 40) {
            //	numberOfDots = 40;
            //}
            numberOfDots = GameManager.instance.dots.Length;

            


            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);


            if (hit.collider != null && ballIsClicked2 == false)
            {
                if (hit.collider.gameObject.name == ballClick.gameObject.name)
                {
                    ballIsClicked = true;
                }
                else
                {
                    ballIsClicked = false;
                }
            }
            else
            {
                ballIsClicked = false;
            }

            if (ballIsClicked2 == true)
            {
                ballIsClicked = true;
            }



            if ((ballRB.velocity.x * ballRB.velocity.x) + (ballRB.velocity.y * ballRB.velocity.y) <= 0.0085f)
            {
                ballRB.velocity = new Vector2(0f, 0f);
                idleTimer -= Time.deltaTime;

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
                Debug.Log("draggedDistanceDragged" + draggedDistance);


                Debug.Log("Mouse0GetKey");
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

                shotForce = new Vector2(ballFingerDiff.x * shootingPowerX * -1, ballFingerDiff.y * shootingPowerY * -1);

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

                for (int k = 0; k < numberOfDots; k++)
                {
                    x1 = ballPos.x + shotForce.x * Time.fixedDeltaTime * (dotSeparation * k + dotShift);
                    y1 = ballPos.y + shotForce.y * Time.fixedDeltaTime * (dotSeparation * k + dotShift) - (-Physics2D.gravity.y / 2f * Time.fixedDeltaTime * Time.fixedDeltaTime * (dotSeparation * k + dotShift) * (dotSeparation * k + dotShift)); //Y position for each point is found
                    GameManager.instance.dots[k].transform.position = new Vector3(x1, y1, GameManager.instance.dots[k].transform.position.z);
                }
            }


            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                Invoke("SpawnReset", 0.5f);
                if (draggedDistance > MinDragDistance)
                {

                }
                else
                {
                    //this.enabled = false;
                    ballIsClicked2 = false;
                    trajectoryDots.SetActive(false);
                    GameManager.instance.fingertouch = false;
                    return;
                }
                Debug.Log("Mouse0GetKeyUp");
                GameManager.instance.fingertouch = false;
                ballIsClicked2 = false;

                if (GameManager.instance.TrajectoryDots.activeInHierarchy)
                {
                    if (explodeEnabled == true)
                    {
                        StartCoroutine(explode());
                    }
                    Debug.Log("Mouse1GetKeyUp");

                }
                if (MachineryCollider != null)
                    MachineryCollider.enabled = false;
                if (weaponType == WeaponType.FireRock || weaponType == WeaponType.Rock)
                {
                    StartCoroutine(ThrowRock());
                }
                else if (weaponType == WeaponType.FireArrow || weaponType == WeaponType.Arrow )
                {
                    StartCoroutine(ShootArrow());
                }
                else if (weaponType == WeaponType.Cannon)
                {
                    StartCoroutine(ShootCanon()); 
                }
            }
        }
    }

    void SpawnReset()
    {
        GameManager.instance.SpawnNew = true;
    }
    void InitiateTrack()
    {
        isTrack = true;

        if(MachineryCollider != null)
        MachineryCollider.enabled = false;
    }

    void TrackMovement()
    {
        Debug.Log("TRACKINGMOVEMENT");
        Vector2 direction = ballRB.velocity;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 36, Vector3.forward);
    }
    public IEnumerator ThrowRock()
    {
        isThrown = true;
        GameManager.instance.IsDustParticles = true;
        trajectoryDots.SetActive(false);
        animator.SetInteger("Catapult", 1);
        CatapultSFXsource.clip = CatapultSFXclips[0];
        CatapultSFXsource.Play();
        yield return new WaitForSeconds(0.15f);
        CharacterAttack.instance.GamePlayThrowAnimation();
        ThisObject.enabled = true;
        if (isFire)
        {
            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(2).gameObject.SetActive(true);
        }
        Invoke("InitiateTrack", 0f);
        ballRB.velocity = new Vector2(shotForce.x, shotForce.y);

        if (ballRB.isKinematic == true)
        {
            ballRB.isKinematic = false;
        }
        
        animator.SetInteger("Catapult", 2);
        yield return new WaitForSeconds(0.15f);
        
        animator.SetInteger("Catapult", 3);

        if (MachineryCollider != null)
            MachineryCollider.enabled = true;
        GameManager.instance.SpawnNew = true;
        Debug.Log("GAMEPLAYTHROWANIMATION");
        
    }

    public IEnumerator ShootArrow()
    {
        Debug.Log("SHOOTARROW");
        isThrown = true;
        trajectoryDots.SetActive(false);
        animator.SetInteger("Catapult", 1);
        CatapultSFXsource.clip = CatapultSFXclips[0];
        CatapultSFXsource.Play();
        CharacterAttack.instance.GamePlayThrowAnimation();
        ThisObject.enabled = true;
        if (isFire)
        {
            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(2).gameObject.SetActive(true);
        }
        Invoke("InitiateTrack", 0f);
        ballRB.velocity = new Vector2(shotForce.x, shotForce.y);
        yield return new WaitForSeconds(0.5f);

        if (ballRB.isKinematic == true)
        {
            ballRB.isKinematic = false;
        }
        
        animator.SetInteger("Catapult", 0);

        if (MachineryCollider != null)
            MachineryCollider.enabled = true;
        GameManager.instance.SpawnNew = true;
        Debug.Log("GAMEPLAYTHROWANIMATION");

    }

    public IEnumerator ShootCanon()
    {
        Debug.Log("ShootCanon");
        isThrown = true;
        trajectoryDots.SetActive(false);
        animator.SetInteger("Catapult", 1);
        CatapultSFXsource.clip = CatapultSFXclips[0];
        CatapultSFXsource.Play();

        CharacterAttack.instance.GamePlayThrowAnimation();
        ThisObject.enabled = true;
        if (isFire)
        {
            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(2).gameObject.SetActive(true);
        }
        Invoke("InitiateTrack", 0f);
        yield return new WaitForSeconds(.05f);
        ballRB.velocity = new Vector2(shotForce.x, shotForce.y);
        yield return new WaitForSeconds(.5f);
        if (ballRB.isKinematic == true)
        {
            ballRB.isKinematic = false;
        }

        animator.SetInteger("Catapult", 0);

        if (MachineryCollider != null)
            MachineryCollider.enabled = true;
        GameManager.instance.SpawnNew = true;
        Debug.Log("GAMEPLAYTHROWANIMATION");

    }

    public IEnumerator explode() {
        yield return new WaitForSeconds(Time.fixedDeltaTime * (dotSeparation * (numberOfDots - 1f)));
        Debug.Log("exploded");
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if(collision.gameObject.tag == "Ground")


        if (collision.gameObject.tag == "Ground")
        {
            Blast(collision);
            HitGround = true;
            DestroyPreviousArrow();
        }
        else if (collision.gameObject.tag == "Castle" && !GameManager.instance.GameOver)
        {
            HitGround = true;
            DestroyPreviousArrow();

            GameManager.instance.currentCastleHP -= MachineryManager.instance.machineryDamage;

            if (GameManager.instance.currentCastleHP > 0)
            //if (GameManager.instance.CastleHealthFillbar.fillAmount > 0.11f)
            {
                float temp = ((float)MachineryManager.instance.machineryDamage / GameManager.instance.levelCastleHP);
                Debug.LogError(temp + " -"+ MachineryManager.instance.machineryDamage +" / "+ GameManager.instance.levelCastleHP);
                GameManager.instance.CastleHealthFillbar.fillAmount -= ((float)MachineryManager.instance.machineryDamage/GameManager.instance.levelCastleHP);
                GameManager.instance.PlayerScore = GameManager.instance.PlayerScore + 15;
                Blast(collision);
            }
            else
            {
                for (int i = 0; i < GameManager.instance.Flame.Length - 1; i++)
                {
                    GameManager.instance.Flame[i].transform.localScale = new Vector3(6.2f, 6.2f, 6.2f);
                }
                GameManager.instance.Flame[6].SetActive(true);
                GameManager.instance.Victory(0);

                EnemyManager.insance.StopEnemyShooting();
                GameManager.instance.GameOver = true;

                GameManager.instance.CallBonusRound();
                return;
                //Victory
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
        ContactPoint2D contact = collision.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 pos = contact.point;

        if (collision.gameObject.CompareTag("Castle") || collision.gameObject.CompareTag("Ground"))
            Instantiate(GameManager.instance.BlastPrefab, pos, rot);
    }
	void DestroyPreviousArrow()
    {
        
        GameManager.instance.IsDustParticles = false;
        animator.SetInteger("Catapult", 3);
        if (GameManager.instance.TotalAmmo == 0)
            return;
        this.enabled = false;
        Destroy(this.gameObject);
    }
    public void collided(GameObject dot){

		for (int k = 0; k < numberOfDots; k++) {
			if (dot.name == "Dot (" + k + ")") {
				
				for (int i = k + 1; i < numberOfDots; i++) {

                    GameManager.instance.dots [i].gameObject.GetComponent<SpriteRenderer> ().enabled = false;
				}

			}

		}
	}
	public void uncollided(GameObject dot){
		for (int k = 0; k < numberOfDots; k++) {
			if (dot.name == "Dot (" + k + ")") {

				for (int i = k-1; i > 0; i--) {
				
					if (GameManager.instance.dots [i].gameObject.GetComponent<SpriteRenderer> ().enabled == false) {
						Debug.Log ("nigggssss");
						return;
					}
				}

				if (GameManager.instance.dots [k].gameObject.GetComponent<SpriteRenderer> ().enabled == false) {
					for (int i = k; i > 0; i--) {

                        GameManager.instance.dots [i].gameObject.GetComponent<SpriteRenderer> ().enabled = true;

					}

				}
			}

		}
	}
}

