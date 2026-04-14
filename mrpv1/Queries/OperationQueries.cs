using mrpv1.Models;
namespace mrpv1.Queries;
public class OperationQueries

{
    public static string GetOperations()
    {
        return "SELECT * FROM operation;";
    }
        public static string CreateOperation(Operation newOp)
    {
        return "INSERT into operation (instruction, partProduced, partConsumed, mPartProduced, mPartConsumed, "
            + "material, tool, equipment, machine) "
            + $"VALUES ('{newOp.Instruction}', {newOp.PartProduced}, {newOp.PartConsumed}, "
            + $"{newOp.MPartProduced}, {newOp.MPartConsumed}, "
            + $"{newOp.Material}, {newOp.Tool}, {newOp.Equipment}, {newOp.Machine});";
    }
    //  create m-stack
}