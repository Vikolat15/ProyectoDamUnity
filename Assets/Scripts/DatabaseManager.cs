using Mono.Data.Sqlite;
using System.Data;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    private string connectionString;

    void Start()
    {
        connectionString = "URI=file:" + Application.persistentDataPath + "/GameData.db";
        Debug.Log("Base de datos en: " + Application.persistentDataPath);

        CrearEstructuraBaseDeDatos();
        AnadirCamposBase();

        GetDanoBala(0,0);
        GetDanoEnemigo(0,0);
        GetSaludEnemigo(0,0);
        GetSaludPersonaje(0,0);
    }

    public void CrearEstructuraBaseDeDatos()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "PRAGMA foreign_keys = ON;";
                command.ExecuteNonQuery();

                command.CommandText = @"CREATE TABLE IF NOT EXISTS Juego (
                    id INTEGER PRIMARY KEY,
                    nombre TEXT,
                    version TEXT
                );";
                command.ExecuteNonQuery();

                command.CommandText = @"CREATE TABLE IF NOT EXISTS Nivel (
                    id INTEGER PRIMARY KEY,
                    idjuego INTEGER,
                    nombre TEXT,
                    puntuacionMaxima INTEGER,
                    FOREIGN KEY (idjuego) REFERENCES Juego(id) ON DELETE CASCADE
                );";
                command.ExecuteNonQuery();

                command.CommandText = @"CREATE TABLE IF NOT EXISTS Personaje (
                    id INTEGER PRIMARY KEY,
                    idnivel INTEGER,
                    salud INTEGER,
                    FOREIGN KEY (idnivel) REFERENCES Nivel(id) ON DELETE CASCADE
                );";
                command.ExecuteNonQuery();

                command.CommandText = @"CREATE TABLE IF NOT EXISTS Enemigo (
                    id INTEGER PRIMARY KEY,
                    idnivel INTEGER,
                    salud INTEGER,
                    dano INTEGER,
                    FOREIGN KEY (idnivel) REFERENCES Nivel(id) ON DELETE CASCADE
                );";
                command.ExecuteNonQuery();

                command.CommandText = @"CREATE TABLE IF NOT EXISTS Bala (
                    id INTEGER PRIMARY KEY,
                    idpersonaje INTEGER,
                    dano INTEGER,
                    FOREIGN KEY (idpersonaje) REFERENCES Personaje(id) ON DELETE CASCADE
                );";
                command.ExecuteNonQuery();

                Debug.Log("Tablas insertadas");
            }
        }
    }

    public void AnadirCamposBase()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            using (var transaction = connection.BeginTransaction())
            {
                using (var command = connection.CreateCommand())
                {
                    try
                    {
                        command.CommandText = "INSERT OR IGNORE INTO Juego (id, nombre, version) VALUES (0, 'LogFall', '1.0');";
                        command.ExecuteNonQuery();

                        command.CommandText = "INSERT OR IGNORE INTO Nivel (id, idjuego, nombre, puntuacionMaxima) VALUES (0, 0, 'Tutorial', 0);";
                        command.ExecuteNonQuery();

                        command.CommandText = "INSERT OR IGNORE INTO Personaje (id, idnivel, salud) VALUES (0, 0, 100);";
                        command.ExecuteNonQuery();

                        command.CommandText = "INSERT OR IGNORE INTO Enemigo (id, idnivel, salud, dano) VALUES (0, 0, 300, 30);";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO Enemigo (id, idnivel, salud, dano) VALUES (1, 0, 300, 50);";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO Enemigo (id, idnivel, salud, dano) VALUES (2, 0, 200, 25);";
                        command.ExecuteNonQuery();

                        command.CommandText = "INSERT OR IGNORE INTO Bala (id, idpersonaje, dano) VALUES (0, 0, 50)";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO Bala (id, idpersonaje, dano) VALUES (0, 0, 40);";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO Bala (id, idpersonaje, dano) VALUES (0, 0, 45);";
                        command.ExecuteNonQuery();

                        transaction.Commit();
                        Debug.Log("Datos añadidos");
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError("Error en base de datos" + e.Message);
                        transaction.Rollback();
                    }
                }
            }
        }
    }

    public int GetSaludPersonaje(int id, int idNivel)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT salud FROM Personaje WHERE id = {id} AND idnivel = {idNivel};";
                object result = command.ExecuteScalar();
                return result != null ? System.Convert.ToInt32(result) : 0;
            }
        }
    }

    public int GetSaludEnemigo(int id, int idNivel)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT salud FROM Enemigo WHERE id = {id} AND idnivel = {idNivel};";
                object result = command.ExecuteScalar();
                return result != null ? System.Convert.ToInt32(result) : 0;
            }
        }
    }

    public int GetDanoEnemigo(int id, int idNivel)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT dano FROM Enemigo WHERE id = {id} AND idnivel = {idNivel};";
                object result = command.ExecuteScalar();
                return result != null ? System.Convert.ToInt32(result) : 0;
            }
        }
    }

    public int GetDanoBala(int id, int idPersonaje)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT dano FROM Bala WHERE id = {id} AND idpersonaje = {idPersonaje};";
                object result = command.ExecuteScalar();
                return result != null ? System.Convert.ToInt32(result) : 0;
            }
        }
    }

    public int GetPuntuacionNivel(int id, int idJuego)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT puntuacionMaxima FROM Nivel WHERE id = {id} AND idjuego = {idJuego};";
                
                object result = command.ExecuteScalar();
                
                return result != null ? System.Convert.ToInt32(result) : 0;
            }
        }
    }

    public void insertarPunutuacionMaxima(int id, int idJuego, string nombre, int puntos)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $@"
                    INSERT OR REPLACE INTO Nivel (id, idjuego, nombre, puntuacionMaxima) 
                    VALUES ({id}, {idJuego}, '{nombre}', {puntos});";
                
                command.ExecuteNonQuery();
            }
        }
    }
}