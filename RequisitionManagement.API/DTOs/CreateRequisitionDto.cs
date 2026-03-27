namespace RequisitionManagement.API.DTOs
{
    public class CreateRequisitionDto
    {
        public string Department { get; set; }
        public string Skillset { get; set; }
        public string ExperienceLevel { get; set; }
        public int NumberOfPositions { get; set; }
    }
}