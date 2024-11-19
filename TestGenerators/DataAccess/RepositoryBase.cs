using Microsoft.Data.Sqlite;

namespace TestGenerators.DataAccess
{
    public abstract class RepositoryBase<T>
    {
        private readonly string _connectionString;

        internal abstract string QueryGetAll { get; }

        internal abstract string QueryGetOne { get; }

        internal abstract string QueryUpdate { get; }

        internal abstract string QueryInsert { get; }

        internal abstract string QueryDelete { get; }

        protected RepositoryBase(string connectionString)
        {
            _connectionString = connectionString;
        }

        internal abstract T GetElement(SqliteDataReader reader);

        internal IDictionary<string, Func<T, object>> QueryParameters = new Dictionary<string, Func<T, object>>();

        public virtual IEnumerable<T> GetAll()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            using SqliteCommand command = new(QueryGetAll, connection);
            using SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                yield return GetElement(reader);
            }
        }

        public virtual T? GetElement(int id)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            using SqliteCommand command = new(QueryGetOne, connection);
            command.Parameters.AddWithValue("@ID", id);
            using SqliteDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                return GetElement(reader);
            }
            else
            {
                return default;
            }
        }

        public virtual void Delete(int id)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            using SqliteCommand command = new(QueryDelete, connection);
            command.Parameters.AddWithValue("@ID", id);
            command.ExecuteNonQuery();
        }

        public virtual void Update(T element, int id)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            using SqliteCommand command = new(QueryUpdate, connection);
            command.Parameters.AddWithValue("@ID", id);
            SetParameters(command, element);
            command.ExecuteNonQuery();
        }

        public virtual void Insert(T element)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            using SqliteCommand command = new(QueryInsert, connection);
            SetParameters(command, element);
            command.ExecuteNonQuery();
        }

        private void SetParameters(SqliteCommand sqliteCommand, T element)
        {
            foreach (var parameter in QueryParameters)
            {
                sqliteCommand.Parameters.AddWithValue("@" + parameter.Key, parameter.Value?.Invoke(element));
            }
        }
    }
}
