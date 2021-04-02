using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClienteServidorTiago
{
    class Servidor
    {
        static TcpListener tcpListener;
        List<ClienteModel> clientes = new List<ClienteModel>();
        private readonly Report<string> _report;

        public Servidor(Report<string> report)
        {
            _report = report;
        }
        protected internal void AddConnection(ClienteModel clienteModel)
        {
            if (!clientes.Any(c => c.nickName.Equals(clienteModel.nickName)))
                clientes.Add(clienteModel);
            else
                _report.Reportar("Desculpe, o apelido já está sendo utilizado! Por favor escolha outro:");
                Console.WriteLine("*** Desculpe, o apelido já está sendo utilizado! Por favor escolha outro:");
        }
        protected internal void RemoveConnection(string id)
        {
            ClienteModel cliente = clientes.Where(c => c.Id.Equals(id)).FirstOrDefault();
            if (cliente != null)
                clientes.Remove(cliente);
        }
        protected internal void Listen()
        {
            try
            {
                tcpListener = new TcpListener(IPAddress.Any, 8888);
                tcpListener.Start();
                _report.Reportar("O servidor está em execução! Aguardando conexões...");
                while (true)
                {
                    TcpClient tcpClient = tcpListener.AcceptTcpClient();

                    ClienteModel clienteModel = new ClienteModel(tcpClient, this, _report);
                    Thread clientThread = new Thread(new ThreadStart(clienteModel.Process));
                    clientThread.Start();
                }

            }
            catch (Exception exception)
            {
                _report.Reportar(exception.Message);
                Console.WriteLine(exception.Message);
                Disconnect();
            }
        }

        protected internal void Disconnect()
        {
            tcpListener.Stop();

            for (int i = 0; i < clientes.Count; i++)
            {
                clientes[i].Close();
            }
            Environment.Exit(0);
        }
    }
}
