// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SEP6.DB;

namespace SEP6.DB.Migrations
{
    [DbContext(typeof(MoviesDbContext))]
    [Migration("20210526160814_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.6")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MovieTopLists", b =>
                {
                    b.Property<int>("MoviesId")
                        .HasColumnType("int");

                    b.Property<int>("TopListsesId")
                        .HasColumnType("int");

                    b.HasKey("MoviesId", "TopListsesId");

                    b.HasIndex("TopListsesId");

                    b.ToTable("MovieTopLists");
                });

            modelBuilder.Entity("SEP6.DB.Director", b =>
                {
                    b.Property<int>("MovieId")
                        .HasColumnType("int")
                        .HasColumnName("movie_id");

                    b.Property<int>("PersonId")
                        .HasColumnType("int")
                        .HasColumnName("person_id");

                    b.HasKey("MovieId", "PersonId")
                        .HasName("directors_pk")
                        .IsClustered(false);

                    b.HasIndex("PersonId");

                    b.ToTable("directors");
                });

            modelBuilder.Entity("SEP6.DB.Movie", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)")
                        .HasColumnName("title");

                    b.Property<int>("Year")
                        .HasColumnType("int")
                        .HasColumnName("year");

                    b.HasKey("Id")
                        .HasName("movies_pk")
                        .IsClustered(false);

                    b.ToTable("movies");
                });

            modelBuilder.Entity("SEP6.DB.Person", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<short?>("Birth")
                        .HasColumnType("smallint")
                        .HasColumnName("birth");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.ToTable("people");
                });

            modelBuilder.Entity("SEP6.DB.Rating", b =>
                {
                    b.Property<int>("MovieId")
                        .HasColumnType("int")
                        .HasColumnName("movie_id");

                    b.Property<float>("Rating1")
                        .HasColumnType("real")
                        .HasColumnName("rating");

                    b.Property<int>("Votes")
                        .HasColumnType("int")
                        .HasColumnName("votes");

                    b.HasIndex("MovieId");

                    b.ToTable("ratings");
                });

            modelBuilder.Entity("SEP6.DB.Star", b =>
                {
                    b.Property<int>("MovieId")
                        .HasColumnType("int")
                        .HasColumnName("movie_id");

                    b.Property<int>("PersonId")
                        .HasColumnType("int")
                        .HasColumnName("person_id");

                    b.HasIndex("MovieId");

                    b.HasIndex("PersonId");

                    b.ToTable("stars");
                });

            modelBuilder.Entity("SEP6.DB.TopLists", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "UserId" }, "IX_TopLists_UserId");

                    b.ToTable("TopLists");
                });

            modelBuilder.Entity("SEP6.DB.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Birthday")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Country")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Email")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<byte[]>("PasswordSalt")
                        .HasMaxLength(16)
                        .HasColumnType("varbinary(16)");

                    b.Property<string>("Token")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Username")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("Token")
                        .IsUnique()
                        .HasFilter("[Token] IS NOT NULL");

                    b.HasIndex("Username")
                        .IsUnique()
                        .HasFilter("[Username] IS NOT NULL");

                    b.HasIndex(new[] { "Token" }, "Users_Token_uindex")
                        .IsUnique()
                        .HasFilter("[Token] IS NOT NULL");

                    b.HasIndex(new[] { "Username" }, "Users_Username_uindex")
                        .IsUnique()
                        .HasFilter("[Username] IS NOT NULL");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("UserUser", b =>
                {
                    b.Property<int>("FollowersId")
                        .HasColumnType("int");

                    b.Property<int>("FollowsId")
                        .HasColumnType("int");

                    b.HasKey("FollowersId", "FollowsId");

                    b.HasIndex("FollowsId");

                    b.ToTable("Followers");
                });

            modelBuilder.Entity("MovieTopLists", b =>
                {
                    b.HasOne("SEP6.DB.Movie", null)
                        .WithMany()
                        .HasForeignKey("MoviesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SEP6.DB.TopLists", null)
                        .WithMany()
                        .HasForeignKey("TopListsesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SEP6.DB.Director", b =>
                {
                    b.HasOne("SEP6.DB.Movie", "Movie")
                        .WithMany("Directors")
                        .HasForeignKey("MovieId")
                        .HasConstraintName("directors_movies_id_fk")
                        .IsRequired();

                    b.HasOne("SEP6.DB.Person", "Person")
                        .WithMany("Directors")
                        .HasForeignKey("PersonId")
                        .HasConstraintName("directors_people_id_fk")
                        .IsRequired();

                    b.Navigation("Movie");

                    b.Navigation("Person");
                });

            modelBuilder.Entity("SEP6.DB.Rating", b =>
                {
                    b.HasOne("SEP6.DB.Movie", "Movie")
                        .WithMany()
                        .HasForeignKey("MovieId")
                        .HasConstraintName("ratings_movies_id_fk")
                        .IsRequired();

                    b.Navigation("Movie");
                });

            modelBuilder.Entity("SEP6.DB.Star", b =>
                {
                    b.HasOne("SEP6.DB.Movie", "Movie")
                        .WithMany()
                        .HasForeignKey("MovieId")
                        .HasConstraintName("stars_movies_id_fk")
                        .IsRequired();

                    b.HasOne("SEP6.DB.Person", "Person")
                        .WithMany()
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Movie");

                    b.Navigation("Person");
                });

            modelBuilder.Entity("SEP6.DB.TopLists", b =>
                {
                    b.HasOne("SEP6.DB.User", "User")
                        .WithMany("TopLists")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("UserUser", b =>
                {
                    b.HasOne("SEP6.DB.User", null)
                        .WithMany()
                        .HasForeignKey("FollowersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SEP6.DB.User", null)
                        .WithMany()
                        .HasForeignKey("FollowsId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SEP6.DB.Movie", b =>
                {
                    b.Navigation("Directors");
                });

            modelBuilder.Entity("SEP6.DB.Person", b =>
                {
                    b.Navigation("Directors");
                });

            modelBuilder.Entity("SEP6.DB.User", b =>
                {
                    b.Navigation("TopLists");
                });
#pragma warning restore 612, 618
        }
    }
}
