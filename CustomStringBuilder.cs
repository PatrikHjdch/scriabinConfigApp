using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace scriabinWPF
{
    static class CustomStringBuilder
    {
        public static string GetErrorMessage(byte[] bytes)
        {
            string message;
            message = "Received error from device: ";
            switch (bytes[1])
            {
                case Constants.IN_ERR_I2C:
                    message += "I2C error: " + bytes[2];
                    break;
                case Constants.IN_ERR_OUT_OF_ORDER:
                    message += "Links out of order.";
                    break;
                default:
                    message += "Unknown error:";
                    for (int i = 1; i < bytes.Length; i++)
                    {
                        message += " " + bytes[i];
                    }
                    break;

                }
            return message;
        }
        public static string GetChannelModeMessage(byte[] dataBytes)
        {
            switch (dataBytes[0])
            {
                case Constants.MIDI_CHANNEL_MODE_ALL_SOUND_OFF:
                    return "All Sound Off (" + dataBytes[1].ToString() + ")";
                case Constants.MIDI_CHANNEL_MODE_RESET_ALL_CONTROLLERS:
                    return "Reset All Controllers (" + dataBytes[1].ToString() + ")";
                case Constants.MIDI_CHANNEL_MODE_LOCAL_CONTROL:
                    return "Local Control (" + dataBytes[1].ToString() + (dataBytes[1] == 0 ? " - OFF)" : dataBytes[1] == 127 ? " - ON)" : ")");
                case Constants.MIDI_CHANNEL_MODE_ALL_NOTES_OFF:
                    return "All Notes Off (" + dataBytes[1].ToString() + ")";
                case Constants.MIDI_CHANNEL_MODE_OMNI_MODE_OFF:
                    return "Omni Mode Off (" + dataBytes[1].ToString() + ")";
                case Constants.MIDI_CHANNEL_MODE_OMNI_MODE_ON:
                    return "Omni Mode On (" + dataBytes[1].ToString() + ")";
                case Constants.MIDI_CHANNEL_MODE_MONO_MODE_ON:
                    return "Mono Mode On (" + dataBytes[1].ToString() + ")";
                case Constants.MIDI_CHANNEL_MODE_POLY_MODE_ON:
                    return "Poly Mode On (" + dataBytes[1].ToString() + ")";
                default:
                    return "Unknown";
            }
        }
        public static string ParseMidiMessage(byte[] msg, int startPos)
        {
            if (msg.Length <= startPos) { return "?"; }
            if (msg[startPos] >= 0b11111000)
            {
                switch (msg[startPos])
                {
                    case Constants.MIDI_TIMING_CLOCK:
                        return "Timing Clock";
                    case Constants.MIDI_START:
                        return "Start";
                    case Constants.MIDI_CONTINUE:
                        return "Continue";
                    case Constants.MIDI_STOP:
                        return "Stop";
                    case Constants.MIDI_ACTIVE_SENSING:
                        return "Active Sensing";
                    case Constants.MIDI_RESET:
                        return "Reset";
                }
            }
            if (msg.Length > startPos + 1)
            {
                if (msg[startPos] < Constants.MIDI_SYSTEM_EXCLUSIVE)
                {
                    switch (msg[startPos] & Constants.MIDI_STATUS_BYTE_TYPE_MASK)
                    {
                        case Constants.MIDI_PROGRAM_CHANGE:
                            return "Program Change: " + msg[startPos + 1].ToString();
                        case Constants.MIDI_AFTERTOUCH:
                            return "Aftertouch: " + msg[startPos + 1].ToString();
                    }
                }
                else
                {
                    switch (msg[startPos])
                    {
                        case Constants.MIDI_TIME_CODE_QUARTER_FRAME:
                            return "Quarter Frame: " + (msg[startPos + 1] >> 3).ToString() + " " + (msg[startPos + 1] & 0b00001111).ToString();
                        case Constants.MIDI_SONG_SELECT:
                            return "Song Select: " + msg[startPos + 1].ToString();
                    }
                }
            }
            if (msg.Length > startPos + 2)
            {
                if (msg[startPos] < Constants.MIDI_SYSTEM_EXCLUSIVE)
                {
                    switch (msg[startPos] & Constants.MIDI_STATUS_BYTE_TYPE_MASK)
                    {
                        case Constants.MIDI_NOTE_OFF:
                            return "Note Off: { " +
                                "Channel: " + ((msg[startPos] & Constants.MIDI_STATUS_BYTE_CHANNEL_MASK) + 1).ToString() +
                                ", Pitch: " + msg[startPos + 1].ToString() +
                                ", Velocity: " + msg[startPos + 2].ToString() +
                                " }";
                        case Constants.MIDI_NOTE_ON:
                            return "Note On: { " +
                                "Channel: " + ((msg[startPos] & Constants.MIDI_STATUS_BYTE_CHANNEL_MASK) + 1).ToString() +
                                ", Pitch: " + msg[startPos + 1].ToString() +
                                ", Velocity: " + msg[startPos + 2].ToString() +
                                " }";
                        case Constants.MIDI_POLY_AFTERTOUCH:
                            return "Poly Aftertouch: { " +
                                "Channel: " + ((msg[startPos] & Constants.MIDI_STATUS_BYTE_CHANNEL_MASK) + 1).ToString() +
                                ", Pitch: " + msg[startPos + 1].ToString() +
                                ", Pressure: " + msg[startPos + 2].ToString() +
                                " }";
                        case Constants.MIDI_CONTROL_CHANGE:
                            if (msg[startPos + 1] < 120)
                            {
                                return "Control Change: { " +
                                    "Channel: " + ((msg[startPos] & Constants.MIDI_STATUS_BYTE_CHANNEL_MASK) + 1).ToString() +
                                    ", Controller: " + msg[startPos + 1].ToString() +
                                    ", Value: " + msg[startPos + 2].ToString() +
                                    " }";
                            }
                            else
                            {
                                return "Channel Mode: { " +
                                    "Channel: " + ((msg[startPos] & Constants.MIDI_STATUS_BYTE_CHANNEL_MASK) + 1).ToString() +
                                    ", " + GetChannelModeMessage([msg[startPos + 1], msg[startPos + 2]]) +
                                    " }";
                            }
                        case Constants.MIDI_PITCH_BEND:
                            return "Pitch Bend: { " +
                                "Channel: " + ((msg[startPos] & Constants.MIDI_STATUS_BYTE_CHANNEL_MASK) + 1).ToString() +
                                ", Value: " + (msg[startPos + 2] << 7 + msg[startPos + 1]).ToString() +
                                " }";
                    }
                }
                else
                {
                    switch (msg[startPos])
                    {
                        case Constants.MIDI_SONG_POSITION_POINTER:
                            return "Song Position Pointer: { " +
                                "Value: " + (msg[startPos + 2] << 7 + msg[startPos + 1]).ToString() +
                                " }";
                    }
                }
            }
            string s = "";
            if (msg[startPos] == Constants.MIDI_SYSTEM_EXCLUSIVE)
            {
                bool ended = false;
                s = "SysEx: {";
                for (int i = startPos + 1; i < msg.Length; i++)
                {
                    if (msg[i] != Constants.MIDI_END_OF_EXCLUSIVE)
                    {
                        ended = true;
                        break;
                    }
                    else
                    {
                        s += " " + msg[i].ToString();
                    }
                }
                if (!ended) s += " [UNFINISHED]";
                s += " }";
                return s;
            }
            for (int i = startPos; i < msg.Length; i++)
            {
                s += msg[i].ToString() + " ";
            }
            return s;
        }

        public static string ParseMidiMessage(byte[] msg)
        {
            return ParseMidiMessage(msg, 0);
        }

    }
}
