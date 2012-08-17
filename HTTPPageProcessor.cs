/* 
 * Version 0.3:
	SENSORS.RENDER.BYTYPE(Type) : Render all sensors by type ('DIGITAL','ANALOG')
	SENSORS[SENSOR_NAME].RENDER() : Same as SENSORS.RENDER.BYNAME(SENSOR_NAME), in fact depreciates it...
	SENSORS[SENSOR_NAME].RENDERATTRIBUTE(ATTR_NAME) : Render the specific attribute of the sensor
	SERIAL.RENDER() : Render the current status of the client's serial connection
	SERIAL.RENDER.PORT(): Render the port being used by the serial connection
	SERIAL.RENDER.RATE(): Render the baud rate currently in use for said connection
 */


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
                String[] parsedCmd = input.Split('.');
                String specificName = "";
                specificName = Regex.Match(parsedCmd[0], @"\[(.*)\]").Groups[1].Value;

                if (parsedCmd[0].Contains("SENSORS"))
                {
                    return ProcessSensorCommand(parsedCmd[1] +"."+ parsedCmd[2], specificName);
                }
                else if (parsedCmd[0].Contains("SERIAL"))
                {
                    return ProcessSerialCommand(parsedCmd[1],specificName);
                }
                else if (parsedCmd[0].Contains("RENDERING"))
                {
                    return "Render Commands not implemented yet.. sorry.";
                }
                else
                {
                    return ErrorCode(0, input);
                }

            }
            catch (Exception ex)
            {
                return ErrorCode(0,input);
            }

        }
        /*
    -SENSORS.RENDER.BYTYPE(Type) : Render all sensors by type ('DIGITAL','ANALOG')
	-SENSORS[SENSOR_NAME].RENDER() : Same as SENSORS.RENDER.BYNAME(SENSOR_NAME), in fact depreciates it...
	SENSORS[SENSOR_NAME].RENDER.ATTRIBUTE(ATTR_NAME) : Render the specific attribute of the sensor
    -SENSORS.RENDER.ALL() : Render all available sensors currently active in the client.
	-SENSORS.RENDER() : Same as SENSORS.RENDER()
     */

        private String ProcessSensorCommand(string command,string name)
        {
            String[] cmd = command.Split('.');
            if (name != "")
            {
                if (cmd[0] == "RENDER")
                {
                    if (cmd[1] == "ATTRIBUTE")
                    {
                        return "TODO: Implement Attribute lookup";
                    }
                    else
                    {
                        return RenderSensor(name);
                    }
                }
            }
            else
            {
                if (cmd[0] == "RENDER")
                {
                    if (cmd.Length > 1)
                    {
                        if (cmd[1].Contains("BYTYPE"))
                        {
                            try
                            {
                                return RenderSensorsByType(Regex.Match(cmd[1], @"\((.*)\)").Groups[1].Value);
                            }
                            catch (Exception e)
                            {
                                return ErrorCode(4, command);
                            }
                        }
                        else if(cmd[1].Contains("ALL"))
                        {
                            return RenderAllSensors();
                        }
                    }
                    else
                    {
                        return RenderAllSensors();
                    }
                }
            }
            return "";
        }

        private String ProcessSerialCommand(string command,string name)
        {
            return "";
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

        private String RenderSensorsByType(String type)
        {
            String output = "";
                foreach (MCUDataAsset data in DataSet.MCU.DataItems.Values)
                {
                    if (data.GetType() == type)
                    {
                        output += data.ToHTML();
                    }
                }
                return output;
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

        private String ErrorCode(int code,string context="")
        {
            String output="(CODE ERROR)";
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
                case 3:
                    output += "Sensor type not found in current data definition.";
                    break;
                case 4:
                    output += "Argument Formatting error.";
                    break;
                default:
                    output += "I have no idea what that code is.";
                    break;
            }
            if (context != "")
            {
                output += " -- Around ('"+context+"')";
            }
            return output;

        }
    }
}
