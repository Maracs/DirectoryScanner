using Scanner_lib;

namespace Tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void UsualDirectoryTest()
    {
        DirectoryTree tree = DirectoryScanner.StartScan("C:\\Users\\LEGION Y540\\Desktop\\TestDir");
        
        
        Assert.That(tree.Name,Is.EqualTo("TestDir"));
        Assert.That(tree.Size,Is.EqualTo(26));
        Assert.That(tree.Childs.Count,Is.EqualTo(3));
        Assert.That(tree.Percent,Is.EqualTo(1));
        
        Assert.That(tree.Childs.ToArray()[0].Name,Is.EqualTo("Новая папка"));
        Assert.That(tree.Childs.ToArray()[0].Size,Is.EqualTo(13));
        Assert.That(tree.Childs.ToArray()[0].Childs.Count,Is.EqualTo(1));
        Assert.That(tree.Childs.ToArray()[0].Percent,Is.EqualTo(0.5));
        
        Assert.That(tree.Childs.ToArray()[0].Childs.ToArray()[0].Name,Is.EqualTo("2.txt"));
        Assert.That(tree.Childs.ToArray()[0].Childs.ToArray()[0].Size,Is.EqualTo(13));
        Assert.That(tree.Childs.ToArray()[0].Childs.ToArray()[0].Childs.Count,Is.EqualTo(0));
        Assert.That(tree.Childs.ToArray()[0].Childs.ToArray()[0].Percent,Is.EqualTo(1));
        
        Assert.That(tree.Childs.ToArray()[1].Name,Is.EqualTo("Новая папка (2)"));
        Assert.That(tree.Childs.ToArray()[1].Size,Is.EqualTo(13));
        Assert.That(tree.Childs.ToArray()[1].Childs.Count,Is.EqualTo(1));
        Assert.That(tree.Childs.ToArray()[1].Percent,Is.EqualTo(0.5));
        
        Assert.That(tree.Childs.ToArray()[1].Childs.ToArray()[0].Name,Is.EqualTo("2.txt"));
        Assert.That(tree.Childs.ToArray()[1].Childs.ToArray()[0].Size,Is.EqualTo(13));
        Assert.That(tree.Childs.ToArray()[1].Childs.ToArray()[0].Childs.Count,Is.EqualTo(0));
        Assert.That(tree.Childs.ToArray()[1].Childs.ToArray()[0].Percent,Is.EqualTo(1));
        
        Assert.That(tree.Childs.ToArray()[2].Name,Is.EqualTo("Новая папка (3)"));
        Assert.That(tree.Childs.ToArray()[2].Size,Is.EqualTo(0));
        Assert.That(tree.Childs.ToArray()[2].Childs.Count,Is.EqualTo(0));
        Assert.That(tree.Childs.ToArray()[2].Percent,Is.EqualTo(0));
        
        
    }
    
    [Test]
    public void EmptyDirectoryTest()
    {
        DirectoryTree tree = DirectoryScanner.StartScan("C:\\Users\\LEGION Y540\\Desktop\\EmptyTestDir");
        Assert.That(tree.Name,Is.EqualTo("EmptyTestDir"));
        Assert.That(tree.Size,Is.EqualTo(0));
        Assert.That(tree.Childs.Count,Is.EqualTo(2));
        Assert.That(tree.Percent,Is.EqualTo(1));
        
     
        Assert.That(tree.Childs.ToArray()[0].Name,Is.EqualTo("Новая папка"));
        Assert.That(tree.Childs.ToArray()[0].Size,Is.EqualTo(0));
        Assert.That(tree.Childs.ToArray()[0].Childs.Count,Is.EqualTo(0));
        Assert.That(tree.Childs.ToArray()[0].Percent,Is.EqualTo(0.5));
        
        Assert.That(tree.Childs.ToArray()[1].Name,Is.EqualTo("Новая папка (2)"));
        Assert.That(tree.Childs.ToArray()[1].Size,Is.EqualTo(0));
        Assert.That(tree.Childs.ToArray()[1].Childs.Count,Is.EqualTo(0));
        Assert.That(tree.Childs.ToArray()[1].Percent,Is.EqualTo(0.5));

    }
    
    
    
    [Test]
    public void FileTest()
    {
        DirectoryTree tree = DirectoryScanner.StartScan("C:\\Users\\LEGION Y540\\Desktop\\TestFile.txt");
        Assert.That(tree.Name,Is.EqualTo("TestFile.txt"));
        Assert.That(tree.Size,Is.EqualTo(38));
        Assert.That(tree.Childs.Count,Is.EqualTo(0));
        Assert.That(tree.Percent,Is.EqualTo(1));
    }


    private DirectoryTree Tree
    {
        get;
        set;
    } = new ();
    
    [Test]
    public void StopScanTest()
    {
       
        Task.Run( ()=>
        {
            Tree = DirectoryScanner.StartScan("D:\\Android");
             
        } );
        Thread.Sleep(10);
        DirectoryScanner.StopScan();
        Thread.Sleep(10);
        Assert.That(Tree.Name,Is.EqualTo("Android"));
        Assert.That(Tree.Size,Is.LessThan(4666454481));
        Assert.That(Tree.Childs.Count,Is.EqualTo(1));
        Assert.That(Tree.Percent,Is.EqualTo(1));
    }
    
    [Test]
    public void LinkTest()
    {
       
        
        Tree = DirectoryScanner.StartScan("C:\\Users\\LEGION Y540\\Desktop\\TestLink");
            
        Assert.That(Tree.Name,Is.EqualTo("TestLink"));
        Assert.That(Tree.Size,Is.EqualTo(46));
        Assert.That(Tree.Childs.Count,Is.EqualTo(2));
        Assert.That(Tree.Percent,Is.EqualTo(1));
        
        Assert.That(Tree.Childs.ToArray()[0].Name,Is.EqualTo("File.txt"));
        Assert.That(Tree.Childs.ToArray()[0].Size,Is.EqualTo(46));
        Assert.That(Tree.Childs.ToArray()[0].Childs.Count,Is.EqualTo(0));
        Assert.That(Tree.Childs.ToArray()[0].Percent,Is.EqualTo(1));
        
        Assert.That(Tree.Childs.ToArray()[1].Name,Is.EqualTo("TestLink"));
        Assert.That(Tree.Childs.ToArray()[1].Size,Is.EqualTo(0));
        Assert.That(Tree.Childs.ToArray()[1].Childs.Count,Is.EqualTo(0));
        Assert.That(Tree.Childs.ToArray()[1].Percent,Is.EqualTo(0));
        
    }
    
}