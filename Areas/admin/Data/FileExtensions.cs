namespace EduHome.Areas.admin.Data
{
    public static class FileExtensions
    {

        public static bool IsImage(this IFormFile file)
        {
            return file.ContentType.Contains("image");
        }

        public static bool ImageAllowed(this IFormFile file, int mb)
        {
            if (file.Length > 1024 * 1024 * mb)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async static Task<string> GenerateFile(this IFormFile file, string path)
        {
            if (!Directory.Exists(path))

                Directory.CreateDirectory(path);

            var unicalName = $"{Guid.NewGuid()}-{file.FileName}";
            //var unicalName = $"{DateTime.UtcNow.AddHours(4).ToString("yyyy-MM-dd-HH-mm-ss")}-{Guid.NewGuid()}-{file.FileName}";

            using FileStream fs = new(Path.Combine(path, unicalName), FileMode.Create);

            await file.CopyToAsync(fs);

            return unicalName;
        }

    }
}
