// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using UnityEngine;
/// <summary>
/// Extensions for the color class
/// </summary>
public static class ColorExtensions
{
    /// <summary>
    /// Convert a color to a hex value.
    /// </summary>
    /// <param name="color">The color that will be converted</param>
    /// <returns>The hex value</returns>
    public static string ColorToHex(this Color color)
    {
        string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
        return hex;
    }

    /// <summary>
    /// Convert a hex value to a color.
    /// </summary>
    /// <param name="hex">The hex value that will be converted</param>
    /// <returns>The color.</returns>
    public static Color HexToColor(this string hex)
    {
        hex = hex.Replace("0x", ""); //in case the string is formatted 0xFFFFFF
        hex = hex.Replace("#", ""); //in case the string is formatted #FFFFFF
        byte a = 255; //assume fully visible unless specified in hex
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

        if (hex.Length == 8)
        {
            a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
        }
        return new Color(r, g, b, a);
    }
}