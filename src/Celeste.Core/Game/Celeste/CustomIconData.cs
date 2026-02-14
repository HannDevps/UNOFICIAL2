using System;

namespace Celeste;

public static class CustomIconData
{
    private static readonly byte[] Magic = new byte[6] { (byte)'C', (byte)'I', (byte)'D', (byte)'A', (byte)'T', (byte)'1' };

    private static readonly byte[] Key = new byte[16]
    {
        0x39, 0xA1, 0x4C, 0xE2,
        0x17, 0xC7, 0x53, 0x9B,
        0x2D, 0x84, 0xF1, 0x6A,
        0xBE, 0x08, 0x77, 0xCD
    };

    public static byte[] DecodeIfNeeded(byte[] input)
    {
        if (!TryDecode(input, out byte[] decoded))
        {
            return input;
        }

        return decoded;
    }

    public static byte[] Encode(byte[] raw)
    {
        if (raw == null)
        {
            throw new ArgumentNullException(nameof(raw));
        }

        byte[] output = new byte[Magic.Length + 4 + raw.Length];
        Buffer.BlockCopy(Magic, 0, output, 0, Magic.Length);

        int lengthOffset = Magic.Length;
        output[lengthOffset] = (byte)(raw.Length & 0xFF);
        output[lengthOffset + 1] = (byte)((raw.Length >> 8) & 0xFF);
        output[lengthOffset + 2] = (byte)((raw.Length >> 16) & 0xFF);
        output[lengthOffset + 3] = (byte)((raw.Length >> 24) & 0xFF);

        int dataOffset = Magic.Length + 4;
        for (int i = 0; i < raw.Length; i++)
        {
            byte salt = (byte)((i * 31 + 17) & 0xFF);
            byte k = Key[i % Key.Length];
            output[dataOffset + i] = (byte)(raw[i] ^ k ^ salt);
        }

        return output;
    }

    private static bool TryDecode(byte[] input, out byte[] decoded)
    {
        decoded = Array.Empty<byte>();
        if (input == null || input.Length < Magic.Length + 4)
        {
            return false;
        }

        for (int i = 0; i < Magic.Length; i++)
        {
            if (input[i] != Magic[i])
            {
                return false;
            }
        }

        int lengthOffset = Magic.Length;
        int length = input[lengthOffset]
                     | (input[lengthOffset + 1] << 8)
                     | (input[lengthOffset + 2] << 16)
                     | (input[lengthOffset + 3] << 24);

        if (length < 0 || input.Length < Magic.Length + 4 + length)
        {
            return false;
        }

        decoded = new byte[length];
        int dataOffset = Magic.Length + 4;
        for (int i = 0; i < length; i++)
        {
            byte salt = (byte)((i * 31 + 17) & 0xFF);
            byte k = Key[i % Key.Length];
            decoded[i] = (byte)(input[dataOffset + i] ^ k ^ salt);
        }

        return true;
    }
}
