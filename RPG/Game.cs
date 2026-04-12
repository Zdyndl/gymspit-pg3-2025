namespace RPG;


public class Game
{
    private Character characterOne;

    private Character characterTwo;

    private Die attackDie;

    private Die waitDie;

    public Game(Character characterOne, Character characterTwo, Die attackDie, Die waitDie)
    {
        this.characterOne = characterOne;
        this.characterTwo = characterTwo;
        this.attackDie = attackDie;
        this.waitDie = waitDie;
    }


    public void Run(Log log)
    {
        characterOne.Reset();
        characterTwo.Reset();

        log.GameStart(characterOne, characterTwo);

        Character active = characterOne;
        Character nonActive = characterTwo;

        while (characterOne.Alive && characterTwo.Alive)
        {
            log.CharacterTurn(active);

            active.RegenerateStamina();
            active.StopDefending();

            string action = active.Controller.ChooseAction(active, nonActive);
            switch (action)
            {
                case Controller.TURN_CHOICE_ATTACK:
                    active.Attack(log, nonActive, attackDie);
                    break;

                case Controller.TURN_CHOICE_WAIT:
                    active.Wait(log, waitDie);
                    break;

                case Controller.TURN_CHOICE_DEFEND:
                    active.Defend(log);
                    break;

                case Controller.TURN_CHOICE_HEAL:
                    active.Heal(log);
                    break;
                default:
                    log.CharacterDoesNothing(active);
                    break;
            }

            log.CharacterStatus(characterOne);
            log.CharacterStatus(characterTwo);

            Character tmp = active;
            active = nonActive;
            nonActive = tmp;
        }

        Character? winner = null;
        if (characterOne.Alive)
        {
            winner = characterOne;
        }
        else if (characterTwo.Alive)
        {
            winner = characterTwo;
        }

        log.GameOver(winner);
    }
}