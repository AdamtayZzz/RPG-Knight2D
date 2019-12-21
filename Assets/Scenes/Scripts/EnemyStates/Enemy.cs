using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    //public Transform feet;
    public GameObject enemy;
    public GameObject feet;
    private Animator anim;
    public Knight pc;
    public Transform moveSpot;
    private Transform target;
    //sprite属性
    private SpriteRenderer spr_pc;
    private SpriteRenderer spr_enemy;
    //敌人和角色得属性
    private int health = 20;
    private int damage_pc;
    private int defend_pc;
    private int damage_enemy;
    private int defend_enemy;
    private bool alive = true;
    //间隔时间
    private float timer_attack = 2.0f;
    private float timer_color = 0.5f;
    private float timer_rebirth = 1.0f;
    //A*寻路插件
    public AIPath aipath;
    public Transform Target
    {
        get
        {
            return target;
        }
        set
        {
            target = value;
        }

    }

    private void Start()
    {
        Initalization();
        anim = GetComponent<Animator>();
        spr_pc = pc.GetComponent<SpriteRenderer>();
        spr_enemy = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (alive)
        {
            Timer();
            Move();
            Change_direction();
            Attack();
            Hited();
        }
        EnemyHealthBar.instance.UpdateBar(health, 20);
    }
    /// <summary>
    /// 初始化属性
    /// </summary>
    void Initalization()
    {
        //角色属性
        damage_pc = pc.attack;
        defend_pc = pc.defend;

        //敌人属性每一关攻击力和防御各不同
        int location = SceneManager.GetActiveScene().buildIndex;
        switch (location)
        {
            case 2:
                damage_enemy = 10;
                defend_enemy = 5;
                break;
            case 3:
                damage_enemy = 15;
                defend_enemy = 8;
                break;
            case 4:
                damage_enemy = 20;
                defend_enemy = 10;
                break;
            default:
                break;
        }

    }
    /// <summary>
    /// 判断敌人朝向
    /// </summary>
    void Change_direction()
    {
        float a, b;
        a = moveSpot.position.x - this.transform.position.x;
        b = moveSpot.position.y - this.transform.position.y;
        if (a < 0 && b <= 0 && a / b > 1)
        {
            anim.SetFloat("Look_x", -1);
            anim.SetFloat("Look_y", 0);
        }
        else
        if ((a > 0 && b > 0 && a / b > 1) || (a > 0 && b == 0))
        {
            anim.SetFloat("Look_x", 1);
            anim.SetFloat("Look_y", 0);
        }
        else
            if (a > 0 && b > 0 && a / b >= 0 && a / b <= 1)
        {
            anim.SetFloat("Look_y", 1);
            anim.SetFloat("Look_x", 0);
        }
        else
            if (a < 0 && b < 0 && a / b >= 0 && a / b <= 1)
        {
            anim.SetFloat("Look_y", -1);
            anim.SetFloat("Look_x", 0);
        }
        else
            if (a > 0 && b < 0 && a / b <= -1)
        {
            anim.SetFloat("Look_x", 1);
            anim.SetFloat("Look_y", 0);
        }
        else
            if (a < 0 && b > 0 && a / b <= -1)
        {
            anim.SetFloat("Look_x", -1);
            anim.SetFloat("Look_y", 0);
        }
        else
            if (a > 0 && b < 0 && a / b > -1)
        {
            anim.SetFloat("Look_y", -1);
            anim.SetFloat("Look_x", 0);
        }
        else
            if (a < 0 && b > 0 && a / b > -1)
        {
            anim.SetFloat("Look_y", 1);
            anim.SetFloat("Look_x", 0);
        }
    }
    /// <summary>
    /// 敌人朝角色移动
    /// </summary>
    private void Move()
    {
        if (aipath.desiredVelocity.magnitude >= 0)
            anim.SetFloat("speed", aipath.desiredVelocity.magnitude);
    }
    /// <summary>
    /// 判断敌人朝向
    /// </summary>
    private void Attack()
    {
        Vector2 EnemyToPlayer = moveSpot.position - this.transform.position;
        float distance = EnemyToPlayer.magnitude;
        if (distance < 1.0f && timer_attack <= 0)
        {
            anim.SetTrigger("E1Attack");
            float damage = (-1) * (damage_enemy - defend_pc);
            pc.ChangeHealth(damage);
            Debug.Log("角色受到伤害");
            spr_pc.color = Color.red; //受到伤害时变成红色
            timer_color = 0.5f;
            timer_attack = 2.0f;
        }
    }
    /// <summary>
    /// 敌人被击中的检测
    /// </summary>
    private void Hited()
    {
        Vector2 EnemyToPlayer = moveSpot.position - this.transform.position;
        float distance = EnemyToPlayer.magnitude;
        if (pc.is_attack_sword == true && distance < 1.5f) //角色攻击距离更远
        {
            spr_enemy.color = Color.red;
            Debug.Log("敌人受到sword伤害");
            health -= (damage_pc - defend_enemy);

            EnemyHealthBar.instance.UpdateBar(health, 20);

            Debug.Log("sword" + "enemy" + health);
            timer_color = 0.5f;
        }
        else if (pc.is_attack_shield == true && distance < 1.5f)
        {
            spr_enemy.color = Color.red;
            Debug.Log("敌人受到shield伤害");
            health -= (damage_pc - 3 - defend_enemy); //拿盾攻击攻击力少3

            EnemyHealthBar.instance.UpdateBar(health, 20);

            Debug.Log("shiled" + "enemy" + health);

            timer_color = 0.5f;
        }
        pc.is_attack_sword = false;
        pc.is_attack_shield = false;
        Status_test();
    }
    /// <summary>
    /// 计时功能（敌人攻击频率以及角色颜色恢复）
    /// </summary>
    private void Timer()
    {
        timer_attack -= Time.deltaTime;
        timer_color -= Time.deltaTime;
        if (timer_color <= 0)
        {
            spr_pc.color = Color.white;
            spr_enemy.color = Color.white;
        }

    }
    /// <summary>
    /// 检查是否死亡
    /// </summary>
    void Status_test()
    {
        if (health <= 0)
        {
            anim.SetTrigger("E1Death");
            spr_enemy.color = Color.white;
            alive = false;
            //对人物的经验和金币等 增加

            pc.ChangeAttribute(0, 0, 0, 50);
            pc.ChangeLevel(20);
            //杀敌数增加
            pc.count++;
            if (pc.count != 5)
            {
                timer_rebirth = 2.0f;
                while (timer_rebirth >= 0)
                    timer_rebirth -= Time.deltaTime;
                if (timer_rebirth <= 0)
                {
                    Instantiate(feet, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
                    GameObject.Destroy(enemy, 2.5f);
                    aipath.enabled = false;
                }
            }
        }
    }
}

