using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;

using System.Collections.Generic;
using System.Linq;
using System.Web;

using Azure;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;
using Azure.Storage.Sas;


namespace WebItNow.Controllers
{
    public class AlmacenamientoArchivos
    {
        //-------------------------------------------------
        // Create a file share
        //-------------------------------------------------
        public async Task CreateShareAsync(string shareName)
        {
            // Get the connection string from app settings
            string connectionString = ConfigurationManager.AppSettings["StorageConnectionString"];

            // Instantiate a ShareClient which will be used to create and manipulate the file share
            ShareClient share = new ShareClient(connectionString, shareName);

            // Create the share if it doesn't already exist
            await share.CreateIfNotExistsAsync();

            // Ensure that the share exists
            if (await share.ExistsAsync())
            {
                Console.WriteLine($"Share created: {share.Name}");

                // Get a reference to the sample directory
                ShareDirectoryClient directory = share.GetDirectoryClient("CustomLogs");

                // Create the directory if it doesn't already exist
                await directory.CreateIfNotExistsAsync();

                // Ensure that the directory exists
                if (await directory.ExistsAsync())
                {
                    // Get a reference to a file object
                    ShareFileClient file = directory.GetFileClient("Log1.txt");

                    // Ensure that the file exists
                    if (await file.ExistsAsync())
                    {
                        Console.WriteLine($"File exists: {file.Name}");

                        // Download the file
                        ShareFileDownloadInfo download = await file.DownloadAsync();

                        // Save the data to a local file, overwrite if the file already exists
                        using (FileStream stream = File.OpenWrite(@"downloadedLog1.txt"))
                        {
                            await download.Content.CopyToAsync(stream);
                            await stream.FlushAsync();
                            stream.Close();

                            // Display where the file was saved
                            Console.WriteLine($"File downloaded: {stream.Name}");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine($"CreateShareAsync failed");
            }
        }

    }

}