using API.Dtos;
using API.Errors;
using API.Extensions;
using AutoMapper;
using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Services;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IUserRepository _userRepository;

        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ITokenService tokenService,
            IMapper mapper,
            IEmailService emailService,
            IUserRepository userRepository)
        {
            _mapper = mapper;
            _emailService = emailService;
            _userRepository = userRepository;
            _tokenService = tokenService;
            _userManager = userManager;
            _signInManager = signInManager;
        }


        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) return Unauthorized(new ApiResponse(401));
            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!isPasswordCorrect) return Unauthorized(new ApiResponse(401));

            // if (EmailConfirmationRequired(user))
            // {

            //                List<string> errorMessages = new List<string>();
            //                errorMessages.Add("User Email is not confirmed");
            //                if (canSignIn.PhoneConfirmationRequired) errorMessages.Add("User Phone number is not confirmed");
            //                if (await _signInManager.UserManager.IsLockedOutAsync(user)) errorMessages.Add("User is blocked"); 
            //                return Unauthorized(new ApiResponse(401, errorMessages));
            // }

            //var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            //            var result = await _signInManager.PasswordSignInAsync(user, loginDto.Password, false, false);
            //            if (!EmailConfirmationRequired(user)) await _signInManager.SignInAsync(user, true);

            //            if (!result.Succeeded) return Unauthorized(new ApiResponse(401));

            var emailConfirmationRequired = _userRepository.EmailConfirmationRequired(user);
            var userDto = await CreateUserDto(user, emailConfirmationRequired, loginDto.RememberMe);

            return Ok(userDto);
        }

        [HttpGet("emailconfirmation")]
        public async Task<ActionResult<UserDto>> EmailConfirmationGet([FromQuery] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            var securityCode = await _userManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider);

            //            var scode = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            //            await _emailService.SendAsync("opt@mobileplus.com.ua", user.Email, "Enter security code", $"Please use this code as OTP: {securityCode}");

            //            this.EmailMFA.SecurityCode = string.Empty;
            //            this.EmailMFA.RememberMe = rememberMe;
            return Ok(securityCode);
        }

        [HttpPost("emailconfirm")]
        public async Task<ActionResult> EmailConfirmationPost(LoginEmailConfirmationDto confirmationDto)
        {
            var user = await _userManager.FindByEmailAsync(confirmationDto.Email);
            if (user != null)
            {
                var result = await _signInManager.UserManager.VerifyTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider, confirmationDto.ConfirmationCode);
                if (result == true)
                {
                    // sign in, All is ok
                    user.EmailConfirmed = true;
                    await _userManager.UpdateAsync(user);
                    await _signInManager.SignInAsync(user, confirmationDto.RememberMe);
                    var userDto = await CreateUserDto(user, false, confirmationDto.RememberMe);
                    return Ok(userDto);
                }
                else
                {
                    return Unauthorized(new ApiResponse(401, "Wrong confirmation code. Try again"));
                }
            }
            return Unauthorized(new ApiResponse(401, "User not found"));
        }

        // [Authorize]
        // [HttpGet]
        // public async Task<ActionResult<UserDto>> GetCurrentUser()
        // {
        //     var user = await _userManager.FindByEmailFromClaimsPrincipleAsync(HttpContext.User);

        //     return new UserDto
        //     {
        //         DisplayName = user.DisplayName,
        //         Token = await _tokenService.CreateToken(user),
        //         Email = user.Email,
        //         PhoneNumber = user.PhoneNumber
        //     };
        // }

        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("address")]
        public async Task<ActionResult<AddressDto>> GetUserAddressAsync()
        {
            var user = await _userManager.FindUserByClaimsPrincipleWithAddressAsync(HttpContext.User);

            return _mapper.Map<Address, AddressDto>(user.Address);
        }

        [Authorize]
        [HttpPut("address")]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto address)
        {
            var user = await _userManager.FindUserByClaimsPrincipleWithAddressAsync(HttpContext.User);

            user.Address = _mapper.Map<AddressDto, Address>(address);
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded) return Ok(_mapper.Map<Address, AddressDto>(user.Address));

            return BadRequest("Problem updating the user");

        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {

            if (CheckEmailExistsAsync(registerDto.Email).Result.Value)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse { Errors = new[] { "Email is in use" } });
            }

            var user = new AppUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded) return BadRequest(new ApiResponse(400));

            var roleAddResult = await _userManager.AddToRoleAsync(user, "User");

            if (!roleAddResult.Succeeded) return BadRequest("Failed to add to role");

            var emailConfirmationRequired = _userRepository.EmailConfirmationRequired(user);
            var userDto = await CreateUserDto(user, emailConfirmationRequired, registerDto.RememberMe);

            return userDto;
        }

        // private bool EmailConfirmationRequired(AppUser user)
        // {
        //     if (!_signInManager.Options.SignIn.RequireConfirmedEmail) return false;
        //     else if (user.EmailConfirmed) return false;
        //     return true;
        // }

        // class CanSignIn
        // {
        //     public bool SignInAllowed { get; } = true;
        //     public bool EmailConfirmationRequired { get; } = false;
        //     public bool PhoneConfirmationRequired { get; } = false;
        //     //            public bool AccountConfirmationRequired { get; } = false;
        //     //            public bool AccountLocked {get;} = false;

        //     public CanSignIn(AppUser user, SignInManager<AppUser> signInManager)
        //     {
        //         EmailConfirmationRequired = signInManager.Options.SignIn.RequireConfirmedEmail ? user.EmailConfirmed ? false : true : false;
        //         PhoneConfirmationRequired = signInManager.Options.SignIn.RequireConfirmedPhoneNumber ? user.PhoneNumberConfirmed ? false : true : false;
        //         var a = signInManager.UserManager.IsPhoneNumberConfirmedAsync(user);
        //         //                AccountConfirmationRequired = signInManager.Options.SignIn.RequireConfirmedAccount ? user. ? false : true : false;
        //         SignInAllowed = !EmailConfirmationRequired && !PhoneConfirmationRequired;
        //     }

        // }
        private async Task<UserDto> CreateUserDto(AppUser user, bool emailConfirmationRequired, bool rememberMe)
        {
            var userDto = new UserDto
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                EmailConfirmationRequired = emailConfirmationRequired,
                Token = emailConfirmationRequired ? "" : await _tokenService.CreateToken(user, rememberMe),
                PhoneNumber = user.PhoneNumber,
                RememberMe = rememberMe
            };

            return userDto;
        }


    }


}