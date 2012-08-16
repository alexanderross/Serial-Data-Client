using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using CCTVClient.DataManagers;
using CCTVClient.Data;

namespace CCTVClient.Web
{
    public class HTTPPageProcessor
    {
        private DataManagerPool DataSet;

        public HTTPPageProcessor(DataManagerPool data){
            DataSet=data;
        }

        public MemoryStream ProcessPage(System.IO.Stream rawStream){
            Console.Write("PROCESSING FILE");
            MemoryStream returnStream = new MemoryStream();
           /* int length = (int)rawStream.Length;
            byte[] buffer = new byte[length];
            int count; 
            int sum = 0;
            while ((count = rawStream.Read(buffer, sum, length - sum)) > 0)
                sum += count;  // sum is a buffer offset for next reading
            */
            //rawStream.CopyTo(returnStream);
            StreamWriter tempWriter = new StreamWriter(returnStream);
            StreamReader tempReader = new StreamReader(rawStream);
            string line="";
            // Read and display lines from the file until the end of  
            // the file is reached. 
            while ((line = tempReader.ReadLine()) != null)
            {
                tempWriter.WriteLine(ProcessLine(line));
            }
            tempWriter.Flush();
            returnStream.Seek(0, SeekOrigin.Begin);
            tempReader.Close();
            //tempWriter.Close();
            return returnStream;
        }

        private String ProcessLine(String input)
        {
            String inside;

            if ((inside = Regex.Match(input, @"\(\!(.*)\!\)").Groups[1].Value) != "")
            {
                return ProcessTag(inside.Trim());
            }
            else
            {
                return input;
            }
        }

        private String ProcessTag(String input)
        {
            try
            {
                String[] parsedCmd = input.Split('(');
                parsedCmd[0] = parsedCmd[0].ToUpper();
                parsedCmd[1] = parsedCmd[1].Replace(")", "");
                switch (parsedCmd[0])
                {
                    case "SENSORS.RENDER":
                        return RenderAllSensors();
                    case "SENSORS.RENDER.BYNAME":
                        return RenderSensor(parsedCmd[1]);
                    case "SENSORS.RENDER.ALL":
                        return RenderAllSensors();
                    default:
                        return ErrorCode(0);
                }
            }
            catch (Exception ex)
            {
                return ErrorCode(0);
            }

        }

        private String RenderAllSensors()
        {
            String output="";
            foreach(KeyValuePair<String,MCUDataAsset> item in DataSet.MCU.DataItems){
                output+=item.Value.ToHTML()+"\r\n"; 
            }
            return output;
        }

        private String RenderAllSensors(String[] attribs)
        {
            return String.Empty;
        }

        private String RenderSensor(String sensorName){
            if(DataSet.MCU.DataItems.ContainsKey(sensorName)){
                return DataSet.MCU.DataItems[sensorName].ToHTML();
            }else{
                return ErrorCode(1)+":: Sensor '"+sensorName+"'... are you sure it's spelled correctly?";
            }
        }

        private String RenderSensor(String sensorName,String[] attribs){
            return String.Empty;
        }

        private String ErrorCode(int code)
        {
            String output="";
            switch (code) {
                case 0:
                    output += "Invalid Command syntax, please check the documentation for proper use.";
                    break;
                case 1:
                    output += "Sensor not found in current data definition.";
                    break;
                case 2:
                    output += "Sensor attribute not found in current data definition.";
                    break;
                default:
                    output += "I have no idea what that code is.";
                    break;
            }
            return output;

        }
    }
}
