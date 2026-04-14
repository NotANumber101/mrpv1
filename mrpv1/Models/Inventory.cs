namespace mrpv1.Models;
public class Inventory()
{
    public int Id { get; set; }
    public required string Location { get; set; }
    public required string Description { get; set; }
}
