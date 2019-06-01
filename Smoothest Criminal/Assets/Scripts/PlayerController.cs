using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PhysicsObject
{
    public float maxSpeed = 7;
    public float jumpForce = 7;

    public SpriteRenderer sprite;
    Animator anim;

    float initGravityMult = 1;

    // dash
    public float dashMult = 2;
    public float dashLength = 0.5f;
    bool canDash = true;
    public bool isDashing = false;
    float dashVelocity;

    public float killZ = -10;

    bool prevGrounded = false;

    public bool canInput = true;
    public float h_input = 0;
    public bool doJump = false;
    public bool doDash = false;
    private bool doSword;
    private bool doPlaceExplosive;
    public bool doShoot = false;
    public bool doKick = false;
    public float facing = 1;

    bool canJump = true;

    public Vector2 startPos;
    Vector2 move = Vector2.zero;

    float deathFallTime = 6.5f;//1.5f;
    bool deathFall = false;
    public float fallTime = 0;

    bool dead = false;

    public GameObject blood;
    public GameObject bloodParticle;
    public GameObject soundBlast;
    public GameObject explosive;

    public LineRenderer gunLine;

    public bool invulnerable;
    

    public enum PlayerState
    {
        normal,
        jump,
        dash,
        hang,
        wallJump,
        fall,
        shootGround,
        shootAir,
        punch,
        kick,
        sword,
        death
    }

    public PlayerState state = PlayerState.normal;
    private float prevY;
    private bool doDetonate;

    public float actions = 1;
    public float startingActions = 1;

    private void Start()
    {
        anim = sprite.GetComponent<Animator>();
        startPos = transform.position;
        startingActions = actions;
    }

    public void Restart()
    {
        fallTime = 0;
        transform.position = startPos;
        actions = startingActions;
        dead = false;
        state = PlayerState.normal;
        deathFall = false;
        Debug.Log("RESTART: " + state);
        anim.SetBool("running", false);
        anim.SetBool("damaged", false);
        anim.SetBool("fall", false);
        anim.SetBool("hang", false);
        anim.SetTrigger("restart");
    }

    // instead of update
    protected override void ComputeVelocity()
    {
        //Debug.Log(state);
        switch (state)
        {
            case PlayerState.normal:
                UpdateNormal();
                break;

            case PlayerState.fall:
                break;

            case PlayerState.hang:
                UpdateHang();
                break;

            case PlayerState.dash:
                UpdateDash();
                break;

            case PlayerState.shootAir:
                UpdateShoot();
                break;

            case PlayerState.shootGround:
                UpdateShoot();
                break;

            case PlayerState.punch:
                break;
            case PlayerState.kick:
                UpdateKick();
                break;

            case PlayerState.wallJump:
                UpdateWallJump();
                break;
            case PlayerState.death:
                UpdateDead();
                break;

        }

        targetVelocity = move * maxSpeed;


        if (!prevGrounded && grounded)
        {
            // hit ground so squash down
            //squashAnim.SetTrigger("squash");
        }

        if (prevGrounded && !grounded)
        {
            // hit ground so squash down
            //squashAnim.SetTrigger("stretch");
        }
        prevGrounded = grounded;


    }

    public new void Update()
    {
        base.Update();

        if (grounded && !isDashing)
            canDash = true;

        if (transform.position.y < killZ)
        {

        }

        sprite.flipX = facing != 1;
    }


    void UpdateNormal()
    {
        invulnerable = false;

        h_input = Input.GetAxis("Horizontal");
        doJump = Input.GetButton("Jump");
        doKick = Input.GetButtonDown("Kick");
        doShoot = Input.GetButtonDown("Shoot") && LevelManager.instance.ammo > 0;
        doDash = Input.GetButtonDown("Dash");
        doPlaceExplosive = Input.GetButtonDown("Explosive") && FindObjectOfType<Explosive>() == null && LevelManager.instance.bombs > 0;
        doDetonate = Input.GetButtonDown("Explosive") && FindObjectOfType<Explosive>() != null;
        doSword = Input.GetButton("Sword") && LevelManager.instance.sword > 0;

        if (h_input != 0)
            facing = Mathf.Sign(h_input);

        anim.SetBool("running", h_input != 0);

        move = Vector2.zero;
        //float h_input = Input.GetAxis("Horizontal");
        /*if (!canInput)
            h_input = 0;*/

        move.x = h_input;// * facing;

        if (canInput && canJump && doJump && grounded)
        {
            velocity.y = jumpForce;
            canJump = false;
        }

        if(canInput && doDash && grounded)
        {
            state = PlayerState.dash;
            anim.SetTrigger("dash");
            Timer t = new Timer(0.35f, ReturnToNormal);
            GameManager.instance.AddTimer(t, gameObject);
        }

        if (canInput && doKick)
        {
            anim.SetTrigger("kick");
            state = PlayerState.kick;
            Timer t = new Timer(1, ReturnToNormal);
            GameManager.instance.AddTimer(t, gameObject);

            Timer tk = new Timer(0.3f, DoKick);
            GameManager.instance.AddTimer(tk, gameObject);
        }

        if(canInput && grounded && doPlaceExplosive)
        {
            LevelManager.instance.bombs -= 1;
            Vector3 offset = new Vector3(0, -0.35f, 0);
            Instantiate(explosive, transform.position + offset, Quaternion.Euler(Vector3.zero));
        }

        if(canInput && doDetonate)
        {
            foreach(Explosive e in FindObjectsOfType<Explosive>())
            {
                e.splode = true;
            }
        }

        if(canInput && doShoot)
        {
            LevelManager.instance.ammo -= 1;
            if (grounded)
            {
                state = PlayerState.shootGround;
                anim.SetTrigger("shoot");
                Timer tBullet = new Timer(0.5f, ShootGun);
                Timer tLine = new Timer(0.7f, DisableGunLine);
                Timer tNormal = new Timer(1.25f, ReturnToNormal);

                Debug.Log(tBullet.id);
                Debug.Log(tNormal.id);

                GameManager.instance.AddTimer(tBullet, gameObject);
                GameManager.instance.AddTimer(tLine, gameObject);
                GameManager.instance.AddTimer(tNormal, gameObject);
            }

            else
            {
                state = PlayerState.shootAir;
                anim.SetTrigger("shoot");
                Timer tBullet = new Timer(0.5f, ShootGunAir);
                Timer tLine = new Timer(0.7f, DisableGunLine);
                Timer tNormal = new Timer(1.25f, ReturnToNormal);

                Debug.Log(tBullet.id);
                Debug.Log(tNormal.id);

                GameManager.instance.AddTimer(tBullet, gameObject);
                GameManager.instance.AddTimer(tLine, gameObject);
                GameManager.instance.AddTimer(tNormal, gameObject);
            }
            
        }

        if(canInput && doSword)
        {
            if(grounded)
            {
                anim.SetTrigger("sword");
                state = PlayerState.sword;
                Timer t = new Timer(0.85f, ReturnToNormal);
                GameManager.instance.AddTimer(t, gameObject);
                move.x = 0;
            }
        }

        //stop jump
        else if (canInput && !doJump)
        {
            if (velocity.y > 0)
            {
                velocity.y = velocity.y * .5f;
            }
        }

        if (!doJump)
            canJump = true;

        if (!grounded)
        {

            RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0.5f * facing, 0), Vector2.right * facing, 0.01f);
            if (hit)
            {
                if (hit.transform.tag == "Ground")
                {
                    state = PlayerState.hang;
                    GameManager.instance.ReturnToNormalSpeed();
                    facing *= -1;
                }
            }
        }

        // falling to death
        if (!grounded)
        {
            fallTime += (Mathf.Abs(prevY - transform.position.y));
            //Debug.Log("FALLTIME: " + fallTime);
            if (fallTime > deathFallTime)
            {
                if(!deathFall && SfxManager.instance != null)
                    SfxManager.instance.DoScream();
                deathFall = true;
                anim.SetBool("fall", true);

                CameraManager.instance.CamEstablishingShot();
                GameManager.instance.speed = 0.5f;

                float duration = 2.0f;
                Timer t = new Timer(duration, CameraManager.instance.ReturnToNormal);
                GameManager.instance.AddTimer(t, gameObject);

                //Timer t2 = new Timer(duration * 0.5f, GameManager.instance.ReturnToNormalSpeed);
                //GameManager.instance.AddTimer(t2, gameObject);
            }

            // use an enemy to break your fall
            for (int i = 0; i < 5; i++)
            {
                Vector3 offset = new Vector2(-0.5f + (i * 0.25f), -1.25f);
                Debug.DrawLine(transform.position + offset, (transform.position + offset) + (Vector3.down * 0.5f));
                RaycastHit2D hit = Physics2D.Raycast(transform.position + offset, Vector2.down, 0.5f);
                if (hit)
                {
                    //Debug.Log("Hit " + hit.transform.name);
                    if (hit.transform.tag == "Enemy" && fallTime > deathFallTime)
                    {
                        if (SfxManager.instance != null)
                            SfxManager.instance.PlaySFX(SfxManager.instance.fallDeath);
                        hit.transform.GetComponent<Enemy>().Die();
                        fallTime = 0;
                        deathFall = false;
                        anim.SetBool("fall", false);
                        GameManager.instance.ReturnToNormalSpeed();
                    }
                }
            }

        }
        else
        {
            fallTime = 0;
            if (deathFall && !dead)
            {
                if (SfxManager.instance != null)
                    SfxManager.instance.PlaySFX(SfxManager.instance.fallDeath);
                GameManager.instance.ReturnToNormalSpeed();
                DieByFall();
                anim.SetTrigger("death");
            }
        }

        prevY = transform.position.y;

        anim.SetBool("grounded", grounded);
    }

    void DieByFall()
    {
        /*if (dead)
            return;*/

        FindObjectOfType<HUD>().deadText.SetActive(true);
        /*Instantiate(blood, transform.position + new Vector3(0, -0.75f, -0.1f), Quaternion.Euler(Random.Range(0, 360),
                                                                Random.Range(0, 360),
                                                                Random.Range(0, 360)));*/
        state = PlayerState.death;

        Instantiate(blood, transform.position + new Vector3(0, -0.75f, -0.1f), Quaternion.Euler(Random.Range(0, 360),
                                                                Random.Range(0, 360),
                                                                Random.Range(0, 360)));
    }

    public void Die()
    {
        /*if (dead)
            return;*/
        if (SfxManager.instance != null)
            SfxManager.instance.DoScream();
        anim.SetTrigger("hit");
        state = PlayerState.death;
        FindObjectOfType<HUD>().deadText.SetActive(true);

        Instantiate(blood, transform.position + new Vector3(0, -0.75f, -0.1f), Quaternion.Euler(Random.Range(0, 360),
                                                                Random.Range(0, 360),
                                                                Random.Range(0, 360)));

        List<string> idsToRemove = new List<string>();

        foreach(KeyValuePair<string, GameObject> kvp in GameManager.instance.registry)
        {
            if(kvp.Value == gameObject)
            {
                //GameManager.instance.registry.Remove(kvp.Key);
                idsToRemove.Add(kvp.Key);
            }
        }

        foreach(string s in idsToRemove)
        {
            GameManager.instance.registry.Remove(s);
        }

        CameraManager.instance.CamEstablishingShot();
        GameManager.instance.ReturnToNormalSpeed();
    }

    void ShootGun()
    {
        if (SfxManager.instance != null)
            SfxManager.instance.PlaySFX(SfxManager.instance.gunshot, true);
        //instantiate effect
        Instantiate(soundBlast, transform.position + new Vector3(0.5f * facing, 0), Quaternion.Euler(0, 0, 0));
        // alert others
        foreach (Enemy e in FindObjectsOfType<Enemy>())
        {
            if (e != this)
            {
                if (e.transform.position.y < transform.position.y + 2 &&
                    e.transform.position.y > transform.position.y - 2)
                {
                    if (Vector2.Distance(e.transform.position, transform.position) < 20)
                    {
                        e.suspicious = true;
                        e.suspiciousPoint = transform.position;
                    }
                }
            }
        }

        // do raycast
        gunLine.SetPosition(0, transform.position + new Vector3(0.5f * facing, -0.05f));
        gunLine.SetPosition(1, transform.position + new Vector3(100f * facing, -0.05f));
        gunLine.enabled = true;
        RaycastHit2D gunHit = Physics2D.Raycast(transform.position + new Vector3(0.5f * facing, 0), Vector2.right * facing, 100f);
        if(gunHit)
        {
            gunLine.SetPosition(1, gunHit.point + new Vector2(0, -0.05f));
            Debug.Log("Shot something");
            Debug.Log(gunHit.transform.name);
            if(gunHit.transform.tag == "Enemy")
            {
                CinematicZoom(1f, gunHit.transform);
                GameManager.instance.FreezeFrame();
                gunHit.transform.GetComponent<Enemy>().Die();
                Instantiate(bloodParticle, gunHit.transform.position, Quaternion.Euler(0, 0, 0));
            }
        }
    }

    void ShootGunAir()
    {
        if (SfxManager.instance != null)
            SfxManager.instance.PlaySFX(SfxManager.instance.gunshot, true);

        //instantiate effect
        Instantiate(soundBlast, transform.position + new Vector3(0.5f * facing, 0), Quaternion.Euler(0, 0, 0));
        // alert others
        foreach (Enemy e in FindObjectsOfType<Enemy>())
        {
            if (e != this)
            {
                if (e.transform.position.y < transform.position.y + 2 &&
                    e.transform.position.y > transform.position.y - 2)
                {
                    if (Vector2.Distance(e.transform.position, transform.position) < 20)
                    {
                        e.suspicious = true;
                        e.suspiciousPoint = transform.position;
                    }
                }
            }
        }

        // find nearest enemy
        float distanceToNearest = 9999;
        Enemy closestEnemy = null;

        foreach (Enemy e in FindObjectsOfType<Enemy>())
        {
            if(Vector2.Distance(transform.position, e.transform.position) < distanceToNearest &&
                e.state != Enemy.EnemyState.dead)
            {
                distanceToNearest = Vector2.Distance(transform.position, e.transform.position);
                closestEnemy = e;
            }
        }

        // auto-aim at closest enemy
        if(closestEnemy != null)
        {
            Vector3 targetPos = closestEnemy.transform.position;

            facing = targetPos.x > transform.position.x ? 1 : -1;

            Vector3 gunBarrel = transform.position + new Vector3(0.5f * facing, 0);
            
            gunLine.SetPosition(0, gunBarrel);

            var dir = targetPos - gunBarrel;
            gunLine.SetPosition(1, gunBarrel + dir * 100);

            //transform.rotation = Quaternion.LookRotation(dir, transform.up);
            //sprite.transform.right = -dir;

            RaycastHit2D gunHit = Physics2D.Raycast(gunBarrel, dir, 100f);

            gunLine.enabled = true;

            if (gunHit)
            {

                if (gunHit.transform.tag == "Enemy")
                {
                    CinematicZoom(1f, gunHit.transform);
                    GameManager.instance.FreezeFrame();
                    gunHit.transform.GetComponent<Enemy>().Die();
                    Instantiate(bloodParticle, gunHit.transform.position, Quaternion.Euler(0, 0, 0));
                }
            }

            
        }

        else
        {
            // do raycast
            gunLine.SetPosition(0, transform.position + new Vector3(0.5f * facing, 0));
            gunLine.SetPosition(1, transform.position + new Vector3(100f * facing, 0));
            gunLine.enabled = true;
            RaycastHit2D gunHit = Physics2D.Raycast(transform.position + new Vector3(0.5f * facing, 0), Vector2.right * facing, 100f);
            if (gunHit)
            {
                gunLine.SetPosition(1, gunHit.point);
                Debug.Log("Shot something");
                Debug.Log(gunHit.transform.name);
                if (gunHit.transform.tag == "Enemy")
                {
                    GameManager.instance.FreezeFrame();
                    gunHit.transform.GetComponent<Enemy>().Die();
                    Instantiate(bloodParticle, gunHit.transform.position, Quaternion.Euler(0, 0, 0));
                }
            }
        }
    }

    void DisableGunLine()
    {
        gunLine.enabled = false;
        sprite.transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    void DoKick()
    {

        RaycastHit2D kickHit = Physics2D.Raycast(transform.position + new Vector3(0.5f * facing, 0), Vector2.right * facing, 0.1f);

        if (kickHit)
        {
            if (SfxManager.instance != null)
                SfxManager.instance.PlaySFX(SfxManager.instance.hit, true);
            Debug.Log("HIT SOMETHING");
            Debug.Log(kickHit.transform.name);

            if (kickHit.transform.tag == "Enemy")
            {
                GameManager.instance.FreezeFrame();

                CinematicZoom(1.0f, kickHit.transform);
                
                Instantiate(bloodParticle, kickHit.transform.position, Quaternion.Euler(0, 0, 0));
                kickHit.transform.GetComponent<Enemy>().GetHit(Vector2.right * facing, 6);
            }
        }
    }

    void UpdateHang()
    {
        fallTime = 0;
        deathFall = false;
        anim.SetBool("fall", false);
        anim.SetBool("hang", true);
        doJump = Input.GetButton("Jump");
        

        /*if (doJump)
        {
            anim.SetBool("hang", false);
            anim.SetBool("running", true);
        }*/

        move = Vector2.zero;
        velocity.y = -1;
        GameManager.instance.speed = 1;

        if (canInput && doJump && canJump)
        {
            anim.SetBool("hang", false);
            velocity.y = jumpForce;
            move.x = jumpForce * 0.25f * facing;
            //state = PlayerState.normal;
            //facing = -facing;
            state = PlayerState.wallJump;
            Timer t = new Timer(0.1f, ReturnToNormal);
            GameManager.instance.AddTimer(t, gameObject);
            canJump = false;
        }

        if (canInput && doDetonate)
        {
            foreach (Explosive e in FindObjectsOfType<Explosive>())
            {
                e.splode = true;
            }
        }

        if (!doJump)
            canJump = true;

        if (grounded)
        {
            anim.SetBool("hang", false);
            state = PlayerState.normal;
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0.5f * -facing, 0), Vector2.right * facing, 0.1f);
        if (!hit)
        {
            anim.SetBool("hang", false);
            state = PlayerState.normal;
            /*if (hit.transform.tag == "Ground")
            {
                state = PlayerState.hang;
                facing *= -1;
            }*/
        }
    }

    void UpdateWallJump()
    { 
}

    void UpdateDash()
    {
        move.x = facing * 2;
        invulnerable = true;
        //GetComponent<BoxCollider2D>().enabled = false;
    }

    void UpdateDead()
    {
        move.x = 0;
    }

    void ReturnToNormal()
    {
        if (dead)
            return;

        anim.SetBool("hang", false);
        //anim.SetBool("dodge", false);
        state = PlayerState.normal;
        GetComponent<BoxCollider2D>().enabled = true;
        
    }

    void UpdateKick()
    {
        move = Vector2.zero;
        velocity = Vector2.zero;
    }

    void UpdateShoot()
    {
        move = Vector2.zero;
        velocity = Vector2.zero;
    }

    public void SwordHit()
    {
        if (SfxManager.instance != null)
            SfxManager.instance.PlaySFX(SfxManager.instance.sword, true);
        Debug.Log("SWORDHIT");
        RaycastHit2D swordHit = Physics2D.Raycast(transform.position + new Vector3(0.5f * facing, 0), Vector2.right * facing, 1);
        if (swordHit)
        {
            Debug.Log("SWORDHIT HIT");
            //Debug.Log("Sword Hit <" + swordHit.transform.name + ">");
            if (swordHit.transform.tag == "Enemy" &&
                swordHit.transform.GetComponent<Enemy>().state != Enemy.EnemyState.dead)
            {
                Debug.Log("HIT THE RIGHT THING");
                CinematicZoom(1f, swordHit.transform);
                Instantiate(bloodParticle, swordHit.point, Quaternion.Euler(0, 0, 0));
                GameManager.instance.FreezeFrame();
                swordHit.transform.GetComponent<Enemy>().Die();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.transform.tag == "Spikes")
        {
            if (collision.transform.parent.GetComponent<Spikes>() != null &&
                collision.transform.parent.GetComponent<Spikes>().on &&
                state != PlayerState.death)
            {
                Instantiate(bloodParticle, transform.position, Quaternion.Euler(0, 0, 0));
                Die();
            }

        }

        if (collision.transform.tag == "Explosion")
        {
            if (state != PlayerState.death)
            {
                Instantiate(bloodParticle, transform.position, Quaternion.Euler(0, 0, 0));
                GameManager.instance.FreezeFrame();
                Die();
            }

        }
    }

    void CinematicZoom(float duration, Transform target)
    {
        GameManager.instance.speed = 0.5f;
        CameraManager.instance.CamZoom();
        CameraManager.instance.SetDutch();

        if (target != null)
            CameraManager.instance.SetFollowTarget(target);

        if(duration != 0)
        {
            Timer t = new Timer(duration, CameraManager.instance.ReturnToNormal);
            GameManager.instance.AddTimer(t, gameObject);

            Timer t2 = new Timer(duration * 0.5f, GameManager.instance.ReturnToNormalSpeed);
            GameManager.instance.AddTimer(t2, gameObject);
        }
    }
}