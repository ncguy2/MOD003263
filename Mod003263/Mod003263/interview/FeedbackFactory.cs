using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mod003263.macro;

/**
 * Author: Nick Guy
 * Date: 30/11/2016
 * Contains: FeedbackFactory, FeedbackMacros, SharedRef
 */
namespace Mod003263.interview {

    public class FeedbackFactory {

        private static FeedbackFactory instance;
        public static FeedbackFactory GetInstance() {
            return instance ?? (instance = new FeedbackFactory());
        }

        private SharedRef<Question> questionRef;
        private SharedRef<Interview> interviewRef;
        private SharedRef<Answer> answerRef;

        private FeedbackFactory() {
            FeedbackMacros.InitRefs(this);
        }

        public void QuestionRef(SharedRef<Question> qRef) {
            this.questionRef = qRef;
        }
        public void InterviewRef(SharedRef<Interview> iRef) {
            this.interviewRef = iRef;
        }
        public void AnswerRef(SharedRef<Answer> aRef) {
            this.answerRef = aRef;
        }

        public String GenerateFeedback(KeyValuePair<Question, Answer> entry) {
            return GenerateFeedback(entry.Key, entry.Value);
        }

        public String GenerateFeedback(Question q, Answer a) {
            questionRef.Payload = q;
            answerRef.Payload = a;
            int w = a.Weight;
            if (w >= 100) return Generate100(q);
            if (w >= 80)  return Generate80(q);
            if (w >= 60)  return Generate60(q);
            if (w >= 40)  return Generate40(q);
            if (w >= 20)  return Generate20(q);
            if (w >= 0)   return Generate0(q);
            questionRef.Payload = null;
            answerRef.Payload = null;
            return new MacroTemplate("Error generating feedback for {question}").Populate();
        }

        public Dictionary<Question, String> GenerateMassFeedback(Interview interview) {
            interviewRef.Payload = interview;
            Dictionary<Question, String> dictionary = interview.GetFoundationInstance()
                .GetAnswerMap()
                .ToDictionary(entry => entry.Key, GenerateFeedback);
            interviewRef.Payload = null;
            return dictionary;
        }

        // Metric of 0+
        private String Generate0(Question q) {
            return GenerateFromTemplate(q, GetTemplate0());
        }
        // Metric of 20+
        private String Generate20(Question q) {
            return GenerateFromTemplate(q, GetTemplate20());
        }
        // Metric of 40+
        private String Generate40(Question q) {
            return GenerateFromTemplate(q, GetTemplate40());
        }
        // Metric of 60+
        private String Generate60(Question q) {
            return GenerateFromTemplate(q, GetTemplate60());
        }
        // Metric of 80+
        private String Generate80(Question q) {
            return GenerateFromTemplate(q, GetTemplate80());
        }
        // Metric of 100+
        private String Generate100(Question q) {
            return GenerateFromTemplate(q, GetTemplate100());
        }

        private String GenerateFromTemplate(Question q, MacroTemplate t) {
            return t.Populate();
        }

        private MacroTemplate GetTemplate0() {
            return template0 ?? (template0 = MacroTemplate.FromFile("feedback/template_0.txt"));
        }
        private MacroTemplate GetTemplate20() {
            return template20 ?? (template20 = MacroTemplate.FromFile("feedback/template_20.txt"));
        }
        private MacroTemplate GetTemplate40() {
            return template40 ?? (template40 = MacroTemplate.FromFile("feedback/template_40.txt"));
        }
        private MacroTemplate GetTemplate60() {
            return template60 ?? (template60 = MacroTemplate.FromFile("feedback/template_60.txt"));
        }
        private MacroTemplate GetTemplate80() {
            return template80 ?? (template80 = MacroTemplate.FromFile("feedback/template_80.txt"));
        }
        private MacroTemplate GetTemplate100() {
            return template100 ?? (template100 = MacroTemplate.FromFile("feedback/template_100.txt"));
        }

        private MacroTemplate template0;
        private MacroTemplate template20;
        private MacroTemplate template40;
        private MacroTemplate template60;
        private MacroTemplate template80;
        private MacroTemplate template100;

    }

    public class FeedbackMacros {

        private static SharedRef<Question> questionRef;
        private static SharedRef<Interview> interviewRef;
        private static SharedRef<Answer> answerRef;

        public static void InitRefs(FeedbackFactory fac) {
            fac.QuestionRef(questionRef = new SharedRef<Question>());
            fac.InterviewRef(interviewRef = new SharedRef<Interview>());
            fac.AnswerRef(answerRef = new SharedRef<Answer>());
            MacroManager.Instance().RegisterMacroDomain(typeof(FeedbackMacros));
            MacroFinder.FindAndRegister();
        }

        [Macro("question")]
        public static String Question() {
            return questionRef.Payload.Text();
        }

        [Macro("weight")]
        public static String Weight() {
            return answerRef.Payload.Weight + "%";
        }

        [Macro("grade")]
        public static String Grade() {
            return answerRef.Payload.Text ?? "Unknown Grade";
        }

    }

    public class SharedRef<T> {
        public T Payload { get; set; }
        public SharedRef() : this(default(T)) {}
        public SharedRef(T payload) {
            Payload = payload;
        }
    }

}