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
        public static DatabaseAccessor GetInstance() {
            return instance ?? (instance = new DatabaseAccessor());
        }

        private DatabaseAccessor() {
            DatabaseAccessorListeners.GetInstance(this);
        }

        // 3 identical methods to aid readability
        public int Update(String query) { return DatabaseConnection.GetInstance().ExecuteQuery(query); }
        public int Insert(String query) { return DatabaseConnection.GetInstance().ExecuteQuery(query); }
        public int Delete(String query) { return DatabaseConnection.GetInstance().ExecuteQuery(query); }

        public int InsertBatch<T>(List<T> list, Func<T, string> queryBuilder) {
            return DatabaseConnection.GetInstance().InsertCollection(list, queryBuilder);
        }
        public int InsertBatch<T, U>(Dictionary<T, U> list, Func<KeyValuePair<T, U>, string> queryBuilder) {
            return DatabaseConnection.GetInstance().InsertCollection(list, queryBuilder);
        }

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
            foundations.Add(new InterviewFoundation(-1, "Test1", "Test/First"));
            foundations.Add(new InterviewFoundation(-1, "Test2", "Test/First"));
            foundations.Add(new InterviewFoundation(-1, "Test3", "Test/Second"));
            foundations.Add(new InterviewFoundation(-1, "Test4", "Test/Second"));
            foundations.Add(new InterviewFoundation(-1, "Test5", "Test/Second/Third"));
            return foundations;
        }

        public List<Applicant> PullApplicantData() {
            DbDataReader applicantReader = DatabaseConnection.GetInstance()
                .Select("SELECT ApplicantId, FirstName, LastName, ApplyingPosition, PictureCode, Address, Flag, " +
                        "EmailAddress, PhoneNumber, DateOfBirth, DateOfEntry FROM applicants");
            List<Applicant> apps = new List<Applicant>();
            while (applicantReader.Read()) {
                Applicant app = new Applicant {
                    Id = (int)applicantReader["ApplicantId"],
                    First_Name = (string)applicantReader["FirstName"],
                    Last_Name = (string)applicantReader["LastName"],
                    Applying_Position = (string)applicantReader["ApplyingPosition"],
                    Picture = (string)applicantReader["PictureCode"],
                    Address = (string)applicantReader["Address"],
                    Flag = (int) applicantReader["Flag"],
                    Phone_Number = (string)applicantReader["PhoneNumber"],
                    Email = (string)applicantReader["EmailAddress"],
                    Dob = applicantReader.GetInt64(applicantReader.GetOrdinal("DateOfBirth")),
                    Doe = applicantReader.GetInt64(applicantReader.GetOrdinal("DateOfEntry"))
                };
                apps.Add(app);
            }
            applicantReader.Close();
            return apps;
        }


        public List<InterviewFoundation> PullInterviewFoundation()
        {
            DbDataReader interviewFoundationReader = DatabaseConnection.GetInstance()
                .Select("SELECT Foundation_ID, Name, Category FROM interview_foundation");
            List<InterviewFoundation> ifound = new List<InterviewFoundation>();
            while (interviewFoundationReader.Read())
            {
             InterviewFoundation interviewFoundation =  new InterviewFoundation((Int32) interviewFoundationReader["Foundation_ID"]
                 ,interviewFoundationReader["Category"].ToString()
                 , interviewFoundationReader["Name"].ToString());
                ifound.Add(interviewFoundation);
            }
            interviewFoundationReader.Close();
            foreach (InterviewFoundation interviewFoundation in ifound)
            {
                interviewFoundation.GetQuestions();
            }
            return ifound;

        }

        public List<Question> PullQuestionData()
        {
            DbDataReader questionReader = DatabaseConnection.GetInstance()
                .Select("SELECT Question_ID, Question, Category FROM questions");
            List<Question> ques = new List<Question>();
            if (!questionReader.HasRows) return ques;
            while (questionReader.Read()) {
                Question question = new Question((Int32) questionReader["Question_ID"]);
                question.SetText((String) questionReader["Question"]);
                question.SetCategory((String) questionReader["Category"]);
                ques.Add(question);
            }
            questionReader.Close();
            foreach (Question question in ques)
                question.AddAnswers(PullAnswersFromQuestionId(question.Id).ToArray());
            return ques;
        }

        //pull the answers from question id, 
        public List<Answer> PullAnswersFromQuestionId(int questionId) {
            bool openHere = false;
            if (!DatabaseConnection.GetInstance().isLongOpen()) {
                openHere = true;
            }
            DbDataReader answerReader = DatabaseConnection.GetInstance()
                .Select($"SELECT Answer_ID, Question_ID, Value, Weight FROM Question_Answers WHERE Question_ID={questionId}");
            List<Answer> answ = new List<Answer>();
            while (answerReader.Read())
            {
                Answer answer = new Answer((Int32)answerReader["Answer_ID"]);
                answer.SetText((string)answerReader["Value"]);
                answer.SetWeight((Int32)answerReader["Weight"]);
                answ.Add(answer);
            }
//            if (openHere)
//                DatabaseConnection.GetInstance().CloseLongConnection();
            answerReader.Close();
            return answ;
        }

        public List<AvailablePosition> GetAvailablePositions() {
            List<AvailablePosition> availablePositions = new List<AvailablePosition>();
            DbDataReader posReader = DatabaseConnection.GetInstance()
                .Select("SELECT ID, Position, Seats FROM positions WHERE Seats > 0");
            while (posReader.Read())
                availablePositions.Add(new AvailablePosition {
                    Id = (int) posReader["ID"],
                    Position = posReader["Position"].ToString(),
                    Seats = (int) posReader["Seats"]
                });
            posReader.Close();
            return availablePositions;
        }

        public List<AvailablePosition> GetAllPositions() {
            List<AvailablePosition> positions = new List<AvailablePosition>();
            DbDataReader posReader = DatabaseConnection.GetInstance()
                .Select("SELECT ID, Position, Seats FROM positions");
            while (posReader.Read())
                positions.Add(new AvailablePosition {
                    Id = (int) posReader["ID"],
                    Position = posReader["Position"].ToString(),
                    Seats = (int) posReader["Seats"]
                });
            posReader.Close();
            return positions;
        }
    }
}