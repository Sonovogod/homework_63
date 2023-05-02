namespace instagram.Models;

public class UserFollower
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
    public string FollowerId { get; set; }
    public User Follower { get; set; }
    public DateTime DateOfFollowing { get; set; }
}