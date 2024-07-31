namespace AdminDashboard.Helper
{
	public class DocumentSettings
	{
		public static string UploadFile(IFormFile file, string foldername)
		{
			// 1. GET folder Path
			var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", foldername);

			// 2. GET fileName and Make it Unique
			var fileName = Guid.NewGuid() + file.FileName;

			// 3. GET File Path
			var filePath = Path.Combine(folderPath, fileName);

			// 4. use file stream to make a copy
			using var fileStream = new FileStream(filePath, FileMode.Create);

			file.CopyTo(fileStream);

			//return filePath;
			return Path.Combine("images\\products", fileName);
		}

		public static void DeleteFile(string folderName, string fileName)
		{
			var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", folderName, fileName);
			
			if (File.Exists(filePath))
				File.Delete(filePath);
			 
		}
	}
}
