﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Web;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace CCTVClient.Web
{
    enum RState
    {
        METHOD, URL, URLPARM, URLVALUE, VERSION,
        HEADERKEY, HEADERVALUE, BODY, OK
    };

    enum RespState
    {
        OK = 200,
        BAD_REQUEST = 400,
        NOT_FOUND = 404
    }

    public struct HTTPRequestStruct
    {
        public string Method;
        public string URL;
        public string Version;
        public Hashtable Args;
        public bool Execute;
        public Hashtable Headers;
        public int BodySize;
        public byte[] BodyData;
    }

    public struct HTTPResponseStruct
    {
        public int status;
        public string version;
        public Hashtable Headers;
        public int BodySize;
        public byte[] BodyData;
        public System.IO.MemoryStream fs;
    }

    /// <SUMMARY>
    /// Summary description for CslocHTTPRequest.
    /// </SUMMARY>
    public class HTTPRequest
    {
        private TcpClient client;

        private RState ParserState;

        private HTTPRequestStruct locHTTPRequest;

        private HTTPResponseStruct HTTPResponse;

        private StreamReader reader;

        byte[] myReadBuffer;

        HTTPServer Parent;

        public HTTPRequest(TcpClient client, HTTPServer Parent)
        {
            this.client = client;
            this.Parent = Parent;

            this.HTTPResponse.BodySize = 0;
        }

        public void Process()
      {
         myReadBuffer = new byte[client.ReceiveBufferSize];
         String myCompleteMessage = "";
         int numberOfBytesRead = 0;

         Parent.WriteLog("Connection accepted. Buffer: " + client.ReceiveBufferSize.ToString());
         NetworkStream ns = client.GetStream();
         StreamWriter writer = new StreamWriter(ns);

         string hValue = "";
         string hKey = "";

         try 
         {
            // binary data buffer index
            int bfndx = 0;

            // Incoming message may be larger than the buffer size.
            do
            {
               numberOfBytesRead = ns.Read(myReadBuffer, 0,myReadBuffer.Length);  
               myCompleteMessage = 
                  String.Concat(myCompleteMessage, 
                     Encoding.ASCII.GetString(myReadBuffer, 0,numberOfBytesRead));  
               
               // read buffer index
               int ndx = 0;
               do
               {
                  switch ( ParserState )
                  {
                     case RState.METHOD:
                        if (myReadBuffer[ndx] != ' ')
                           locHTTPRequest.Method += (char)myReadBuffer[ndx++];
                        else 
                        {
                           ndx++;
                           ParserState = RState.URL;
                        }
                        break;
                     case RState.URL:
                        if (myReadBuffer[ndx] == '?')
                        {
                           ndx++;
                           hKey = "";
                           locHTTPRequest.Execute = true;
                           locHTTPRequest.Args = new Hashtable();
                           ParserState = RState.URLPARM;
                        }
                        else if (myReadBuffer[ndx] != ' ')
                           locHTTPRequest.URL += (char)myReadBuffer[ndx++];
                        else
                        {
                           ndx++;
                           locHTTPRequest.URL= HttpUtility.UrlDecode(locHTTPRequest.URL);
                           ParserState = RState.VERSION;
                        }
                        break;
                     case RState.URLPARM:
                        if (myReadBuffer[ndx] == '=')
                        {
                           ndx++;
                           hValue="";
                           ParserState = RState.URLVALUE;
                        }
                        else if (myReadBuffer[ndx] == ' ')
                        {
                           ndx++;
                        
                           locHTTPRequest.URL= HttpUtility.UrlDecode(locHTTPRequest.URL);
                           ParserState = RState.VERSION;
                        }
                        else
                        {
                           hKey += (char)myReadBuffer[ndx++];
                        }
                        break;
                     case RState.URLVALUE:
                        if (myReadBuffer[ndx] == '&')
                        {
                           ndx++;
                           hKey=HttpUtility.UrlDecode(hKey);
                           hValue=HttpUtility.UrlDecode(hValue);
                           locHTTPRequest.Args[hKey] =  locHTTPRequest.Args[hKey] != null ? 
                                    locHTTPRequest.Args[hKey] + ", " + hValue : hValue;
                           hKey="";
                           ParserState = RState.URLPARM;
                        }
                        else if (myReadBuffer[ndx] == ' ')
                        {
                           ndx++;
                           hKey=HttpUtility.UrlDecode(hKey);
                           hValue=HttpUtility.UrlDecode(hValue);
                           locHTTPRequest.Args[hKey] = locHTTPRequest.Args[hKey] != null ?
                                   locHTTPRequest.Args[hKey] + ", " + hValue : hValue;
                           
                           locHTTPRequest.URL= HttpUtility.UrlDecode(locHTTPRequest.URL);
                           ParserState = RState.VERSION;
                        }
                        else
                        {
                           hValue += (char)myReadBuffer[ndx++];
                        }
                        break;
                     case RState.VERSION:
                        if (myReadBuffer[ndx] == '\r') 
                           ndx++;
                        else if (myReadBuffer[ndx] != '\n') 
                           locHTTPRequest.Version += (char)myReadBuffer[ndx++];
                        else 
                        {
                           ndx++;
                           hKey = "";
                           locHTTPRequest.Headers = new Hashtable();
                           ParserState = RState.HEADERKEY;
                        }
                        break;
                     case RState.HEADERKEY:
                        if (myReadBuffer[ndx] == '\r') 
                           ndx++;
                        else if (myReadBuffer[ndx] == '\n')
                        {
                           ndx++;
                           if (locHTTPRequest.Headers["Content-Length"] != null)
                           {
                              locHTTPRequest.BodySize = 
                       Convert.ToInt32(locHTTPRequest.Headers["Content-Length"]);
                              this.locHTTPRequest.BodyData= new byte[this.locHTTPRequest.BodySize];
                              ParserState = RState.BODY;
                           }
                           else
                              ParserState = RState.OK;
                           
                        }
                        else if (myReadBuffer[ndx] == ':')
                           ndx++;
                        else if (myReadBuffer[ndx] != ' ')
                           hKey += (char)myReadBuffer[ndx++];
                        else 
                        {
                           ndx++;
                           hValue = "";
                           ParserState = RState.HEADERVALUE;
                        }
                        break;
                     case RState.HEADERVALUE:
                        if (myReadBuffer[ndx] == '\r') 
                           ndx++;
                        else if (myReadBuffer[ndx] != '\n')
                           hValue += (char)myReadBuffer[ndx++];
                        else 
                        {
                           ndx++;
                           locHTTPRequest.Headers.Add(hKey, hValue);
                           hKey = "";
                           ParserState = RState.HEADERKEY;
                        }
                        break;
                     case RState.BODY:
                        // Append to request BodyData
                        Array.Copy(myReadBuffer, ndx, this.locHTTPRequest.BodyData,bfndx, numberOfBytesRead - ndx);
                        bfndx += numberOfBytesRead - ndx;
                        ndx = numberOfBytesRead;
                        if ( this.locHTTPRequest.BodySize <=  bfndx)
                        {
                           ParserState = RState.OK;
                        }
                        break;
                        //default:
                        //   ndx++;
                        //   break;

                  }
               }
               while(ndx < numberOfBytesRead);

            }
            while(ns.DataAvailable);

            // Print out the received message to the console.
            Parent.WriteLog("You received the following message : \n" +
               myCompleteMessage);
            
            HTTPResponse.version = "HTTP/1.1";

            if (ParserState != RState.OK)
               HTTPResponse.status = (int)RespState.BAD_REQUEST;
            else
               HTTPResponse.status = (int)RespState.OK;

            this.HTTPResponse.Headers = new Hashtable();
            this.HTTPResponse.Headers.Add("Server", Parent.Name);
            this.HTTPResponse.Headers.Add("Date", DateTime.Now.ToString("r"));
            
            // if (HTTPResponse.status == (int)RespState.OK)
            this.Parent.OnResponse(ref this.locHTTPRequest,ref this.HTTPResponse);

            string HeadersString = this.HTTPResponse.version + " " 
               + this.Parent.respStatus[this.HTTPResponse.status] + "\n";
            
            foreach (DictionaryEntry Header in this.HTTPResponse.Headers) 
            {
               HeadersString += Header.Key + ": " + Header.Value + "\n";
            }

            HeadersString += "\n";
            byte[] bHeadersString = Encoding.ASCII.GetBytes(HeadersString);

            // Send headers   
            ns.Write(bHeadersString, 0, bHeadersString.Length);
   
            // Send body
            if (this.HTTPResponse.BodyData != null)
            ns.Write(this.HTTPResponse.BodyData, 0, this.HTTPResponse.BodyData.Length);

            if (this.HTTPResponse.fs != null)
            {
                reader = new StreamReader(this.HTTPResponse.fs);
                string line = "";
                while( (line = reader.ReadLine())!=null){
                    writer.WriteLine(line);
                }
                writer.Flush();
                
                
                /*using (this.HTTPResponse.fs)
                {
                    byte[] b = new byte[client.SendBufferSize];
                    int bytesRead;
                    while ((bytesRead = this.HTTPResponse.fs.Read(b, 0, b.Length)) > 0)
                    {
                        ns.Write(b, 0, bytesRead);
                    }

                    this.HTTPResponse.fs.Close();
                }*/
            }
   
         }
         catch (Exception e) 
         {
             Console.WriteLine(e);

         }
         finally 
         {
            ns.Close();
            client.Close();
            if (this.HTTPResponse.fs != null)
               this.HTTPResponse.fs.Close();
            Thread.CurrentThread.Abort();
         }
      }

    }
}
