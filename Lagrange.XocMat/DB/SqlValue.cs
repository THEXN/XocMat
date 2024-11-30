﻿using Lagrange.XocMat.Extensions;
using System.Data;

namespace Lagrange.XocMat.DB;

public class SqlValue
{
    public string Name { get; set; }
    public object Value { get; set; }

    public SqlValue(string name, object value)
    {
        Name = name;
        Value = value;
    }
}

public class SqlTableEditor
{
    private readonly IDbConnection database;
    private readonly IQueryBuilder creator;

    public SqlTableEditor(IDbConnection db, IQueryBuilder provider)
    {
        database = db;
        creator = provider;
    }

    public void UpdateValues(string table, List<SqlValue> values, List<SqlValue> wheres)
    {
        database.Query(creator.UpdateValue(table, values, wheres));
    }

    public void InsertValues(string table, List<SqlValue> values)
    {
        database.Query(creator.InsertValues(table, values));
    }

    public List<object> ReadColumn(string table, string column, List<SqlValue> wheres)
    {
        List<object> values = [];

        using (var reader = database.QueryReader(creator.ReadColumn(table, wheres)))
        {
            while (reader.Read())
                values.Add(reader.Reader.Get<object>(column));
        }

        return values;
    }
}