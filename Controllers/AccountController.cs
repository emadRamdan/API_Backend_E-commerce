using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplicationProject.DAL.models;
using WebApplicationProject.DBL.DTOS;

namespace WebApplicationProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManger;
        private readonly IConfiguration _configration;

        public  AccountController(UserManager<AppUser> userManager , IConfiguration configuration) { 
            _userManger = userManager;
            _configration = configuration;
        }

        #region client register 
            [HttpPost]
            [Route("clients/signup")]
            public async Task<ActionResult> Register(RegisterDto registerDto)
            {
                //vaidatae incomming data 
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // creat new user 
                AppUser user = new AppUser
                {
                    UserName = registerDto.UserName,
                    Email = registerDto.Email,
                    Address = registerDto.address,
                    PhoneNumber = registerDto.PhoneNumber
                };

                //Store  new user 
                var Result = await _userManger.CreateAsync(user, registerDto.password);
                if (!Result.Succeeded)
                {
                    return BadRequest(Result.Errors);
                }

                // create new claims for the new user 
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim (ClaimTypes.Email, user.Email),
                    new Claim (ClaimTypes.StreetAddress, user.Address.ToString()),
                    new Claim (ClaimTypes.Role , "Client")

                };

                await _userManger.AddClaimsAsync(user, claims);
                return Created();
            }
        #endregion

        #region employee register 
            [HttpPost]
            [Route("employees/signup")]
            public async Task<ActionResult> RegisterEmployees(RegisterDto registerDto)
            {
                //vaidatae incomming data 
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // creat new user 
                AppUser user = new AppUser
                {
                    UserName = registerDto.UserName,
                    Email = registerDto.Email,
                    Address = registerDto.address,
                    PhoneNumber = registerDto.PhoneNumber
                };

                //Store  new user 
                var Result = await _userManger.CreateAsync(user, registerDto.password);
                if (!Result.Succeeded)
                {
                    return BadRequest(Result.Errors);
                }

                // create new claims for the new user 
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim (ClaimTypes.Email, user.Email),
                        new Claim (ClaimTypes.StreetAddress, user.Address.ToString()),
                        new Claim (ClaimTypes.Role , "Employee")

                    };

                await _userManger.AddClaimsAsync(user, claims);
                return Created();
        }
        #endregion


        #region Users login 
        [HttpPost]
        [Route("login")]
        public async  Task<ActionResult<TokenDto>> Login(LoginDto loginDto)
        {

            //check that there is a email and password 
            if (string.IsNullOrEmpty(loginDto.Email) || string.IsNullOrEmpty(loginDto.PassWord))
            {
                return BadRequest("Email and password are required  ");
            }

           //check the user is exist by email 
          var user = await _userManger.FindByEmailAsync(loginDto.Email);
            if(user == null)
            {
                return Unauthorized("Email not found.");
            }

            // check user password 
            bool isAuthed = await _userManger.CheckPasswordAsync(user, loginDto.PassWord);
            if (!isAuthed) {
                return Unauthorized("Incorrect Email or  password.");
            }


            //get user claims 
            var userClaims = await _userManger.GetClaimsAsync(user);


            //generate user token 
           return generateToken((List<Claim>)userClaims);

        }
        #endregion

        #region t0oken hellper 
          private ActionResult<TokenDto> generateToken(List<Claim> userClaims)
        {

            // generta the secret key for the token
            var userSKey= _configration.GetValue<string>("TokenSecret");
            var KeyByites = Encoding.ASCII.GetBytes(userSKey);
            var Key = new SymmetricSecurityKey(KeyByites);

            // generat the expire date of the token 
            DateTime ExpireDate = DateTime.Now.AddHours(2);

            // generat signingCredentials with the secret key and any of hash algorithm 

            var MysigningCredentials = new SigningCredentials(Key , SecurityAlgorithms.HmacSha256Signature);

            var jwt = new JwtSecurityToken(
                claims: userClaims,
                expires: ExpireDate,
                signingCredentials: MysigningCredentials

                );

            // create the token as a blain string 

            var JwtasString = new JwtSecurityTokenHandler().WriteToken(jwt);

            //return dto of the token 
            return new TokenDto(JwtasString, ExpireDate);

        }
        #endregion
    }
}
