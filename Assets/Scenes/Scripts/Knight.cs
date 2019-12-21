using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))] //挂载脚本时一定要有刚体和动画组件
public class Knight : MonoBehaviour
{
    private Rigidbody2D m_rigid;
    private Animator m_animator;
    [SerializeField]
    private GameObject CheckPanel;
    //定义与玩家对话框相关的变量
    private bool speaking = false;
    //定义与人物移动相关的变量
    private float Speed = 5;
    private Vector2 lookDirection = new Vector2(0, -1);
    //定义与角色目前关卡数的定义 stage表示目前关数 通过战斗胜利后修改
    //location表示所处场景位置 
    private int stage;
    private int location = 0;
    private bool backtown = false;
    //定义与人物攻击相关的变量
    public float timer_sword = 0.0f;
    public float timer_shield = 0.0f;
    //恢复血量蓝量的频率
    private float timer_recover = 1.0f;
    //退出副本等待时间
    private float timer_wait = 15.0f;
    //玩家最大生命值
    private float maxHealth;
    //玩家最大蓝量值
    private float maxMana;
    //玩家当前生命值
    private float currentHealth;
    //玩家当前蓝量值
    private float currentMana;
    //玩家攻击力
    public int attack;
    //玩家防御力
    public int defend;
    //玩家等级
    private int level;
    //玩家当前经验值
    private int exp;
    //玩家升级需要的经验值
    private int maxExp;
    //玩家金钱
    public int money;
    //玩家血瓶数量
    private int healthPotion;
    //玩家蓝瓶数量
    private int manaPotion;
    //玩家综合恢复瓶数量
    private int bothPotion;

    //玩家无敌时间0.5秒
    private float invincibleTime = 0.5f;
    //无敌时间计时器
    private float invincibleTimer;
    //是否处于无敌状态
    private bool isInvincible = false;

    //角色是否进行攻击
    public bool is_attack_sword = false;
    public bool is_attack_shield = false;
    //一张地图中敌人的个数
    private int total_enemy = 5;
    public int count=0;

    public float MyMaxHealth { get { return maxHealth; } }
    public float MyMaxMana { get { return maxMana; } }
    public float MyCurrentHealth { get { return currentHealth; } }
    public float MyCurrentMana { get { return currentMana; } }
    public int MyAttack { get { return attack; } }
    public int MyDefend { get { return defend; } }
    public int MyLevel { get { return level; } }
    public int MyExp { get { return exp; } }
    public int MyMaxExp { get { return maxExp; } }
    public int MyMoney { get { return money; } }
    public int MyHealthPotion { get { return healthPotion; } }
    public int MyManaPotion { get { return manaPotion; } }
    public int MyBothPotion { get { return bothPotion; } }
    public int Mystage { get { return stage; } }

    void Start()
    {
        m_rigid = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();

        // PlayerPrefs.DeleteAll();

        //角色数据初始化
        Initialize();

        location = SceneManager.GetActiveScene().buildIndex;
        if (location == 1)
        {
            SpeakStart();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //若在对话中，则冻结角色
        if (!speaking)
        {
            //检测现在所在场景 关卡数
            location = SceneManager.GetActiveScene().buildIndex;
            //技能冷却 属性回复 计时
            Timer();
            //角色移动
            Role_move();
            //角色行为
            Action();
            //角色蓝量、血量自动回复
            Role_recover();
            //检查是否跳转副本
            OnPorterDetect();
        }
        Check_kills();
        //与游戏中数值显示有关的更新
        //更新血条显示
        HealthBar.instance.UpdateBar(currentHealth, maxHealth);
        //更新蓝条显示
        ManaBar.instance.UpdateBar(currentMana, maxMana);
        //更新呢经验条显示
        ExpBar.instance.UpdateBar(exp, maxExp);
        //更新人物属性显示
        AttributeManager.instance.UpdateAttribute(attack, defend, level, currentHealth, currentMana, money);
        AttributeManager.instance.UpdateHealthPotion(healthPotion);
        AttributeManager.instance.UpdateManaPotion(manaPotion);
        AttributeManager.instance.UpdateBothPotion(bothPotion);

        //无敌计时
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;

            //计时结束，取消无敌状态
            if (invincibleTimer < 0)
            {
                isInvincible = false;
            }
        }
    }

    /// <summary>
    /// 角色数据初始化函数
    /// </summary>
    public void Initialize()
    {
        //初始化当前血量和最大生命值
        currentHealth = PlayerPrefs.GetFloat("currentHealth", 100);
        maxHealth = PlayerPrefs.GetFloat("maxHealth", 100);

        //初始化当前蓝量值和最大蓝量值
        currentMana = PlayerPrefs.GetFloat("currentMana", 50);
        maxMana = PlayerPrefs.GetFloat("maxMana", 50);

        //初始化攻击力和防御力
        attack = PlayerPrefs.GetInt("attack", 10);
        defend = PlayerPrefs.GetInt("defend", 5);

        //初始化等级和经验值
        level = PlayerPrefs.GetInt("level", 1);
        exp = PlayerPrefs.GetInt("exp", 0);
        maxExp = PlayerPrefs.GetInt("maxExp", 100);

        //初始化金币数量
        money = PlayerPrefs.GetInt("money", 0);

        //初始化物品数量
        healthPotion = PlayerPrefs.GetInt("healthPotion", 5);
        manaPotion = PlayerPrefs.GetInt("manaPotion", 5);
        bothPotion = PlayerPrefs.GetInt("bothPotion", 2);

        //关卡数
        stage = PlayerPrefs.GetInt("stage", 1);
    }

    /// <summary>
    /// 角色移动函数
    /// </summary>
    void Role_move()
    {
        float horizontal = 0;
        float vertical = 0;
        float move_x = 0;
        float move_y = 0;
        //定义角色移动用到的变量
        horizontal = Input.GetAxis("Horizontal"); // A -1 D 1
        vertical = Input.GetAxis("Vertical"); //S -1 W 1
        //得到方向键操控的移动方向  
        Vector2 move_vec = new Vector2(horizontal, vertical);
        if (move_vec.x != 0 || move_vec.y != 0)
            lookDirection = move_vec;
        //比较是否有方向改变 有方向改变赋新值
        m_animator.SetFloat("Look_x", lookDirection.x);
        m_animator.SetFloat("Look_y", lookDirection.y);
        m_animator.SetFloat("speed", move_vec.magnitude);
        //设置animator控制器中三个参数
        move_x = horizontal * Speed;
        move_y = vertical * Speed;
        m_rigid.velocity = new Vector2(move_x, move_y);
        //将move_x,move_y作为2D刚体运动的velocity写入
    }

    /// <summary>
    /// 检测传送门位置 是否有角色 如有则弹出消息确认框
    /// </summary>
    void OnPorterDetect()
    {
        float position_x, position_y;
        position_x = m_rigid.transform.localPosition.x;
        position_y = m_rigid.transform.localPosition.y;
        if (position_x <= 3.6 && position_x >= 2.3 && position_y >= -3.4 && position_y <= -2.6)
        //控制了传送门触发的位置 在一个特定的范围内
        {
            if (backtown == false && location == 1)
            //避免触发backtown函数后又一直检测到角色在传送门上使面板一直为显示
            {
                //Speed = 0;  //弹出界面框不允许角色移动
                SpeakStart();
                CheckPanel.SetActive(true);
            }
        }
        else
        {   //走出范围内才会再次变为false 可重新判定
            backtown = false;
        }
    }

    /// <summary>
    /// 进入副本前，保存数据
    /// </summary>
    public void Save()
    {
        PlayerPrefs.SetFloat("maxHealth", maxHealth);
        PlayerPrefs.SetFloat("maxMana", maxMana);
        PlayerPrefs.SetFloat("currentHealth", currentHealth);
        PlayerPrefs.SetFloat("currentMana", currentMana);
        PlayerPrefs.SetInt("attack", attack);
        PlayerPrefs.SetInt("defend", defend);
        PlayerPrefs.SetInt("level", level);
        PlayerPrefs.SetInt("exp", exp);
        PlayerPrefs.SetInt("maxExp", maxExp);
        PlayerPrefs.SetInt("money", money);
        PlayerPrefs.SetInt("healthPotion", healthPotion);
        PlayerPrefs.SetInt("manaPotion", manaPotion);
        PlayerPrefs.SetInt("bothPotion", bothPotion);
        PlayerPrefs.SetInt("stage", stage);

    }

    /// <summary>
    /// 利用消息确认框Button OnClick触发 切换至副本场景
    /// </summary>
    public void ChangeScene()
    {
        Save();
        switch (stage)
        {
            case 1:
                SceneManager.LoadScene(2);
                break;
            case 2:
                SceneManager.LoadScene(3);
                break;
            case 3:
                SceneManager.LoadScene(4);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 利用消息确认框Button OnClick触发 返回
    /// </summary>
    public void BackTown()
    {
        //Speed = 5;//恢复速度
        SpeakOver();
        backtown = true;
        CheckPanel.SetActive(false);
        //将人物移到传送点外 否则会一直触发
    }

    /// <summary>
    ///改变玩家生命值
    /// </summary>
    /// <param name="amount"></param>
    public void ChangeHealth(float amount)
    {
        //玩家受到伤害
        if (amount < 0)
        {
            //若玩家此时无敌，则不受到伤害
            if (isInvincible == true)
            {
                return;
            }
            isInvincible = true;
            invincibleTimer = invincibleTime;
        }

        //改变血量
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        //更新血条显示
        HealthBar.instance.UpdateBar(currentHealth, maxHealth);

        Debug.Log("currentHealth" + currentHealth);
    }


    /// <summary>
    /// 改变玩家最大血量值
    /// </summary>
    /// <param name="amount"></param>
    public void ChangeMaxHealth(float amount)
    {
        //改变最大血量
        maxHealth = maxHealth + amount;
        currentHealth = Mathf.Clamp(currentHealth + 1000, 0, maxHealth);//血量直接加满
        //更新血条显示
        ManaBar.instance.UpdateBar(currentHealth, maxHealth);

        Debug.Log("MaxHealth" + maxHealth);
    }

    /// <summary>
    /// 改变玩家蓝量值
    /// </summary>
    /// <param name="amount"></param>
    public void ChangeMana(float amount)
    {
        //改变血量
        currentMana = Mathf.Clamp(currentMana + amount, 0, maxMana);

        //更新蓝条显示
        ManaBar.instance.UpdateBar(currentMana, maxMana);

        Debug.Log("currentMana" + currentMana);
    }

    /// <summary>
    /// 改变最大蓝量
    /// </summary>
    /// <param name="amount"></param>
    public void ChangeMaxMana(float amount)
    {
        //改变最大蓝量量
        maxMana = maxMana + amount;
        currentMana = Mathf.Clamp(currentMana + 1000, 0, maxMana);//蓝量直接加满
        //更新蓝条显示
        ManaBar.instance.UpdateBar(currentMana, maxMana);

        Debug.Log("maxMana" + maxMana);
    }
    /// <summary>
    /// 改变等级和经验值
    /// </summary>
    /// <param name="amount"></param>
    public void ChangeLevel(int amount)
    {
        exp = exp + amount;

        //当前等级经验值满，等级上升
        if (exp >= maxExp)
        {
            level++;
            //升级后加属性 攻击力等
            ChangeAttribute(5, 2, 0, 0);
            exp = exp - maxExp;
        }

        //更新经验条显示
        ExpBar.instance.UpdateBar(exp, maxExp);
        
       
        //更新等级显示
        AttributeManager.instance.UpdateAttribute(attack, defend, level, currentHealth, currentMana, money);

        Debug.Log("exp" + exp);
    }

    /// <summary>
    /// 角色属性变化
    /// </summary>
    /// <param name="amount1"></param>
    /// <param name="amount2"></param>
    /// <param name="amount3"></param>
    /// <param name="amount4"></param>
    public void ChangeAttribute(int amount1, int amount2, int amount3, int amount4)
    {
        //更新属性
        attack = attack + amount1;
        defend = defend + amount2;
        level = level + amount3;
        money = money + amount4;

        //更新属性显示
        AttributeManager.instance.UpdateAttribute(attack, defend, level, currentHealth, currentMana, money);

        Debug.Log("Change attribute.");
    }

    /// <summary>
    /// 药瓶数量变化
    /// </summary>
    /// <param name="amount1"></param>
    /// <param name="amount2"></param>
    /// <param name="amount3"></param>
    public void ChangePotion(int amount1, int amount2, int amount3)
    {
        healthPotion = healthPotion + amount1;
        manaPotion = manaPotion + amount2;
        bothPotion = bothPotion + amount3;
    }

    /// <summary>
    /// 玩家行为：攻击、打开npc对话框等
    /// </summary>
    public void Action()
    {
        //按下空格与建筑物交互 在城镇地图下才能交互
        if (Input.GetKeyDown(KeyCode.Space) && location == 1)
        {
            RaycastHit2D hit = Physics2D.Raycast(m_rigid.position, lookDirection, 1f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                Debug.Log("碰到了NPC");
                NPCmanager npc = hit.collider.GetComponent<NPCmanager>();
                if (npc != null)
                {
                    Debug.Log("显示选项框");
                    npc.ShowNPCPanel();
                }

                //待实现禁止玩家行动，除非玩家点击离开按钮
                //m_rigid.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }

        //只允许在副本地图下才攻击 并且冷却时间结束
        if (Input.GetKeyDown(KeyCode.O) && location !=1 && timer_sword <= 0) //按下O键 剑武器攻击
        {
            if (currentMana >= 10)  //要有一定蓝量才能进行 剑武器攻击
            {
                ChangeMana(-10); //耗蓝
                m_animator.SetTrigger("Attack_sword"); //攻击动画
                timer_sword = 3.0f;
                is_attack_sword = true;
                Debug.Log("剑攻击一次");
            }
        }
        if (Input.GetKeyDown(KeyCode.P) && location !=1 && timer_shield <= 0) //按下P键 盾武器攻击
        {
            if (currentMana >= 7)  //要有一定蓝量才能进行 盾武器攻击
            {
                ChangeMana(-7); //耗蓝
                m_animator.SetTrigger("Attack_shield"); //攻击动画
                timer_shield = 2.0f;
                is_attack_shield = true;
                Debug.Log("盾攻击一次");
            }
        }
        //血药
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //有血药时，才能使用
            if (healthPotion >= 1)
            {
                //限制血药数量大于等于0
                healthPotion = Mathf.Clamp(healthPotion - 1, 0, 5);
                ChangeHealth(20);
            }
        }
        //蓝药
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (manaPotion >= 1)
            {
                manaPotion = Mathf.Clamp(manaPotion - 1, 0, 5);
                ChangeMana(10);
            }
        }
        //红蓝药回复
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (bothPotion >= 1)
            {
                bothPotion = Mathf.Clamp(bothPotion - 1, 0, 2);
                ChangeMana(10);
                ChangeHealth(20);
            }
        }

        //仅用于测试使用
        if (Input.GetKeyDown(KeyCode.J))
        {
            ChangeLevel(11);
        }
    }

    /// <summary>
    /// 角色蓝量、血量自动回复
    /// </summary>
    public void Role_recover()
    {
        //血量、蓝量需要回复且每秒回复一次
        if (timer_recover <= 0)
        {
            if (currentMana < 50 && currentMana >= 0)
            {
                ChangeMana(1);
                Debug.Log("回复蓝量1点");
            }
            if (currentHealth < 100 && currentHealth >= 0)
            {
                ChangeHealth(1);
                Debug.Log("回复生命值1点");
            }
            //回复完后置为1，使得时间重新计算
            timer_recover = 1.0f;
        }
    }

    /// <summary>
    /// 计时器
    /// </summary>
    public void Timer()
    {
        timer_recover -= Time.deltaTime;
        timer_sword -= Time.deltaTime;
        timer_shield -= Time.deltaTime;
    }

    /// <summary>
    /// 对话框开始 - 解冻行动
    /// </summary>
    public void SpeakOver()
    {
        speaking = false;
    }

    /// <summary>
    /// 对话框开始 - 冻结行动
    /// </summary>
    public void SpeakStart()
    {
        speaking = true;
        m_rigid.velocity = new Vector2(0, 0);
    }

    /// <summary>
    /// 对话框状态读取函数
    /// </summary>
    public bool IsSpeaking()
    {
        return speaking;
    }
    /// <summary>
    /// 确认杀敌数
    /// </summary>
    public void Check_kills()
    {
        if (count == total_enemy)
        {
            timer_wait = 15.0f;
            while (timer_wait >= 0)
                timer_wait -= Time.deltaTime;
            if (timer_wait <= 0)
            {
                SceneManager.LoadScene(1); //返回到城镇界面
                stage++; //关卡数加1
                Save();
            }
           
        }
    }
}

