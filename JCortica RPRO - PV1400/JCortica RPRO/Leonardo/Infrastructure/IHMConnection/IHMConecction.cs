using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using System.Xml.Linq;
using TesteImpresao.Domain.Errors;
using TesteImpresao.Domain.Result;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace TesteImpresao.Infrastructure.IHMConnection
{
    public class IHMConnection
    {
        public string IP { get; set; }
        public string Port { get; set; } = "21";
        public string PathDirectory { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public IHMConnection SetAnonymousUser()
        {
            Username = "Anonymous";
            Password = "";
            return this;
        }
        public Result SetIP(string Ip)
        {
            string ipv4Pattern = @"^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$";

            if(!Regex.IsMatch(Ip, ipv4Pattern))
            {
                return Result.Failure(ConnectionError.InvalidIPAdress());
            }
            IP = Ip;

            return Result.Sucess();

        }
        public IHMConnection SetPort(string port)
        {
            Port = port;
            return this;
        }

        public IHMConnection SetDirectory(string path)
        {
            PathDirectory = path;
            return this;
        }

        public async Task DownloadFile(string ihmFilePath, string localFilePath)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ihmFilePath);

                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.Credentials = new NetworkCredential(Username, Password);
                request.UseBinary = true;

                using (FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync())
                using (Stream responseStream = response.GetResponseStream())
                using (FileStream fileStream = new FileStream(localFilePath, FileMode.Create))
                {
                    responseStream.CopyTo(fileStream);
                }

            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task GetFileByName(string name)
        {
            List<string> fileNames = await GetListFileNames();

            if (fileNames.Contains(name))
            {
                Console.WriteLine(name);
            } else
            {
                Console.WriteLine($"O arquivo: {name} não existe");
            }
            
        }

        public async Task<string> GetNameLastUpdatedFile()
        {
            List<string> fileNames= await GetListFileNames();
            string lastUpdatedFile = null;
            try
            {
                foreach (var fileName in fileNames)
                {
                    Console.WriteLine(fileName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return lastUpdatedFile;
        }


        private async Task<List<string>> GetListFileNames()
        {
            List<string> result = new List<string>();
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(GetURI());

                request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                request.Credentials = new NetworkCredential(Username, Password);


                using (FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string line;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        string[] parts = line.Split(' ');
                        string fileName = parts[parts.Length - 1];
                        result.Add(fileName);
                    }
                }
            }
            catch (WebException ex)
            {
                Console.WriteLine($"Erro ao acessar o FTP: {ex.Message}");
            }
            return result;
        }

        private async Task<List<FileDetails>> GetListFileDetails()
        {
            List<FileDetails> result = new List<FileDetails>();
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(GetURI());

                request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                request.Credentials = new NetworkCredential(Username, Password);

            
                using (FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string line;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        var fileDetail = ParseFileDetails(line);
                        
                        if (fileDetail != null)
                        {
                            result.Add(fileDetail);
                             Console.WriteLine($"{line}");
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                Console.WriteLine($"Erro ao acessar o FTP: {ex.Message}");
            }
            return result;
        }
        public FileDetails ParseFileDetails(string line)
        {
            string[] parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            var IsInvalidLine = parts.Length < 6;
            if (IsInvalidLine)
            {
                return null;
            }
            try
            {
                string fileName;
                string dateString;
                DateTime modificationDate;
                var isCurrentYear = parts[7].Contains(":");

                if (isCurrentYear)
                {
                    fileName = parts[parts.Length - 1];
                    dateString = $"{parts[5]} {parts[6]} 2024";
                    modificationDate = DateTime.Parse(dateString);
                    return new FileDetails(fileName, modificationDate);
                }

                fileName = parts[parts.Length - 1];
                dateString = $"{parts[5]} {parts[6]} {parts[7]}";
                modificationDate = DateTime.Parse(dateString);
                return new FileDetails(fileName, modificationDate);
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }


        public string GetURI()
        {
            return $"ftp://{IP}:{Port}/{PathDirectory}";
        }
    }
}

