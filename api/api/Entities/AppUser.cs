namespace api.Entities
{
  public class AppUser
  {
    public int Id { get; set; }
    public string UserName { get; set; }
    public byte[] PaswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
  }
}
