using MySql.Data.MySqlClient;
using System;
using UnityEngine;

public class ServerDatabaseManager : MonoBehaviour
{
    private string connectionString;
    private bool conexionIniciada = false;
    public static ServerDatabaseManager Instance { get; private set; }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); 
            return;
        }
    }

    void Start()
    {

    }

    public bool IniciarConexion(string user, string pass)
    {
        connectionString = $"Server=localhost;Port=3306;Database=LogFall;User ID={user};Password={pass};Pooling=false;";

        if (ProbarConexion())
        {
            conexionIniciada = true;

            CrearTablaPuntuacionesServer();
            AnadirNivelesServer();

            return true;
        }

        conexionIniciada = false;
        return false;
    }

    public bool ProbarConexion()
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                Debug.Log("Conectado a MySQL");
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                return false;
            }
        }
    }

    private bool HayConexion()
    {
        if (!conexionIniciada)
        {
            return false;
        }

        return true;
    }

    public void AnadirNivelesServer()
    {
        if (!HayConexion())
            return;

        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            using (var command = new MySqlCommand())
            {
                command.Connection = connection;

                try
                {
                    command.CommandText = @"
                        INSERT IGNORE INTO TablaNivel (id, nombre) VALUES (0, 'Tutorial');
                        INSERT IGNORE INTO TablaNivel (id, nombre) VALUES (1, 'Nivel1');
                        INSERT IGNORE INTO TablaNivel (id, nombre) VALUES (2, 'Nivel2');
                        INSERT IGNORE INTO TablaNivel (id, nombre) VALUES (3, 'Nivel3');
                    ";

                    command.ExecuteNonQuery();

                }
                catch (Exception e)
                {
                    Debug.LogError("Error insertando niveles: " + e.Message);
                }
            }
        }
    }

    public void CrearTablaPuntuacionesServer()
    {
        if (!HayConexion())
            return;

        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            using (var command = new MySqlCommand())
            {
                command.Connection = connection;

                command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS TablaNivel (
                        id INT PRIMARY KEY,
                        nombre VARCHAR(100)
                    );

                    CREATE TABLE IF NOT EXISTS Tablajugador (
                        id INT AUTO_INCREMENT PRIMARY KEY,
                        nombreJugador VARCHAR(100),
                        idNivel INT,
                        puntuacionMaxima INT DEFAULT 0,
                        UNIQUE KEY (nombreJugador, idNivel),
                        FOREIGN KEY (idNivel) REFERENCES TablaNivel(id)
                    );";

                command.ExecuteNonQuery();

            }
        }
    }

    public void InsertarPuntuacionMaximaServer(string nombreJugador, int idNivel, int puntos)
    {
        if (!HayConexion())
            return;

        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            using (var command = new MySqlCommand())
            {
                command.Connection = connection;

                command.CommandText = @"
                    INSERT INTO Tablajugador (nombreJugador, idNivel, puntuacionMaxima) 
                    VALUES (@nombre, @idNivel, @puntos)
                    ON DUPLICATE KEY UPDATE 
                    puntuacionMaxima = IF(@puntos > puntuacionMaxima, @puntos, puntuacionMaxima);";

                command.Parameters.AddWithValue("@nombre", nombreJugador);
                command.Parameters.AddWithValue("@idNivel", idNivel);
                command.Parameters.AddWithValue("@puntos", puntos);

                command.ExecuteNonQuery();

            }
        }
    }
}