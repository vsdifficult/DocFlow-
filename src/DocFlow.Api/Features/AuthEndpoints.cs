
using DocFlow.Application.Services.Interfaces;
using DocFlow.Domain.Dtos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DocFlow.Api.Features;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/auth");

        group.MapPost("/signup", async (IAuthenticationService authService, UserRegistrationDto registrationDto) =>
        {
            var result = await authService.SignUpAsync(registrationDto);
            return result.Success ? Results.Ok(result) : Results.BadRequest(result.ErrorMessage);
        });

        group.MapPost("/signin", async (IAuthenticationService authService, UserLoginDto loginDto) =>
        {
            var result = await authService.SignInAsync(loginDto);
            return result.Success ? Results.Ok(result) : Results.BadRequest(result.ErrorMessage);
        });

        group.MapPost("/signout", async (IAuthenticationService authService, string token) =>
        {
            await authService.SignOutAsync(token);
            return Results.Ok();
        });

        group.MapPost("/verify-email", async (IAuthenticationService authService, EmailVerificationDto verificationDto) =>
        {
            var result = await authService.VerifyEmailAsync(verificationDto);
            return result.Success ? Results.Ok() : Results.BadRequest(result.ErrorMessage);
        });

        group.MapPost("/send-code-again", async (IAuthenticationService authService, string email) =>
        {
            var result = await authService.SendCodeAgain(email);
            return result.Success ? Results.Ok() : Results.BadRequest(result.ErrorMessage);
        });

        group.MapPost("/request-password-reset", async (IAuthenticationService authService, PasswordResetRequestDto dto) =>
        {
            var result = await authService.RequestPasswordResetAsync(dto);
            return result.Success ? Results.Ok() : Results.BadRequest(result.ErrorMessage);
        });

        group.MapPost("/confirm-password-reset", async (IAuthenticationService authService, PasswordResetConfirmDto dto) =>
        {
            var result = await authService.ConfirmPasswordResetAsync(dto);
            return result.Success ? Results.Ok() : Results.BadRequest(result.ErrorMessage);
        });

        group.MapDelete("/users/{id}", async (IAuthenticationService authService, Guid id) =>
        {
            var result = await authService.DeleteUserAsync(id);
            return result.Success ? Results.Ok() : Results.NotFound();
        });
    }
}
