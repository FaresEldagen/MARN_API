using System.Net.Http.Headers;

namespace MARN_API.Services.Implementations
{
    public class OpenTimestampsService
    {
        private static readonly byte[] OtsHeaderPrefix =
        [
            0x00, 0x4f, 0x70, 0x65, 0x6e, 0x54, 0x69, 0x6d, 0x65, 0x73, 0x74, 0x61, 0x6d, 0x70, 0x73, 0x00,
            0x00, 0x50, 0x72, 0x6f, 0x6f, 0x66, 0x00, 0xbf, 0x89, 0xe2, 0xe8, 0x84, 0xe8, 0x92, 0x94,
            0x01,
            0x08
        ];

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<OpenTimestampsService> _logger;

        public OpenTimestampsService(HttpClient httpClient, IConfiguration configuration, ILogger<OpenTimestampsService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
        }

        private string[] GetCalendarServers()
        {
            return _configuration.GetSection("OpenTimestamps:CalendarServers").Get<string[]>()
                ?? [
                    "https://alice.btc.calendar.opentimestamps.org",
                    "https://bob.btc.calendar.opentimestamps.org",
                    "https://finney.calendar.eternitywall.com"
                ];
        }

        private static byte[] ConvertHexStringToByteArray(string hex)
        {
            if (hex.Length % 2 != 0)
            {
                throw new ArgumentException("Invalid hex string length.");
            }

            var bytes = new byte[hex.Length / 2];
            for (var index = 0; index < bytes.Length; index++)
            {
                bytes[index] = Convert.ToByte(hex.Substring(index * 2, 2), 16);
            }

            return bytes;
        }

        public byte[] BuildDetachedOtsFile(string hashHex, byte[] serverAttestation)
        {
            var hashBytes = ConvertHexStringToByteArray(hashHex);
            var otsFile = new byte[OtsHeaderPrefix.Length + hashBytes.Length + serverAttestation.Length];

            Buffer.BlockCopy(OtsHeaderPrefix, 0, otsFile, 0, OtsHeaderPrefix.Length);
            Buffer.BlockCopy(hashBytes, 0, otsFile, OtsHeaderPrefix.Length, hashBytes.Length);
            Buffer.BlockCopy(serverAttestation, 0, otsFile, OtsHeaderPrefix.Length + hashBytes.Length, serverAttestation.Length);

            return otsFile;
        }

        public string CalculateCommitmentHash(byte[] otsFile)
        {
            if (otsFile.Length < 65)
            {
                throw new Exception("Invalid OTS file format.");
            }

            var currentState = new byte[32];
            Buffer.BlockCopy(otsFile, 33, currentState, 0, 32);

            var pointer = 65;
            while (pointer < otsFile.Length)
            {
                var operation = otsFile[pointer++];
                if (operation == 0x00)
                {
                    break;
                }

                if (operation == 0x08)
                {
                    using var sha256 = System.Security.Cryptography.SHA256.Create();
                    currentState = sha256.ComputeHash(currentState);
                }
                else if (operation == 0xf0 || operation == 0xf1)
                {
                    var length = otsFile[pointer++];
                    var newState = new byte[currentState.Length + length];

                    if (operation == 0xf1)
                    {
                        Buffer.BlockCopy(otsFile, pointer, newState, 0, length);
                        Buffer.BlockCopy(currentState, 0, newState, length, currentState.Length);
                    }
                    else
                    {
                        Buffer.BlockCopy(currentState, 0, newState, 0, currentState.Length);
                        Buffer.BlockCopy(otsFile, pointer, newState, currentState.Length, length);
                    }

                    pointer += length;
                    currentState = newState;
                }
                else
                {
                    break;
                }
            }

            return Convert.ToHexString(currentState).ToLowerInvariant();
        }

        public byte[] MergeOtsUpgrade(byte[] pendingOtsBytes, byte[] upgradeAttestation)
        {
            var pointer = 65;
            while (pointer < pendingOtsBytes.Length)
            {
                var operation = pendingOtsBytes[pointer++];
                if (operation == 0x00)
                {
                    var prefixLength = pointer - 1;
                    var merged = new byte[prefixLength + upgradeAttestation.Length];
                    Buffer.BlockCopy(pendingOtsBytes, 0, merged, 0, prefixLength);
                    Buffer.BlockCopy(upgradeAttestation, 0, merged, prefixLength, upgradeAttestation.Length);
                    return merged;
                }

                if (operation == 0xf0 || operation == 0xf1)
                {
                    var length = pendingOtsBytes[pointer++];
                    pointer += length;
                }
            }

            return pendingOtsBytes;
        }

        public async Task<byte[]> SubmitHashAsync(string hashHex)
        {
            var hashBytes = ConvertHexStringToByteArray(hashHex);
            foreach (var server in GetCalendarServers())
            {
                try
                {
                    using var request = new HttpRequestMessage(HttpMethod.Post, $"{server.TrimEnd('/')}/digest");
                    request.Content = new ByteArrayContent(hashBytes);
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                    var response = await _httpClient.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        var attestation = await response.Content.ReadAsByteArrayAsync();
                        return BuildDetachedOtsFile(hashHex, attestation);
                    }

                    _logger.LogWarning("Calendar server {Server} returned status {Status}.", server, response.StatusCode);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to submit to calendar server {Server}.", server);
                }
            }

            throw new Exception("All OpenTimestamps calendar servers failed to process the request.");
        }

        public async Task<byte[]?> UpgradeOtsAsync(byte[] pendingOtsBytes)
        {
            var commitmentHashHex = CalculateCommitmentHash(pendingOtsBytes);

            foreach (var server in GetCalendarServers())
            {
                try
                {
                    var response = await _httpClient.GetAsync($"{server.TrimEnd('/')}/timestamp/{commitmentHashHex}");
                    if (response.IsSuccessStatusCode)
                    {
                        var attestation = await response.Content.ReadAsByteArrayAsync();
                        return MergeOtsUpgrade(pendingOtsBytes, attestation);
                    }

                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        continue;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to check upgrade from calendar server {Server}.", server);
                }
            }

            return null;
        }
    }
}
