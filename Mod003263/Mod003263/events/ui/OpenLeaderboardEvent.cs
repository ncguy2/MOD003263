namespace Mod003263.events.ui {
    public class OpenLeaderboardEvent : AbstractEvent {

        public interface OpenLeaderboardListener {
            [Event]
            void OnOpenLeaderboard(OpenLeaderboardEvent e);
        }

    }
}