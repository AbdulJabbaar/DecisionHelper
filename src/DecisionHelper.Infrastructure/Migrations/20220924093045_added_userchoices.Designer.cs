// <auto-generated />
using System;
using DecisionHelper.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DecisionHelper.Infrastructure.Migrations
{
    [DbContext(typeof(DecisionHelperDbContext))]
    [Migration("20220924093045_added_userchoices")]
    partial class added_userchoices
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("DecisionHelper.Domain.Entities.Adventure", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Adventures");
                });

            modelBuilder.Entity("DecisionHelper.Domain.Entities.Message", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AdventureId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("ByAnswer")
                        .HasColumnType("int");

                    b.Property<bool>("IsQuestion")
                        .HasColumnType("bit");

                    b.Property<Guid?>("ParentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AdventureId");

                    b.HasIndex("ParentId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("DecisionHelper.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DecisionHelper.Domain.Entities.UserChoice", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("AddedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("AdventureId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Answer")
                        .HasColumnType("int");

                    b.Property<Guid>("MessageId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("MessageId");

                    b.HasIndex("UserId");

                    b.ToTable("UserChoices");
                });

            modelBuilder.Entity("DecisionHelper.Domain.Entities.Message", b =>
                {
                    b.HasOne("DecisionHelper.Domain.Entities.Adventure", "Adventure")
                        .WithMany()
                        .HasForeignKey("AdventureId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DecisionHelper.Domain.Entities.Message", "Parent")
                        .WithMany()
                        .HasForeignKey("ParentId");

                    b.Navigation("Adventure");

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("DecisionHelper.Domain.Entities.UserChoice", b =>
                {
                    b.HasOne("DecisionHelper.Domain.Entities.Message", "Message")
                        .WithMany()
                        .HasForeignKey("MessageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DecisionHelper.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Message");

                    b.Navigation("User");
                });
#pragma warning restore 612, 618
        }
    }
}
