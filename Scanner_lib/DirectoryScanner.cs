using System.Collections.Concurrent;
using System.ComponentModel.Design.Serialization;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;


namespace Scanner_lib;

public static class DirectoryScanner
{
    
    private static ConcurrentQueue<Task> _waitableTasks = new ConcurrentQueue<Task>();
    
    private static int _maxTasksCount = 100;
    
    private static int curTasksCount = 0;
    
    private static  List<Task> _curScanDirectoryTasks = new List<Task>();
    
    private static  List<Task> _curCountPercentTasks = new List<Task>();

    private static CancellationTokenSource? token;

    private static bool isStartDoingTasksWorking = false;

    public static void StopScan()
    {
        token.Cancel();
    }
    
    private static void DoTaskInThread(Object st)
    {
        
            Task curTask = (Task)st;
            curTask.Start();
    }
    
    private static async void StartDoingTasks(List<Task> curTasks,Task firstTask)
    {
        isStartDoingTasksWorking = true;
        _waitableTasks.Enqueue(firstTask);
        
           curTasksCount = 0;
           while (curTasksCount < _maxTasksCount && _waitableTasks.Count != 0) 
           {

              
               
                _waitableTasks.TryDequeue(out var task);
                if (task != null)
                {
                    curTasksCount++;
                    curTasks.Add(task);

                   
                    //  Работа через Thread
                    ThreadPool.QueueUserWorkItem(DoTaskInThread,task); 
                    //  Работа через Task
                     // task.Start( );

                }
           }

           while(curTasks.Count != 0  || _waitableTasks.Count != 0)
           {

              
               
                try
                {
                    
                    Task doneTask = await Task.WhenAny(curTasks);
                    curTasks.Remove(doneTask);
                    await doneTask;
                    // curTasksCount--;
                }
                catch(Exception exc) {}
                
               
                    
                _waitableTasks.TryDequeue(out var task);
                if (task != null)
                {
                    curTasksCount++;
                    curTasks.Add(task);
                    
                    
                    //  Работа через Thread
                    ThreadPool.QueueUserWorkItem(DoTaskInThread,task); 
                    //  Работа через Task
                    //  task.Start( );
                    
                }
               
           }
           isStartDoingTasksWorking = false;
            
           
    }
    
    private static void AddSizeToParent(DirectoryTree tree,long size)
    {
        if (tree.Parent != null)
        {
            _waitableTasks.Enqueue(new Task(() =>
            {
                tree.Parent.Size += size;
                    AddSizeToParent(tree.Parent, size);
            }));
        }
           
    }
    
    private static void CountPercent(DirectoryTree tree, long parentSize)
    {

        if (parentSize != 0)
        {

            tree.Percent = (double)tree.Size / parentSize;

            foreach (var child in tree.Childs)
            {
                _waitableTasks.Enqueue(new Task(() => { CountPercent(child, tree.Size); }));
            }

        }
        else{
            if (tree.Parent!=null && tree.Parent.Childs.Count != 0)
            {
                tree.Percent = (double)1 / tree.Parent.Childs.Count;
            }
            else
            {
                tree.Percent = 1;
            }
            
            foreach (var child in tree.Childs)
            {
                _waitableTasks.Enqueue(new Task(() => { CountPercent(child, tree.Size); }));
            }
            
        }
    }
   
    public static  DirectoryTree  StartScan(string path)
    {
        
            token = new CancellationTokenSource();
        
            DirectoryTree newDirectoryTree = new DirectoryTree();
            
            StartDoingTasks(_curScanDirectoryTasks,
                new  Task( ()=> { newDirectoryTree = ScanDirectory(path,null); }));
            
            while (isStartDoingTasksWorking) { }
            
            StartDoingTasks(_curCountPercentTasks,
                new Task(()=> { CountPercent(newDirectoryTree,newDirectoryTree.Size); }));
            
            while (isStartDoingTasksWorking) { }
            
           
            
            
            return newDirectoryTree;
    }
  
    private static  DirectoryTree ScanDirectory(string path,DirectoryTree parent)
    {
        
        DirectoryTree curTree;
          
        if (Directory.Exists(path))
        {
             
            DirectoryInfo curDirectory = new DirectoryInfo(path);
              
            curTree = new DirectoryTree(path,curDirectory.Name,DirectoryTree.FileType.Folder,parent);
              
            string[] filePaths = Directory.GetFiles(path);
            foreach (var filePath in filePaths)
            {
                curTree.Childs.Enqueue(ScanDirectory(filePath,curTree));
            }
                
               
            string[] directoryPaths = Directory.GetDirectories(path);
            foreach (var directoryPath in directoryPaths)
            {
                _waitableTasks.Enqueue(new Task(() =>
                    {
                        if(!token.Token.IsCancellationRequested)
                            curTree.Childs.Enqueue(ScanDirectory(directoryPath, curTree));

                    }));
                    
            }
               
            return curTree;
        }
        else
        {
            
                FileInfo curFile = new FileInfo(path);

                
                    // if (curFile.Extension != ".lnk")
                if (curFile.LinkTarget == null)
                {
                    curTree = new DirectoryTree(path, curFile.Length, curFile.Name, DirectoryTree.FileType.File,parent);
                    AddSizeToParent(curTree,curTree.Size);
                }
                else
                {
                    
                    curTree = new DirectoryTree(path, curFile.Name, DirectoryTree.FileType.File,parent);
                    
                }

                return curTree;
        }
          
    }
   
}