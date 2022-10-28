using System.Collections.Concurrent;
using System.IO.Compression;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Scanner_lib;

public class DirectoryTree
{
    public string Path { get; set; }
    
    public FileType Extension { get; set; }

    public long Size { get; set; } = 0;

    public double Percent { get; set; }

    public string Name { get; set; } = "";
    

    [JsonIgnore]
    public DirectoryTree? Parent{ get; set; } = null;

    public ConcurrentQueue<DirectoryTree> Childs { get; set; } = new ConcurrentQueue<DirectoryTree>();

    
    //ctr for file
    public DirectoryTree(string path,long size,string name,FileType extension,DirectoryTree parent)
    {
        this.Path = path;
        this.Size = size;
        this.Name = name;
        this.Extension = extension;
        this.Parent = parent;
    }
    
    //ctr for directory
    public DirectoryTree(string path,string name,FileType extension,DirectoryTree parent)
    {
        this.Path = path; 
        this.Name = name;
        this.Extension = extension;
        this.Parent = parent;
    }

    public DirectoryTree() { }
    public void ToJson()
    {
        var options = new JsonSerializerOptions
            { WriteIndented = true };
        var json = JsonSerializer.Serialize( this, options );
        using var fileStream = new FileStream($"resultJSON.txt", FileMode.Create);
        fileStream.Write(Encoding.Default.GetBytes(json));
       
    }
    
    public enum FileType
    {
        Folder,
        File
    }
    
}