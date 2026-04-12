namespace RPG;

public class Player : Controller
{
    private TextReader input;

    private TextWriter? prompt;

    public Player(TextReader input, TextWriter? prompt = null)
    {
        this.input = input;
        this.prompt = prompt;
    }


    public string ChooseAction(Character character, Character enemy)
    {
        while (true)
        {
            if (prompt != null)
            {
                prompt.WriteLine("Choose an action:");
                prompt.WriteLine("(A)ttack");
                prompt.WriteLine("(W)ait");
                prompt.WriteLine("(D)efend");
                prompt.WriteLine("(H)eal");
            }

            string choice = input.ReadLine();
            if (choice == null)
            {
                return null;
            }

            switch (choice.ToLower())
            {
                case "a":
                case "attack":
                    return Controller.TURN_CHOICE_ATTACK;
                case "w":
                case "wait":
                    return Controller.TURN_CHOICE_WAIT;
                case "d":
                case "defend":
                    return Controller.TURN_CHOICE_DEFEND;
                case "h":
                case "heal":
                    return Controller.TURN_CHOICE_HEAL;
            }

            if (prompt != null)
            {
                prompt.WriteLine("Invalid choice!");
            }
        }
    }
}