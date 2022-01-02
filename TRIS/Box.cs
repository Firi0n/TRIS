
namespace TRIS
{
    internal class Box
    {
        //Variable for the state of the box.
        private byte boxStatus = 0;
        //String for on-screen construction of symbols.
        private string[] symbol { get; } = new string[6] { "\\ /", " X ", "/ \\", "/^\\", "|O|", "\\_/" };
        //BoxStatus accepts only values between 0 and 2.
        public byte BoxStatus
        {
            get { return boxStatus; }
            set
            {
                if (value >= 0 && value <= 2) boxStatus = value;
                else throw new Exception("Invalid box value");
            }
        }

        public void fillBox(byte left = 0, byte top = 0)
        {
            //On-screen construction of the box, if the box is empty then it does not write anything.
            if (boxStatus != 0)
            {
                Console.CursorTop = top;
                for (byte i = 0; i < 3; i++)
                {
                    Console.CursorLeft = left;
                    Console.WriteLine(boxStatus == 1 ? symbol[i] : symbol[i + 3]);
                }
            }
        }
    }

}
