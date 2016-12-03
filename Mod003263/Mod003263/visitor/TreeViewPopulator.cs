using System.Windows.Controls;
using Mod003263.interview;
using Utils.Tree;
using Utils.Tree.Builder;

namespace Mod003263.visitor {

    public class TreeViewPopulator<T> : IVisitor<T> {

        private TreeView rootTree;
        private TreeViewItem currentItem;
        private TreeViewItem nextItem;

        public TreeViewPopulator(TreeView rootTree, TreeViewItem currentItem) {
            this.rootTree = rootTree;
            this.currentItem = currentItem;
        }

        public IVisitor<T> Visit(IVisitable<T> visitable) {
//            this.currentItem.Items.Add(nextItem);
            return new TreeViewPopulator<T>(this.rootTree, nextItem);
        }

        public void VisitData(IVisitable<T> visitable, T data) {
            TreeViewItem dataItem = new TreeViewItem { Header = data };
            this.nextItem = dataItem;
            this.currentItem.Items.Add(dataItem);
        }

    }
}