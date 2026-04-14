using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Npgsql;

namespace mrpv1.Helpers;

// temp solution maybe, jsut learning
// its a good idea, with so few inventory lkcoations and wcs,
// its better to ALSO store these in the app memory
// it elminates guess and is faster but requires more user cpu

// i think a map would be random mem location but these enums are not?


public static class Locations
{

    // INSERT into inventory(id, location, description) VALUES(1, 'fridge', 'cold storage facility');
    // INSERT into inventory(id, location, description) VALUES(2, 'cabinet A', 'sanitary facility');
    // INSERT into inventory(id, location, description) VALUES(3, 'closet A', 'short term storage');
    // INSERT into inventory(id, location, description) VALUES(4, 'bloomroom', 'humidity controlled storage');
    // INSERT into inventory(id, location, description) VALUES(5, 'garage', 'long term storage');
    // INSERT into inventory(id, location, description) VALUES(6, 'kitchen', 'work center');
    public enum InventoryLocations
    {
        Fridge=1,
        CabinetA=2,
        ClosetA=3,
        Garage=4,
        Kitchen=5
    }

    public enum WorkCenterLocations
    {
        BloomChamber=1,
        wc1=2,
        wc2=3
    }

    // public static Orientation ToOrientation(Direction direction) => direction switch
    // {
    //     Direction.Up => Orientation.North,
    //     Direction.Right => Orientation.East,
    //     Direction.Down => Orientation.South,
    //     Direction.Left => Orientation.West,
    //     _ => throw new ArgumentOutOfRangeException(nameof(direction), $"Not expected direction value: {direction}"),
    // };

    // public static void Main()
    // {
    //     var direction = Direction.Right;
    //     Console.WriteLine($"Map view direction is {direction}");
    //     Console.WriteLine($"Cardinal orientation is {ToOrientation(direction)}");
    //     // Output:
    //     // Map view direction is Right
    //     // Cardinal orientation is East
    // }
}