
namespace vgwb
{
    public class Statics : SingletonMonoBehaviour<Statics>
    {
        private static DataManager data; public static DataManager Data => data ??= new DataManager();
        // private static CardsManager cards; public static CardsManager Cards => cards ??= new CardsManager();
        // private static ScreensManager screens; public static ScreensManager Screens => screens ??= new ScreensManager();
        // private static InputManager input; public static InputManager Input => input ??= FindObjectOfType<InputManager>();

        private static AppManager app; public static AppManager App => app ??= FindObjectOfType<AppManager>();
        // private static PointsManager points; public static PointsManager Points => points ??= FindObjectOfType<PointsManager>();
        // private static ModeManager mode; public static ModeManager Mode => mode ??= FindObjectOfType<ModeManager>();
        // private static SessionFlowManager sessionFlow; public static SessionFlowManager SessionFlow => sessionFlow ??= FindObjectOfType<SessionFlowManager>();
        // private static ActivityFlowManager activityFlow; public static ActivityFlowManager ActivityFlow => activityFlow ??= FindObjectOfType<ActivityFlowManager>();
        // private static OnlineAnalyticsService analytics; public static OnlineAnalyticsService Analytics => analytics ??= FindObjectOfType<OnlineAnalyticsService>();
        // private static NotificationService notifications; public static NotificationService Notifications => notifications ??= FindObjectOfType<NotificationService>();
        // private static TutorialManager tutorial; public static TutorialManager Tutorial => tutorial ??= FindObjectOfType<TutorialManager>();
        // private static ArtManager art; public static ArtManager Art => art ??= FindObjectOfType<ArtManager>();
    }
}
