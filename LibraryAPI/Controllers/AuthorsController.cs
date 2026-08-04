using AutoMapper;
using LibraryAPI.DTOs;
using LibraryAPI.Models;
using LibraryAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace LibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        // ركز هنا: استخدمنا نفس الريبوزيتوري بس مع كلاس Author
        private readonly IGenericRepository<Author> _repository;
        private readonly IMapper _mapper;

        public AuthorsController(IGenericRepository<Author> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // 1. عرض كل المؤلفين
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDTO>>> GetAuthors()
        {
            var authors = await _repository.GetAllAsync();
            var authorsDto = _mapper.Map<IEnumerable<AuthorDTO>>(authors);
            return Ok(authorsDto);
        }

        // 2. إضافة مؤلف جديد
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddAuthor([FromBody] AuthorCreateDTO dto)
        {
            if (string.IsNullOrEmpty(dto.Name))
                return BadRequest("اسم المؤلف مطلوب");

            var author = _mapper.Map<Author>(dto);
            await _repository.AddAsync(author);

            return Ok(new { Message = "تم إضافة المؤلف بنجاح", AuthorId = author.Id });
        }
    }
}