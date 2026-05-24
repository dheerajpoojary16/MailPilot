namespace BulkMailSender.Models
{
    public class EmailTemplate
    {
        public int Id { get; set; }

        public string? TemplateName  { get; set; }
        public string? sub { get; set; }

        public string body { get; set; }
        public string? AttachmentPath { get; set; }
    }
}
