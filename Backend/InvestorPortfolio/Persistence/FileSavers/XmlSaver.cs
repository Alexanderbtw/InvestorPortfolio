
using System.Xml.Serialization;
using Persistence.Interfaces;

namespace Persistence.FileSavers;

public class XmlSaver<T> : IFileSaver<T>
{
    public T Recovery(string path)
    {
        try 
        {
            XmlSerializer x = new XmlSerializer(typeof(T));
            return (T)x.Deserialize(File.OpenRead(path))!;
        }
        catch 
        {
            return default(T)!;
        }
    }

    public bool Save(T objectToSave, string path)
    {
        try 
        {
            XmlSerializer x = new XmlSerializer(typeof(T));
            using TextWriter writer = new StreamWriter(path);
            x.Serialize(writer, objectToSave);
            return true;
        }
        catch 
        {
            return false;
        }
    }
}