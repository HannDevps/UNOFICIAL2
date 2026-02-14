using System;
using System.IO;

if (args.Length == 0)
{
    Console.Error.WriteLine("Usage: IconDatPacker <dir> [<dir> ...]");
    Environment.Exit(1);
}

int generated = 0;
for (int i = 0; i < args.Length; i++)
{
    string root = args[i];
    if (!Directory.Exists(root))
    {
        Console.WriteLine($"[skip] missing directory: {root}");
        continue;
    }

    string[] pngFiles = Directory.GetFiles(root, "*.png", SearchOption.TopDirectoryOnly);
    Array.Sort(pngFiles, StringComparer.OrdinalIgnoreCase);

    for (int j = 0; j < pngFiles.Length; j++)
    {
        string pngPath = pngFiles[j];
        string datPath = Path.ChangeExtension(pngPath, ".dat");
        byte[] raw = File.ReadAllBytes(pngPath);
        byte[] packed = Encode(raw);
        File.WriteAllBytes(datPath, packed);
        generated++;
        Console.WriteLine($"[ok] {datPath}");
    }
}

Console.WriteLine($"generated: {generated}");

static byte[] Encode(byte[] raw)
{
    byte[] magic = new byte[6] { (byte)'C', (byte)'I', (byte)'D', (byte)'A', (byte)'T', (byte)'1' };
    byte[] key = new byte[16]
    {
        0x39, 0xA1, 0x4C, 0xE2,
        0x17, 0xC7, 0x53, 0x9B,
        0x2D, 0x84, 0xF1, 0x6A,
        0xBE, 0x08, 0x77, 0xCD
    };

    byte[] output = new byte[magic.Length + 4 + raw.Length];
    Buffer.BlockCopy(magic, 0, output, 0, magic.Length);

    int len = raw.Length;
    int lengthOffset = magic.Length;
    output[lengthOffset] = (byte)(len & 0xFF);
    output[lengthOffset + 1] = (byte)((len >> 8) & 0xFF);
    output[lengthOffset + 2] = (byte)((len >> 16) & 0xFF);
    output[lengthOffset + 3] = (byte)((len >> 24) & 0xFF);

    int dataOffset = magic.Length + 4;
    for (int i = 0; i < raw.Length; i++)
    {
        byte salt = (byte)((i * 31 + 17) & 0xFF);
        byte k = key[i % key.Length];
        output[dataOffset + i] = (byte)(raw[i] ^ k ^ salt);
    }

    return output;
}
