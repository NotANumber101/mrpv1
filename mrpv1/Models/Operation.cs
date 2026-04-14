namespace mrpv1.Models;
public class Operation()
{
    public int Id { get; set; }
    public required string Instruction { get; set; }
    public int PartProduced { get; set; }
    public int PartConsumed { get; set; }
    public int MPartProduced { get; set; }
    public int MPartConsumed { get; set; }
}