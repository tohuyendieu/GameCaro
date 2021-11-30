using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCaro
{
    class Cons
    {
        public static int CHESS_WIDTH = 30;
        public static int CHESS_HEIGHT = 30;

        private static int cHESS_BOARD_WIDTH;
        private static int cHESS_BOARD_HEIGHT;

        public static int COUNT_DOWN_STEP = 100;
        public static int COUNT_DOWN_TIME = 10000;
        public static int COUNT_DOWN_INTERVAL = 100;

        public static int CHESS_BOARD_WIDTH { get => cHESS_BOARD_WIDTH; set => cHESS_BOARD_WIDTH = value; }
        public static int CHESS_BOARD_HEIGHT { get => cHESS_BOARD_HEIGHT; set => cHESS_BOARD_HEIGHT = value; }
    }
}
