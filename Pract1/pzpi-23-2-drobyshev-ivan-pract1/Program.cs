// 1. Продукт — складний об'єкт, який ми будуємо
public class SqlQuery
{
    public string Table { get; set; } = "";
    public List<string> Columns { get; set; } = new List<string>();
    public string WhereClause { get; set; } = "";
    public string OrderByClause { get; set; } = "";
    public override string ToString()
    {
        var cols = Columns.Any() ? string.Join(", ", Columns) : "*";
        var query = $"SELECT {cols} FROM {Table}";
        
        if (!string.IsNullOrEmpty(WhereClause))
            query += $" WHERE {WhereClause}";
            
        if (!string.IsNullOrEmpty(OrderByClause))
            query += $" ORDER BY {OrderByClause}";

        return query + ";";
    }
}
// 2. Інтерфейс Будівельника
public interface ISqlQueryBuilder
{
    ISqlQueryBuilder SelectColumns(params string[] columns);
    ISqlQueryBuilder FromTable(string table);
    ISqlQueryBuilder Where(string condition);
    ISqlQueryBuilder OrderBy(string column);
    SqlQuery Build();
}

// 3. Конкретний Будівельник
public class MySqlQueryBuilder : ISqlQueryBuilder
{
    private SqlQuery _query = new SqlQuery();
    public ISqlQueryBuilder SelectColumns(params string[] columns)
    {
        _query.Columns.AddRange(columns);
        return this; // Повертаємо current object для ланцюжка методів
    }
    public ISqlQueryBuilder FromTable(string table)
    {
        _query.Table = table;
        return this;
    }
    public ISqlQueryBuilder Where(string condition)
    {
        _query.WhereClause = condition;
        return this;
    }
    public ISqlQueryBuilder OrderBy(string column)
    {
        _query.OrderByClause = column;
        return this;
    }
    public SqlQuery Build()
    {
        var result = _query;
        _query = new SqlQuery(); // Скидання для наступного використання
        return result;
    }
}

// 4. Демонстрація (результат виконання)
class Program
{
    static void Main(string[] args)
    {
        var builder = new MySqlQueryBuilder();

        // Використання Fluent API
        var query = builder
            .FromTable("Users")
            .SelectColumns("Id", "Name", "Email")
            .Where("Age > 18")
            .OrderBy("Name")
            .Build();

        Console.WriteLine("Generated SQL Query:");
        Console.WriteLine(query.ToString());
    }
}