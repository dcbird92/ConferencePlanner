using Microsoft.EntityFrameworkCore;

namespace BackEnd.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Attendee>()
            .HasIndex(a => a.UserName)
            .IsUnique();

            modelBuilder.Entity<SessionAttendee>()
                .HasKey(ca => new {ca.SessionId, ca.AttendeeId});

            modelBuilder.Entity<SessionSpeaker>()
                .HasKey(ss => new {ss.SessionId, ss.SpeakerID});
        }

        public DbSet<Session> Sessions => Set<Session>();

        public DbSet<Track> Tracks => Set<Track>();

        public DbSet<Attendee> Attendees => Set<Attendee>();

        public DbSet<Speaker> Speakers => Set<Speaker>();
    }
}
