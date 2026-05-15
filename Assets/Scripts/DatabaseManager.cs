using Mono.Data.Sqlite;
using System.Data;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager Instance { get; private set; }

    private string connectionString;

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
        connectionString = "URI=file:" + Application.persistentDataPath + "/GameData.db";

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
                    version TEXT,
                    nombrePerfil TEXT
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

                command.CommandText = @"CREATE TABLE IF NOT EXISTS ProgresoNivel (
                    nivelId INTEGER PRIMARY KEY,
                    completado INTEGER NOT NULL DEFAULT 0
                );";
                command.ExecuteNonQuery();

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
                    command.CommandText = "INSERT OR IGNORE INTO Juego (id, nombre, version, nombrePerfil) VALUES (0, 'LogFall', '1.0', 'Default');";
                    command.ExecuteNonQuery();

                    // NIVEL 0
                    command.CommandText = "INSERT OR IGNORE INTO Nivel (id, idjuego, nombre, puntuacionMaxima) VALUES (0, 0, 'Tutorial', 0);";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT OR IGNORE INTO Personaje (id, idnivel, salud) VALUES (0, 0, 100);";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT OR IGNORE INTO Enemigo (id, idnivel, salud, dano) VALUES (0, 0, 200, 35);";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT OR IGNORE INTO Enemigo (id, idnivel, salud, dano) VALUES (1, 0, 200, 35);";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT OR IGNORE INTO Enemigo (id, idnivel, salud, dano) VALUES (2, 0, 150, 25);";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT OR IGNORE INTO Enemigo (id, idnivel, salud, dano) VALUES (3, 0, 200, 35);";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT OR IGNORE INTO Enemigo (id, idnivel, salud, dano) VALUES (4, 0, 300, 50);";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT OR IGNORE INTO Bala (id, idpersonaje, dano) VALUES (0, 0, 50);";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT OR IGNORE INTO Bala (id, idpersonaje, dano) VALUES (1, 0, 40);";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT OR IGNORE INTO Bala (id, idpersonaje, dano) VALUES (2, 0, 35);";
                    command.ExecuteNonQuery();


                    // NIVEL 1
                    command.CommandText = "INSERT OR IGNORE INTO Nivel (id, idjuego, nombre, puntuacionMaxima) VALUES (1, 0, 'Nivel 1', 0);";
                    command.ExecuteNonQuery();

                    // Se corrigió el id del personaje a 1 para que coincida con sus balas asignadas
                    command.CommandText = "INSERT OR IGNORE INTO Personaje (id, idnivel, salud) VALUES (0, 1, 100);";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT OR IGNORE INTO Enemigo (id, idnivel, salud, dano) VALUES (0, 1, 200, 35);";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT OR IGNORE INTO Enemigo (id, idnivel, salud, dano) VALUES (1, 1, 200, 35);";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT OR IGNORE INTO Enemigo (id, idnivel, salud, dano) VALUES (2, 1, 150, 25);";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT OR IGNORE INTO Enemigo (id, idnivel, salud, dano) VALUES (3, 1, 200, 35);";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT OR IGNORE INTO Enemigo (id, idnivel, salud, dano) VALUES (4, 1, 300, 50);";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT OR IGNORE INTO Bala (id, idpersonaje, dano) VALUES (0, 1, 50);";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT OR IGNORE INTO Bala (id, idpersonaje, dano) VALUES (1, 1, 40);";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT OR IGNORE INTO Bala (id, idpersonaje, dano) VALUES (2, 1, 35);";
                    command.ExecuteNonQuery();


                    // NIVEL 2
                    command.CommandText = "INSERT OR IGNORE INTO Nivel (id, idjuego, nombre, puntuacionMaxima) VALUES (2, 0, 'Nivel 2', 0);";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT OR IGNORE INTO Personaje (id, idnivel, salud) VALUES (0, 2, 100);";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT OR IGNORE INTO Enemigo (id, idnivel, salud, dano) VALUES (0, 2, 200, 35);";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT OR IGNORE INTO Enemigo (id, idnivel, salud, dano) VALUES (1, 2, 200, 35);";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT OR IGNORE INTO Enemigo (id, idnivel, salud, dano) VALUES (2, 2, 150, 25);";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT OR IGNORE INTO Enemigo (id, idnivel, salud, dano) VALUES (3, 2, 200, 35);";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT OR IGNORE INTO Enemigo (id, idnivel, salud, dano) VALUES (4, 2, 300, 50);";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT OR IGNORE INTO Bala (id, idpersonaje, dano) VALUES (0, 2, 50);";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT OR IGNORE INTO Bala (id, idpersonaje, dano) VALUES (1, 2, 40);";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT OR IGNORE INTO Bala (id, idpersonaje, dano) VALUES (2, 2, 35);";
                    command.ExecuteNonQuery();


                    // NIVEL 3
                    command.CommandText = "INSERT OR IGNORE INTO Nivel (id, idjuego, nombre, puntuacionMaxima) VALUES (3, 0, 'Nivel 3', 0);";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT OR IGNORE INTO Personaje (id, idnivel, salud) VALUES (0, 3, 100);";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT OR IGNORE INTO Enemigo (id, idnivel, salud, dano) VALUES (0, 3, 200, 35);";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT OR IGNORE INTO Enemigo (id, idnivel, salud, dano) VALUES (1, 3, 200, 35);";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT OR IGNORE INTO Enemigo (id, idnivel, salud, dano) VALUES (2, 3, 150, 25);";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT OR IGNORE INTO Enemigo (id, idnivel, salud, dano) VALUES (3, 3, 200, 35);";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT OR IGNORE INTO Enemigo (id, idnivel, salud, dano) VALUES (4, 3, 300, 50);";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT OR IGNORE INTO Bala (id, idpersonaje, dano) VALUES (0, 3, 50);";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT OR IGNORE INTO Bala (id, idpersonaje, dano) VALUES (1, 3, 40);";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT OR IGNORE INTO Bala (id, idpersonaje, dano) VALUES (2, 3, 35);";
                    command.ExecuteNonQuery();

                    // PROGRESO
                    command.CommandText = "INSERT OR IGNORE INTO ProgresoNivel (nivelId, completado) VALUES (0, 1);";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT OR IGNORE INTO ProgresoNivel (nivelId, completado) VALUES (1, 0);";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT OR IGNORE INTO ProgresoNivel (nivelId, completado) VALUES (2, 0);";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT OR IGNORE INTO ProgresoNivel (nivelId, completado) VALUES (3, 0);";
                    command.ExecuteNonQuery();

                    transaction.Commit();
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

    public bool IsNivelDesbloqueado(int nivelId)
    {
        if (nivelId == 0) return true;

        int nivelAnterior = nivelId - 1;
        return GetNivelCompletado(nivelAnterior);
    }

    public bool GetNivelCompletado(int nivelId)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT completado FROM ProgresoNivel WHERE nivelId = {nivelId};";
                object result = command.ExecuteScalar();
                return result != null && System.Convert.ToInt32(result) == 1;
            }
        }
    }

    public void MarcarNivelCompletado(int nivelId)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"UPDATE ProgresoNivel SET completado = 1 WHERE nivelId = {nivelId};";
                command.ExecuteNonQuery();
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

    public void insertarNombrePerfil(int idJuego, string nombre)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"UPDATE Juego SET nombrePerfil = '{nombre}' WHERE id = {idJuego};";
                command.ExecuteNonQuery();
            }
        }
    }

    public string GetNombrePerfil(int idJuego)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT nombrePerfil FROM Juego WHERE id = {idJuego};";
                object result = command.ExecuteScalar();
                
                return (result != null && result != System.DBNull.Value) ? result.ToString() : "Jugador";
            }
        }
    }

    public void insertarPunutuacionMaxima(int idNivel, int puntos)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $@"UPDATE Nivel SET puntuacionMaxima = {puntos} WHERE id = {idNivel} AND {puntos} > puntuacionMaxima;";
                
                int filasAfectadas = command.ExecuteNonQuery();
                
            }
        }
    }
}