using System.Collections.Generic;
using System.Windows;
using Mod003263.interview;
using Utils.Tree;
using Utils.Tree.Builder;

/**
 * Author: Nick Guy
 * Date: 07/11/2016
 * Contains: QuestionTreeDataAdapter
 */
namespace Mod003263.controllerview.adapter {

    /// <summary>
    /// A concrete implementation for a question tree UI adapter
    /// </summary>
    public class QuestionTreeDataAdapter : AbstractDataAdapter<VisitableTree<TreeObjectWrapper<Question>>, UIElement> {

        public VisitableTree<TreeObjectWrapper<Question>> CreateTree(List<Question> questionList) {
            VisitableTree<TreeObjectWrapper<Question>> tree = new VisitableTree<TreeObjectWrapper<Question>>(new TreeObjectWrapper<Question>(""));
            TreePopulator.Populate(tree, questionList, '|', question => question.Path());
            return tree;
        }

        public override VisitableTree<TreeObjectWrapper<Question>> GetFromUI(UIElement element) {
            throw new System.NotImplementedException();
        }

        public override UIElement PushToUI(VisitableTree<TreeObjectWrapper<Question>> data) {
            throw new System.NotImplementedException();
        }
    }
}