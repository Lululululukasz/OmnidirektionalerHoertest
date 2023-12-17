using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace VerarbeitungTest
{
    /// <summary>
    /// Package Class.
    /// <para>Used to simplyfy Saveing and Loading Procedure</para>
    /// </summary>
    internal class JsonSavePackage
    {
        public DateTimeOffset Date { get; set; }
        public double Score {  get; set; }
        public string Name { get; set; }
    }
    /// <summary>
    /// Main Save Class.
    /// <para>Saves Testresults in JSON File</para>
    /// <para>to Save just call SaveResult(string fileName,double score,[OPTIONAL]string userName)</para>
    /// <para>to Load just call LoadResults(string fileName) it returns an List(JsonSavePackage) with every saved Result</para>
    /// </summary>
    internal class SaveTestResult
    {
        /// <summary>
        /// Use this Method to Save TestResults to a File
        /// <para>fileName : filename of the SaveFile</para>
        /// <para>score : user score of the finished Test</para>
        /// <para>userName : Optional name of user if not defined it saves the name "Unknown_User"</para>
        /// </summary>
        public static void SaveResult(string fileName,double score,string userName = "Unknown_User")
        {
            if (!File.Exists(fileName))
            {
                FileStream f = File.Create(fileName);
                f.Close();
            }
            string loadedFile = File.ReadAllText(fileName);
            List<JsonSavePackage> packages = null;
            try
            {
                packages = JsonSerializer.Deserialize<List<JsonSavePackage>>(loadedFile);
            }catch(Exception ex)
            {

            }
            
            if(packages == null)
            {
                packages = new List<JsonSavePackage>();
            }
            JsonSavePackage package = new JsonSavePackage();
            package.Score = score;
            package.Name = userName;
            package.Date = DateTimeOffset.Now;
            packages.Add(package);
            string newFile = JsonSerializer.Serialize(packages);
            File.WriteAllText(fileName, newFile);
        }
        /// <summary>
        /// Use this mehod to load and Parse an SaveFile into an JsonSavePackage-List
        /// <para>filename : the filename of the SaveFile</para>
        /// </summary>
        public static List<JsonSavePackage> LoadResults(string fileName)
        {
            if (File.Exists(fileName))
            {
                string loadedFile = File.ReadAllText(fileName);
                List<JsonSavePackage> packages = JsonSerializer.Deserialize<List<JsonSavePackage>>(loadedFile);
                if (packages == null)
                {
                    packages = new List<JsonSavePackage>();
                }
                return packages;
            }
            else
            {
                return new List<JsonSavePackage>();
            }
        }
    }
}
