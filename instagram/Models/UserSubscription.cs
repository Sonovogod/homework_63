namespace instagram.Models;

public class UserSubscription
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
    public string SubscriptionId { get; set; }
    public User Subscription { get; set; }
    public DateTime DateOfSubscription { get; set; }
}