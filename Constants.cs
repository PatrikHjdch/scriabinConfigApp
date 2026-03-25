using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace scriabinWPF
{
    static class Constants
    {
        public const byte IN_HELLO = 0x01;
        public const byte IN_ACK = 0x02;
        public const byte IN_ERR = 0x03;
        public const byte IN_ERR_OUT_OF_ORDER = 0x01;
        public const byte IN_ERR_I2C = 0x02;

        public const byte IN_EVENT_MIDI_RECEIVED = 0x10;
        public const byte IN_EVENT_DMX_CHANGED = 0x11;
        public const byte IN_EVENT_PROFILE_CHANGED = 0x12;
        public const byte IN_EVENT_LINK_ADDRESSES_READ = 0x13;

        public static ReadOnlySet<byte> IN_EVENTS = new(
            new SortedSet<byte>
            {
                IN_EVENT_MIDI_RECEIVED,
                IN_EVENT_DMX_CHANGED,
                IN_EVENT_PROFILE_CHANGED,
                IN_EVENT_LINK_ADDRESSES_READ,
            }
        );

        public const byte OUT_HELLO = 0x01;
        public const byte OUT_ACK = 0x02;
        public const byte OUT_START_UPLOAD = 0x03;
        public const byte OUT_END_UPLOAD = 0x04;
        public const byte OUT_CHANNEL = 0x05;
        public const byte OUT_TYPE = 0x06;
        public const byte OUT_PITCH = 0x07;
        public const byte OUT_PROFILE = 0x08;
        public const byte OUT_ABORT = 0x10;

        public const string STR_ERR_OUT_OF_ORDER = "Links are being sent out of order.";
        public const string STR_ERR_I2C = "Error communicating with I2C memory.";

        public const byte TYPE_NOTE_LINK = 0x00;
        public const byte TYPE_NOTE_ON_LINK = 0x01;
        public const byte TYPE_NOTE_OFF_LINK = 0x02;
        public const byte TYPE_CONTROL_CHANGE_LINK = 0x03;

        #region MIDI
        public const byte MIDI_NOTE_OFF = 0b10000000;
        public const byte MIDI_NOTE_ON = 0b10010000;
        public const byte MIDI_POLY_AFTERTOUCH = 0b10100000;
        public const byte MIDI_CONTROL_CHANGE = 0b10110000;
        public const byte MIDI_PROGRAM_CHANGE = 0b11000000;
        public const byte MIDI_AFTERTOUCH = 0b11010000;
        public const byte MIDI_PITCH_BEND = 0b11100000;
        public const byte MIDI_SYSTEM_EXCLUSIVE = 0b11110000;
        public const byte MIDI_TIME_CODE_QUARTER_FRAME = 0b11110001;
        public const byte MIDI_SONG_POSITION_POINTER = 0b11110010;
        public const byte MIDI_SONG_SELECT = 0b11110011;
        public const byte MIDI_TUNE_REQUEST = 0b11110110;
        public const byte MIDI_END_OF_EXCLUSIVE = 0b11110111;
        public const byte MIDI_TIMING_CLOCK = 0b11111000;
        public const byte MIDI_START = 0b11111010;
        public const byte MIDI_CONTINUE = 0b11111011;
        public const byte MIDI_STOP = 0b11111100;
        public const byte MIDI_ACTIVE_SENSING = 0b11111110;
        public const byte MIDI_RESET = 0b11111111;

        public const byte MIDI_CHANNEL_MODE_ALL_SOUND_OFF = 120;
        public const byte MIDI_CHANNEL_MODE_RESET_ALL_CONTROLLERS = 121;
        public const byte MIDI_CHANNEL_MODE_LOCAL_CONTROL = 122;
        public const byte MIDI_CHANNEL_MODE_ALL_NOTES_OFF = 123;
        public const byte MIDI_CHANNEL_MODE_OMNI_MODE_OFF = 124;
        public const byte MIDI_CHANNEL_MODE_OMNI_MODE_ON = 125;
        public const byte MIDI_CHANNEL_MODE_MONO_MODE_ON = 126;
        public const byte MIDI_CHANNEL_MODE_POLY_MODE_ON = 127;

        public const byte MIDI_STATUS_BYTE_TYPE_MASK = 0b11110000;
        public const byte MIDI_STATUS_BYTE_CHANNEL_MASK = 0b00001111;

        public static ReadOnlySet<byte> MIDI_0_DATA_BYTE_MESSAGES = new(
            new SortedSet<byte>
            {
                MIDI_TUNE_REQUEST,
                MIDI_TIMING_CLOCK,
                MIDI_START,
                MIDI_CONTINUE,
                MIDI_STOP,
                MIDI_ACTIVE_SENSING,
                MIDI_RESET
            }
        );

        public static ReadOnlySet<byte> MIDI_1_DATA_BYTE_MESSAGES = new(
            new SortedSet<byte>
            {
                MIDI_PROGRAM_CHANGE,
                MIDI_POLY_AFTERTOUCH,
                MIDI_TIME_CODE_QUARTER_FRAME,
                MIDI_SONG_SELECT
            }
        );

        public static ReadOnlySet<byte> MIDI_2_DATA_BYTE_MESSAGES = new(
            new SortedSet<byte> {
                MIDI_NOTE_OFF,
                MIDI_NOTE_ON,
                MIDI_POLY_AFTERTOUCH,
                MIDI_CONTROL_CHANGE,
                MIDI_PITCH_BEND,
                MIDI_SONG_POSITION_POINTER
            }
        );
        #endregion

        public const string LOG_DIRECTORY = "%appdata%\\PatrikHjdch\\Scriabin\\logs\\";
    }
}
