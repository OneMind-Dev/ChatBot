using BOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DAOs
{
    public class ChatBotDBcontext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<Message> Messages { get; set; }

        public ChatBotDBcontext() { }

        public ChatBotDBcontext(DbContextOptions<ChatBotDBcontext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        {
            // If this class called for migrations, the connection string will already be added
            // (DAL/DBContextFactory to know what it is)
            // If this class called when run WPF app, GetConnectionString() will be called
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(GetConnectionString());
            }
        }

        // appsettings.json when running WPF app will be in the same domain
        // (when set "Do not copy" property of appsettings.json to "copy always")
        private string GetConnectionString()
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
            return config["ConnectionStrings:DefaultConnection"];
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Role>().ToTable("Roles");
            modelBuilder.Entity<Conversation>().ToTable("Conversations");
            modelBuilder.Entity<Message>().ToTable("Messages");

            modelBuilder.Entity<User>().HasData(DataSeeding.Users);
            modelBuilder.Entity<Role>().HasData(DataSeeding.Roles);
            modelBuilder.Entity<Conversation>().HasData(DataSeeding.Conversations);
            modelBuilder.Entity<Message>().HasData(DataSeeding.Messages);

        }
    }
}
