function openManageTicketsModal(cinemaId, cinemaName, cinemaLocation) {
    fetch(`/Manager/api/TicketApi/GetMoviesByCinema/${cinemaId}`, {
        method: 'GET',
        credentials: 'include'
    })
        .then(response => {
            if (!response.ok) {
                throw new Error("Failed to load movies.");
            }
            return response.json();
        })
        .then(movies => {
            renderMoviesInModal(movies, cinemaName, cinemaLocation);
            $('#manageTicketsModal').modal('show');
        })
        .catch(error => {
            console.error("Error loading movies:", error);
            alert("An error occurred while loading movies.");
        });
}

function renderMoviesInModal(movies, cinemaName, cinemaLocation) {
    let modalHtml = `
        <div id="manageTicketsModal" class="modal fade" tabindex="-1" aria-labelledby="manageTicketsModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content bg-dark text-white">
                    <div class="modal-header border-0">
                        <h5 class="modal-title fw-bold">
                            <i class="bi bi-film"></i> ${cinemaName} - <span class="text-muted">${cinemaLocation}</span>
                        </h5>
                        <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">`;

    if (movies.length === 0) {
        modalHtml += `<p class="text-center text-warning">No movies are included in the cinema program.</p>`;
    } else {
        modalHtml += `
            <div class="table-responsive">
                <table class="table table-dark table-striped text-center">
                    <thead class="table-warning text-dark">
                        <tr>
                            <th>Movie Title</th>
                            <th>Available Tickets</th>
                            <th>Action</th>
                        </tr>
                    </thead>
                    <tbody id="moviesListContainer">`;

        movies.forEach(movie => {
            modalHtml += `
                <tr>
                    <td class="fw-bold">${movie.title}</td>
                    <td>
                        <input type="number" id="availableTickets-${movie.id}" 
                               value="${movie.availableTickets}" min="0" 
                               class="form-control text-center bg-secondary text-white" />
                    </td>
                    <td>
                        <button class="btn btn-warning btn-sm" onclick="updateAvailableTickets('${movie.id}', '${movie.cinemaId}')">
                            <i class="bi bi-save"></i> Update
                        </button>
                    </td>
                </tr>`;
        });

        modalHtml += `</tbody></table></div>`;
    }

    modalHtml += `
                    </div>
                    <div class="modal-footer border-0">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>`;

    document.getElementById("manageTicketsModalContainer").innerHTML = modalHtml;
    $('#manageTicketsModal').modal('show');
}

function updateAvailableTickets(movieId, cinemaId) {
    const availableTickets = document.getElementById(`availableTickets-${movieId}`).value;

    fetch('/api/TicketApi/UpdateAvailableTickets', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
            CinemaId: cinemaId,
            MovieId: movieId,
            AvailableTickets: parseInt(availableTickets, 10)
        })
    })
        .then(response => {
            if (!response.ok) throw new Error("Failed to update tickets.");
            alert("Ticket availability updated successfully.");
        })
        .catch(error => {
            console.error("Error:", error);
            alert("An error occurred while updating tickets.");
        });
}
