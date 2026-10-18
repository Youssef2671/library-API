using LibraryAPI.Data;
using LibraryAPI.DTOs;
using LibraryAPI.Models; // عشان يشوف كلاس الـ User
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly BooksContext _context; // حقن الداتابيز هنا

        public AuthController(IConfiguration configuration, BooksContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        // 1. دالة التسجيل الجديدة
        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRegisterDTO dto)
        {
            // التأكد إن اليوزرنيم مش متكرر
            if (_context.Users.Any(u => u.Username == dto.Username))
            {
                return BadRequest(new { Message = "اسم المستخدم موجود بالفعل، اختر اسماً آخر." });
            }

            // تشفير الباسورد باستخدام BCrypt
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            // إنشاء كائن المستخدم الجديد
            var user = new User
            {
                Username = dto.Username,
                PasswordHash = hashedPassword
            };

            // حفظ في الداتابيز
            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok(new { Message = "تم إنشاء الحساب بنجاح!" });
        }

        // 2. دالة تسجيل الدخول (بعد التعديل)
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginDTO dto)
        {
            // البحث عن المستخدم في الداتابيز
            var user = _context.Users.FirstOrDefault(u => u.Username == dto.Username);

            // لو اليوزر مش موجود، أو الباسورد (بعد ما نفرمه) مش مطابق للمتخزن
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            {
                return Unauthorized(new { Message = "اسم المستخدم أو كلمة المرور غير صحيحة." });
            }

            // لو كله تمام، طلعله التوكين
            var token = GenerateJwtToken(user.Username);
            return Ok(new { Token = token });
        }

        // دالة توليد التوكين (زي ما هي بدون تغيير)
        private string GenerateJwtToken(string username)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}