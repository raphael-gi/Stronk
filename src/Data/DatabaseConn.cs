using System.Data;
using System.Data.SqlClient;
using Stronk.Models;

namespace Stronk.Data;

public class DatabaseConn
{
    private SqlConnection _sqlConnection = new ("data source=DESKTOP-F6CFEVJ\\MSSQLSERVER01; initial catalog=Stronk;persist security info=True; Integrated Security=SSPI");
    private SqlCommand _sqlCommand;
    public DatabaseConn(string command)
    {
        _sqlCommand = new SqlCommand(command, _sqlConnection);
    }

    public bool Login(User user)
    {
        _sqlCommand.CommandType = CommandType.StoredProcedure;
        _sqlCommand.Parameters.Add("@username", SqlDbType.NVarChar).Value = user.Username;
        _sqlCommand.Parameters.Add("@password", SqlDbType.NVarChar).Value = user.Password;
        _sqlConnection.Open();
        SqlDataReader _sqlDataReader = _sqlCommand.ExecuteReader();
        int result = 0;
        while (_sqlDataReader.Read())
        {
            result = _sqlDataReader.GetInt32(0);
        }
        _sqlConnection.Close();
        return result > 0;
    }
    public bool Register(User user)
    {
        _sqlCommand.CommandType = CommandType.StoredProcedure;
        _sqlCommand.Parameters.Add("@username", SqlDbType.NVarChar).Value = user.Username;
        _sqlCommand.Parameters.Add("@password", SqlDbType.NVarChar).Value = user.Password;
        _sqlConnection.Open();
        int result = _sqlCommand.ExecuteNonQuery();
        _sqlConnection.Close();
        return result > 0;
    }

    public List<Exercise> Exercise()
    {
        _sqlConnection.Open();
        SqlDataReader _sqlDataReader = _sqlCommand.ExecuteReader();
        List<Exercise> exercises = new List<Exercise>();
        while (_sqlDataReader.Read())
        {
            Exercise exercise = new Exercise();
            exercise.Name = _sqlDataReader.GetString(0);
            exercises.Add(exercise);
        }
        _sqlConnection.Close();
        return exercises;
    }
}