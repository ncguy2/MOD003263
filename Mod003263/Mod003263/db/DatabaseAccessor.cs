using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Navigation;
using Mod003263.interview;
using Mod003263.threading;
using Newtonsoft.Json;

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

        private Thread dbThread;
        private bool dbThreadAlive;
        private List<ThreadStart> dbThreadTasks;

        private DatabaseAccessor() {
            dbThreadTasks = new List<ThreadStart>();
            dbThreadAlive = true;
            dbThread = ThreadFactory.GetInstance().CreateManagedDaemonThread(ThreadLoop);
            dbThread.Start();
            DatabaseAccessorListeners.GetInstance(this);
        }

        private void ThreadLoop() {
            while (dbThreadAlive) {
                if (dbThreadTasks.Count <= 0) {
                    Thread.Sleep(100);
                    continue;
                }
                ThreadStart dbThreadTask = dbThreadTasks[0];
                dbThreadTask();
                dbThreadTasks.RemoveAt(0);
            }
        }

        // 3 identical methods to aid readability
        public int Update(String query) { return DatabaseConnection.GetInstance().ExecuteQuery(query); }
        public int Insert(String query) { return DatabaseConnection.GetInstance().ExecuteQuery(query); }
        public int Delete(String query) { return DatabaseConnection.GetInstance().ExecuteQuery(query); }

        public int LatestId(string query, string id = "ID") { return DatabaseConnection.GetInstance().GetLatestId(query, id); }

        public int NonQueryParameters(string q, params KeyValuePair<string, object>[] args) {
            return DatabaseConnection.GetInstance().ExecuteParameterQuery(q, args);
        }

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

        public Interview GetInterviewForApplicant(Applicant app) {
            int appId = app.Id;
            String query = $"SELECT * FROM interview WHERE Applicant_Id={appId} ORDER BY Interview_ID ASC LIMIT 1";
            DbDataReader reader = DatabaseConnection.GetInstance().Select(query);
            if (!reader.Read()) return null;
            Interview interview = null;
            InterviewFoundation f =
                PullInterviewFoundationFromId(reader.GetInt32(reader.GetOrdinal("Foundation_ID")));
            interview = new Interview(reader.GetInt32(reader.GetOrdinal("Interview_ID")), f);
            interview.Subject = app;
            interview.Flag = reader.GetInt32(reader.GetOrdinal("Flag"));
            interview.SetResultMetric(reader.GetInt16(reader.GetOrdinal("Result")));
            String answersJson = reader.GetString(reader.GetOrdinal("Answers"));
            Dictionary<int, int> answersMap = JsonConvert.DeserializeObject<Dictionary<int, int>>(answersJson);
            Dictionary<Question, Answer> answers = interview.GetFoundationInstance().GetAnswerMap();

            foreach (KeyValuePair<int, int> pair in answersMap) {
                Question q = PullSingleQuestionData(pair.Key);
                Answer a = q?.GetAnswers().Find(ans => ans.Id == pair.Value);
                if (a == null) continue;
                answers.Add(q, a);
            }
            return interview;
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

        public Applicant PullSingleApplicantData(int appId) {
            DbDataReader applicantReader = DatabaseConnection.GetInstance()
                .Select("SELECT ApplicantId, FirstName, LastName, ApplyingPosition, PictureCode, Address, Flag, " +
                        "EmailAddress, PhoneNumber, DateOfBirth, DateOfEntry FROM applicants");
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
                applicantReader.Close();
                return app;
            }
            return null;
        }


        public List<InterviewFoundation> PullInterviewFoundation() {
            DbDataReader reader = DatabaseConnection.GetInstance()
                .Select("SELECT Foundation_ID, Name, Category, Position FROM interview_foundation");
            List<InterviewFoundation> ifound = new List<InterviewFoundation>();
            while (reader.Read()) {
                InterviewFoundation interviewFoundation = new InterviewFoundation(
                    reader.GetInt32(reader.GetOrdinal("Foundation_ID")),
                    reader.GetString(reader.GetOrdinal("Name")),
                    reader.GetString(reader.GetOrdinal("Category"))
                );
                interviewFoundation.Position = reader.GetString(reader.GetOrdinal("Position"));
                ifound.Add(interviewFoundation);
            }
            reader.Close();
            foreach (InterviewFoundation f in ifound) {
                int id = f.Id();
                foreach (KeyValuePair<Question, int> pair in PullQuestionDataFromFoundation(id))
                    f.GetQuestions().Add(pair.Key, pair.Value);
            }

            return ifound;
        }

        public InterviewFoundation PullInterviewFoundationFromId(int i) {
            DbDataReader interviewFoundationReader = DatabaseConnection.GetInstance()
                .Select($"SELECT Foundation_ID, Name, Category FROM interview_foundation WHERE Foundation_ID={i}");
            InterviewFoundation foundation;
            if(interviewFoundationReader.Read()) {
                foundation = new InterviewFoundation(
                    (Int32) interviewFoundationReader["Foundation_ID"],
                    interviewFoundationReader["Category"].ToString(),
                    interviewFoundationReader["Name"].ToString());
            }else return null;

            interviewFoundationReader.Close();
            int id = foundation.Id();
            foreach (KeyValuePair<Question, int> pair in PullQuestionDataFromFoundation(id))
                foundation.GetQuestions().Add(pair.Key, pair.Value);

            return foundation;
        }

        public List<KeyValuePair<Question, int>> PullQuestionDataFromFoundation(int id) {
            DbDataReader questionReader = DatabaseConnection.GetInstance()
                .Select("SELECT q.Question_ID, q.Question, q.Category, i.Weight FROM interview_questions i, questions q " +
                       $"WHERE i.Question_ID=q.Question_ID AND i.Foundation_ID={id}");
            List<KeyValuePair<Question, int>> ques = new List<KeyValuePair<Question, int>>();
            if (!questionReader.HasRows) return ques;
            while (questionReader.Read()) {
                Question question = new Question((Int32) questionReader["Question_ID"]);
                question.SetText((String) questionReader["Question"]);
                question.SetCategory((String) questionReader["Category"]);
                ques.Add(new KeyValuePair<Question, int>(question, questionReader.GetInt16(questionReader.GetOrdinal("Weight"))));
            }
            questionReader.Close();
            ques.ForEach(pair => pair.Key.AddAnswers(PullAnswersFromQuestionId(pair.Key.Id).ToArray()));
            return ques;
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

        public Question PullSingleQuestionData(int id) {
            DbDataReader questionReader = DatabaseConnection.GetInstance()
                .Select($"SELECT Question_ID, Question, Category FROM questions WHERE Question_ID={id}");
            Question q;
            if (questionReader.Read()) {
                q = new Question((Int32) questionReader["Question_ID"]);
                q.SetText((String) questionReader["Question"]);
                q.SetCategory((String) questionReader["Category"]);
            }else return null;
            questionReader.Close();
                q.AddAnswers(PullAnswersFromQuestionId(q.Id).ToArray());
            return q;
        }

        //pull the answers from question id, 
        public List<Answer> PullAnswersFromQuestionId(int questionId) {
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

        // Concurrent

        // TODO Possibly move these 2 methods to a more suitable class
        private void InvokeOnMainThread<T>(Action<T> action, Func<T> argSupplier) {
            InvokeOnMainThread(action, argSupplier());
        }

        private void InvokeOnMainThread<T>(Action<T> action, T arg) {
            Application.Current.Dispatcher.Invoke(action, arg);
        }

        public void UsingInterviewFoundations(Action<List<InterviewFoundation>> action) {
            dbThreadTasks.Add(() => InvokeOnMainThread(action, PullInterviewFoundation));
        }

        public void UsingApplicantData(Action<List<Applicant>> action) {
            dbThreadTasks.Add(() => InvokeOnMainThread(action, PullApplicantData));
        }

        public void UsingQuestionData(Action<List<Question>> action) {
            dbThreadTasks.Add(() => InvokeOnMainThread(action, PullQuestionData));
        }

        public void UsingAnswerData(int questionId, Action<List<Answer>> action) {
            dbThreadTasks.Add(() => InvokeOnMainThread(action, PullAnswersFromQuestionId(questionId)));
        }

        public void UsingAvailablePositions(Action<List<AvailablePosition>> action) {
            dbThreadTasks.Add(() => InvokeOnMainThread(action, GetAvailablePositions));
        }

        public void UsingAllPositions(Action<List<AvailablePosition>> action) {
            dbThreadTasks.Add(() => InvokeOnMainThread(action, GetAllPositions));
        }

        public void UsingInterviewForApplicant(Applicant app, Action<Interview> action) {
            dbThreadTasks.Add(() => InvokeOnMainThread(action, GetInterviewForApplicant(app)));
        }

    }
}