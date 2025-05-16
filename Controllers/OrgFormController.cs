using OrganizationForm.Models.cs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Runtime.InteropServices;
using Azure;
using Azure.Storage.Files.Shares;
using Azure.Storage.Blobs;


namespace OrganizationForm.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class OrgFormController : ControllerBase
    {
        private readonly OrgFormDbContext orgFormDbContext;
        private readonly IConfiguration _config;
        public OrgFormController(OrgFormDbContext _orgFormDbContext, IConfiguration config)
        {
            orgFormDbContext = _orgFormDbContext;
            _config = config;
           
        }

        [HttpPost]
        [Route("postData")]
        public async Task<IActionResult> PostDetails(OrgForm organization)      // async can do  multiple task. i.e like multiple threading
        {  
            var newOrgForm = new OrgForm
            {
                OrganizationType = organization.OrganizationType,
                Brand = organization.Brand,
                CustomerExperience = organization.CustomerExperience,
                OrganizationName = organization.OrganizationName,
                Street1 = organization.Street1,
                Street2 = organization.Street2,
                City = organization.City,
                State = organization.State,
                ZipCode = organization.ZipCode,
                Country = organization.Country,
                RegulatoryRegion = organization.RegulatoryRegion,
                PriceList = organization.PriceList,
                AccountNumber = organization.AccountNumber,
                StorePrefix = organization.StorePrefix,
            };
            await orgFormDbContext.OrgForm1.AddAsync(newOrgForm);            
                                                                           
            try
            {
                await orgFormDbContext.SaveChangesAsync();                
                return Ok("Data Inserted Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);                              
            }
        }

        [HttpGet]
        [Route("getData")]                      

        public async Task<ActionResult<IEnumerable<OrgForm>>> GetDetails()
        {
            return await orgFormDbContext.OrgForm1.ToListAsync();
        }

        [HttpPut]
        [Route("updateData/{id}")]
        public async Task<IActionResult> UpdateDetails(Guid id, [FromBody] OrgForm organization)
        {
            var existingOrg = await orgFormDbContext.OrgForm1.FindAsync(id);

            if (existingOrg == null)
            {
                return NotFound("Record not found with the given Id.");
            }

            // Update fields
            existingOrg.OrganizationType = organization.OrganizationType;
            existingOrg.Brand = organization.Brand;
            existingOrg.CustomerExperience = organization.CustomerExperience;
            existingOrg.OrganizationName = organization.OrganizationName;
            existingOrg.Street1 = organization.Street1;
            existingOrg.Street2 = organization.Street2;
            existingOrg.City = organization.City;
            existingOrg.State = organization.State;
            existingOrg.ZipCode = organization.ZipCode;
            existingOrg.Country = organization.Country;
            existingOrg.RegulatoryRegion = organization.RegulatoryRegion;
            existingOrg.PriceList = organization.PriceList;
            existingOrg.AccountNumber = organization.AccountNumber;
            existingOrg.StorePrefix = organization.StorePrefix;

            try
            {
                await orgFormDbContext.SaveChangesAsync();
                return Ok("Data Updated Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error while updating: {ex.Message}");
            }
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var organization = await orgFormDbContext.OrgForm1.FindAsync(id);
            if (organization == null)
                return NotFound("Organization details not found");

            return Ok(organization);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteById(Guid id)
        {
            var organization = await orgFormDbContext.OrgForm1.FindAsync(id);
            if (organization == null)
                return NotFound("Organization details not found");

            orgFormDbContext.OrgForm1.Remove(organization);

            try
            {
                await orgFormDbContext.SaveChangesAsync();
                return Ok("Organization form data deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest($"Delete failed: {ex.Message}");
            }
        }


        [HttpPost("submit")]
        public async Task<IActionResult> SubmitForm([FromForm] OrgForm form, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var blobConnectionString = _config["AzureBlobStorage:ConnectionString"];
            var containerName = _config["AzureBlobStorage:ContainerName"];

            // Create blob client
            var blobServiceClient = new BlobServiceClient(blobConnectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            // Ensure the container exists
            await containerClient.CreateIfNotExistsAsync();

            // Upload the file
            var blobClient = containerClient.GetBlobClient(file.FileName);
            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, overwrite: true);
            }

            // Save to database
            form.Id = Guid.NewGuid();
            form.UploadedFileName = file.FileName;
            form.UploadedFileUrl = blobClient.Uri.ToString();

            orgFormDbContext.OrgForm1.Add(form);
            await orgFormDbContext.SaveChangesAsync();

            return Ok("Form submitted and file uploaded to Azure Blob Storage.");
        }

        [HttpPut("update-file/{id}")]
        public async Task<IActionResult> UpdateUploadedFile(Guid id, IFormFile file)
        {
            var organization = await orgFormDbContext.OrgForm1.FindAsync(id);
            if (organization == null)
                return NotFound("Organization record not found");

            if (file == null || file.Length == 0)
                return BadRequest("Invalid file");

            var blobConnectionString = _config["AzureBlobStorage:ConnectionString"];
            var containerName = _config["AzureBlobStorage:ContainerName"];

            var blobServiceClient = new BlobServiceClient(blobConnectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync();

            // Use same file name to overwrite
            var blobClient = containerClient.GetBlobClient(file.FileName);

            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, overwrite: true);
            }

            // Update DB with new file name/url
            organization.UploadedFileName = file.FileName;
            organization.UploadedFileUrl = blobClient.Uri.ToString();

            await orgFormDbContext.SaveChangesAsync();

            return Ok("File updated successfully.");
        }
    }
}