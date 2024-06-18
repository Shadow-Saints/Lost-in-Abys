using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System.Globalization;

public class Save : MonoBehaviour
{
    
    [Header("Data Bases")]
    private IDbConnection db;

    #region Primary Functions
    private IDbConnection CreateAndOpenDatabase()
    {

        // Abrindo a conexao com o banco de dados
        string dbUri = "URI=file:MyDatabase.Sqlite";
        IDbConnection dbConnection = new SqliteConnection(dbUri);
        dbConnection.Open();

        // Criando a tabela para as valvulas
        using (var dbCommandCreateTable = dbConnection.CreateCommand())
        {
            dbCommandCreateTable.CommandText = "CREATE TABLE IF NOT EXISTS VALVULAS( id INTEGER PRIMARY KEY NOT NULL, value BOOL, valveMultiply BOOL);";
            dbCommandCreateTable.CommandText += "CREATE TABLE IF NOT EXISTS HUD (id INTEGER PRIMARY KEY NOT NULL, value REAL, show BOOL);";
            dbCommandCreateTable.CommandText += "CREATE TABLE IF NOT EXISTS INVENTORY (id INTEGER PRIMARY KEY NOT NULL, name TEXT,quantity INTEGER,Description TEXT,type TEXT,position INTEGER);";
            dbCommandCreateTable.CommandText += "CREATE TABLE IF NOT EXISTS POSITIONS(id INTEGER PRIMARY KEY NOT NULL, x REAL, y REAL, z REAL)";
            dbCommandCreateTable.ExecuteNonQuery();
        }

        return dbConnection;
    }

    public void Close()
    {
        db.Close();
    }

    #endregion

    #region Valves
    public IDataReader CheckValveValues(int valveID)
    {
        db = CreateAndOpenDatabase();
        IDbCommand dbCommandCheckValve = db.CreateCommand();
        dbCommandCheckValve.CommandText = "SELECT * FROM VALVULAS WHERE id LIKE " + valveID ;
        IDataReader read = dbCommandCheckValve.ExecuteReader();
        return read;
        
    }

    public void UpdateValveValues(int valveID, bool valveValue, bool valveMultiply)
    {
        db = CreateAndOpenDatabase(); 
        IDbCommand dbCommandInsertValve = db.CreateCommand();
        dbCommandInsertValve.CommandText = "INSERT OR REPLACE INTO VALVULAS (id, value, valveMultiply) VALUES ( " + valveID + ", " + valveValue + ", " + valveMultiply + ")";
        dbCommandInsertValve.ExecuteNonQuery();
        db.Close();
    }

    #endregion

    #region Hud
    public void UpdateHudValues(int id, float value, bool show)
    {
        db = CreateAndOpenDatabase();
        IDbCommand insertHudCommand = db.CreateCommand();
        string commandText = "INSERT OR REPLACE INTO HUD(id, value, show) VALUES("+ id.ToString() + "," + value + "," + show +");";
        insertHudCommand.CommandText = commandText;
        Debug.Log(commandText);
        insertHudCommand.ExecuteNonQuery();
        Close();
    }
    
    public IDataReader ReadHudValues(int id)
    {
        db = CreateAndOpenDatabase();
        IDbCommand readCommand = db.CreateCommand();
        readCommand.CommandText = $"SELECT * FROM HUD WHERE id LIKE {id};";
        IDataReader reader = readCommand.ExecuteReader();
        return reader;
    }
    #endregion

    #region Inventory
    public void UpdateInventoryValues(int id, string name, int quantity, string description, string type, int position)
    {
        db = CreateAndOpenDatabase();
        IDbCommand insertInventoryCommand = db.CreateCommand();
        insertInventoryCommand.CommandText = $"INSERT OR REPLACE INTO INVENTORY(id, name, quantity, description, type, position) VALUES({id}, {name}, {quantity}, {description}, {type}, {position});";
        insertInventoryCommand.ExecuteNonQuery();
        Close();
    }

    public IDataReader ReadInventory(int id)
    {
        db = CreateAndOpenDatabase();
        IDbCommand readCommand = db.CreateCommand();
        readCommand.CommandText = $"SELECT * FROM INVENTORY WHERE id Like {id}";
        IDataReader reader = readCommand.ExecuteReader();
        return reader;
    }
    #endregion

    #region Positions
    public void UpdatePossitionValues(int id, float x, float y, float z)
    {
        CultureInfo global = CultureInfo.InvariantCulture;
        db = CreateAndOpenDatabase();
        IDbCommand updateCommand = db.CreateCommand();
        updateCommand.CommandText = $"INSERT OR REPLACE INTO POSITIONS(id, x, y, z) VALUES({id}, {x.ToString(global)}, {y.ToString(global)}, {z.ToString(global)});";
        Debug.Log(updateCommand.CommandText);
        updateCommand.ExecuteNonQuery();
        Close();
    }
    public IDataReader ReadPossition(int id)
    {
        
        db = CreateAndOpenDatabase();
        IDbCommand readCommand = db.CreateCommand();
        readCommand.CommandText = $"SELECT * FROM POSITIONS WHERE id LIKE {id}";
        IDataReader reader = readCommand.ExecuteReader(); 
        return reader;
    }
    #endregion

}