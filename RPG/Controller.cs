namespace RPG;


public interface Controller
{
    public const string TURN_CHOICE_ATTACK = "attack";
    public const string TURN_CHOICE_WAIT = "wait";
    public const string TURN_CHOICE_DEFEND = "defend";
    public const string TURN_CHOICE_HEAL = "heal";

    string ChooseAction(Character character, Character enemy);
}