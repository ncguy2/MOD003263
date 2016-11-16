using Mod003263.interview;

/**
 * Author: Callum Highley
 * Date: 14/11/2016
 * Contains: DatabaseAccessor
 */
namespace Mod003263.DBstuff
{
    class DatabaseAccessor
    {

        // TODO fix connection insert calls

        /// <summary>
        /// Insert statement
        /// </summary>
        /// <param name="applicant"></param>
        public void Insert(Applicant applicant) {
            string q = "INSERT INTO Current_JA (First_Name, Last_Name, Dob, EmailAddress, Applying_Position, Picture, Phone_Number) "
                       + $"VALUES('{applicant.First_Name}','{applicant.Last_Name}','{applicant.Dob}','{applicant.Email}',"
                       + $"'{applicant.Applying_Position}','{applicant.Picture}','{applicant.Phone_Number}')";
            //DatabaseConnection.Insert(q);
        }
        public void Insert(Question question) {
            string q = $"INSERT INTO Questions (Question, Category) VALUES('{question.Text()}', '{question.Cat()}')";
            //DatabaseConnection.Insert(q);
        }
        public void Insert(int questionId, Answer answer) {
            string q = $"INSERT INTO Question_Answers (Question_ID, Value, Weight) " +
                       $"VALUES( '{questionId}','{answer.Text}', '{answer.Weight}')";
            //DatabaseConnection.Insert(q);
        }

        public void PullAllData(string table) {

        }
    }
}