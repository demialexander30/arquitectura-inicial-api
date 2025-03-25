document.addEventListener('DOMContentLoaded', function() {
    loadTours();
    setupEventListeners();
});

function setupEventListeners() {
    document.getElementById('saveTour').addEventListener('click', saveTour);
    document.getElementById('showAvailable').addEventListener('click', loadAvailableTours);
    document.getElementById('searchPrice').addEventListener('click', searchByPriceRange);
    document.getElementById('destinationSearch').addEventListener('input', debounce(searchByDestination, 500));
}

function debounce(func, wait) {
    let timeout;
    return function executedFunction(...args) {
        const later = () => {
            clearTimeout(timeout);
            func(...args);
        };
        clearTimeout(timeout);
        timeout = setTimeout(later, wait);
    };
}

async function loadTours() {
    try {
        const response = await fetch('/api/tours');
        const tours = await response.json();
        displayTours(tours);
    } catch (error) {
        console.error('Error loading tours:', error);
        alert('Error al cargar los tours');
    }
}

async function loadAvailableTours() {
    try {
        const response = await fetch('/api/tours/available');
        const tours = await response.json();
        displayTours(tours);
    } catch (error) {
        console.error('Error loading available tours:', error);
        alert('Error al cargar los tours disponibles');
    }
}

async function searchByDestination(event) {
    const destination = event.target.value;
    if (destination.length < 2) {
        loadTours();
        return;
    }

    try {
        const response = await fetch(`/api/tours/by-destination/${destination}`);
        const tours = await response.json();
        displayTours(tours);
    } catch (error) {
        console.error('Error searching tours by destination:', error);
        alert('Error al buscar tours por destino');
    }
}

async function searchByPriceRange() {
    const minPrice = document.getElementById('minPrice').value;
    const maxPrice = document.getElementById('maxPrice').value;

    try {
        const response = await fetch(`/api/tours/by-price-range?minPrice=${minPrice}&maxPrice=${maxPrice}`);
        const tours = await response.json();
        displayTours(tours);
    } catch (error) {
        console.error('Error searching tours by price range:', error);
        alert('Error al buscar tours por rango de precio');
    }
}

function displayTours(tours) {
    const grid = document.getElementById('toursGrid');
    grid.innerHTML = '';

    tours.forEach(tour => {
        const card = document.createElement('div');
        card.className = 'col-md-4 tour-card';
        card.innerHTML = `
            <div class="card">
                <img src="${tour.imageUrl || 'https://via.placeholder.com/300x200'}" class="card-img-top tour-image" alt="${tour.name}">
                <div class="card-body">
                    <h5 class="card-title">${tour.name}</h5>
                    <p class="card-text">${tour.description}</p>
                    <p class="card-text">
                        <small class="text-muted">
                            Destino: ${tour.destination}<br>
                            Precio: $${tour.price}<br>
                            Duración: ${tour.duration} días<br>
                            Estado: ${tour.isAvailable ? 'Disponible' : 'No disponible'}
                        </small>
                    </p>
                    <div class="btn-group">
                        <button onclick="editTour(${tour.id})" class="btn btn-primary btn-sm">Editar</button>
                        <button onclick="deleteTour(${tour.id})" class="btn btn-danger btn-sm">Eliminar</button>
                    </div>
                </div>
            </div>
        `;
        grid.appendChild(card);
    });
}

async function saveTour() {
    const tourData = {
        id: document.getElementById('tourId').value || 0,
        name: document.getElementById('name').value,
        description: document.getElementById('description').value,
        price: parseFloat(document.getElementById('price').value),
        destination: document.getElementById('destination').value,
        duration: parseInt(document.getElementById('duration').value),
        maxGroupSize: parseInt(document.getElementById('maxGroupSize').value),
        startDate: document.getElementById('startDate').value,
        endDate: document.getElementById('endDate').value,
        imageUrl: document.getElementById('imageUrl').value,
        isAvailable: document.getElementById('isAvailable').checked
    };

    try {
        const url = tourData.id ? `/api/tours/${tourData.id}` : '/api/tours';
        const method = tourData.id ? 'PUT' : 'POST';

        const response = await fetch(url, {
            method: method,
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(tourData)
        });

        if (response.ok) {
            bootstrap.Modal.getInstance(document.getElementById('tourModal')).hide();
            loadTours();
            clearForm();
        } else {
            throw new Error('Error al guardar el tour');
        }
    } catch (error) {
        console.error('Error saving tour:', error);
        alert('Error al guardar el tour');
    }
}

async function editTour(id) {
    try {
        const response = await fetch(`/api/tours/${id}`);
        const tour = await response.json();

        document.getElementById('tourId').value = tour.id;
        document.getElementById('name').value = tour.name;
        document.getElementById('description').value = tour.description;
        document.getElementById('price').value = tour.price;
        document.getElementById('destination').value = tour.destination;
        document.getElementById('duration').value = tour.duration;
        document.getElementById('maxGroupSize').value = tour.maxGroupSize;
        document.getElementById('startDate').value = tour.startDate.split('T')[0];
        document.getElementById('endDate').value = tour.endDate.split('T')[0];
        document.getElementById('imageUrl').value = tour.imageUrl;
        document.getElementById('isAvailable').checked = tour.isAvailable;

        document.getElementById('modalTitle').textContent = 'Editar Tour';
        const modal = new bootstrap.Modal(document.getElementById('tourModal'));
        modal.show();
    } catch (error) {
        console.error('Error loading tour for edit:', error);
        alert('Error al cargar el tour para editar');
    }
}

async function deleteTour(id) {
    if (!confirm('¿Está seguro de que desea eliminar este tour?')) {
        return;
    }

    try {
        const response = await fetch(`/api/tours/${id}`, {
            method: 'DELETE'
        });

        if (response.ok) {
            loadTours();
        } else {
            throw new Error('Error al eliminar el tour');
        }
    } catch (error) {
        console.error('Error deleting tour:', error);
        alert('Error al eliminar el tour');
    }
}

function clearForm() {
    document.getElementById('tourForm').reset();
    document.getElementById('tourId').value = '';
    document.getElementById('modalTitle').textContent = 'Agregar Nuevo Tour';
}
