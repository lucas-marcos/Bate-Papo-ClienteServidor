using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteServidorTiago
{
    public static class BancoDeDados
    {
        private static SQLiteConnection conexao;

        private static SQLiteConnection ConexaoBanco()
        {
            conexao = new SQLiteConnection($"Data Source={AppDomain.CurrentDomain.BaseDirectory}\\ClienteServidorData.db");
            conexao.Open();
            return conexao;
        }

        public static void SalvarMensagem(List<string> mensagem)
        {
            try
            {
                var cmd = ConexaoBanco().CreateCommand();
                cmd.CommandText = $"INSERT INTO BatePapo (Usuario, Mensagem) VALUES ('{ mensagem[0]}', '{ mensagem[1]}')";
                cmd.ExecuteNonQuery();
                ConexaoBanco().Close();
            }
            catch (Exception ex)
            {
                ConexaoBanco().Close();
            }

        }
    }
}
