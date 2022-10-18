﻿using BuberDinner.Application.Common.Errors;
using BuberDinner.Application.Services.Authentication;
using BuberDinner.Contracts.Authentication;
using ErrorOr;
//using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("register")]
    public IActionResult Register(RegisterRequest request)
    {
        ErrorOr<AuthenticationResult> authResult = _authenticationService.Register(
            request.FirstName,
            request.LastName,
            request.Email,
            request.Password);

        return authResult.MatchFirst(
            authResult => Ok(MapAuthResult(authResult)),
            firstError => Problem(statusCode: StatusCodes.Status409Conflict, title: firstError.Description));
        //if (registerResult.IsSuccess) 
        //{
        //    return Ok(MapAuthResult(registerResult.Value));
        //}
        //var firstError = registerResult.Errors[0];

        //if (firstError is DuplicateEmailError)
        //{
        //    return Problem(statusCode: StatusCodes.Status409Conflict, detail: "Email already exists.");
        //}

        //return Problem();
        //return registerResult.Match(
        //    authResult => Ok(MapAuthResult(authResult)),
        //    _ => Problem(statusCode: StatusCodes.Status409Conflict, title: "Email already exists."));

        //if (registerResult.IsT0)
        //{
        //    var authResult = registerResult.AsT0;
        //    AuthenticationResponse response = MapAuthResult(authResult);
        //    return Ok(response);
        //}
        //return Problem(statusCode: StatusCodes.Status409Conflict, title: "Email already exists.");
    }

    private static AuthenticationResponse MapAuthResult(AuthenticationResult authResult)
    {
        return new AuthenticationResponse(
            authResult.User.Id,
            authResult.User.FirstName,
            authResult.User.LastName,
            authResult.User.Email,
            authResult.Token);
    }

    [HttpPost("login")]
    public IActionResult Login(LoginRequest request)
    {
        var authResult = _authenticationService.Login(
            request.Email,
            request.Password);
        var response = new AuthenticationResponse(
            authResult.User.Id,
            authResult.User.FirstName,
            authResult.User.LastName,
            authResult.User.Email,
            authResult.Token);

        return Ok(response);
    }
}
