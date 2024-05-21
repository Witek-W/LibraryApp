using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Model;

public class LibraryDbContext : DbContext
{
	public DbSet<Books> Book { get; set; }

	public DbSet<Readers> Readers {  get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		string connectionString = "Server=bjf8qvoacsli0xdg4sc9-mysql.services.clever-cloud.com;Port=3306;Database=bjf8qvoacsli0xdg4sc9;User=uorfgn1pjbsyonep;Password=dl0kEPveXxvEQC0zHYpj;";
		//string connectionString = "Server=sql7.freemysqlhosting.net;Database=sql7708171;User=sql7708171;Password=EmyrAk44gX;";
		//string connectionString = "Server=10.0.2.2;Database=library;User=root;Password=;";
		optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
	}
}
