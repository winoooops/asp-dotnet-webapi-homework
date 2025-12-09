using MyWebAPI.DTOs;

namespace MyWebAPI.Services.Auth;

public interface IAuthService
{
   public Task<AuthResponse> SignUpAsync(SignUpRequest req);

   public Task<AuthResponse> SignInAsync(SignInRequest req);

   public Task<AuthResponse> RefreshAccessTokenAsync(RefreshRequest req);
}