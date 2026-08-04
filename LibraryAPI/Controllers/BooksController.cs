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
    public class BooksController : ControllerBase
    {
        private readonly IGenericRepository<Book> _bookRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        // Dependency Injection: حقن الريبوزيتوري والأوتوماب
        public BooksController(IGenericRepository<Book> bookRepository, IMapper mapper, IWebHostEnvironment env)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
            _env = env;
        }

        // 1. عرض كل الكتب
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetBooks()
        {
            // هنا بنمرر جدول المؤلف عشان ييجي مع الكتاب
            var books = await _bookRepository.GetAllAsync(b => b.Author);

            // الـ AutoMapper هيقوم بدوره عادي جداً ويحول الداتا
            var booksDto = _mapper.Map<IEnumerable<BookDTO>>(books);

            return Ok(booksDto);
        }

        // 2. إضافة كتاب جديد مع رفع صورة
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreateBook([FromForm] BookCreateDTO bookCreateDto)
        {
            // تحويل الـ DTO لـ Book موديل (صورة الغلاف هيتم تجاهلها زي ما حددنا في الـ Profile)
            var book = _mapper.Map<Book>(bookCreateDto);

            // لوجيك رفع الصورة
            if (bookCreateDto.ImageFile != null && bookCreateDto.ImageFile.Length > 0)
            {
                // تحديد مسار حفظ الصورة في فولدر wwwroot/images
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(bookCreateDto.ImageFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await bookCreateDto.ImageFile.CopyToAsync(stream);
                }

                // حفظ مسار الصورة كنص في قاعدة البيانات
                book.ImagePath = $"/images/{fileName}";
            }

            // إضافة الكتاب للداتابيز
            await _bookRepository.AddAsync(book);

            return Ok(new { Message = "تم إضافة الكتاب بنجاح" });
        }
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            // 1. بندور على الكتاب
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
            {
                return NotFound(new { Message = "الكتاب غير موجود." });
            }

            // 2. بنمسح الصورة الحقيقية من السيرفر
            if (!string.IsNullOrEmpty(book.ImagePath))
            {
                // بنشيل علامة السلاش الأولى عشان المسار يتركب صح
                var imagePath = Path.Combine(_env.WebRootPath, book.ImagePath.TrimStart('/'));

                // بنتأكد إن الملف موجود فعلاً قبل ما نمسحه عشان السيرفر ميضربش إيرور
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            // 3. بنمسح الريكورد من الداتابيز
            await _bookRepository.DeleteAsync(book);

            return Ok(new { Message = "تم حذف الكتاب وصورته بنجاح." });
        }
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromForm] BookUpdateDTO dto)
        {
            // 1. بنتأكد إن الكتاب موجود أصلاً
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
            {
                return NotFound(new { Message = "الكتاب غير موجود." });
            }

            // 2. بنحدث البيانات العادية
            book.Title = dto.Title;
            book.AuthorId = dto.AuthorId;

            // 3. لو اليوزر بعت صورة جديدة، لازم نمسح القديمة ونحفظ الجديدة
            if (dto.ImageFile != null)
            {
                // أ - مسح الصورة القديمة
                if (!string.IsNullOrEmpty(book.ImagePath))
                {
                    var oldImagePath = Path.Combine(_env.WebRootPath, book.ImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                // ب - رفع الصورة الجديدة
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.ImageFile.FileName);
                var newImagePath = Path.Combine(_env.WebRootPath, "images", fileName);

                using (var stream = new FileStream(newImagePath, FileMode.Create))
                {
                    await dto.ImageFile.CopyToAsync(stream);
                }

                // ج - تحديث المسار في الداتابيز
                book.ImagePath = "/images/" + fileName;
            }
            // لو مبعتش صورة، الكود هيتجاهل الخطوة دي وهيفضل محتفظ بمسار الصورة القديم

            // 4. بنحفظ التعديلات في الداتابيز
            await _bookRepository.UpdateAsync(book);

            return Ok(new { Message = "تم تعديل بيانات الكتاب بنجاح." });
        }
    }
}