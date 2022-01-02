
namespace TRIS
{
    internal class Table
    {
        //Boxs.
        public Box[] box = new Box[9];
        //X and y table coordinates.
        private byte[] tablePosition = new byte[2] { 0, 0 };
        //TablePosition makes location public tablePosition but only read.
        public byte[] TablePosition { get { return tablePosition; } }

        public Table()
        {
            //Boxs init.
            for (byte i = 0; i < box.Length; i++) box[i] = new Box();
        }

        public void printTable(byte left = 0, byte top = 0)
        {
            //Support variable.
            byte h = 0;
            //Saved x coordinate;
            tablePosition[0] = left;
            //Saved y coordinate;
            tablePosition[1] = top;

            //The y coordinate should be invoked only once.
            Console.CursorTop = top;
            //Cycle to build the table line by line.
            for (byte i = 0; i < 11; i++)
            {
                //The x coordinate moves the cursor to the right in each new line.
                Console.CursorLeft = left;
                //Choice to understand which characters to print on each line.
                if (i == 3 || i == 7) Console.WriteLine("---+---+---");
                else if (i == 1 || i == 5 || i == 9)
                {
                    //Calculation of the box number in the line and in the middle column of each box.
                    h = (byte)(i / 4 * 3);
                    Console.WriteLine($" {h} | {h + 1} | {h + 2} ");
                }
                else Console.WriteLine("   |   |   ");
            }
        }

        public void fillTable(byte numberBox, byte turn)
        {
            //Filling the status variable BoxStatus by turn.
            box[numberBox].BoxStatus = turn;
            //Filling the right box in the table graphically by calculating on the coordinates.
            box[numberBox].fillBox
            (
                (byte)(tablePosition[0] + (4 * (numberBox % 3))),
                (byte)(tablePosition[1] + (4 * (numberBox / 3)))
            );
        }
    }

}
