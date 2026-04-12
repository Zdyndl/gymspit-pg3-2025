using RPG;

public class AI : Controller
{
    private Random random;


    public AI(Random random)
    {
        this.random = random;
    }


    public string ChooseAction(Character character, Character enemy)
    {
        double healthRatio = character.HealthRatio;
        int stamina = character.Stamina;
        int staminaRegen = character.StaminaRegen;

        const double lowHealthThreshold = 0.35;

        if (healthRatio <= lowHealthThreshold)
        {
            if (stamina > 10)
            {
                return Controller.TURN_CHOICE_HEAL;
            }

            return Controller.TURN_CHOICE_DEFEND;
        }

        if (stamina <= 2*staminaRegen)
        {
            return Controller.TURN_CHOICE_WAIT;
        }

        return Controller.TURN_CHOICE_ATTACK;
    }
}