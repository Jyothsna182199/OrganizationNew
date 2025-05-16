using System.ComponentModel.DataAnnotations;

namespace OrganizationForm.Models.cs
{
    public class OrgForm
    {
        [Key]
        public Guid Id { get; set; }
        public string? OrganizationType { get; set; }
        public string? Brand { get; set; }               
        public string? CustomerExperience  { get; set; }

        public string? OrganizationName { get; set;}
        public string? Street1 { get; set; }
        public string? Street2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public int? ZipCode { get; set; }
        public string? Country { get; set; }
        public string? RegulatoryRegion { get; set; }

        public string? PriceList { get; set; }
        public int? AccountNumber { get; set; }
        public string? StorePrefix { get; set;}


        public string UploadedFileName { get; set; }
        public string? UploadedFileUrl { get; set; }
    }
}
