using Microsoft.EntityFrameworkCore;
using Web_Api.Models.AuthenticationModels;

namespace Web_Api.Models;

public class WebApiContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<ToDoTask> ToDoTasks { get; set; }
    public DbSet<TaskGroup> TaskGroups { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public WebApiContext(DbContextOptions<WebApiContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }
}

