using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClienteServidorTiago
{
    public class Report<T>
    {
        private readonly TaskScheduler _taskScheduler;
        private readonly Action<T> _handler;

        public Report(Action<T> handler)
        {
            _taskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            _handler = handler;
        }
        public void Reportar(T value)
        {
            Task.Factory.StartNew(() => _handler(value),
                CancellationToken.None,
                TaskCreationOptions.None,
                _taskScheduler);
            SalvarMensagem(value.ToString());
        }

        public void SalvarMensagem(string value)
        {
            List<string> s = new List<string>(
                value.Split(new string[] { ": " }, StringSplitOptions.None));

            if (s.Count >= 2)
                BancoDeDados.SalvarMensagem(s);
            else
                return;

        }
    }
}
