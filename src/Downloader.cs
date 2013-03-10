using System;
using System.Net;
using System.Net.NetworkInformation;
using FileHelper;
using System.IO;
using System.Net.Sockets;
using System.ComponentModel;

namespace GameUpdater {
	public class Downloader {
		private string FilePath;
		private string DatePath;
		private string FileSite;
		private string DateSite;
		private bool HasInternet;
		private double Percentage;
		private bool DownloadComplete = false;
		public Downloader(string FileName, string DateName, string FileSite, string DateSite){
			FilePath = PathGetter.GetDirectoryPath() + FileName;
			DatePath = PathGetter.GetDirectoryPath() + DateName;
			this.FileSite = FileSite;
			this.DateSite = DateSite;
			Console.WriteLine("Checking if internet connection is available...");
			try {
				Ping Google = new Ping();
				string GoogleURL = "www.google.com";
				IPAddress[] GoogleIPs = Dns.GetHostAddresses(GoogleURL);
				PingReply Reply = Google.Send(GoogleIPs[new Random().Next(0, GoogleIPs.Length)]);
				if(Reply.Status == IPStatus.Success){
					HasInternet = true;
					Console.WriteLine("Internet connection available");
				}
				else
					HasInternet = false;
				Google.Dispose();
			} catch (SocketException) {
				HasInternet = false;
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
						if(DateTime.Compare(TimeOnFile, TimeOfUpdate) > 0){
							DownloadComplete = true;
							Console.WriteLine("No download necessary");
						}
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
			else {
				DownloadComplete = true;
				Console.WriteLine("No internet detected, no update will be downloaded");
			}
		}
		private void DownloadFiles(){
			Console.WriteLine("Downloading...");
			WebClient webClient = new WebClient();
			webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(webClient_DownloadProgressChanged);
			webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(webClient_DownloadFileCompleted);
			webClient.DownloadFileAsync(new Uri(FileSite), FilePath);
			webClient.Dispose();
		}
		private DateTime ConvertFileToDateTime(string FilePath){
			string[] Text = File.ReadAllLines(FilePath);
			int[] NecessaryValues = new int[Text.Length];
			for(int i = 0; i < Text.Length; i++)
				NecessaryValues[i] = Convert.ToInt32(Text[i]);
			return new DateTime(NecessaryValues[0], NecessaryValues[1], NecessaryValues[2], NecessaryValues[3], NecessaryValues[4], NecessaryValues[5]);
		}
		private void webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e){
			double BytesIn = double.Parse(e.BytesReceived.ToString());
			double TotalBytes = double.Parse(e.TotalBytesToReceive.ToString());
			Percentage = Math.Round(BytesIn / TotalBytes * 1000) / 10;
			Console.WriteLine(Percentage + "% done");
		}
		private void webClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e){
			Console.WriteLine("Success");
			Console.WriteLine("Making datestamp...");
			File.CreateText(DatePath).Close();
			string[] Values = new string[6];
			Values[0] = Convert.ToString(DateTime.UtcNow.Year);
			Values[1] = Convert.ToString(DateTime.UtcNow.Month);
			Values[2] = Convert.ToString(DateTime.UtcNow.Day);
			Values[3] = Convert.ToString(DateTime.UtcNow.Hour);
			Values[4] = Convert.ToString(DateTime.UtcNow.Minute);
			Values[5] = Convert.ToString(DateTime.UtcNow.Second);
			File.WriteAllLines(DatePath, Values);
			Console.WriteLine("Datestamp completed");
			DownloadComplete = true;
		}
		public double GetPercentage(){
			return Percentage;
		}
		public bool GetDownloadComplete(){
			return DownloadComplete;
		}
		public bool GetHasInternet(){
			return HasInternet;
		}
	}
}
