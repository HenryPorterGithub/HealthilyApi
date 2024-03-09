namespace HealthilyAPI
{
    public class ChatResponse
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class Attributes
        {
            public string additionalProp { get; set; }
        }

        public class ChatEvent
        {
            public string action_type { get; set; }
            public Attributes attributes { get; set; }
            public string event_name { get; set; }
            public Meta meta { get; set; }
            public Metrics metrics { get; set; }
        }

        public class Choice
        {
            public string link_url { get; set; }
            public string url_type { get; set; }
        }

        public class ConsultationTriage
        {
            public string level { get; set; }
            public string triage { get; set; }
            public string triage_advice { get; set; }
        }

        public class Conversation
        {
            public bool conversation { get; set; }
            public string type { get; set; }
        }

        public class ConversationContext
        {
            public string id { get; set; }
            public string type { get; set; }
        }

        public class ConversationModel
        {
            public ConversationContext conversation_context { get; set; }
            public Report report { get; set; }
            public SymptomsSummary symptoms_summary { get; set; }
        }

        public class Excluded
        {
            public string cui { get; set; }
            public string name { get; set; }
            public string severity { get; set; }
        }

        public class HealthBackground
        {
            public string answer { get; set; }
            public string long_name { get; set; }
            public string name { get; set; }
        }

        public class Ignored
        {
            public string cui { get; set; }
            public string name { get; set; }
            public string severity { get; set; }
        }

        public class Included
        {
            public string name { get; set; }
            public string severity { get; set; }
        }

        public class Main
        {
            public string name { get; set; }
            public string severity { get; set; }
        }

        public class Message
        {
            public List<ChatEvent> chat_events { get; set; }
            public Conversation conversation { get; set; }
            public string id { get; set; }
            public Meta meta { get; set; }
            public string type { get; set; }
            public string value { get; set; }
            public int wait_millis { get; set; }
        }

        public class Meta
        {
            public string additionalProp { get; set; }
            public List<string> chat_context { get; set; }
            public string image { get; set; }
            public string name { get; set; }
            public List<Section> sections { get; set; }
            public string snippet { get; set; }
            public string source { get; set; }
            public string summary { get; set; }
            public string title { get; set; }
            public string type { get; set; }
            public string url_type { get; set; }
        }

        public class Metrics
        {
            public int additionalProp { get; set; }
        }

        public class PossibleCause
        {
            public string name { get; set; }
            public string snippet { get; set; }
            public Triage triage { get; set; }
            public string yourmd_id { get; set; }
        }

        public class Question
        {
            public List<Choice> choices { get; set; }
            public bool mandatory { get; set; }
            public bool multiple { get; set; }
            public int wait_millis { get; set; }
        }

        public class Report
        {
            public ConsultationTriage consultation_triage { get; set; }
            public string duration { get; set; }
            public List<HealthBackground> health_background { get; set; }
            public List<PossibleCause> possible_causes { get; set; }
            public Symptoms symptoms { get; set; }
            public UserProfile user_profile { get; set; }
        }

        public class Root
        {
            public List<string> available_commands { get; set; }
            public string conversation_id { get; set; }
            public ConversationModel conversation_model { get; set; }
            public List<Message> messages { get; set; }
            public bool popup { get; set; }
            public Question question { get; set; }
        }

        public class Section
        {
            public string id { get; set; }
            public string markdown { get; set; }
            public string name { get; set; }
            public string public_url { get; set; }
            public string summary { get; set; }
        }

        public class Selected
        {
            public string cui { get; set; }
            public string name { get; set; }
            public string severity { get; set; }
        }

        public class Symptoms
        {
            public List<string> excluded { get; set; }
            public List<Included> included { get; set; }
            public List<Main> main { get; set; }
            public List<string> unsure { get; set; }
        }

        public class SymptomsSummary
        {
            public List<Excluded> excluded { get; set; }
            public List<Ignored> ignored { get; set; }
            public List<Selected> selected { get; set; }
        }

        public class Triage
        {
            public string triage { get; set; }
            public string triage_advice { get; set; }
            public string triage_diagnostic { get; set; }
            public string triage_level { get; set; }
            public string triage_message { get; set; }
            public string triage_treatment { get; set; }
            public string triage_worries { get; set; }
        }

        public class UserProfile
        {
            public string gender { get; set; }
            public int year_of_birth { get; set; }
        }


    }
}
