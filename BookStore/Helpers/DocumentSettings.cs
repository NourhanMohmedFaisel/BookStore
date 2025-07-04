namespace BookStore.Helpers
{
    public static class DocumentSettings
    {
        public static string UploadFile(IFormFile file, string folderName)
        {
            //string folderPath=$"{Directory.GetCurrentDirectory()}\\wwwroot\\{folderName}";


            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderName);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";


            string filePath = Path.Combine(folderPath, fileName);


            using var fileStream = new FileStream(filePath, FileMode.Create);

            file.CopyTo(fileStream);

            return fileName;

        }
    }
}
