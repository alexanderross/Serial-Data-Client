using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Web;

using Microsoft.Win32;
using CCTVClient.DataManagers;

namespace CCTVClient.Web
{
    
    public class CCTVWebServer : HTTPServer
    {
        public string Folder;
        public HTTPPageProcessor Processor;

        public CCTVWebServer(DataManagerPool ProcData): base()
        {
            Processor = new HTTPPageProcessor (ProcData);
            this.Folder = "www";
        }

        public CCTVWebServer(DataManagerPool ProcData,int thePort, string theFolder)
            : base(thePort)
        {
            Processor = new HTTPPageProcessor(ProcData);
            this.Folder = Directory.GetCurrentDirectory() + "\\" + theFolder;
            if (!Directory.Exists(Folder))
            {
                Directory.CreateDirectory(Folder);
            }
        }

        public override void OnResponse(ref HTTPRequestStruct rq, ref HTTPResponseStruct rp)
        {
            string path = this.Folder + "\\" + rq.URL.Replace("/", "\\");

            if (Directory.Exists(path))
            {
                if (File.Exists(path + "default.htm"))
                    path += "\\default.htm";
                else
                {
                    string[] dirs = Directory.GetDirectories(path);
                    string[] files = Directory.GetFiles(path);

                    string bodyStr = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">\n";
                    bodyStr += "<HTML><HEAD>\n";
                    bodyStr += "<META http-equiv=Content-Type content=\"text/html; charset=windows-1252\">\n";
                    bodyStr += "</HEAD>\n";
                    bodyStr += "<BODY><p>Folder listing, to do not see this add a 'default.htm' document\n<p>\n";
                    for (int i = 0; i < dirs.Length; i++)
                        bodyStr += "<br><a href = \"" + rq.URL + Path.GetFileName(dirs[i]) + "/\">[" + Path.GetFileName(dirs[i]) + "]</a>\n";
                    for (int i = 0; i < files.Length; i++)
                        bodyStr += "<br><a href = \"" + rq.URL + Path.GetFileName(files[i]) + "\">" + Path.GetFileName(files[i]) + "</a>\n";
                    bodyStr += "</BODY></HTML>\n";

                    rp.BodyData = Encoding.ASCII.GetBytes(bodyStr);
                    return;
                }
            }

            if (File.Exists(path))
            {
                RegistryKey rk = Registry.ClassesRoot.OpenSubKey(Path.GetExtension(path), true);

                // Get the data from a specified item in the key.
                String s = (String)rk.GetValue("Content Type");

                // Open the stream and read it back.
                FileStream tempFS = File.Open(path, FileMode.Open);
                rp.fs = Processor.ProcessPage(tempFS);
                if (s != "")
                    rp.Headers["Content-type"] = s;
            }
            else
            {
                rp.status = (int)RespState.NOT_FOUND;
                string bodyStr = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">\n";
                bodyStr += "<HTML><HEAD>\n";
                bodyStr += "<META http-equiv=Content-Type content=\"text/html; charset=windows-1252\">\n";
                bodyStr += "</HEAD>\n";
                bodyStr += "<BODY>File not found at "+path+"!!</BODY></HTML>\n";

                rp.BodyData = Encoding.ASCII.GetBytes(bodyStr);

            }

        }
    }
}
