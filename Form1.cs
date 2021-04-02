using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClienteServidorTiago
{
    public partial class Form1 : Form
    {
        static Servidor servidor;
        static Thread listenThread;
        private Report<string> _report;
        public Form1()
        {
            InitializeComponent();

            _report = new Report<string>(str =>
            {
                txtChat.Text += $"{DateTime.Now.ToString("HH:mm:ss")} {str} {Environment.NewLine}";
                txtChat.SelectionStart = txtChat.Text.Length - 1;
                txtChat.SelectionLength = 0;
                txtChat.ScrollToCaret();
            });
            try
            {
                servidor = new Servidor(_report);
                listenThread = new Thread(new ThreadStart(servidor.Listen));
                listenThread.Start();

            }
            catch
            {
                servidor.Disconnect();
            }
        }
    }
}
