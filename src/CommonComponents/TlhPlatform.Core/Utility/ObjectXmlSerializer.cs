using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml.Serialization; 
namespace TlhPlatform.Core.Utility
{


    public class ObjectXmlSerializer
    {
        private const string LogCategory = "Framework.ObjectXmlSerializer";
        private const int LogEventLoadFileException = 1;
        private const int LogEventXmlDeserializeException = 2;

        public static T LoadFromXml<T>(string fileName) where T: class
        {
            return LoadFromXml<T>(fileName, true);
        }

        public static T LoadFromXml<T>(string fileName, bool needLog) where T: class
        {
            FileStream stream = null;
            T local;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                local = (T) serializer.Deserialize(stream);
            }
            catch //(Exception exception)
            {
                if (needLog)
                {
                    //LogLoadFileException(fileName, exception);
                }
                local = default(T);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
            return local;
        }

        public static T LoadFromXmlMessage<T>(string xmlMessage, bool needLog) where T: class
        {
            StringReader textReader = null;
            T local;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                textReader = new StringReader(xmlMessage);
                local = (T) serializer.Deserialize(textReader);
            }
            catch //(Exception exception)
            {
                if (needLog)
                {
                    //LogXmlDeserializeException(xmlMessage, exception);
                }
                local = default(T);
            }
            finally
            {
                if (textReader != null)
                {
                    textReader.Dispose();
                }
            }
            return local;
        }
         
 
        public static string ToStringXmlMessage<T>(T t, bool needLog) where T: class
        {
            StringWriter writer = null;
            string str;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                writer = new StringWriter();
                serializer.Serialize((TextWriter) writer, t);
                str = writer.ToString();
            }
            catch
            {
                str = string.Empty;
            }
            finally
            {
                if (writer != null)
                {
                    writer.Dispose();
                }
            }
            return str;
        }
 
    }


}

