using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Sanford.Multimedia.Midi;

namespace midiRemapCmdPL1
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private Sanford.Multimedia.Midi.InputDevice inputDevice;
        private Sanford.Multimedia.Midi.OutputDevice outputDevice;
        private Sanford.Multimedia.Midi.InputDevice loopInputDevice;
        private Sanford.Multimedia.Midi.OutputDevice loopOutputDevice;
        private int inputDeviceCount = 0;
        private int outputDeviceCount = 0;
        private String inputLog = String.Empty;
        private String outputLog = String.Empty;

        public MainWindow()
        {
            InitializeComponent();

            // check input devices
            inputDeviceCount = Sanford.Multimedia.Midi.InputDevice.DeviceCount;
            if (inputDeviceCount > 0)
            {
                for (int i = 0; i < inputDeviceCount; i++)
                {
                    MidiInCaps devCaps = Sanford.Multimedia.Midi.InputDevice.GetDeviceCapabilities(i);                        
                    inputTextBlock.Text += "\n InputDevice " + i + " " + devCaps.name + " Support: " + devCaps.support;

                    if (devCaps.name == "CMD PL-1")
                    {
                        inputDevice = new Sanford.Multimedia.Midi.InputDevice(i);
                        inputTextBlock.Text += "\n CMD-PL1 found and connected.";
                        inputDevice.ChannelMessageReceived += delegate (object sender, ChannelMessageEventArgs e)
                        {
                            showInputMessage(e);
                        };
                        inputDevice.StartRecording();
                    }
                }
            }
            else
            {
                inputTextBlock.Text += "\n Error! no input devices found.";              
            }

            // check ouput devices
            outputDeviceCount = Sanford.Multimedia.Midi.OutputDevice.DeviceCount;
            if (outputDeviceCount > 0)
            {
                for (int i = 0; i < outputDeviceCount; i++)
                {
                    MidiOutCaps devCaps = Sanford.Multimedia.Midi.OutputDevice.GetDeviceCapabilities(i);
                    outputTextBlock.Text += "\n OutputDevice " + i + " " + devCaps.name + " Support: " + devCaps.support;
                }
            }
            else
            {
                inputTextBlock.Text += "\n Error! no output devices found.";
            }
        }

        public void showInputMessage(ChannelMessageEventArgs e)
        {
            inputTextBlock.Text += "\n Chan: " + e.Message.MidiChannel + "Msg: " + e.Message.Command + " " + e.Message.Data1 + " " + e.Message.Data2;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                inputDevice.Close();
            }
            catch
            {
                //nothing
            }
        }
    }
}
