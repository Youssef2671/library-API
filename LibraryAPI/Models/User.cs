namespace LibraryAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;

        // ركز هنا: إحنا مش هنسميها Password، هنسميها PasswordHash
        // عشان نأكد إن اللي هيتخزن هنا هو "النسخة المشفرة" مش الباسورد الحقيقي
        public string PasswordHash { get; set; } = string.Empty;
    }
}