using System;
using System.Windows.Forms;

namespace SalesOnline.WinForm
{
    public partial class FlightForm : Form
    {
        public Flight FlightData { get; private set; }

        public FlightForm(Flight flight = null)
        {
            InitializeComponent();
            if (flight != null)
            {
                numberTextBox.Text = flight.Number;
                originTextBox.Text = flight.Origin;
                destinationTextBox.Text = flight.Destination;
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(numberTextBox.Text) ||
                string.IsNullOrWhiteSpace(originTextBox.Text) ||
                string.IsNullOrWhiteSpace(destinationTextBox.Text))
            {
                MessageBox.Show("Please fill in all fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            FlightData = new Flight(
                new Random().Next(1000), // Generate random ID for demo
                numberTextBox.Text,
                originTextBox.Text,
                destinationTextBox.Text
            );

            DialogResult = DialogResult.OK;
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
