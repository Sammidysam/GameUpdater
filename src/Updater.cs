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
			Console.WriteLine("File updated.  Creating timestamp...");
			File.CreateText(UploadPath + DateName).Close();
			string[] Values = new string[6];
			Values[0] = Convert.ToString(DateTime.UtcNow.Year);
			Values[1] = Convert.ToString(DateTime.UtcNow.Month);
			Values[2] = Convert.ToString(DateTime.UtcNow.Day);
			Values[3] = Convert.ToString(DateTime.UtcNow.Hour);
			Values[4] = Convert.ToString(DateTime.UtcNow.Minute);
			Values[5] = Convert.ToString(DateTime.UtcNow.Second);
			File.WriteAllLines(UploadPath + DateName, Values);
			Console.WriteLine("Update complete");
		}
	}
}
