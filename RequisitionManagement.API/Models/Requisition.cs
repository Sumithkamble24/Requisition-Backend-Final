namespace RequisitionManagement.API.Models
{
    public class Requisition
    {
        public int Id { get; set; }
        public string Department { get; set; }
        public string Skillset { get; set; }
        public string ExperienceLevel { get; set; }
        public int NumberOfPositions { get; set; }
        public string Status { get; set; } = "Pending";
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public User Creator { get; set; }
    }
}