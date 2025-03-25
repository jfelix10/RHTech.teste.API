using RHTech.Teste.API.Domain.Entities.Users;

namespace RHTech.Teste.API.Interfaces.Application.Authservices;

public interface IAuthService
{
    Task<string> RegisterUser(UserRequest userRegister);
    string Authenticate(string email, string password);
    string HashPassword(string password);
    string UpdateRole(UserUpdateRequest userUpdateRequest);
}
