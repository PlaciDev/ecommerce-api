namespace EcommerceApi.ViewModel
{
    public class UserListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public RoleListViewModel Role { get; set; }
    }
}
