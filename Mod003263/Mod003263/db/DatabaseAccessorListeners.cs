using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using Mod003263.events;
using Mod003263.events.io;
using Mod003263.events.ui;
using Mod003263.interview;
using Mod003263.utils;
using Mod003263.wpf;
using Newtonsoft.Json;

/**
 *  Author: Nick Guy
 *  Date: 01/12/2016
 *  Contains: DatabaseAccessorListeners
 */
namespace Mod003263.db {

    public class DatabaseAccessorListeners : SaveApplicantEvent.SaveApplicantListener,
        SaveFoundationEvent.SaveFoundationListener, SaveQuestionEvent.SaveQuestionListener,
        SaveInterviewEvent.SaveInterviewListener, SavePositionEvent.SavePositionListener,
        DeleteFoundationEvent.DeleteFoundationListener, DeleteQuestionEvent.DeleteQuestionListener,
        DeletePositionEvent.DeletePositionListener {

        private static DatabaseAccessorListeners instance;
        public static DatabaseAccessorListeners GetInstance() {
            return instance;
        }

        public static DatabaseAccessorListeners GetInstance(DatabaseAccessor accessor) {
            return instance ?? (instance = new DatabaseAccessorListeners(accessor));
        }

        private DatabaseAccessor accessor;
        private FixedSizedQueue<String> sqlBuffer;
        private DatabaseAccessorListeners(DatabaseAccessor accessor) {
            this.accessor = accessor;
            this.sqlBuffer = new FixedSizedQueue<string>(8);
            EventBus.GetInstance().Register(this);
        }

        #region Event listeners

        [Event]
        public void OnSaveApplicant(SaveApplicantEvent e) {
            try {
                if (e.Applicant.Id > 0) UpdateApplicant(e.Applicant);
                else InsertApplicant(e.Applicant);
            }catch (Exception exc) {
                WPFMessageBoxFactory.CreateErrorAndShow(exc);
            }
        }

        [Event]
        public void OnSaveFoundation(SaveFoundationEvent e) {
            try {
                if (e.Foundation.Id() > 0) UpdateFoundation(e.Foundation);
                else InsertFoundation(e.Foundation);
            }catch (Exception exc) {
                WPFMessageBoxFactory.CreateErrorAndShow(exc);
            }
        }

        [Event]
        public void OnSaveQuestion(SaveQuestionEvent e) {
            try {
                if (e.Payload.Id > 0) UpdateQuestion(e.Payload);
                else InsertQuestion(e.Payload);
            }catch (Exception exc) {
                WPFMessageBoxFactory.CreateErrorAndShow(exc);
            }
        }

        [Event]
        public void OnSaveInterview(SaveInterviewEvent e){
            try {
                if (e.Interview.ID > 0) UpdateInterview(e.Interview);
                else InsertInterview(e.Interview);
            }catch (Exception exc) {
                WPFMessageBoxFactory.CreateErrorAndShow(exc);
            }
        }

        [Event]
        public void OnSavePosition(SavePositionEvent e) {
            try {
                if (e.Position.Id > 0) UpdatePosition(e.Position);
                else InsertPosition(e.Position);
            }catch (Exception exc) {
                WPFMessageBoxFactory.CreateErrorAndShow(exc);
            }
        }

        [Event]
        public void OnDeleteFoundation(DeleteFoundationEvent e) {
            try {
                DeleteFoundation(e.Foundation);
            }catch (Exception exc){
                WPFMessageBoxFactory.CreateErrorAndShow(exc);
            }
        }

        [Event]
        public void OnDeleteQuestion(DeleteQuestionEvent e) {
            try {
                DeleteQuestion(e.Question);
            }catch (Exception exc){
                WPFMessageBoxFactory.CreateErrorAndShow(exc);
            }
        }

        [Event]
        public void OnDeletePosition(DeletePositionEvent e) {
            try {
                DeletePosition(e.Position);
            }catch (Exception exc){
                WPFMessageBoxFactory.CreateErrorAndShow(exc);
            }
        }

        #endregion

        #region Updates and Inserts

        // Applicant


        private void UpdateApplicant(Applicant app) {
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE applicants SET ");
            sb.Append($"FirstName='{app.First_Name}', ");
            sb.Append($"LastName='{app.Last_Name}', ");
            sb.Append($"ApplyingPosition='{app.Applying_Position}', ");
            sb.Append($"PictureCode='{app.Picture}', ");
            sb.Append($"Address='{app.Address}', ");
            sb.Append($"Flag='{app.Flag}', ");
            sb.Append($"EmailAddress='{app.Email}', ");
            sb.Append($"PhoneNumber='{app.Phone_Number}' ");
            sb.Append($"WHERE ApplicantId='{app.Id}';");
            string s = sb.ToString();
            accessor.Update(s);
        }

        private void InsertApplicant(Applicant app) {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO applicants ");
            sb.Append(
                "(FirstName, LastName, ApplyingPosition, PictureCode, Address, Flag, EmailAddress, PhoneNumber, " +
                "DateOfBirth, DateOfEntry) ");
            sb.Append("VALUES (");
            sb.Append($"'{app.First_Name}', '{app.Last_Name}', '{app.Applying_Position}', '{app.Picture}', " +
                      $"'{app.Address}', '{app.Flag}', '{app.Email}', '{app.Phone_Number}', '{app.Dob}', UNIX_TIMESTAMP(NOW()))");
            string s = sb.ToString();
            accessor.Insert(s);
        }

        // Foundation


        private void UpdateFoundation(InterviewFoundation f) {
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE interview_foundation SET ");
            sb.Append($"Name='{f.Name()}', ");
            sb.Append($"Category='{f.Cat()}', ");
            sb.Append($"Position='{f.Position}' ");
            sb.Append($"WHERE Foundation_ID='{f.Id()}'");
            String s = sb.ToString();
            accessor.Update(s);
            String del = $"DELETE FROM interview_questions WHERE Foundation_ID='{f.Id()}'";
            accessor.Delete(del);
            InsertFoundationQuestions(f);
        }

        private void InsertFoundation(InterviewFoundation f) {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO interview_foundation ");
            sb.Append("(Name, Category, Position) VALUES ");
            sb.Append($"('{f.Name()}', '{f.Cat()}', '{f.Position}')");
            String s = sb.ToString();
            accessor.Insert(s);
            int newId = accessor.LatestId("interview_foundation", "Foundation_ID");
            f.Id(newId);
            InsertFoundationQuestions(f);
        }

        private void InsertFoundationQuestions(InterviewFoundation f) {
            accessor.InsertBatch(f.GetQuestions(), pair => {
                StringBuilder sb = new StringBuilder();
                sb.Append("INSERT INTO interview_questions (Foundation_ID, Question_ID, Weight) ");
                sb.Append($"VALUES ('{f.Id()}', '{pair.Key.Id}', '{pair.Value}')");
                return sb.ToString();
            });
//            foreach (KeyValuePair<Question, int> pair in f.GetQuestions()) {
//                StringBuilder sb = new StringBuilder();
//                sb.Append("INSERT INTO interview_questions (Foundation_ID, Question_ID, Weight) ");
//                sb.Append($"VALUES ('{f.Id()}', '{pair.Key.Id}', '{pair.Value}')");
//                accessor.Insert(sb.ToString());
//            }
        }

        private void DeleteFoundation(InterviewFoundation f) {
            String d1 = $"DELETE FROM interview_foundation WHERE Foundation_ID='{f.Id()}'";
            String d2 = $"DELETE FROM interview_questions WHERE Foundation_ID='{f.Id()}'";
            accessor.Delete(d2);
            accessor.Delete(d1);
        }

        // Question


        private void UpdateQuestion(Question q) {
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE questions SET ");
            sb.Append($"Question='{q.Text()}', ");
            sb.Append($"Category='{q.Cat()}' ");
            sb.Append($"WHERE Question_ID='{q.Id}'");
            String s = sb.ToString();
            accessor.Update(s);
            String del = $"DELETE FROM question_answers WHERE Question_ID='{q.Id}'";
            accessor.Delete(del);
            InsertQuestionAnswers(q);
        }

        private void InsertQuestion(Question q) {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO questions ");
            sb.Append("(Question, Category) VALUES ");
            sb.Append($"('{q.Text()}', '{q.Cat()}')");
            String s = sb.ToString();
            accessor.Insert(s);
            int newId = accessor.LatestId("questions", "Question_ID");
            q.Id = newId;
            InsertQuestionAnswers(q);
        }

        private void InsertQuestionAnswers(Question q) {
            accessor.InsertBatch(q.GetAnswers(), a => {
                StringBuilder sb = new StringBuilder();
                sb.Append("INSERT INTO question_answers (Question_ID, Value, Weight) ");
                sb.Append($"VALUES ('{q.Id}', '{a.Text}', '{a.Weight}')");
                string sql = sb.ToString();
                return sql;
            });
        }

        private void DeleteQuestion(Question q) {
            String d1 = $"DELETE FROM questions WHERE Question_ID='{q.Id}'";
            String d2 = $"DELETE FROM question_answers WHERE Question_ID='{q.Id}'";
            accessor.Delete(d2);
            accessor.Delete(d1);
        }

        // Interview


        private void UpdateInterview(Interview i) {

            Dictionary<Question, Answer> answerMap = i.GetFoundationInstance().GetAnswerMap();
            Dictionary<int, int> idMap = answerMap.ToDictionary(pair => pair.Key.Id, pair => pair.Value.Id);
            string answerJson = JsonConvert.SerializeObject(idMap);

            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE interview SET ");
            sb.Append($"Foundation_ID='{i.GetFoundationInstance().GetFoundation().Id()}', ");
            sb.Append($"Applicant_Id='{i.Subject.Id}', ");
            sb.Append($"Flag='{i.Flag}', ");
            sb.Append($"Result='{i.GetResultMetric()}', ");
            sb.Append($"Answers='{answerJson}' ");
            sb.Append($"WHERE Question_ID='{i.ID}'");
            accessor.Update(sb.ToString());
        }

        private void InsertInterview(Interview i) {
            Dictionary<Question, Answer> answerMap = i.GetFoundationInstance().GetAnswerMap();
            Dictionary<int, int> idMap = answerMap.ToDictionary(pair => pair.Key.Id, pair => pair.Value.Id);
            string answerJson = JsonConvert.SerializeObject(idMap);

            int fId = i.GetFoundationInstance().GetFoundation().Id();

            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO interview (Foundation_ID, Applicant_Id, Flag, Result, Answers) VALUES ");
            sb.Append($"('{fId}', '{i.Subject.Id}', '{i.Flag}', '{i.GetResultMetric()}', '{answerJson}'");
            accessor.Insert(sb.ToString());
        }

        // Position

        private void UpdatePosition(AvailablePosition pos) {
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE positions SET ");
            sb.Append($"Position='{pos.Position}', ");
            sb.Append($"Seats='{pos.Seats}' ");
            sb.Append($"WHERE ID='{pos.Id}'");
            string s = sb.ToString();
            accessor.Update(s);
        }

        private void InsertPosition(AvailablePosition pos) {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO positions (Position, Seats) VALUES ");
            sb.Append($"('{pos.Position}', '{pos.Seats}')");
            string s = sb.ToString();
            accessor.Insert(s);
        }

        private void DeletePosition(AvailablePosition pos) {
            String del = $"DELETE FROM positions WHERE ID='{pos.Id}'";
            accessor.Delete(del);
        }

        #endregion
    }
}