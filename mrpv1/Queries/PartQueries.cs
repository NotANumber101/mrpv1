using mrpv1.Models;
namespace mrpv1.Queries;
public class PartQueries

{
    public static string GetParts()
    {
        return "SELECT * FROM part;";
    }
        public static string CreatePart(Part newPart)
    {
        return "INSERT into part (inventoryId, name, quantity) "
            + $"VALUES ({newPart.InventoryId}, '{newPart.Name}', {newPart.Quantity}) RETURNING id;";
    }
}
