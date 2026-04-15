namespace mrpv1.Helpers;
public static class Locations
{
    public enum InventoryLocations
    {
        Fridge=1,
        CabinetA=2,
        ClosetA=3,
    }

    // get these ints from db instead of hard coding.
    public enum WorkCenterLocations
    {
        Kitchen=1,
        Garage=2,
        InocChamber=3,
        BloomChamber=4
    }
}
// 1. this is buggy, because i am creating data in different locations.
//      One place shuould crate the data, and pass it down.
//             ---- basically the term is: a single source of truth


// 2. MOVE THIS TO INVENTORY MODEL