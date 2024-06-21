using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 7;
    public float normal_speed = 7;
    public Rigidbody2D rb;
    public float moveInput;
    public float jumpForce = 5;
    public bool isgrounded = true; // bool - eto *true* ili zhe *false*
    public LayerMask ground; // LayerMask - eto kakoyto sloy
    public Transform foot; // transform - eto kordinata, razmer i ugol povorota
    public float checkradius = 0.21f;
    public bool MovingLeft = false;
    public Animator anim;
    public int Coins = 0;
    public PlayerUI UI;
    public AudioSource Source;
    public AudioClip Walk;
    public AudioClip Jump;
    public AudioClip EatingNuggets;
    public bool walkSound = false;
    public bool IsLadder = false;
    public int HP = 100;
    public int MaxHeight = 4;
    public float MaxY;
    public float LastY;
    public float MaxHP = 100;
    public float DamageDelay = 0.75f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        foot = transform.GetChild(1); // GetChild - zaprashivaet vtoroy dacherniy element (tot kotoriy vnutri hierarchy)
        UI = GetComponent<PlayerUI>();
        Source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        checkground();
        if (transform.position.y < LastY&&MaxY<LastY)
        {
            MaxY = transform.position.y;
        }
        LastY = transform.position.y;
 
    if (MaxY-LastY>MaxHeight&&isgrounded)
        {
            int damage = ((int)(MaxY - Mathf.Abs(LastY)) - 7) * 7; // Mathf.Abs - tozhe samoe chislo, tolko bez znaka minus
            if (damage > 0)
            {
                HP -= damage;
                MaxY = 0;
                UI.ChangeHP();
                CheckDeath();
            }
            
        }
    }
    
    public void MovePlayer()
    {
        moveInput = Input.GetAxisRaw("Horizontal"); // Input.GetAxisRaw - zaprashivaet znachtnie osi (nazvanie osi pishetsya v kruglih skobkah)
        rb.velocity = new Vector2 (moveInput * speed, rb.velocity.y);
        if (Input.GetKey(KeyCode.Space)&&isgrounded == true&&!IsLadder) // !IsLadder - tozhe samoe chto i Ladder = False
        {
            rb.velocity = Vector2.up * 5;
            Source.clip = Jump;
            Source.Play();
        }
        if (Input.GetKey(KeyCode.Space)&&IsLadder) // isladder tozhe samoe chto isladder = true
        {
            rb.velocity = new Vector2(rb.velocity.x, speed);
        }
        if ((moveInput > 0&&MovingLeft)||(moveInput < 0&&!MovingLeft)) // && - logicheskoe *i*. oba usloviya dolzhni bit pravdoy
                                                                       // || - logik *ili* hotyzbi odno uslovie dolzhno bit pravdoy // ! - logicheskoe otricanie
        {
            Vector3 scale = transform.localScale; // localscale - razmer objecta
            scale.x *= -1;
            transform.localScale = scale;
            MovingLeft = !MovingLeft;
        }
        if (moveInput == 0)
        {
            anim.SetBool("Walk", false);// SetBool - vistavlyaet opridelyonnoy peremonnoy iz animatora nuzhnoe znachenie ("Nazvanie peremennoy iz animatora", znachenie)
            if (walkSound)
            {
                Source.Stop();
                walkSound = false;
            }
        }
        else
        {
            anim.SetBool("Walk", true);
            if (!walkSound)
            {
                Source.clip = Walk;
                Source.Play();
                walkSound = true;
            }
        }
    }
    public void checkground ()
    {
        isgrounded = Physics2D.OverlapCircle(foot.position, checkradius, ground); // Physics2D.OverlapCircle(zentr kruga, radius kruge, sloy) - sozdaet krug s opridelennim radiusom kotoriy ishet vse colideri na opridelennom sloye
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Spikes"))
        {
            StartCoroutine(SpikeDamage());
        }
        if (other.CompareTag("Coin"))
        {
            Coins++;
            Source.clip = EatingNuggets;
            Source.Play();
            UI.CoinText.text = "Nuggets: "+Coins.ToString();
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Ladder"))
        {
            IsLadder = true;
        }
        if (other.CompareTag("Spring") && !isgrounded)
        {
            rb.velocity = Vector2.up * 14;
        } 
        if (other.CompareTag("Bush"))
        {
            MaxY = transform.position.y;
            HideBush(true, other.gameObject.transform);
        }
    }
    public void OnTriggerExit2D(Collider2D collision) // OnTriggerExit2D - eto kogda ti vihodish iz kakogoto triggera
    {
        if (collision.CompareTag("Spikes"))
        {
            StopCoroutine(SpikeDamage());
            speed = normal_speed;
        }
        if (collision.CompareTag("Ladder"))
        {
            IsLadder = false;
        }
        if (collision.CompareTag("Bush"))
        {
            HideBush(false, collision.gameObject.transform);
        }    
    }
    public void CheckDeath()
    {
        if (HP <= 0)
        {
            //Destroy(gameObject);
        }
    }
    public void HideBush(bool Hide, Transform Bush)
    {
        foreach (Transform b in Bush)
        {
            Color c = b.gameObject.GetComponent<SpriteRenderer>().color;

            if (Hide)
            {
                c = new Color(c.r, c.g, c.b, 0.25f);
            }
            else
            {
                c = new Color(c.r, c.g, c.b, 1);
            }
            b.gameObject.GetComponent<SpriteRenderer>().color = c;
        }
    }
    public IEnumerator SpikeDamage()
    {
        speed = 2.75f;
        while (true)
        {
            HP -= 15;
            yield return new WaitForSeconds(DamageDelay);
        }
    }
}
