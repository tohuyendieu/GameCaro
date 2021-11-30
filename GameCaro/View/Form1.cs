using GameCaro.Controller;
using GameCaro.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static GameCaro.Entity.SocketData;

namespace GameCaro
{
    public partial class Form1 : Form
    {
        #region Properties
        ChessBoardManager ChessBoard;

        SocketManager socket;

        #endregion

        public Form1()
        {
            InitializeComponent();
            ChessBoard = new ChessBoardManager(pnlChessBroad, txtPlayerName, pctbMark);
            ChessBoard.EndedGame += ChessBoard_EndGame;
            ChessBoard.PlayerMarked += ChessBoard_PlayerMarked;

            prcbCountDown.Step = Cons.COUNT_DOWN_STEP;
            prcbCountDown.Maximum = Cons.COUNT_DOWN_TIME;
            prcbCountDown.Value = 0;

            tmCountDown.Interval = Cons.COUNT_DOWN_INTERVAL;

            socket = new SocketManager();

            NewGame();
        }             
        void EndGame()
        {
            tmCountDown.Stop();
            pnlChessBroad.Enabled = false;
            undoToolStripMenuItem.Enabled = false;
            MessageBox.Show("Game Over");
        }
        void NewGame()
        {
            prcbCountDown.Value = 0;
            tmCountDown.Stop();
            undoToolStripMenuItem.Enabled = true;            
            ChessBoard.DrawChessBoard();
        }
        void Quit()
        {
           Application.Exit();
        }
        void Undo()
        {
            ChessBoard.Undo();
        }
        void ChessBoard_EndGame(object sender, EventArgs e)
        {
            EndGame();
        }
        void ChessBoard_PlayerMarked(object sender, ButtonClickEvent e)
        {
            tmCountDown.Start();
            pnlChessBroad.Enabled = false;
            prcbCountDown.Value = 0;

            socket.Send(new SocketData((int)SocketCommand.SEND_POINT,"", e.ClickedPoint));

            Listen();
        }

        private void tmCountDown_Tick(object sender, EventArgs e)
        {
            prcbCountDown.PerformStep();

            if(prcbCountDown.Value >= prcbCountDown.Maximum)
            {               
                EndGame();                
            }
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewGame();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Undo();
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Quit();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Do you want to exit?", "Notice", MessageBoxButtons.OKCancel) != System.Windows.Forms.DialogResult.OK)
                e.Cancel = true;
        }

        private void btnLan_Click(object sender, EventArgs e)
        {
            socket.IP = txtIP.Text;

            if (!socket.ConnectServer())
            {
                socket.isServer = true;
                pnlChessBroad.Enabled = true;
                socket.CreateServer();
            }
            else
            {
                socket.isServer = false;
                pnlChessBroad.Enabled = false;
                Listen();
             }            
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            txtIP.Text = socket.GetLocalIPv4(NetworkInterfaceType.Wireless80211);
            if (string.IsNullOrEmpty(txtIP.Text))
            {
                txtIP.Text = socket.GetLocalIPv4(NetworkInterfaceType.Ethernet);
            }
        }

        void Listen()
        {            
            Thread listenThread = new Thread(() =>
            {
                try
                {
                    SocketData data = (SocketData)socket.Receive();

                    ProcessData(data);
                }
                catch
                {

                }
                
            });
            listenThread.IsBackground = true;
            listenThread.Start();
        }

        private void ProcessData(SocketData data)
        {
            switch (data.Command)
            {
                case (int)SocketCommand.NOTIFY:
                    MessageBox.Show(data.Message);
                    break;
                case (int)SocketCommand.NEW_GAME:
                    break;
                case (int)SocketCommand.SEND_POINT:
                    this.Invoke((MethodInvoker)(() =>
                    {
                        prcbCountDown.Value = 0;
                        pnlChessBroad.Enabled = true;
                        tmCountDown.Start();
                        ChessBoard.OtherPlayerMark(data.Point);
                    }));                    
                    break;
                case (int)SocketCommand.UNDO:
                    break;
                case (int)SocketCommand.END_GAME:
                    break;
                case (int)SocketCommand.QUIT:
                    break;
                default:
                    break;
            }

            Listen();
        }
    }
}
