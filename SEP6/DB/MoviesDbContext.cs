using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace SEP6.DB
{
    public partial class MoviesDbContext : DbContext
    {
        public MoviesDbContext()
        {
        }

        public MoviesDbContext(DbContextOptions<MoviesDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Director> Directors { get; set; }
        public virtual DbSet<Movie> Movies { get; set; }
        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<Rating> Ratings { get; set; }
        public virtual DbSet<Star> Stars { get; set; }
        public virtual DbSet<TopLists> TopLists { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=tcp:sep6moviesdb.database.windows.net,1433;Initial Catalog=movies;Persist Security Info=False;User ID=webapi;Password=icSBETu47pL9f7n;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Director>(entity =>
            {
                entity.HasKey(e => new { e.MovieId, e.PersonId })
                    .HasName("directors_pk")
                    .IsClustered(false);

                entity.ToTable("directors");

                entity.Property(e => e.MovieId).HasColumnName("movie_id");

                entity.Property(e => e.PersonId).HasColumnName("person_id");

                entity.HasOne(d => d.Movie)
                    .WithMany(p => p.Directors)
                    .HasForeignKey(d => d.MovieId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("directors_movies_id_fk");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.Directors)
                    .HasForeignKey(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("directors_people_id_fk");
            });

            modelBuilder.Entity<Movie>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("movies_pk")
                    .IsClustered(false);

                entity.ToTable("movies");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(300)
                    .HasColumnName("title");

                entity.Property(e => e.Year).HasColumnName("year");
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.ToTable("people");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Birth).HasColumnName("birth");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Rating>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ratings");

                entity.Property(e => e.MovieId).HasColumnName("movie_id");

                entity.Property(e => e.Rating1).HasColumnName("rating");

                entity.Property(e => e.Votes).HasColumnName("votes");

                entity.HasOne(d => d.Movie)
                    .WithMany()
                    .HasForeignKey(d => d.MovieId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ratings_movies_id_fk");
            });

            modelBuilder.Entity<Star>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("stars");

                entity.Property(e => e.MovieId).HasColumnName("movie_id");

                entity.Property(e => e.PersonId).HasColumnName("person_id");

                entity.HasOne(d => d.Movie)
                    .WithMany()
                    .HasForeignKey(d => d.MovieId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("stars_movies_id_fk");
            });

            modelBuilder.Entity<TopLists>(entity =>
            {
                entity.HasIndex(e => e.UserId, "IX_TopLists_UserId");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TopLists)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Token, "Users_Token_uindex")
                    .IsUnique();

                entity.HasIndex(e => e.Username, "Users_Username_uindex")
                    .IsUnique();

                entity.Property(e => e.Birthday)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Country)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PasswordSalt).HasMaxLength(16);

                entity.Property(e => e.Token)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<User>()
                .HasMany<User>(m => m.Follows)
                .WithMany(m => m.Followers)
                .UsingEntity(b =>
                {
                    b.ToTable("Followers");
                });
            
            modelBuilder.Entity<User>()
                .HasMany<User>(m => m.Follows)
                .WithMany(m => m.Followers)
                .UsingEntity(b =>
                {
                    b.ToTable("Followers");
                });

            modelBuilder.Entity<User>().HasIndex(a => a.Username).IsUnique();
            modelBuilder.Entity<User>().HasIndex(a => a.Token).IsUnique();

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
