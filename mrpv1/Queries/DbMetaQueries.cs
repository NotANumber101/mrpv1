namespace mrpv1.Queries;
public class DbMetaQueries
{
    public static string GetDbNamesQuery()
    {
        return "SELECT datname FROM pg_database WHERE datistemplate = false;";
    }
    public static string GetDbTableNamesQuery()
    {
        return "SELECT table_name, table_type "
            + "FROM information_schema.tables WHERE table_schema "
            + "NOT IN ('pg_catalog', 'information_schema') "
            + "ORDER BY table_schema, table_name;";
    }
    public static string GetTableFieldNamesQuery(string tableName)
    {
        return $"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS "
            + $"WHERE TABLE_NAME = '{tableName}' "
            + $"ORDER BY ORDINAL_POSITION;";
    }
}
