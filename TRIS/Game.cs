
namespace TRIS
{
    internal class Game
    {
        //Object to make the turn random at the beginning.
        private Random rand = new Random();
        private byte turn = 0;
        //Game table.
        private Table table = new Table();
        //Shortcut box.
        private Box[] box { get { return table.box; } set { table.box = value; } }
        //Variable for user input reading.
        private byte reading = 9;

        public Game()
        {
            Console.Title = "TRIS!";
            Console.CursorVisible = false;
            //Random turn at the beginning with values equal to the BoxStatus.
            turn = (byte)rand.Next(1, 3);
        }

        public void executeTurn()
        {
            //Table creation and initial printouts.
            table.printTable(18, 1);
            Console.Write("\nTurn of:\n\nEnter the box number (0 to 8):");

            for (byte i = 0; i < 9; i++)
            {
                //Turn chart update.
                Console.SetCursorPosition(9, table.TablePosition[1] + 12);
                Console.Write(turnInChar());
                //Entering the box by the user with its controls.
                do
                {
                    try
                    {
                        reading = (byte)char.GetNumericValue(Console.ReadKey(true).KeyChar);
                    }
                    catch (Exception) { reading = 9; };

                } while (reading < 0 || reading > 8 || table.box[reading].BoxStatus != 0);

                //Filling the box both graphically and in the variable.
                table.fillTable(reading, turn);
                /*Winner control and in case exit from the cycle, but only after that 
                 * players made two moves each.*/
                if (i > 3 && controlWinner() == true) break;
                else if (i == 8) { turn = 0; break; }
                //Player change.
                turn = (byte)(turn == 1 ? 2 : 1);
            }
            //Winner writing.
            Console.CursorTop = table.TablePosition[1] + 16;
            Console.WriteLine(turn == 0 ? "Drew!" : $"Won {turnInChar()}!\n");
        }

        private bool controlWinner()
        {
            for (byte i = 0; i < 3; i++)
            {
                if (
                    (
                        //Horizontal control.
                        box[i * 3].BoxStatus != 0 &&
                        box[i * 3].BoxStatus == box[(i * 3) + 1].BoxStatus &&
                        box[i * 3].BoxStatus == box[(i * 3) + 2].BoxStatus
                    ) || (
                        //Vertical control.
                        box[i].BoxStatus != 0 &&
                        box[i].BoxStatus == box[i + 3].BoxStatus &&
                        box[i].BoxStatus == box[i + 6].BoxStatus
                    ) || (
                        //Oblique control.
                        i < 2 && box[4].BoxStatus != 0 &&
                        box[4].BoxStatus == box[i * 2].BoxStatus &&
                        box[4].BoxStatus == box[8 - (i * 2)].BoxStatus
                    )
                    ) return true;
            }
            return false;
        }

        //Transforming the turn into a printable character.
        private char turnInChar()
        {
            return turn == 1 ? 'X' : 'O';
        }
    }
}
