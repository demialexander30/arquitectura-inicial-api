using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SalesOnline.WinForm
{
    public partial class MainForm : Form
    {
        private List<Flight> flights = new List<Flight>();

        public MainForm()
        {
            InitializeComponent();
            LoadFlights();
        }

        private void LoadFlights()
        {
            flights.Add(new Flight(1, "AA101", "New York", "Los Angeles"));
            flights.Add(new Flight(2, "BB202", "London", "Paris"));
            UpdateFlightList();
        }

        private void UpdateFlightList()
        {
            flightListBox.Items.Clear();
            foreach (var flight in flights)
            {
                flightListBox.Items.Add(flight);
            }
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            var flightForm = new FlightForm();
            if (flightForm.ShowDialog() == DialogResult.OK)
            {
                flights.Add(flightForm.FlightData);
                UpdateFlightList();
            }
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            if (flightListBox.SelectedItem is Flight selectedFlight)
            {
                var flightForm = new FlightForm(selectedFlight);
                if (flightForm.ShowDialog() == DialogResult.OK)
                {
                    int index = flights.FindIndex(f => f.Id == selectedFlight.Id);
                    flights[index] = flightForm.FlightData;
                    UpdateFlightList();
                }
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (flightListBox.SelectedItem is Flight selectedFlight)
            {
                var result = MessageBox.Show("Are you sure you want to delete this flight?", "Confirm", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    flights.Remove(selectedFlight);
                    UpdateFlightList();
                }
            }
        }
    }

    public class Flight
    {
        public int Id { get; }
        public string Number { get; }
        public string Origin { get; }
        public string Destination { get; }

        public Flight(int id, string number, string origin, string destination)
        {
            Id = id;
            Number = number;
            Origin = origin;
            Destination = destination;
        }

        public override string ToString()
        {
            return $"{Number} - {Origin} to {Destination}";
        }
    }
}
