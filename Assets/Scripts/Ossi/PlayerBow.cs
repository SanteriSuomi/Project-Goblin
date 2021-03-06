using System.Collections;
using UnityEngine;

public class PlayerBow : MonoBehaviour
{
    [SerializeField]
    KeyCode fireButton = KeyCode.Mouse0;

    [SerializeField]
    Animator bowAnimator;
    [SerializeField]
    Animator playerAnimator;

    AudioManager audioManager;

    public Transform firePoint;
    public Transform aim;
    public GameObject bulletPrefab;
    private Rigidbody bulletRb;
    public LayerMask mouseAimMask;
    Animator anim;

    public Vector3 mousePoint;

    public float chargeTime;
    public float speed;

    //private float nextFire = 0f;

    bool shot = false;
    public bool IsCharging { get; set; }

    private Arrow arrow;

    void Awake()
    {
        speed = 5f;
        chargeTime = 0f;
        arrow = bulletPrefab.GetComponent<Arrow>();
        anim = GetComponent<Animator>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    void Update()
    {
        Aim();
        if (Input.GetKey(fireButton))
        {	
        	if(Input.GetKeyDown(fireButton)) {
        		audioManager.Play("BowLoad");
        	}
            chargeTime += Time.deltaTime;
            bowAnimator.SetTrigger("Shoot");
            playerAnimator.SetTrigger("Shoot");
            bowAnimator.SetFloat("DrawDist", chargeTime);
            playerAnimator.SetFloat("DrawDist", chargeTime);
            IsCharging = true;
            shot = false;
        }
        else
        {
            IsCharging = false;
        }

        if (Input.GetKeyUp(fireButton) && (chargeTime > 0.6f))
        {
            if (chargeTime > 1.5f)
            {
                speed = speed * 6f;
                arrow.speed = speed;
                shoot();
            }
            else
            {
                speed = speed * (chargeTime * 4);
                arrow.speed = speed;
                shoot();
            }
            StartCoroutine(nameof(ShotCountdown));
        }
        else if(Input.GetKeyUp(fireButton)) {
        	playerAnimator.SetTrigger("Interrupt");
        	shot = true;
        }

        if (shot)
        {
            chargeTime = 0f;
            speed = 5f;
        }
    }

    IEnumerator ShotCountdown()
    {
        yield return new WaitForSeconds(0.20f);
        IsCharging = false;
    }

    void shoot()
    {
        bowAnimator.SetTrigger("Shot");
        playerAnimator.SetTrigger("Shot");
        audioManager.Play("BowFire");
        Instantiate(bulletPrefab, firePoint.position, aim.rotation);
        shot = true;
    }

    void Aim()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mouseAimMask))
        {
            mousePoint = hit.point;
            mousePoint.z = 0;
            aim.LookAt(mousePoint);
        }
    }

    /*private void OnAnimatorIK() {
    	anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
    	anim.SetIKPosition(AvatarIKGoal.LeftHand, mousePoint);
    }*/
}
