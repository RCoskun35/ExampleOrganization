namespace ExampleOrganization.Domain.Dtos
{
    public class OrganizationResult
    {
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int? ParentOrganizationId { get; set; }
        public int Degree { get; set; }
        public List<int> Parents { get; set; } = new List<int>();
        public List<int> SubOrganizations { get; set; } = new List<int>();
    }
}
