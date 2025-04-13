namespace EvidenceVault.DTO
{
    public class UserRegisterModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public bool? CreatedByAdmin { get; set; }  // ✅ Make it nullable
        public string? CreatedByAdminID { get; set; }
    }
}
