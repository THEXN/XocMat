﻿
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace Lagrange.XocMat.Extensions;

/// <summary>
/// Database extensions
/// </summary>
public static class DbExt
{
    /// <summary>
    /// Executes a query on a database.
    /// </summary>
    /// <param name="olddb">Database to query</param>
    /// <param name="query">Query string with parameters as @0, @1, etc.</param>
    /// <param name="args">Parameters to be put in the query</param>
    /// <returns>Rows affected by query</returns>
    [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
    public static int Query(this IDbConnection olddb, string query, params object[] args)
    {
        using (var db = olddb.CloneEx())
        {
            db.Open();
            using (var com = db.CreateCommand())
            {
                com.CommandText = query;
                for (int i = 0; i < args.Length; i++)
                    com.AddParameter("@" + i, args[i] ?? DBNull.Value);
                return com.ExecuteNonQuery();
            }
        }
    }

    /// <summary>
    /// Executes a query on a database.
    /// </summary>
    /// <param name="olddb">Database to query</param>
    /// <param name="query">Query string with parameters as @0, @1, etc.</param>
    /// <param name="args">Parameters to be put in the query</param>
    /// <returns>Query result as IDataReader</returns>
    [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
    public static QueryResult QueryReader(this IDbConnection olddb, string query, params object[] args)
    {
        var db = olddb.CloneEx();
        try
        {
            db.Open();
            var com = db.CreateCommand(); // this will be disposed via the QueryResult instance
            {
                com.CommandText = query;
                for (int i = 0; i < args.Length; i++)
                    com.AddParameter("@" + i, args[i]);

                return new QueryResult(db, com.ExecuteReader(), com);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("failed to connect to MySQL database. See inner exception for details.", ex);
        }
    }

    /// <summary>
    /// Executes a query on a database, returning the first column of the first row of the result set.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="olddb">Database to query</param>
    /// <param name="query">Query string with parameters as @0, @1, etc.</param>
    /// <param name="args">Parameters to be put in the query</param>
    /// <returns></returns>
    public static T QueryScalar<T>(this IDbConnection olddb, string query, params object[] args)
    {
        using (var db = olddb.CloneEx())
        {
            db.Open();
            using (var com = db.CreateCommand())
            {
                com.CommandText = query;
                for (int i = 0; i < args.Length; i++)
                    com.AddParameter("@" + i, args[i]);

                object output = com.ExecuteScalar();
                if (output.GetType() != typeof(T))
                {
                    if (typeof(IConvertible).IsAssignableFrom(output.GetType()))
                    {
                        return (T)Convert.ChangeType(output, typeof(T));
                    }
                }

                return (T)output;
            }
        }
    }

    public static QueryResult QueryReaderDict(this IDbConnection olddb, string query, Dictionary<string, object> values)
    {
        var db = olddb.CloneEx();
        db.Open();
        var com = db.CreateCommand(); // this will be disposed via the QueryResult instance
        {
            com.CommandText = query;
            foreach (var kv in values)
                com.AddParameter("@" + kv.Key, kv.Value);

            return new QueryResult(db, com.ExecuteReader(), com);
        }
    }

    public static IDbDataParameter AddParameter(this IDbCommand command, string name, object data)
    {
        var parm = command.CreateParameter();
        parm.ParameterName = name;
        parm.Value = data;
        command.Parameters.Add(parm);
        return parm;
    }

    public static IDbConnection CloneEx(this IDbConnection conn)
    {
        var clone = (IDbConnection)Activator.CreateInstance(conn.GetType());
        clone.ConnectionString = conn.ConnectionString;
        return clone;
    }

    public static SqlType GetSqlType(this IDbConnection conn)
    {
        var name = conn.GetType().Name;
        if (name == "SqliteConnection" || name == "SQLiteConnection")
            return SqlType.Sqlite;
        if (name == "MySqlConnection")
            return SqlType.Mysql;
        return SqlType.Unknown;
    }

    private static readonly Dictionary<Type, Func<IDataReader, int, object>> ReadFuncs = new()
    {
        {
            typeof (bool),
            (s, i) => s.GetBoolean(i)
        },
        {
            typeof (bool?),
            (s, i) => s.IsDBNull(i) ? null : s.GetBoolean(i)
        },
        {
            typeof (byte),
            (s, i) => s.GetByte(i)
        },
        {
            typeof (byte?),
            (s, i) => s.IsDBNull(i) ? null : s.GetByte(i)
        },
        {
            typeof (short),
            (s, i) => s.GetInt16(i)
        },
        {
            typeof (short?),
            (s, i) => s.IsDBNull(i) ? null : s.GetInt16(i)
        },
        {
            typeof (int),
            (s, i) => s.GetInt32(i)
        },
        {
            typeof (int?),
            (s, i) => s.IsDBNull(i) ? null : s.GetInt32(i)
        },
        {
            typeof (long),
            (s, i) => s.GetInt64(i)
        },
        {
            typeof (long?),
            (s, i) => s.IsDBNull(i) ? null : s.GetInt64(i)
        },
        {
            typeof (string),
            (s, i) => s.GetString(i)
        },
        {
            typeof (decimal),
            (s, i) => s.GetDecimal(i)
        },
        {
            typeof (decimal?),
            (s, i) => s.IsDBNull(i) ? null : s.GetDecimal(i)
        },
        {
            typeof (float),
            (s, i) => s.GetFloat(i)
        },
        {
            typeof (float?),
            (s, i) => s.IsDBNull(i) ? null : s.GetFloat(i)
        },
        {
            typeof (double),
            (s, i) => s.GetDouble(i)
        },
        {
            typeof (double?),
            (s, i) => s.IsDBNull(i) ? null : s.GetDouble(i)
        },
        {
            typeof (DateTime),
            (s, i) => s.IsDBNull(i) ? null : s.GetDateTime(i)
        },
        {
            typeof (object),
            (s, i) => s.GetValue(i)
        },
    };

    public static T Get<T>(this IDataReader reader, string column)
    {
        return reader.Get<T>(reader.GetOrdinal(column));
    }

    public static T Get<T>(this IDataReader reader, int column)
    {
        if (reader.IsDBNull(column))
            return default;

        if (ReadFuncs.ContainsKey(typeof(T)))
            return (T)ReadFuncs[typeof(T)](reader, column);

        Type t;
        if (typeof(T) != (t = reader.GetFieldType(column)))
        {
            string columnName = reader.GetName(column);
            throw new InvalidCastException($"Received type '{typeof(T).Name}', however column '{columnName}' expects type '{t.Name}'");
        }

        if (reader.IsDBNull(column))
        {
            return default;
        }

        return (T)reader.GetValue(column);
    }
}

public enum SqlType
{
    Unknown,
    Sqlite,
    Mysql
}

public class QueryResult : IDisposable
{
    public IDbConnection Connection { get; protected set; }
    public IDataReader Reader { get; protected set; }
    public IDbCommand Command { get; protected set; }

    public QueryResult(IDbConnection conn, IDataReader reader, IDbCommand command)
    {
        Connection = conn;
        Reader = reader;
        Command = command;
    }

    ~QueryResult()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (Reader != null)
            {
                Reader.Dispose();
                Reader = null;
            }
            if (Command != null)
            {
                Command.Dispose();
                Command = null;
            }
            if (Connection != null)
            {
                Connection.Dispose();
                Connection = null;
            }
        }
    }

    public bool Read()
    {
        if (Reader == null)
            return false;
        return Reader.Read();
    }

    public T Get<T>(string column)
    {
        if (Reader == null)
            return default;
        return Reader.Get<T>(Reader.GetOrdinal(column));
    }
}
