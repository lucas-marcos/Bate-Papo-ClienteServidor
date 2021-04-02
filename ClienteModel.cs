using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClienteServidorTiago
{
    class ClienteModel
    {
        protected internal string Id { get; set; }
        protected internal NetworkStream Stream { get; private set; }
        protected internal string nickName;
        TcpClient tcpClient;
        Servidor servidorModel;
        private Report<string> _report;

        public ClienteModel(TcpClient _tcpClient, Servidor _servidorModel, Report<string> report)
        {
            Id = Guid.NewGuid().ToString();
            tcpClient = _tcpClient;
            servidorModel = _servidorModel;
            Stream = tcpClient.GetStream();
            nickName = GetMessage();
            _servidorModel.AddConnection(this);
            _report = report;
        }

        public void Process()
        {
            try
            {
                string message = nickName;
                string msg = ">> ";
                msg += message;
                msg += " Entrou no chat!";
                _report.Reportar($"{msg} {Environment.NewLine}");

                while (true)
                {
                    try
                    {
                        message = GetMessage();
                        message = $"{nickName}: {message}";
                        _report.Reportar(message);
                    }
                    catch
                    {
                        message = $"{nickName}: {message} saiu do chat!";
                        break;
                    }

                }

            }
            catch (Exception exception)
            {
                _report.Reportar(exception.Message);
            }
            finally
            {
                servidorModel.RemoveConnection(this.Id);
                Close();
            }
        }

        private string GetMessage()
        {
            byte[] data = new byte[64];
            StringBuilder builder = new StringBuilder();
            int bytes = 0;
            do
            {
                bytes = Stream.Read(data, 0, data.Length);
                builder.Append(Encoding.UTF8.GetString(data, 0, bytes));
            } while (Stream.DataAvailable);

            return builder.ToString();
        }

        protected internal void Close()
        {
            if (Stream != null)
                Stream.Close();
            if (tcpClient != null)
                tcpClient.Close();
        }
    }
}
