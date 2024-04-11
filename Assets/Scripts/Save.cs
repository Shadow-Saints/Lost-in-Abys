 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System.Data.Common;
using System.Linq;

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
            dbCommandCreateTable.CommandText = "CREATE TABLE IF NOT EXISTS VALVULAS( id INTEGER PRIMARY KEY, value BOOL, valveMultiply BOOL);\r\nCREATE TABLE IF NOT EXISTS HUD (id INTEGER PRIMARY KEY, value REAL, show BOOL);";
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
        insertHudCommand.ExecuteNonQuery();
        Close();
    }

    public IDataReader ReadHudValues(int id)
    {
        db = CreateAndOpenDatabase();
        IDbCommand readCommand = db.CreateCommand();
        readCommand.CommandText = "SELECT * FROM HUD WHERE id LIKE " + id + ";";
        IDataReader reader = readCommand.ExecuteReader();
        return reader;
    }
    #endregion

    

}
