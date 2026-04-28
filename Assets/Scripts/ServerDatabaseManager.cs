using MySql.Data.MySqlClient; 
using System;
using UnityEngine;

public class ServerDatabaseManager : MonoBehaviour
{
    // Asegúrate de que la base de datos 'LogFall' ya esté creada en phpMyAdmin
    private string connectionString = "Server=localhost;Port=3306;Database=LogFall;User ID=root;Password=;Pooling=false;";

    public static ServerDatabaseManager Instance { get; private set; }


    void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject); // Opcional: para que no muera al cambiar de escena
            }
            else
            {
                Destroy(gameObject); // Evita que haya dos gestores de servidor a la vez
                return;
            }
        }
    void Start()
    {
            CrearTablaPuntuacionesServer();
    }

    public bool ProbarConexion()
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            try {
                connection.Open();
                Debug.Log("Conexión exitosa a MySQL");
                return true;
            } catch (Exception e) {
                Debug.LogError("Error conectando al servidor: " + e.Message);
                return false;
            }
        }
    }

    // Crea la tabla si no existe
    public void CrearTablaPuntuacionesServer()
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            using (var command = new MySqlCommand())
            {
                command.Connection = connection;
                // En MySQL usamos INT y PRIMARY KEY para el ID
                command.CommandText = @"CREATE TABLE IF NOT EXISTS Puntuaciones (
                    id_nivel INT PRIMARY KEY,
                    id_juego INT,
                    nombre_nivel VARCHAR(100),
                    puntuacion_maxima INT DEFAULT 0
                );";
                command.ExecuteNonQuery();

            }
        }
    }

    // Inserta o actualiza la puntuación solo si es mayor a la actual
    public void InsertarPuntuacionMaximaServer(int idNivel, int idJuego, string nombre, int puntos)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            using (var command = new MySqlCommand())
            {
                command.Connection = connection;

                // Usamos la sintaxis ON DUPLICATE KEY UPDATE de MySQL
                // Esto intenta insertar, y si el id_nivel ya existe, ejecuta el UPDATE
                command.CommandText = $@"
                    INSERT INTO Puntuaciones (id_nivel, id_juego, nombre_nivel, puntuacion_maxima) 
                    VALUES ({idNivel}, {idJuego}, '{nombre}', {puntos})
                    ON DUPLICATE KEY UPDATE 
                    puntuacion_maxima = IF({puntos} > puntuacion_maxima, {puntos}, puntuacion_maxima);";
                
                int result = command.ExecuteNonQuery();
                
                if (result == 1) Debug.Log("Nueva puntuación creada.");
                else if (result == 2) Debug.Log("Puntuación actualizada (si era mayor).");
                else Debug.Log("No hubo cambios en la puntuación.");
            }
        }
    }
}