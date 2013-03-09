using System;
using FileHelper;
using System.IO;

namespace GameUpdater {
	public class Updater {
		private string FilePath;
		private string DatePath;
		private string UploadPath;
		private string FileName;
		private string DateName;
		public Updater(string FileName, string DateName, string UploadPath){
			FilePath = PathGetter.GetDirectoryPath() + FileName;
			DatePath = PathGetter.GetDirectoryPath() + DateName;
			this.UploadPath = UploadPath;
			if(UploadPath[UploadPath.Length - 1] != Path.DirectorySeparatorChar){
				Console.WriteLine("UploadPath must end in the character " + Path.DirectorySeparatorChar);
				throw new Exception();
			}
			this.FileName = FileName;
			this.DateName = DateName;
		}
		public void Update(){
			Console.WriteLine("Deleting obsolete files...");
			File.Delete(UploadPath + FileName);
			File.Delete(UploadPath + DateName);
			Console.WriteLine("Updating files...");
			File.Copy(FilePath, UploadPath + FileName);
			File.Copy(DatePath, UploadPath + DateName);
			Console.WriteLine("Update complete");
		}
	}
}
