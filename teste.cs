using System;
using System.Data.SqlClient;

class Program
{
    static void Main()
    {
        var connectionString = "Server=localhost;Database=AppTDS;Trusted_Connection=True;TrustServerCertificate=True;";
        using (var connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                Console.WriteLine("Conexão bem-sucedida!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao conectar: {ex.Message}");
            }
        }
    }
}
