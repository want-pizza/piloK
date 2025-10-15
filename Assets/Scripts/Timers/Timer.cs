public class Timer
{
    private float timeLeft;
    private string eventName;

    public Timer(float duration, string eventName)
    {
        timeLeft = duration;
        this.eventName = eventName;
    }

    public bool UpdateTimer(float deltaTime)
    {
        timeLeft -= deltaTime;

        if (timeLeft <= 0)
        {
            EventBus.Publish(eventName);
            return true;
        }
        return false;
    }
}
