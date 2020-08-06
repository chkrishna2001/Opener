using Opener.Models;
using System;
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
            var remainingArgs = args.Skip(1).ToArray();
            if (string.Equals(firstArg, "en", StringComparison.OrdinalIgnoreCase))
            {
                ConvertToBase64(remainingArgs[0]);
            }
            else if (string.Equals(firstArg, "de", StringComparison.OrdinalIgnoreCase))
            {
                ConvertFromBase64(remainingArgs[0]);
            }
            else
            {
                ProcessStoredKeys(firstArg, remainingArgs);
            }
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
        private void ProcessStoredKeys(string firstArg, string[] args)
        {
            var key = openerDataHelper.GetKey(firstArg);
            if (key != null)
            {
                if (key.KeyType.Id == (int)KeyTypeId.WebPath)
                {
                    var formattedargs = args.Select(m => WebUtility.UrlEncode(m)).ToArray();
                    System.Diagnostics.Process.Start(string.Format(key.Path, formattedargs));
                }
                else if(key.KeyType.Id == (int)KeyTypeId.Data)
                {
                    Clipboard.SetText(key.Path, TextDataFormat.Text);
                }
            }
        }
        
    }
}
