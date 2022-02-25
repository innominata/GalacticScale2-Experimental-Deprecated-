using System.IO;
using NebulaAPI;
using GalacticScale;

namespace NebulaCompatibility
{
    [RegisterPacketProcessor]
    public class LobbyResponseUpdateSolarSystemsProcessor : BasePacketProcessor<LobbyResponseUpdateSolarSystems>
    {
        public override void ProcessPacket(LobbyResponseUpdateSolarSystems packet, INebulaConnection conn)
        {
            if (IsHost) return;

            var gameDesc = UIRoot.instance.galaxySelect.gameDesc;
            var galaxyData = UIRoot.instance.galaxySelect.starmap.galaxyData;

            if (galaxyData == null)
            {
                if (GS2.Vanilla)
                    galaxyData = UniverseGen.CreateGalaxy(gameDesc);
                else
                    galaxyData = GS2.ProcessGalaxy(gameDesc, true);

                UIRoot.instance.galaxySelect.starmap.galaxyData = galaxyData;
            }

            using (var ms = new MemoryStream(packet.GSSettings))
            {
                using (var r = new BinaryReader(ms))
                {
                    GSSettings.FromString(r.ReadString());
                }
            }

            GSSettings.lobbyReceivedUpdateValues = true;

            UIRoot.instance.galaxySelect.SetStarmapGalaxy();
        }
    }
}