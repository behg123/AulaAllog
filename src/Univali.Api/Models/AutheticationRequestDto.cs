using System.ComponentModel.DataAnnotations;
namespace Univali.Ap.Models;


public class AuthenticationRequestDto{
    public string? Username { get; set; }
    public string? Password { get; set; }
}