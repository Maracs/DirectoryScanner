using System.Collections.Generic;
using System.Collections.ObjectModel;
using Scanner_lib;

namespace WPF.DirectoryTreeMapper
{
    public class DirectoryTreeView
    {
        public string Name { get; set; }
        public long Size { get; set; }
        public double Percent { get; set; }
        public string PhotoPath { get; set; }

        public ObservableCollection<DirectoryTreeView> Childs { get; set; }

        public static string GetStringPath(DirectoryTree.FileType type)
        {
            return _imgPath[type];
        }

        private static Dictionary<DirectoryTree.FileType, string> _imgPath = new()
        {
            {DirectoryTree.FileType.File, "D:\\Университет\\Третий курс\\Первый семестр\\СПП\\Лабараторная работа 3\\directory_scanner\\Scanner_lib\\WPF\\resources\\file.jpg" },
            {DirectoryTree.FileType.Folder, "D:\\Университет\\Третий курс\\Первый семестр\\СПП\\Лабараторная работа 3\\directory_scanner\\Scanner_lib\\WPF\\resources\\folder.jpg" }
        };
        
    }
}