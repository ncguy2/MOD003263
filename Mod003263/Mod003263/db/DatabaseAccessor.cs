using System;
using System.Collections.Generic;
using System.Data.Common;
using Mod003263.interview;

/**
 * Author: Callum Highley, Nick Guy
 * Date: 14/11/2016
 * Contains: DatabaseAccessor
 */
namespace Mod003263.db
{

    public class DatabaseAccessor
    {

        private static DatabaseAccessor instance;
        public static DatabaseAccessor GetInstance()
        {
            return instance ?? (instance = new DatabaseAccessor());
        }

        private DatabaseAccessor() { }

        // TODO fix connection insert calls
        /// <summary>
        /// Insert statement
        /// </summary>
        /// <param name="applicant"></param>
        public int Insert(Applicant applicant)
        {
            string q = "INSERT INTO Current_JA (First_Name, Last_Name, Dob, EmailAddress, Applying_Position, Picture, Phone_Number) "
                       + $"VALUES('{applicant.First_Name}','{applicant.Last_Name}','{applicant.Dob}','{applicant.Email}',"
                       + $"'{applicant.Applying_Position}','{applicant.Picture}','{applicant.Phone_Number}')";
            return DatabaseConnection.GetInstance().ExecuteQuery(q);
        }
        public int Insert(Question question)
        {
            string q = $"INSERT INTO Questions (Question, Category) VALUES('{question.Text()}', '{question.Cat()}')";
            return DatabaseConnection.GetInstance().ExecuteQuery(q);
        }
        public int Insert(int questionId, Answer answer)
        {
            string q = $"INSERT INTO Question_Answers (Question_ID, Value, Weight) " +
                       $"VALUES( '{questionId}','{answer.Text}', '{answer.Weight}')";
            return DatabaseConnection.GetInstance().ExecuteQuery(q);
        }

        public List<InterviewFoundation> SelectAllInterviewFoundations()
        {
            List<InterviewFoundation> foundations = new List<InterviewFoundation>();
            // TODO replace with actual data
            foundations.Add(new InterviewFoundation("Test1", "Test/First"));
            foundations.Add(new InterviewFoundation("Test2", "Test/First"));
            foundations.Add(new InterviewFoundation("Test3", "Test/Second"));
            foundations.Add(new InterviewFoundation("Test4", "Test/Second"));
            foundations.Add(new InterviewFoundation("Test5", "Test/Second/Third"));
            return foundations;
        }

        public List<Applicant> PullApplicantData()
        {
            DbDataReader applicantReader = DatabaseConnection.GetInstance()
                .Select("SELECT applicantId, First_Name, Last_Name, Applying_Position, Picture, Flag, Email_Address, " +
                        "Address, Phone_Number, Date_of_Birth, Date_of_Entry FROM Current_JA");
            List<Applicant> apps = new List<Applicant>();
            while (applicantReader.NextResult())
            {
                apps.Add(new Applicant
                {
                    Id = (Int32)applicantReader["applicantId"],
                    First_Name = (String)applicantReader["First_Name"],
                    Last_Name = (String)applicantReader["Last_Name"],
                    Applying_Position = (String)applicantReader["Applying_Position"],
                    Picture = (String)applicantReader["Picture"],
                    Address = (String)applicantReader["Address"],
                    Phone_Number = (String)applicantReader["Phone_Number"],
                    Email = (String)applicantReader["Email_Address"],
                    Dob = (Int64)applicantReader["Date_of_Birth"],
                    Doe = (Int64)applicantReader["Date_of_Entry"]
                });
            }
            return apps;
        }

        public List<Question> PullQuestionData()
        {
            DbDataReader questionReader = DatabaseConnection.GetInstance()
                .Select("SELECT Question_ID, Question, Category FROM Questions");
            List<Question> ques = new List<Question>();
            while (questionReader.NextResult())
            {
                Question question = new Question((Int32)questionReader["Question_ID"]);
                question.SetText((String)questionReader["Question"]);
                question.SetCategory((String)questionReader["Category"]);
                question.AddAnswers(PullAnswersFromQuestionId(question.Id).ToArray());
                ques.Add(question);
            }
            return ques;
        }

        //pull the answers from question id, 
        public List<Answer> PullAnswersFromQuestionId(int questionId)
        {
            DbDataReader answerReader = DatabaseConnection.GetInstance()
                .Select($"SELECT Answer_ID, Question_ID, Value, Weight FROM Question_Answers WHERE Question_ID={questionId}");
            List<Answer> answ = new List<Answer>();
            while (answerReader.NextResult())
            {
                Answer answer = new Answer((Int32)answerReader["Answer_ID"]);
                answer.SetText((string)answerReader["Value"]);
                answer.SetWeight((Int32)answerReader["Weight"]);
                answ.Add(answer);
            }
            return answ;


        }
    }
}