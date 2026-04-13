using mrpv1.Models;

namespace mrpv1.Queries;

public class PartQueries

{
    public static string GetParts()
    {
        return "SELECT * FROM part;";
    }
        public static string CreatePart()
    {
        return "INSERT into part (name) "
            + "VALUES ('TESTNAMEPART');";
    }
}
