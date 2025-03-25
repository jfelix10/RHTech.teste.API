using Dapper;
using Microsoft.IdentityModel.Tokens;
using RHTech.Teste.API.Domain.Entities.Users;
using RHTech.Teste.API.Interfaces.Application.Authservices;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace RHTech.Teste.API.Application.AuthServices
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IDbConnection _dbConnection;

        public AuthService(IConfiguration configuration, IDbConnection dbConnection)
        {
            _configuration = configuration;
            _dbConnection = dbConnection;
        }

        public async Task<string> RegisterUser(UserRequest userRegister)
        {
            var existingUser = await _dbConnection.QueryFirstOrDefaultAsync<User>(
                "SELECT * FROM Users WHERE Email = @Email",
                new { userRegister.Email }
            );

            if (existingUser != null && !existingUser.FirstAccess)
            {
                throw new Exception("Email já cadastrado!");
            }

            var passwordHash = HashPassword(userRegister.Password);
            if (existingUser == null)
            {

                var result = await _dbConnection.ExecuteAsync(
                @"INSERT INTO Users (Name, Email, PasswordHash, Role) 
                VALUES (@Name, @Email, @PasswordHash, 'Pending')",
                new { userRegister.Name, userRegister.Email, PasswordHash = passwordHash }
                );

                return result > 0 ? Authenticate(userRegister.Email, userRegister.Password) : string.Empty;
            }
            else if (existingUser != null && !existingUser.FirstAccess)
            {
                var result = await _dbConnection.ExecuteAsync(
                @"INSERT INTO Users (Name, Email, PasswordHash, Role) 
                VALUES (@Name, @Email, @PasswordHash, 'Admin')",
                new { userRegister.Name, userRegister.Email, PasswordHash = passwordHash }
                );

                return result > 0 ? Authenticate(userRegister.Email, userRegister.Password) : string.Empty;
            }
            else
            {
                var result = await _dbConnection.ExecuteAsync(
                @"UPDATE Users SET 
                Name = @Name,
                Email = @Email, 
                PasswordHash = @PasswordHash, 
                Role = 'Admin',
                FirstAccess = False
                WHERE Email = @Email",
                new { userRegister.Name, userRegister.Email, PasswordHash = passwordHash }
                );

                return result > 0 ? Authenticate(userRegister.Email, userRegister.Password) : string.Empty;
            }
        }

        public string Authenticate(string email, string password)
        {
            var user = _dbConnection.QueryFirstOrDefault<User>(
                    "SELECT * FROM Users WHERE Email = @Email",
                    new { Email = email }
                );

            if (user == null || !VerifyPassword(password, user.PasswordHash)) 
                return string.Empty;

            if (user.FirstAccess && user.Role == "Admin")
                return "primeiro acesso";

            return GenerateJwtToken(user);
        }

        public string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        private bool VerifyPassword(string password, string passwordHash)
        {
            return HashPassword(password) == passwordHash;
        }

        public string UpdateRole(UserUpdateRequest userUpdateRequest)
        {
            var user = _dbConnection.QueryFirstOrDefault<User>(
                    "SELECT * FROM Users WHERE Email = @Email",
                    new { Email = userUpdateRequest.Email }
                );

            if (user == null) return string.Empty;

            _dbConnection.Execute(
                "UPDATE Users SET Role = @Role WHERE Email = @Email",
                new { userUpdateRequest.Role, userUpdateRequest.Email }
            );

            user.Role = userUpdateRequest.Role;

            return GenerateJwtToken(user);
        }

        private string GenerateJwtToken(User user)
        {
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    [
                        new Claim(ClaimTypes.Name, user.Name),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Role, user.Role),
                        new Claim("idEmpresa", user.IdEmpresa.ToString())
                    ]
                ),
                Expires = DateTime.UtcNow.AddDays(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
