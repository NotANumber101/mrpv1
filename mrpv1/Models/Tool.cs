namespace mrpv1.Models;
public class Tool()
{
    public int Id { get; set; }
    public required int InventoryId { get; set; }
    public required string Name { get; set; }
    public int Quantity { get; set; }
}
