using Newtonsoft.Json;
using Opener.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;

namespace Opener.Helpers
{
    public class OpenerKeyProcessor
    {
        private readonly OpenerDataHelper openerDataHelper;
        public OpenerKeyProcessor(OpenerDataHelper openerDataHelper)
        {
            this.openerDataHelper = openerDataHelper;
        }
        public void ProcessArgs(string[] args)
        {
            var firstArg = args[0];
            var remainingArgs = args.Skip(1).ToList();
            if (string.Equals(firstArg, "en", StringComparison.OrdinalIgnoreCase))
            {
                ConvertToBase64(remainingArgs.First());
            }
            else if (string.Equals(firstArg, "de", StringComparison.OrdinalIgnoreCase))
            {
                ConvertFromBase64(remainingArgs.First());
            }
            else if (string.Equals(firstArg, "tf", StringComparison.OrdinalIgnoreCase))
            {
                OpenTempFile(remainingArgs.First());
            }
            else if (string.Equals(firstArg, "gs", StringComparison.OrdinalIgnoreCase))
            {
                GetJiraSummary(remainingArgs);
            }
            else
            {
                ProcessStoredKeys(firstArg, remainingArgs);
            }
        }
        private void OpenTempFile(string filePath)
        {
            string fileName = filePath;
            if (filePath.StartsWith("."))
            {
                fileName = $"{DateTime.Now.Ticks}{filePath}";
            }
            var requiredPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.InternetCache), fileName);
            Process.Start(openerDataHelper.GetKey("editor").Path, requiredPath);
        }
        private void GetJiraSummary(List<string> remainingArgs)
        {
            string project = openerDataHelper.GetKey("defaultjiraproject").Path;
            string jiraUrl = openerDataHelper.GetKey("jiraurl").Path;
            var projectOptionIndex = remainingArgs.IndexOf("-p");
            if (projectOptionIndex != -1)
            {
                project = remainingArgs.ElementAtOrDefault(projectOptionIndex);
            }
            Process.Start($"{jiraUrl}/{project}-{remainingArgs.First()}");
        }
        private void ConvertToBase64(string data)
        {
            var bytes = Encoding.UTF8.GetBytes(data);
            Clipboard.SetText(Convert.ToBase64String(bytes));
        }
        private void ConvertFromBase64(string data)
        {
            var bytes = Convert.FromBase64String(data);
            Clipboard.SetText(Encoding.UTF8.GetString(bytes));
        }
        private void ProcessStoredKeys(string firstArg, List<string> args)
        {
            var key = openerDataHelper.GetKey(firstArg);
            if (key != null)
            {
                if (key.KeyType.Id == (int)KeyTypeId.WebPath)
                {
                    var formattedargs = args.Select(m => WebUtility.UrlEncode(m)).ToArray();
                    Process.Start(string.Format(key.Path, formattedargs));
                }
                else if (key.KeyType.Id == (int)KeyTypeId.LocalPath)
                {
                    Process.Start(string.Format(key.Path, args));
                }
                else if(key.KeyType.Id == (int)KeyTypeId.Data)
                {
                    Clipboard.SetText(key.Path, TextDataFormat.Text);
                }
                else if (key.KeyType.Id == (int)KeyTypeId.SecureData)
                {
                    Clipboard.SetText(key.Path.Base64Decode(), TextDataFormat.Text);
                }
                else if (key.KeyType.Id == (int)KeyTypeId.JsonData)
                {
                    var data = JsonConvert.DeserializeObject(key.Path);
                    Clipboard.SetText(JsonConvert.SerializeObject(data,Formatting.Indented), TextDataFormat.Text);
                }
            }
        }
        
    }
}
