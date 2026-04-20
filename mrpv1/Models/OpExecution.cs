namespace mrpv1.Models;
public class OpExecution()
{
    public int Id { get; set; }
    public int OperationId { get; set; }
    public int WorkOrderQueueId { get; set; }
    public string? ExecutionLog { get; set; }
    public DateTime TimeStart { get; set; }
    public DateTime TimeStop { get; set; }
}
//   id SERIAL,
//   partProducedSerialNumber INT,
//   workCenterQueueId INT