using System.Collections.Generic;
using Mod003263.db;
using Mod003263.interview;
using Utils.Tree;
using Utils.Tree.Builder;

namespace Mod003263.controllerview.controller {

    public class TemplateSelectionController : BaseController
    {

        private Interview inter;
        private VisitableTree<TreeObjectWrapper<InterviewFoundation>> templateTree;

        public TemplateSelectionController() {
            templateTree = new VisitableTree<TreeObjectWrapper<InterviewFoundation>>(new TreeObjectWrapper<InterviewFoundation>(""));
        }

        public TemplateSelectionController PopulateTree() {
            List<InterviewFoundation> foundations = DatabaseAccessor.GetInstance().SelectAllInterviewFoundations();
            TreePopulator.Populate(templateTree, foundations, '/', f => f.Path());
            return this;
        }

        public VisitableTree<TreeObjectWrapper<InterviewFoundation>> GetTemplates() {
            return templateTree;
        }

    }

}