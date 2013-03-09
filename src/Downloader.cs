using System;
using System.Net;
using FileHelper;
using System.IO;

namespace GameUpdater {
	public class Downloader {
		private string FilePath;
		private string DatePath;
		private string FileSite;
		private string DateSite;
		private bool HasInternet;
		public Downloader(string FileName, string DateName, string FileSite, string DateSite){
			FilePath = PathGetter.GetDirectoryPath() + FileName;
			DatePath = PathGetter.GetDirectoryPath() + DateName;
			this.FileSite = FileSite;
			this.DateSite = DateSite;
			string WackyFileName = MakeWackyFileName();
			while(File.Exists(PathGetter.GetDirectoryPath() + WackyFileName))
				WackyFileName = MakeWackyFileName();
			WebClient webClient = new WebClient();
			try {
				webClient.DownloadFile("https://www.google.com/", PathGetter.GetDirectoryPath() + WackyFileName);
				webClient.Dispose();
				HasInternet = true;
			} catch (WebException) {
				HasInternet = false;
				webClient.Dispose();
				File.Delete(PathGetter.GetDirectoryPath() + WackyFileName);
			}
		}
		public void CheckForUpdate(){
			if(HasInternet){
				if(File.Exists(FilePath)){
					if(File.Exists(DatePath)){
						Console.WriteLine("Checking date of update...");
						WebClient webClient = new WebClient();
						webClient.DownloadFile(DateSite, PathGetter.GetDirectoryPath() + "tempDate.txt");
						webClient.Dispose();
						DateTime TimeOfUpdate = ConvertFileToDateTime(PathGetter.GetDirectoryPath() + "tempDate.txt");
						File.Delete(PathGetter.GetDirectoryPath() + "tempDate.txt");
						DateTime TimeOnFile = ConvertFileToDateTime(DatePath);
						if(DateTime.Compare(TimeOnFile, TimeOfUpdate) > 0)
							Console.WriteLine("No download necessary");
						else {
							Console.WriteLine("Download necessary");
							DownloadFiles();
						}
					}
					else
						DownloadFiles();
				}
				else {
					Console.WriteLine("Download necessary");
					DownloadFiles();
				}
			}
			else
				Console.WriteLine("No internet detected, no update will be downloaded");
		}
		private void DeleteFiles(){
			File.Delete(FilePath);
			File.Delete(DatePath);
		}
		private void DownloadFiles(){
			Console.WriteLine("Downloading...");
			WebClient webClient = new WebClient();
			webClient.DownloadFile(FileSite, FilePath);
			webClient.Dispose();
			Console.WriteLine("Success");
			Console.WriteLine("Making datestamp...");
			File.CreateText(DatePath).Close();
			string[] Values = new string[6];
			Values[0] = Convert.ToString(DateTime.Now.Year);
			Values[1] = Convert.ToString(DateTime.Now.Month);
			Values[2] = Convert.ToString(DateTime.Now.Day);
			Values[3] = Convert.ToString(DateTime.Now.Hour);
			Values[4] = Convert.ToString(DateTime.Now.Minute);
			Values[5] = Convert.ToString(DateTime.Now.Second);
			File.WriteAllLines(DatePath, Values);
			Console.WriteLine("Datestamp completed");
		}
		private DateTime ConvertFileToDateTime(string FilePath){
			string[] Text = File.ReadAllLines(FilePath);
			int[] NecessaryValues = new int[Text.Length];
			for(int i = 0; i < Text.Length; i++)
				NecessaryValues[i] = Convert.ToInt32(Text[i]);
			return new DateTime(NecessaryValues[0], NecessaryValues[1], NecessaryValues[2], NecessaryValues[3], NecessaryValues[4], NecessaryValues[5]);
		}
		private string MakeWackyFileName(){
			return new Random().Next(int.MinValue, int.MaxValue) + ".txt";
		}
	}
}
