using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCaro.Entity
{
    [Serializable]
    public class SocketData
    {       
        private int command;
        private Point point;
        private string message;

        public SocketData(int command, string mesage, Point point)
        {
            this.Command = command;
            this.Point = point;
            this.Message = message;
        }

        public int Command { get => command; set => command = value; }
        public Point Point { get => point; set => point = value; }
        public string Message { get => message; set => message = value; }

        public enum SocketCommand
        {
            SEND_POINT,
            NOTIFY,
            NEW_GAME,
            UNDO,
            END_GAME,
            QUIT
        }
    }
}
