using RPG;

Random random = new Random();
Log log = new Log(Console.Out);

Controller ai = new AI(random);
Character c3PO = new Character(ai, "C-3PO", 40, 30, 8, 3, 12, new Die(random, 6), new Die(random, 4), new Die(random, 4));
Character r2D2 = new Character(ai, "R2-D2", 10, 40, 10, 0, 14, new Die(random, 6), new Die(random, 2), new Die(random, 2));
Character luke = new Character(new Player(Console.In, Console.Out), "Luke", 40, 30, 8, 3, 12, new Die(random, 6), new Die(random, 4), new Die(random, 4));

Game game = new Game(c3PO, r2D2, new Die(random, 20), new Die(random, 6));
game.Run(log);

Game game2 = new Game(c3PO, luke, new Die(random, 20), new Die(random, 6));
game2.Run(log);