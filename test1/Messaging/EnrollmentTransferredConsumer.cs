using LMS___Mini_Version.Messaging;
using MassTransit;

namespace test1.Consumers;

public sealed class EnrollmentTransferredConsumer(
    ILogger<EnrollmentTransferredConsumer> logger)
    : IConsumer<EnrollmentTransferred>
{
    public Task Consume(ConsumeContext<EnrollmentTransferred> context)
    {
        var message = context.Message;

        logger.LogInformation(
            "Enrollment {EnrollmentId} transferred from Track {OldTrack} to {NewTrack}. Transfer: {TransferId}",
            message.EnrollmentId,
            message.PreviousTrackId,
            message.NewTrackId,
            message.TransferId);

        return Task.CompletedTask;
    }
}