using ConferenceDTO;

namespace BackEnd.Data
{
    public static class EntityExtensions
    {
        public static ConferenceDTO.SpeakerResponse MapSpeakerResponse(this Speaker speaker) =>
            new ConferenceDTO.SpeakerResponse
            {
                Id = speaker.Id,
                Name = speaker.Name,
                Bio = speaker.Bio,
                Website = speaker.Website,
                Sessions = speaker.SessionSpeakers?
                    .Select( ss =>
                        new ConferenceDTO.Session
                        {
                            Id = ss.SessionId,
                            Title = ss.Session.Title
                        })
                    .ToList() ?? new ()
            };

        public static ConferenceDTO.AttendeeResponse MapAttendeeResponse(this Attendee attendee) =>
            new ConferenceDTO.AttendeeResponse
            {
                Id = attendee.Id,
                FirstName = attendee.FirstName,
                LastName = attendee.LastName,
                UserName = attendee.UserName,
                EmailAddress = attendee.EmailAddress,
                Sessions = attendee.SessionAttendees?
                    .Select(sa =>
                        new ConferenceDTO.Session
                        {
                            Id = sa.SessionId,
                            Title = sa.Session.Title,
                            StartTime = sa.Session.StartTime,
                            EndTime = sa.Session.EndTime
                        })
                .ToList() ?? new ()
            };

        public static ConferenceDTO.SessionResponse MapSessionResponse(this Session session) =>
            new ConferenceDTO.SessionResponse
            {
                Id = session.Id,
                Title = session.Title,
                StartTime = session.StartTime,
                EndTime = session.EndTime,
                Speakers = session.SessionSpeakers?
                                .Select(ss => new ConferenceDTO.Speaker
                                {
                                    Id = ss.Speaker.Id,
                                    Name = ss.Speaker.Name
                                })
                                .ToList() ?? new(),
                TrackId = session.TrackId,
                Track = new ConferenceDTO.Track
                {
                    Id = session?.TrackId ?? 0,
                    Name = session?.Track?.Name
                },
                Abstract = session?.Abstract
            };

    }
}
