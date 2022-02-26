using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ChiaK
{
    internal class Utils
    {
        const int K32 = 106272212;
        const int K33 = 219027866;
        const int K34 = 450763579;

        static readonly DriveInfo[] allDrives = DriveInfo.GetDrives();

        public static int K32count { get; set; }
        public static int K33count { get; set; }
        public static int K34count { get; set; }

        public static IEnumerable<string> GetAllDrives()
        {
            return allDrives.Select(d => $"{d.Name.TrimEnd('\\')}   {KbToGb(d.AvailableFreeSpace) / 1024} Gb");
        }

        public static string GetSolution(int driveIndex)
        {
            if (driveIndex < 0)
            {
                return "Please select drive.";
            }
            var avaible = allDrives[driveIndex].AvailableFreeSpace / 1024;
            var totalSize = allDrives[driveIndex].TotalSize / 1024;
            List<(long k34, long k33, long k32, long remain)> results = new();
            var k32only = GetKxOnly(avaible, 32);
            var best = k32only.remain;
            results.Add((0, 0, k32only.count, k32only.remain));
            if (KbToGb(best) > 10)
            {
                var for34 = GetKxOnly(avaible, 34, K34count);
                var for33 = GetKxOnly(for34.remain, 33, K33count);
                var for32 = GetKxOnly(for33.remain, 32, K32count);
                var remain = avaible - (for34.count * K34 + for33.count * K33 + for32.count * K32);
                if (remain > 0)
                {
                    results.Add((for34.count, for33.count, for32.count, remain));
                    best = remain;
                }

                // remove k32
                for (int i34 = 0; i34 <= K34count; i34++)
                {
                    for (int i33 = 0; i33 <= K33count; i33++)
                    {
                        for (int i32 = 0; i32 < 10; i32++)
                        {
                            remain = avaible + (K32 * i32);
                            if (remain > totalSize)
                            {
                                break;
                            }
                            for34 = GetKxOnly(remain, 34, i34);
                            for33 = GetKxOnly(for34.remain, 33, i33);
                            for32 = GetKxOnly(for33.remain, 32, K32count);
                            remain -= for34.count * K34 + for33.count * K33 + for32.count * K32;
                            if (remain > 0 && remain < avaible)
                            {
                                results.Add((for34.count, for33.count, for32.count - i32, remain));
                                best = remain;
                            }
                        }
                    }
                }
            }

            var list = results
                .Distinct()
                .OrderBy(r => r.remain)
                .Select(r => $"k34: {r.k34}   k33: {r.k33}   k32: {r.k32}  |  remain {KbToGb(r.remain)} GB");
            return string.Join(Environment.NewLine, list);
        }

        private static (long remain, long count) GetKxOnly(long avaible, long K, long limit = -1)
        {
            var kx = K switch
            {
                33 => K33,
                34 => K34,
                _ => K32,
            };
            var count = avaible / kx;
            var remain = avaible % kx;
            if (limit != -1 && count > limit)
            {
                count = limit;
                remain = avaible - limit * kx;
            }
            return (remain, count);
        }

        public static int KbToGb(long bytes)
        {
            return (int)(bytes / 1024 / 1024);
        }
    }
}
