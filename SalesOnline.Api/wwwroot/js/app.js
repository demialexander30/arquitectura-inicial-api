// Variables globales
let currentTours = [];
let currentCustomerId = 1; // TODO: Implementar autenticación

// Funciones de utilidad
function formatDate(date) {
    return new Date(date).toLocaleDateString('es-ES', {
        year: 'numeric',
        month: 'long',
        day: 'numeric'
    });
}

function formatPrice(price) {
    return new Intl.NumberFormat('es-ES', { 
        style: 'currency', 
        currency: 'USD',
        minimumFractionDigits: 2,
        maximumFractionDigits: 2
    }).format(price);
}

// Mostrar mensaje de error o éxito
function showMessage(message, isError = false) {
    const div = document.createElement('div');
    div.className = `fixed top-4 right-4 p-4 rounded-lg ${isError ? 'bg-red-500' : 'bg-green-500'} text-white`;
    div.textContent = message;
    document.body.appendChild(div);
    setTimeout(() => div.remove(), 3000);
}

// Cargar tours
async function loadTours() {
    try {
        const response = await fetch('/api/Tours');
        if (!response.ok) {
            throw new Error('Error al cargar los tours');
        }
        const tours = await response.json();
        console.log('Tours cargados:', tours); // Para debugging
        currentTours = tours;
        displayTours(tours);
        updateTourSelect(); // Actualizar select de tours en el modal de reseñas
    } catch (error) {
        console.error('Error al cargar los tours:', error);
        document.getElementById('toursList').innerHTML = `
            <div class="col-span-full text-center text-red-600 p-4">
                <p class="text-xl">Error al cargar los tours</p>
                <p class="text-sm mt-2">Por favor, intenta nuevamente más tarde</p>
            </div>
        `;
        showMessage('Error al cargar los tours', true);
    }
}

// Mostrar tours en la página
function displayTours(tours) {
    const toursContainer = document.getElementById('toursList');
    console.log('Mostrando tours:', tours.length); // Para debugging
    
    if (!tours || tours.length === 0) {
        toursContainer.innerHTML = `
            <div class="col-span-full text-center text-gray-500 py-8">
                <p class="text-xl">No se encontraron tours disponibles</p>
                <p class="text-sm mt-2">Intenta con otros criterios de búsqueda</p>
            </div>
        `;
        return;
    }

    toursContainer.innerHTML = tours.map(tour => `
        <div class="bg-white rounded-lg shadow-lg overflow-hidden transform hover:scale-105 transition-transform duration-300">
            <img src="${tour.imageUrl || 'https://images.unsplash.com/photo-1469854523086-cc02fe5d8800'}" 
                 alt="${tour.name}" 
                 class="w-full h-48 object-cover"/>
            
            <div class="p-6">
                <div class="flex justify-between items-start mb-4">
                    <h3 class="text-xl font-bold text-gray-800">${tour.name}</h3>
                    <span class="bg-blue-100 text-blue-800 text-sm font-semibold px-3 py-1 rounded-full">
                        ${tour.duration} días
                    </span>
                </div>
                
                <p class="text-gray-600 mb-4">${tour.description}</p>
                
                <div class="space-y-2 mb-4">
                    <div class="flex items-center text-gray-700">
                        <svg class="w-5 h-5 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" 
                                  d="M17.657 16.657L13.414 20.9a1.998 1.998 0 01-2.827 0l-4.244-4.243a8 8 0 1111.314 0z"/>
                            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" 
                                  d="M15 11a3 3 0 11-6 0 3 3 0 016 0z"/>
                        </svg>
                        ${tour.destination}
                    </div>
                    
                    <div class="flex items-center text-gray-700">
                        <svg class="w-5 h-5 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" 
                                  d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z"/>
                        </svg>
                        ${formatDate(tour.startDate)} - ${formatDate(tour.endDate)}
                    </div>
                    
                    <div class="flex items-center text-gray-700">
                        <svg class="w-5 h-5 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" 
                                  d="M17 20h5v-2a3 3 0 00-5.356-1.857M17 20H7m10 0v-2c0-.656-.126-1.283-.356-1.857M7 20H2v-2a3 3 0 015.356-1.857M7 20v-2c0-.656.126-1.283.356-1.857m0 0a5.002 5.002 0 019.288 0M15 7a3 3 0 11-6 0 3 3 0 016 0zm6 3a2 2 0 11-4 0 2 2 0 014 0zM7 10a2 2 0 11-4 0 2 2 0 014 0z"/>
                        </svg>
                        Máximo ${tour.maxGroupSize} personas
                    </div>
                </div>

                <div class="flex justify-between items-center pt-4 border-t">
                    <div class="text-2xl font-bold text-blue-600">
                        ${formatPrice(tour.price)}
                    </div>
                    <button onclick="openReservationModal(${tour.id})" 
                            class="bg-blue-600 text-white px-6 py-2 rounded-lg hover:bg-blue-700 transition-colors">
                        Reservar
                    </button>
                </div>
            </div>
        </div>
    `).join('');
}

// Búsqueda de tours
document.getElementById('searchForm').addEventListener('submit', async (e) => {
    e.preventDefault();
    const destination = document.getElementById('destination').value.trim();
    const startDate = document.getElementById('startDate').value;
    const endDate = document.getElementById('endDate').value;

    try {
        let url = '/api/Tours';
        if (destination) {
            url = `/api/Tours/by-destination/${encodeURIComponent(destination)}`;
        }

        const response = await fetch(url);
        if (!response.ok) {
            throw new Error('Error en la búsqueda');
        }

        const tours = await response.json();
        console.log('Tours encontrados:', tours); // Para debugging
        
        // Filtrar por fechas si se especificaron
        let filteredTours = tours;
        if (startDate && endDate) {
            const start = new Date(startDate);
            const end = new Date(endDate);
            filteredTours = tours.filter(tour => {
                const tourStart = new Date(tour.startDate);
                const tourEnd = new Date(tour.endDate);
                return tourStart >= start && tourEnd <= end;
            });
        }
        
        displayTours(filteredTours);
    } catch (error) {
        console.error('Error en la búsqueda:', error);
        showMessage('Error al buscar tours', true);
    }
});

// Reservaciones
function openReservationModal(tourId) {
    const tour = currentTours.find(t => t.id === tourId);
    if (tour) {
        document.getElementById('tourId').value = tourId;
        document.getElementById('reservationDate').min = new Date().toISOString().split('T')[0];
        document.getElementById('reservationDate').value = tour.startDate.split('T')[0];
        document.getElementById('reservationModal').classList.remove('hidden');
    }
}

function closeReservationModal() {
    document.getElementById('reservationModal').classList.add('hidden');
    document.getElementById('reservationForm').reset();
}

// Reseñas
async function loadReviews() {
    try {
        const response = await fetch('/api/Reviews');
        if (!response.ok) {
            throw new Error('Error al cargar las reseñas');
        }
        const reviews = await response.json();
        displayReviews(reviews);
    } catch (error) {
        console.error('Error al cargar las reseñas:', error);
        document.getElementById('reviewsList').innerHTML = `
            <div class="col-span-full text-center text-red-600">
                <p>Error al cargar las reseñas</p>
            </div>
        `;
    }
}

function displayReviews(reviews) {
    const reviewsContainer = document.getElementById('reviewsList');
    
    if (!reviews || reviews.length === 0) {
        reviewsContainer.innerHTML = `
            <div class="col-span-full text-center text-gray-500">
                <p>No hay reseñas disponibles</p>
            </div>
        `;
        return;
    }

    reviewsContainer.innerHTML = reviews.map(review => `
        <div class="bg-white p-6 rounded-lg shadow">
            <div class="flex items-center mb-4">
                <div class="flex-1">
                    <h4 class="font-bold">${review.customerName}</h4>
                    <p class="text-sm text-gray-600">${formatDate(review.date)}</p>
                </div>
                <div class="flex">
                    ${'★'.repeat(review.rating)}${'☆'.repeat(5-review.rating)}
                </div>
            </div>
            <p class="text-gray-700">${review.comment}</p>
            <p class="text-sm text-blue-600 mt-2">Tour: ${review.tourName}</p>
        </div>
    `).join('');
}

function openReviewModal() {
    updateTourSelect();
    document.getElementById('reviewModal').classList.remove('hidden');
}

function closeReviewModal() {
    document.getElementById('reviewModal').classList.add('hidden');
    document.getElementById('reviewForm').reset();
}

function updateTourSelect() {
    const select = document.getElementById('tourSelect');
    select.innerHTML = currentTours.map(tour => 
        `<option value="${tour.id}">${tour.name}</option>`
    ).join('');
}

// Actualizar contador de caracteres del comentario
document.getElementById('reviewComment').addEventListener('input', function() {
    const length = this.value.length;
    document.getElementById('commentLength').textContent = length;
});

// Formularios
document.getElementById('reservationForm').addEventListener('submit', async (e) => {
    e.preventDefault();
    const tourId = document.getElementById('tourId').value;
    const reservationDate = document.getElementById('reservationDate').value;
    const numberOfPersons = document.getElementById('numberOfPeople').value;
    const specialRequirements = document.getElementById('specialRequirements').value;

    try {
        const response = await fetch('/api/Reservation', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                tourId: parseInt(tourId),
                customerId: currentCustomerId,
                reservationDate: reservationDate,
                numberOfPersons: parseInt(numberOfPersons),
                specialRequirements: specialRequirements
            })
        });

        if (!response.ok) {
            const errorText = await response.text();
            throw new Error(errorText);
        }

        closeReservationModal();
        showMessage('¡Reservación creada exitosamente!');
    } catch (error) {
        console.error('Error al crear la reservación:', error);
        showMessage('Error al crear la reservación: ' + error.message, true);
    }
});

document.getElementById('reviewForm').addEventListener('submit', async (e) => {
    e.preventDefault();

    // Validar que se haya seleccionado un tour
    const tourId = document.getElementById('tourSelect').value;
    if (!tourId) {
        showMessage('Por favor selecciona un tour', true);
        return;
    }

    // Validar que se haya seleccionado una calificación
    const rating = document.querySelector('input[name="rating"]:checked');
    if (!rating) {
        showMessage('Por favor selecciona una calificación', true);
        return;
    }

    // Validar el comentario
    const comment = document.getElementById('reviewComment').value.trim();
    if (!comment) {
        showMessage('Por favor escribe un comentario', true);
        return;
    }
    if (comment.length > 1000) {
        showMessage('El comentario no puede exceder los 1000 caracteres', true);
        return;
    }

    try {
        const response = await fetch('/api/Reviews', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                tourId: parseInt(tourId),
                customerId: currentCustomerId,
                rating: parseInt(rating.value),
                comment: comment,
                reviewDate: new Date().toISOString(),
                isVerified: false
            })
        });

        if (!response.ok) {
            const errorText = await response.text();
            throw new Error(errorText);
        }

        closeReviewModal();
        showMessage('¡Reseña enviada exitosamente!');
        loadReviews(); // Recargar las reseñas
    } catch (error) {
        console.error('Error al enviar la reseña:', error);
        showMessage('Error al enviar la reseña: ' + error.message, true);
    }
});

document.getElementById('contactForm').addEventListener('submit', async (e) => {
    e.preventDefault();
    const name = document.getElementById('name').value;
    const email = document.getElementById('email').value;
    const message = document.getElementById('message').value;

    try {
        // Aquí normalmente enviarías el mensaje a un endpoint
        showMessage('¡Mensaje enviado exitosamente! Te contactaremos pronto.');
        document.getElementById('contactForm').reset();
    } catch (error) {
        console.error('Error al enviar el mensaje:', error);
        showMessage('Error al enviar el mensaje: ' + error.message, true);
    }
});

// Inicialización
document.addEventListener('DOMContentLoaded', () => {
    console.log('Iniciando aplicación...'); // Para debugging
    loadTours();
    loadReviews();
});
