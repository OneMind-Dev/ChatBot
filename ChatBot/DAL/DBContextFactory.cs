using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace DAL
{
    internal class DBContextFactory : IDesignTimeDbContextFactory<ChatBotDBcontext>
    {
        public ChatBotDBcontext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ChatBotDBcontext>();
            optionsBuilder.UseSqlServer(GetConnectionString());

            return new ChatBotDBcontext(optionsBuilder.Options);
        }

        private string GetConnectionString()
        {
            // This config builder run from ChatBot\DAL\bin\Debug\net8.0\ directory
            // This will navigate to ChatBot\WPF and get the connection string from appsettings.json 
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(string.Join("\\", AppDomain.CurrentDomain.BaseDirectory
                    .Split("\\")
                    .SkipLast(5)
                    .Append("WPF")))
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
            return config["ConnectionStrings:DefaultConnection"];
        }
    }
}
