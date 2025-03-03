using System.ComponentModel.DataAnnotations;

namespace WebApi
{
    public class WebApi
    {
        public Guid id { get; set; }
        //[Required]
        public required string name { get; set; }
        //[Required]
        public required string ownerUserId { get; set; }
        //[Required]
        public required int maxLength { get; set; }
        //[Required]
        public required int maxHeight { get; set; }

        // Constructor to initialize the 'name' field
        public WebApi() { }
        public WebApi(string name)
        {
            this.name = name;
        }
    }
}
