namespace DatingAPI.Controllers.Requests
{
    public class UserUpdateRequest
    {
        public string Introduction { get; set; }
        
        public string LookingFor { get; set; }
        
        public string Interests { get; set; }
        
        public string City { get; set; }
        
        public string Country { get; set; }
    }
}