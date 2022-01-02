using TRIS;

Game game;
char repeat = ' ';
//Repeat the program with key enter or exit with key esc.
do
{
    Console.Clear();
    game = new Game();
    game.executeTurn();
    Console.Write("Press enter for repeat or esc for exit: ");
    do
    {
        repeat = Console.ReadKey(true).KeyChar;
    } while (repeat != 13 && repeat != 27);
} while (repeat == 13);