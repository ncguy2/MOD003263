using System.Collections.Generic;
using Mod003263.interview;

/**
 * Author: Callum Highley, Nick Guy
 * Date: 14/11/2016
 * Contains: DatabaseAccessor
 */
namespace Mod003263.db {

    public class DatabaseAccessor {

        private static DatabaseAccessor instance;
        public static DatabaseAccessor GetInstance() {
            return instance ?? (instance = new DatabaseAccessor());
        }

        private DatabaseAccessor() {}

        // TODO fix connection insert calls
        /// <summary>
        /// Insert statement
        /// </summary>
        /// <param name="applicant"></param>
        public int Insert(Applicant applicant) {
            string q = "INSERT INTO Current_JA (First_Name, Last_Name, Dob, EmailAddress, Applying_Position, Picture, Phone_Number) "
                       + $"VALUES('{applicant.First_Name}','{applicant.Last_Name}','{applicant.Dob}','{applicant.Email}',"
                       + $"'{applicant.Applying_Position}','{applicant.Picture}','{applicant.Phone_Number}')";
            return DatabaseConnection.GetInstance().Insert(q);
        }
        public int Insert(Question question) {
            string q = $"INSERT INTO Questions (Question, Category) VALUES('{question.Text()}', '{question.Cat()}')";
            return DatabaseConnection.GetInstance().Insert(q);
        }
        public int Insert(int questionId, Answer answer) {
            string q = $"INSERT INTO Question_Answers (Question_ID, Value, Weight) " +
                       $"VALUES( '{questionId}','{answer.Text}', '{answer.Weight}')";
            return DatabaseConnection.GetInstance().Insert(q);
        }

        public List<InterviewFoundation> SelectAllInterviewFoundations() {
            List<InterviewFoundation> foundations = new List<InterviewFoundation>();
            // TODO replace with actual data
            foundations.Add(new InterviewFoundation("Test1", "Test/First"));
            foundations.Add(new InterviewFoundation("Test2", "Test/First"));
            foundations.Add(new InterviewFoundation("Test3", "Test/Second"));
            foundations.Add(new InterviewFoundation("Test4", "Test/Second"));
            foundations.Add(new InterviewFoundation("Test5", "Test/Second/Third"));
            return foundations;
        }

        // At very least, follow the established coding standards
        public void PullApplicantData() {

        }

        public void PullQuestionData() {

        }

        public void PullAnswersFromQuestionID(int questionId) {

        } 
    }
}