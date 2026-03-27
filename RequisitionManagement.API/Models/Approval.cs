namespace RequisitionManagement.API.Models
{
    public class Approval
    {
        public int Id { get; set; }
        public int RequisitionId { get; set; }
        public int ApproverId { get; set; }
        public string ApprovalLevel { get; set; }  // "BU" or "L3"
        public string Status { get; set; }          // "Approved" or "Rejected"
        public string Comments { get; set; }
        public DateTime ActionDate { get; set; } = DateTime.Now;
        public Requisition Requisition { get; set; }
        public User Approver { get; set; }
    }
}