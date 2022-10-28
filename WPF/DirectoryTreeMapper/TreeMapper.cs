using System.Collections.ObjectModel;
using Scanner_lib;


namespace WPF.DirectoryTreeMapper
{
    internal static class TreeMapper
    {
        public static DirectoryTreeView ToTreeViewNode(this DirectoryTree treeNode)
        {
            var viewNode = new DirectoryTreeView
            {
                Name = treeNode.Name,
                Size = treeNode.Size,
                Percent = treeNode.Percent,
                PhotoPath = DirectoryTreeView.GetStringPath(treeNode.Extension)
            };

            if (treeNode.Childs != null)
            {
                viewNode.Childs = new ObservableCollection<DirectoryTreeView>();

                foreach (var node in treeNode.Childs)
                {
                    var childNode = node.ToTreeViewNode();
                    viewNode.Childs.Add(childNode);
                }
            }
            return viewNode;
        }
    }
}