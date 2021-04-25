using OpenTokFs;
using OpenTokFs.Api;
using OpenTokFs.OptionUtilities;
using OpenTokFs.RequestDomain;
using System.Threading.Tasks;

namespace CsExample {
    class Program {
        static async Task Main(string[] args) {
            var credentials = new OpenTokProjectCredentials(12345, "secret_here");

            var session = await Session.CreateAsync(credentials, new SessionCreationParameters(
                archiveMode: ArchiveMode.Manual,
                location: Option.Some(System.Net.IPAddress.Parse("104.18.18.242")),
                p2p_preference: P2PPreference.Enabled));

            await Task.Delay(15000);

            var current_archives = await Archive.ListAllAsync(
                credentials,
                1,
                OpenTokSessionId.NewId(session.Session_id));
            if (current_archives.IsEmpty) {
                var archive = await Archive.StartAsync(
                    credentials,
                    new ArchiveStartRequest(
                        sessionId: session.Session_id,
                        hasAudio: true,
                        hasVideo: true,
                        name: Option.Some("My Name Here"),
                        outputType: ArchiveOutputType.NewComposed(
                            Resolution.SD,
                            Layout.NewStandard(StandardLayout.BestFit))));

                await Task.Delay(10000);

                await Archive.SetLayoutAsync(credentials,
                    archive.Id,
                    Layout.NewBestFitOr(ScreenshareType.NewScreenshareType(StandardLayout.Pip)));

                await Task.Delay(10000);

                await Archive.StopAsync(credentials, archive.Id);
            }
        }
    }
}
