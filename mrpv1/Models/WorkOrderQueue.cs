namespace mrpv1.Models;
public class WorkOrderQueue()
{
    public int Id { get; set; }
    public int PartProducedSerialNumber { get; set; }
    public int WorkCenterQueueId { get; set; }
}
//   id SERIAL,
//   partProducedSerialNumber INT,
//   workCenterQueueId INT

