namespace mrpv1.Models;
public class Inventory()
{
    public enum Locations
    {
        Fridge,
        CabinetA,
        ClosetA
    }

    // get these ints from db instead of hard coding.
    // public enum WorkCenterLocations
    // {
    //     Kitchen = 1,
    //     Garage = 2,
    //     InocChamber = 3,
    //     BloomChamber = 4
    // }
    public int Id { get; set; }
    public required string Location { get; set; }
    public required string Description { get; set; }
    
}
