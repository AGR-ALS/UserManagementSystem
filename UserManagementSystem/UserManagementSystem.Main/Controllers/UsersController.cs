using System.Security.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using UserManagementSystem.Application.Abstractions.Mail;
using UserManagementSystem.Application.Exceptions;
using UserManagementSystem.Domain.Abstractions;
using UserManagementSystem.Infrastructure.Authentication.Tokens.Settings;
using UserManagementSystem.Main.Attributes;
using UserManagementSystem.Main.Contracts;

namespace UserManagementSystem.Main.Controllers;

[Route("[controller]")] 
public class UsersController : Controller
{
    private readonly IUsersService _usersService;
    private readonly IMailService _mailService;
    private readonly TokenIdentifiers _tokenIdentifiers;

    public UsersController(IUsersService usersService, IOptions<TokenIdentifiers> tokenIdentifiers, IMailService mailService)
    {
        _usersService = usersService;
        _mailService = mailService;
        _tokenIdentifiers = tokenIdentifiers.Value;
    }
    
    [HttpGet]
    [Authorize]
    [TypeFilter(typeof(NotBlockedAttribute))]
    public async Task<IActionResult> GetAllUsersAsync(CancellationToken cancellationToken)
    {
        var users = await _usersService.GetAllUsersSortedAsync(cancellationToken);
        
        return View("Index", users);
    }

    [HttpPost("block")]
    [Authorize]
    [TypeFilter(typeof(NotBlockedAttribute))]
    public async Task<IActionResult> BlockUsersAsync([FromForm] Guid[] ids, CancellationToken cancellationToken)
    {
        await _usersService.BlockUsersAsync(ids, cancellationToken);
        
        return RedirectToAction("GetAllUsers");
    }
    
    [HttpPost("unblock")]
    [Authorize]
    [TypeFilter(typeof(NotBlockedAttribute))]
    public async Task<IActionResult> UnblockUsersAsync([FromForm] Guid[] ids, CancellationToken cancellationToken)
    {
        await _usersService.UnblockUsersAsync(ids, cancellationToken);
        
        return RedirectToAction("GetAllUsers");
    }
    
    [HttpPost("delete")]
    [Authorize]
    [TypeFilter(typeof(NotBlockedAttribute))]
    public async Task<IActionResult> DeleteUsersAsync([FromForm] Guid[] ids, CancellationToken cancellationToken)
    {
        await _usersService.DeleteUsersAsync(ids, cancellationToken);
        try
        {
            await LoginWithRefreshToken(cancellationToken);
        }
        catch (UnauthorizedAccessException e)
        {
            return RedirectToAction("Login");
        }
        
        return RedirectToAction("GetAllUsers");
    }
    
    [HttpPost("delete-unverified")]
    [Authorize]
    [TypeFilter(typeof(NotBlockedAttribute))]
    public async Task<IActionResult> DeleteUnverifiedUsersAsync(CancellationToken cancellationToken)
    {
        await _usersService.DeleteUnverifiedUsersAsync(cancellationToken);
        try
        {
            await LoginWithRefreshToken(cancellationToken);
        }
        catch (UnauthorizedAccessException e)
        {
            return RedirectToAction("Login");
        }
        
        return RedirectToAction("GetAllUsers");
    }

    [HttpGet("verify")]
    [Authorize]
    [TypeFilter(typeof(NotBlockedAttribute))]
    public async Task<IActionResult> VerifyAccountAsync(string token,
        CancellationToken cancellationToken)
    {
        await _usersService.VerifyUserAsync(token, cancellationToken);
        
        return RedirectToAction("ActivationSuccess");
    }

    [HttpGet("login")]
    public IActionResult LoginAsync(CancellationToken cancellationToken)
    {
        return View("Login", new LoginUserViewModel() {IsRegistration = false});
    }
    
    [HttpGet("register")]
    public IActionResult RegisterAsync(CancellationToken cancellationToken)
    {
        return View("Login", new LoginUserViewModel() {IsRegistration = true});
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUserAsync([FromForm]LoginUserViewModel viewModel, CancellationToken cancellationToken)
    {
        try
        {
            await _usersService.RegisterUserAsync(viewModel.Username ?? 
                                                  throw new ArgumentNullException(viewModel.Username, "Username cannot be null"),
                viewModel.Email, viewModel.Password, cancellationToken);
        }
        catch (EntityCreatingException)
        {
            ModelState.AddModelError(string.Empty, "Such email already exists");
            viewModel.IsRegistration = true;
            
            return View("Login", viewModel);
        }
        await LoginUserAsync(viewModel.Email, viewModel.Password, cancellationToken);
        await _mailService.SendVerificationEmailAsync(viewModel.Email, cancellationToken);
        
        return RedirectToAction("RegistrationSuccess");
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginUserAsync([FromForm] LoginUserViewModel viewModel,
        CancellationToken cancellationToken)
    {
        try
        {
            await LoginUserAsync(viewModel.Email, viewModel.Password, cancellationToken);
        }
        catch (EntityNotFoundException)
        {
            ModelState.AddModelError(string.Empty, "Invalid email or password");
            viewModel.IsRegistration = false;
            return View("Login", viewModel);
        }
        catch (InvalidCredentialException)
        {
            ModelState.AddModelError(string.Empty, "Invalid email or password");
            viewModel.IsRegistration = false;
            return View("Login", viewModel);
        }
        
        return RedirectToAction("GetAllUsers");
    }

    private async Task LoginUserAsync(string email, string password, CancellationToken cancellationToken)
    {
        var (accessToken, refreshToken) =
            await _usersService.LoginAsync(email, password, cancellationToken);
        AddTokenToCookie(_tokenIdentifiers.AccessTokenIdentifier, accessToken);
        AddTokenToCookie(_tokenIdentifiers.RefreshTokenIdentifier, refreshToken);
    }

    [HttpPost("refresh-login")]
    public async Task<ActionResult> LoginWithRefreshToken(CancellationToken cancellationToken)
    {
        var refreshToken = Request.Cookies[_tokenIdentifiers.RefreshTokenIdentifier] ??
                           throw new UnauthorizedAccessException("Invalid Refresh Token");
        var accessToken = await _usersService.LoginAsync(refreshToken, cancellationToken);
        AddTokenToCookie(_tokenIdentifiers.AccessTokenIdentifier, accessToken);
        
        return RedirectToAction("GetAllUsers");
    }
    
    [HttpPost("logout")]
    public async Task<ActionResult> Logout(CancellationToken cancellationToken)
    {
        RemoveTokenFromCookie(_tokenIdentifiers.AccessTokenIdentifier);
        RemoveTokenFromCookie(_tokenIdentifiers.RefreshTokenIdentifier);
        
        return RedirectToAction("Login");
    }
    
    [HttpGet("registration-success")]
    [AllowAnonymous]
    public IActionResult RegistrationSuccess()
    {
        return View();
    }

    [HttpGet("activation-success")]
    [AllowAnonymous]
    public IActionResult ActivationSuccess()
    {
        return View();
    }
    
    private void AddTokenToCookie(string tokenName, string token)
    {
        HttpContext.Response.Cookies.Append(tokenName, token);
    }

    private void RemoveTokenFromCookie(string tokenName)
    {
        HttpContext.Response.Cookies.Delete(tokenName);
    }
}