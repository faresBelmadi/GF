using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
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

            content = CleanupContent(content);

            File.WriteAllText(outputPath, content);
            Debug.Log($"Downloaded CSV for {logInfo}.", AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(outputPath));
        }

        private string CleanupContent(string content)
        {
            var result = content;

            result = Regex.Replace(result, @" (?=[!?:])", "\u00A0"); // non-breakable space before punctuations
            result = Regex.Replace(result, @"[\r\n\u0085\u2028\u2029]+(?=\""?;)", ""); // remove unwanted line jumps

            return result;
        }
    }
}