using MySql.Data.MySqlClient;
using System;
using GestorFinanceiro.Database;
using GestorFinanceiro;

public class HistoricoManager
{
    private Db db = new Db();

    // Método para inserir histórico no banco de dados
    public void InserirHistorico(string nome, decimal valor)
    {
        using (MySqlConnection conn = db.GetConnection())
        {
            conn.Open();
            string query = "INSERT INTO historico (nome, valor) VALUES (@nome, @valor)";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@nome", nome);
                cmd.Parameters.AddWithValue("@valor", valor);
                cmd.ExecuteNonQuery();
            }
        }

        Console.WriteLine("Histórico inserido com sucesso!");
    }

    // Método para somar todos os valores da coluna "valor" na tabela "historico"
    public decimal SomarValores()
    {
        decimal total = 0;

        using (MySqlConnection conn = db.GetConnection())
        {
            try
            {
                conn.Open();
                string query = "SELECT SUM(valor) FROM historico";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    var result = cmd.ExecuteScalar();

                    if (result != DBNull.Value)
                    {
                        total = Convert.ToDecimal(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao acessar o banco de dados: " + ex.Message);
            }
        }

        // Para depuração, vamos imprimir o valor somado no console
        Console.WriteLine("Total somado: " + total);

        return total;
    }

    public List<Transacao> RecentesTransacoes()
    {
        var transacoes = new List<Transacao>();

        using (MySqlConnection conn = db.GetConnection())
        {
            try
            {
                conn.Open();
                string query = "SELECT nome, valor, data FROM historico ORDER BY id DESC LIMIT 3;";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var transacao = new Transacao
                        {
                            Nome = reader.GetString("nome"),
                            Valor = reader.GetDecimal("valor"),
                            Data = reader.GetDateTime("data")
                        };

                        transacoes.Add(transacao);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao acessar o banco de dados: " + ex.Message);
            }
        }

        return transacoes;
    }
}
