using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerarbeitungTest.Tests
{
    /// <summary>
    /// Class for testing the SaveTestResult Class
    /// </summary>
    internal class SaveTestResultTest
    {
        public static void RunTest()
        {
           
            SaveTestResult.SaveResult("debugtest.save", 500,"TestUser1");
            SaveTestResult.SaveResult("debugtest.save", 100, "TestUser2");
            SaveTestResult.SaveResult("debugtest.save", 200, "TestUser3");
            List<JsonSavePackage> packages = SaveTestResult.LoadResults("debugtest.save");
            if(packages == null ) 
            {
                Console.WriteLine("Test -> SaveTestResults : FAILED Code:1 Packages=null");
            }
            else if( packages.Count != 3)
            {
                Console.WriteLine("Test -> SaveTestResults : FAILED false Packcount="+packages.Count);
            }
            else
            {
                JsonSavePackage loadedData = packages[0];
                if(loadedData.Score == 500 && loadedData.Name == "TestUser1")
                {
                    Console.WriteLine("Test -> SaveTestResults : OK");
                }
                else
                {
                    Console.WriteLine("Test -> SaveTestResults : FAILED Data Corruption");
                }
            }
            if (File.Exists("debugtest.save"))//Delete SaveFile after Test
            {
                File.Delete("debugtest.save");
            }


        }
    }
}
