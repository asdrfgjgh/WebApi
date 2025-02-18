using System.ComponentModel.DataAnnotations;

namespace WebApi
{
    public class WebApi
    {
        public Guid id { get; set; }
        [Required]
        public string name { get; set; }
        public string ownerUserId { get; set; }
        public int maxLength { get; set; }
        public int maxHeight { get; set; }

        // Constructor to initialize the 'name' field
        public WebApi() { }
        public WebApi(string name)
        {
            this.name = name;
        }
    }
}
