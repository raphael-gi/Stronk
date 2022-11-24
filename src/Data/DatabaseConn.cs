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

    public int Login(User user)
    {
        _sqlCommand.CommandType = CommandType.StoredProcedure;
        _sqlCommand.Parameters.Add("@username", SqlDbType.NVarChar).Value = user.Username;
        _sqlCommand.Parameters.Add("@password", SqlDbType.NVarChar).Value = user.Password;
        _sqlConnection.Open();
        SqlDataReader sqlDataReader = _sqlCommand.ExecuteReader();
        int id = -1;
        while (sqlDataReader.Read())
        {
            id = sqlDataReader.GetInt32(0);
        }
        _sqlConnection.Close();
        return id;
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
        SqlDataReader sqlDataReader = _sqlCommand.ExecuteReader();
        List<Exercise> exercises = new List<Exercise>();
        while (sqlDataReader.Read())
        {
            Exercise exercise = new Exercise();
            Muscle muscle = new Muscle();
            List<Muscle> muscles = new List<Muscle>();
            exercise.Name = sqlDataReader.GetString(0);
            muscle.Name = sqlDataReader.GetString(2);
            muscles.Add(muscle);
            exercise.Muscles = muscles;
            exercises.Add(exercise);
        }
        _sqlConnection.Close();
        return exercises;
    }
}