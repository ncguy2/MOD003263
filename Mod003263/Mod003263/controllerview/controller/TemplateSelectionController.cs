using System.Collections.Generic;
using Mod003263.db;
using Mod003263.interview;
using Utils.Tree;
using Utils.Tree.Builder;

/**
 * Author: Nick Guy
 * Date: 28/11/2016
 * Contains: TemplateSelectionController
 */
namespace Mod003263.controllerview.controller {

    /// <summary>
    /// The logic for the template selection UI form
    /// </summary>
    public class TemplateSelectionController : BaseController
    {

        private Interview inter;
        private VisitableTree<TreeObjectWrapper<InterviewFoundation>> templateTree;

        public TemplateSelectionController() {
            templateTree = new VisitableTree<TreeObjectWrapper<InterviewFoundation>>(new TreeObjectWrapper<InterviewFoundation>(""));
        }

        /// <summary>
        /// Populates the tree from the database
        /// </summary>
        /// <returns>Self instance for the purpose of method chaining</returns>
        public TemplateSelectionController PopulateTree() {
            List<InterviewFoundation> foundations = DatabaseAccessor.GetInstance().SelectAllInterviewFoundations();
            TreePopulator.Populate(templateTree, foundations, '/', f => f.Path());
            return this;
        }

        /// <summary>
        /// Gets the template tree
        /// </summary>
        /// <returns>The template tree</returns>
        public VisitableTree<TreeObjectWrapper<InterviewFoundation>> GetTemplates() {
            return templateTree;
        }

    }

}