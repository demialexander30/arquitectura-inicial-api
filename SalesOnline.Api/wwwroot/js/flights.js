// DOM Elements
const flightModal = new bootstrap.Modal(document.getElementById('flightModal'));
const flightForm = document.getElementById('flightForm');
const flightId = document.getElementById('flightId');
const flightNumber = document.getElementById('flightNumber');
const origin = document.getElementById('origin');
const destination = document.getElementById('destination');
const modalTitle = document.getElementById('modalTitle');
const saveButton = document.getElementById('saveFlight');
const flightsTableBody = document.getElementById('flightsTableBody');

// Load flights when page loads
document.addEventListener('DOMContentLoaded', loadFlights);

// Event listeners
saveButton.addEventListener('click', saveFlight);

// Load all flights
async function loadFlights() {
    try {
        const response = await fetch('/api/flight');
        const flights = await response.json();
        displayFlights(flights);
    } catch (error) {
        console.error('Error loading flights:', error);
        alert('Error loading flights. Please try again.');
    }
}

// Display flights in table
function displayFlights(flights) {
    flightsTableBody.innerHTML = '';
    flights.forEach(flight => {
        const row = document.createElement('tr');
        row.innerHTML = `
            <td>${flight.flightNumber}</td>
            <td>${flight.origin}</td>
            <td>${flight.destination}</td>
            <td>
                <button class="btn btn-sm btn-primary" onclick="editFlight(${flight.id})">Edit</button>
                <button class="btn btn-sm btn-danger" onclick="deleteFlight(${flight.id})">Delete</button>
            </td>
        `;
        flightsTableBody.appendChild(row);
    });
}

// Show modal for adding new flight
function showAddFlightModal() {
    modalTitle.textContent = 'Add New Flight';
    flightForm.reset();
    flightId.value = '';
    flightModal.show();
}

// Load flight data for editing
async function editFlight(id) {
    try {
        const response = await fetch(`/api/flight/${id}`);
        const flight = await response.json();
        
        modalTitle.textContent = 'Edit Flight';
        flightId.value = flight.id;
        flightNumber.value = flight.flightNumber;
        origin.value = flight.origin;
        destination.value = flight.destination;
        
        flightModal.show();
    } catch (error) {
        console.error('Error loading flight:', error);
        alert('Error loading flight details. Please try again.');
    }
}

// Save or update flight
async function saveFlight() {
    const flight = {
        id: flightId.value ? parseInt(flightId.value) : 0,
        flightNumber: flightNumber.value,
        origin: origin.value,
        destination: destination.value
    };

    try {
        const url = flight.id ? `/api/flight/${flight.id}` : '/api/flight';
        const method = flight.id ? 'PUT' : 'POST';
        
        const response = await fetch(url, {
            method: method,
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(flight)
        });

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        flightModal.hide();
        loadFlights();
    } catch (error) {
        console.error('Error saving flight:', error);
        alert('Error saving flight. Please try again.');
    }
}

// Delete flight
async function deleteFlight(id) {
    if (!confirm('Are you sure you want to delete this flight?')) {
        return;
    }

    try {
        const response = await fetch(`/api/flight/${id}`, {
            method: 'DELETE'
        });

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        loadFlights();
    } catch (error) {
        console.error('Error deleting flight:', error);
        alert('Error deleting flight. Please try again.');
    }
}
