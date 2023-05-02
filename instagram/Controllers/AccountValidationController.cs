using instagram.Services.Abstracts;
using Microsoft.AspNetCore.Mvc;

namespace instagram.Controllers;

public class AccountValidationController : Controller
{
    private readonly IAccountService _service;

    public AccountValidationController(IAccountService service)
    {
        _service = service;
    }

    [AcceptVerbs("GET", "POST")]
    public bool CheckUniqueName(string userName)
        => _service.UserNameUnique(userName);
    
    
    [AcceptVerbs("GET", "POST")]
    public bool CheckUniqueEmail(string email)
        => _service.UserEmailUnique(email);
}