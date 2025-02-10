using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.NetworkInformation;

namespace PingRequestForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string host = textBox2?.Text.Trim(); // Ensure textBox2 is not null

            if (string.IsNullOrEmpty(host))
            {
                MessageBox.Show("Please enter an IP address or domain name.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (Ping pingSender = new Ping()) // Properly instantiate Ping object
                {
                    int packetsSent = 4; // Send 4 requests like cmd
                    int packetsReceived = 0;
                    int packetsLost;
                    long totalTime = 0;
                    string output = "";

                    for (int i = 0; i < packetsSent; i++)
                    {
                        PingReply reply = pingSender.Send(host, 1000); // Timeout 1 second

                        if (reply != null && reply.Status == IPStatus.Success)
                        {
                            packetsReceived++;
                            totalTime += reply.RoundtripTime;

                            output += $"Reply from {reply.Address}: bytes={reply.Buffer.Length} time={reply.RoundtripTime}ms TTL={reply.Options?.Ttl ?? -1}\r\n";
                        }
                        else
                        {
                            output += $"Request timed out.\r\n";
                        }

                        System.Threading.Thread.Sleep(500); // Pause for readability
                    }

                    packetsLost = packetsSent - packetsReceived;
                    double packetLossPercentage = ((double)packetsLost / packetsSent) * 100;

                    output += $"\r\nPing statistics for {host}:\r\n" +
                              $"    Packets: Sent = {packetsSent}, Received = {packetsReceived}, Lost = {packetsLost} ({packetLossPercentage}% loss),\r\n" +
                              $"Approximate round trip times:\r\n" +
                              $"    Minimum = {totalTime / packetsSent}ms, Maximum = {totalTime / packetsReceived}ms, Average = {totalTime / packetsSent}ms";

                    textBox1.Text = output;
                }
            }
            catch (PingException ex)
            {
                MessageBox.Show("Ping request failed: " + ex.Message, "Ping Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unexpected error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
