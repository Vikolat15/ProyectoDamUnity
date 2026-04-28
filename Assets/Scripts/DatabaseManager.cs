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

                // --- NUEVA TABLA: progreso de niveles desbloqueados ---
                // nivelId: 0=Tutorial, 1=Nivel1, 2=Nivel2, 3=Nivel3
                // completado: 1 si el jugador ya lo ha superado
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
                        command.CommandText = "INSERT OR IGNORE INTO Juego (id, nombre, version) VALUES (0, 'LogFall', '1.0');";
                        command.ExecuteNonQuery();

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

                        command.CommandText = "INSERT OR IGNORE INTO Bala (id, idpersonaje, dano) VALUES (0, 0, 50)";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO Bala (id, idpersonaje, dano) VALUES (0, 0, 40);";
                        command.ExecuteNonQuery();
                        command.CommandText = "INSERT OR IGNORE INTO Bala (id, idpersonaje, dano) VALUES (0, 0, 35);";
                        command.ExecuteNonQuery();

                        // --- Inicializar progreso: Tutorial siempre disponible (completado=0),
                        //     el resto bloqueados hasta que se complete el anterior ---
                        // nivelId 0 = Tutorial  (siempre desbloqueado, empieza sin completar)
                        // nivelId 1 = Nivel 1   (bloqueado hasta completar Tutorial)
                        // nivelId 2 = Nivel 2   (bloqueado hasta completar Nivel 1)
                        // nivelId 3 = Nivel 3   (bloqueado hasta completar Nivel 2)
                        // Tutorial empieza como completado para que el Nivel 1 esté desbloqueado desde el inicio
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

    // ---------------------------------------------------------------
    //  PROGRESO DE NIVELES
    // ---------------------------------------------------------------

    /// <summary>
    /// Devuelve true si el nivel con el id dado está desbloqueado.
    /// El Tutorial (nivelId=0) siempre está desbloqueado.
    /// El resto se desbloquean cuando el nivel anterior está completado.
    /// </summary>
    public bool IsNivelDesbloqueado(int nivelId)
    {
        if (nivelId == 0) return true; // Tutorial siempre accesible

        // Comprueba si el nivel anterior está completado
        int nivelAnterior = nivelId - 1;
        return GetNivelCompletado(nivelAnterior);
    }

    /// <summary>
    /// Devuelve true si el nivel indicado ya fue completado.
    /// </summary>
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

    /// <summary>
    /// Marca el nivel indicado como completado en la base de datos.
    /// </summary>
    public void MarcarNivelCompletado(int nivelId)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"UPDATE ProgresoNivel SET completado = 1 WHERE nivelId = {nivelId};";
                command.ExecuteNonQuery();
                Debug.Log($"Nivel {nivelId} marcado como completado.");
            }
        }
    }

    // ---------------------------------------------------------------
    //  MÉTODOS EXISTENTES (sin cambios)
    // ---------------------------------------------------------------

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