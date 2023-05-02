using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace instagram.Models;

public class InstagramContext : IdentityDbContext<User>
{
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<UserSubscription> UserSubscriptions { get; set; }
    public DbSet<UserFollower> UserFollowers { get; set; }
    
    public InstagramContext (DbContextOptions<InstagramContext> options) : base(options){}
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Post>().HasQueryFilter(task => task.IsDelete == false);
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<UserFollower>()
            .HasKey(uf => new { uf.UserId, uf.FollowerId });

        modelBuilder.Entity<UserFollower>()
            .HasOne(uf => uf.User)
            .WithMany(u => u.Followers)
            .HasForeignKey(uf => uf.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserFollower>()
            .HasOne(uf => uf.Follower)
            .WithMany()
            .HasForeignKey(uf => uf.FollowerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserSubscription>()
            .HasKey(us => new { us.UserId, us.SubscriptionId });

        modelBuilder.Entity<UserSubscription>()
            .HasOne(us => us.User)
            .WithMany(u => u.Subscriptions)
            .HasForeignKey(us => us.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserSubscription>()
            .HasOne(us => us.Subscription)
            .WithMany()
            .HasForeignKey(us => us.SubscriptionId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Post>()
            .HasKey(p => p.Id);
        modelBuilder.Entity<Post>()
            .HasOne(p=> p.Creator)
            .WithMany(p=> p.Posts)
            .HasForeignKey(p=> p.CreatorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Post>()
            .HasMany(x => x.Likes)
            .WithOne(x => x.Post)
            .HasForeignKey(x => x.PostId);
    }
}