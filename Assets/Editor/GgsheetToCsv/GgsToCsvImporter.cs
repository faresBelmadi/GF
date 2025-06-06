using System;
using System.IO;
using System.Net;
using UnityEditor;
using UnityEngine;

namespace GF.GgsToCsv
{
    public class GgsToCsvImporter
    {
        public void Import(string macroLink, string outputPath, string logInfo)
        {
            string content = string.Empty;
            try
            {
                using (WebClient client = new WebClient())
                {
                    content = client.DownloadString(macroLink);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to download {logInfo} from Web App: {ex.Message}");
                return;
            }

            if (string.IsNullOrEmpty(content))
            {
                Debug.LogError($"Web App provided empty content for {logInfo}!");
                return;
            }
            else if (content.StartsWith("Error:"))
            {
                Debug.LogError($"Web App returned for {logInfo}: {content}");
                return;
            }

            File.WriteAllText(outputPath, content);
            Debug.Log($"Downloaded CSV for {logInfo}.", AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(outputPath));
        }
    }
}