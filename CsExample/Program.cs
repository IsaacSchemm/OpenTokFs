using OpenTokFs.Api;
using OpenTokFs.Credentials;
using OpenTokFs.RequestDomain;
using System;
using System.Threading.Tasks;

namespace CsExample {
    class Program {
        static async Task Main(string[] args) {
            var credentials = new ProjectCredentials(12345, "secret_here");

            var session = await Session.CreateAsync(credentials, new NewSession(
                archiveMode: ArchiveMode.ManuallyArchive,
                location: SessionLocation.FirstClientConnectionLocation,
                p2p_preference: MediaMode.RoutedSession));

            await Task.Delay(15000);

            var previous_archives = await Archive.ListAllAsync(
                credentials,
                new ListParameters(
                    first_page: new PageBoundaries(0, 5),
                    limit: ListLimit.NewStopAtItemCount(10)),
                SessionIdFilter.NewSingleSessionId(session.Session_id));
            Console.WriteLine(previous_archives);

            var archive = await Archive.StartAsync(
                credentials,
                new ArchiveStartRequest(
                    sessionId: session.Session_id,
                    hasAudio: true,
                    hasVideo: true,
                    name: ArchiveName.NewCustomArchiveName("My Name Here"),
                    outputType: ArchiveOutputType.NewComposedArchive(
                        Resolution.StandardDefinition,
                        Layout.NewStandard(StandardLayout.VerticalPresentation))));

            await Task.Delay(10000);

            await Archive.SetLayoutAsync(credentials,
                archive.Id,
                Layout.NewBestFitOr(ScreenshareType.NewScreenshareType(StandardLayout.Pip)));

            await Task.Delay(10000);

            await Archive.StopAsync(credentials, archive.Id);
        }
    }
}
