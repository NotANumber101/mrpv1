namespace mrpv1.Models;
public class OpExecution()
{
    public int Id { get; set; }
    public int OperationId { get; set; }
    public int WorkOrderQueueId { get; set; }
    public string? ExecutionLog { get; set; }
}
//   id SERIAL,
//   partProducedSerialNumber INT,
//   workCenterQueueId INT