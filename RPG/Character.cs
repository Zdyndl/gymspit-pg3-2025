namespace RPG;

public class Character
{
    private Controller controller;

    private string name;

    private int hitPoints;

    private int maxHitPoints;

    private int stamina;

    private int maxStamina;

    private int staminaRegen;

    private int attackBonus;

    private int armorClass;

    private int defenseBonus;

    private Die defenseDie;

    private Die weaponDie;

    private Die healDie;


    public Character(Controller controller, string name, int maxHitPoints, int maxStamina, int staminaRegen, int attackBonus, int armorClass, Die weponDie, Die defenseDie, Die healDie)
    {
        this.controller = controller;
        this.name = name;
        this.maxHitPoints = maxHitPoints;
        this.maxStamina = maxStamina;
        this.staminaRegen = staminaRegen;
        this.attackBonus = attackBonus;
        this.armorClass = armorClass;
        this.weaponDie = weponDie;
        this.defenseDie = defenseDie;
        this.healDie = healDie;
        Reset();
    }

    public Controller Controller => controller;

    public string Name => name;

    public double HealthRatio => (double)hitPoints / maxHitPoints;
    public int Stamina => stamina;

    public int StaminaRegen => staminaRegen;
    public bool Alive => hitPoints > 0;

    public int Defense => armorClass + defenseBonus;


    public void Reset()
    {
        hitPoints = maxHitPoints;
        stamina = maxStamina;
    }

    public void Attack(Log log, Character target, Die attackDie)
    {   if (stamina < 15)
        {
            log.CharacterTooTired(this);
            return;
        }
    else
        {
            stamina -= 15;

        int attackRoll = RollAttack(attackDie);
        log.CharacterAttack(this, target, attackRoll);
        target.Hit(log, this, attackRoll);
        }
    }

    public void Hit(Log log, Character source, int attackRoll)
    {
        if (attackRoll < Defense)
        {
            log.AttackMiss(source, this);
            return;
        }

        int damageRoll = source.RollDamage();
        log.AttackHit(source, this, damageRoll);
        hitPoints -= damageRoll;
    }

    public void Defend(Log log)
    {
        if (stamina < 5)
        {
            log.CharacterTooTired(this);
            return;
        }
        else
        {
            stamina -= 5;
            int defenseRoll = RollDefense(defenseDie);
            log.CharacterDefend(this, defenseRoll);
            defenseBonus = defenseRoll;
        }
    }

    public void Heal(Log log)
    {
        if (stamina < 10)
        {
            log.CharacterTooTired(this);
            return;
        }
        else
        {
            stamina -= 10;
            int healRoll = healDie.Roll();
            log.CharacterHeal(this, healRoll);
            hitPoints += healRoll;
            if (hitPoints > maxHitPoints)
            {
                hitPoints = maxHitPoints;
            }
        }
    }

    public void Wait(Log log, Die waitDie)
    {
        log.CharacterWait(this, waitDie.Roll());
    }

    public int RollAttack(Die attackDie)
    {
        return attackDie.Roll() + attackBonus;
    }

    public int RollDefense(Die defenseDie)
    {
        return defenseDie.Roll();
    }

    public int RollDamage()
    {
        return weaponDie.Roll() + attackBonus;
    }

    public void RegenerateStamina()
    {
        stamina = stamina + staminaRegen;
        if (stamina > maxStamina)
        {
            stamina = maxStamina;
        }
    }

    public void StopDefending()
    {
        defenseBonus = 0;
    }
}