function openShowtimeSetupModal(cinemaId, cinemaName, cinemaLocation) {
    console.log("Opening Showtime Setup Modal:", cinemaId, cinemaName, cinemaLocation);

    fetch(`/Manager/api/ShowtimeApi/GetMoviesByCinema/${cinemaId}`, {
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
            console.log("Movies received from API:", movies);
            renderShowtimeSetupModal(movies, cinemaName, cinemaLocation);
            $('#showtimeSetupModal').modal('show');
        })
        .catch(error => {
            console.error("Error loading movies:", error);
            Swal.fire({
                title: "Error",
                text: "An error occurred while loading movies.",
                icon: "error",
                confirmButtonColor: "#dc3545"
            });
        });
}

function renderShowtimeSetupModal(movies, cinemaName, cinemaLocation) {
    let modalContainer = document.getElementById("manageShowtimesModalContainer");

    if (!modalContainer) {
        console.error("Modal container not found in the DOM.");
        return;
    }

    let modalHtml = `
        <div id="showtimeSetupModal" class="modal fade" tabindex="-1" aria-labelledby="showtimeSetupModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content bg-dark text-white">
                    <div class="modal-header border-0">
                        <h5 class="modal-title fw-bold">
                            <i class="bi bi-clock-history"></i> ${cinemaName} - <span class="text-muted">${cinemaLocation}</span>
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
                    <thead class="table-info text-dark">
                        <tr>
                            <th>Movie Title</th>
                            <th>Showtimes</th>
                            <th>Action</th>
                        </tr>
                    </thead>
                    <tbody id="moviesListContainer">`;

        movies.forEach(movie => {
            let tempMovieId = movie.movieId;
            let tempCinemaId = movie.cinemaId;
            let tempMovieTitle = movie.title;
            let tempMovieShowtimes = movie.showtimes;

            modalHtml += `
        <tr>
            <td class="fw-bold">${tempMovieTitle}</td>
            <td>
                <div class="d-flex justify-content-center gap-2">
                    ${["12 PM", "3 PM", "6 PM", "8 PM", "10 PM"].map((time, index) => ` 
                        <div class="form-check">
                            <input class="form-check-input" type="checkbox"
                                   id="showtime-${tempMovieId}-${index}" 
                                   ${tempMovieShowtimes[index] ? "checked" : ""}>
                            <label class="form-check-label">${time}</label>
                        </div>`).join('')}
                </div>
            </td>
            <td>
                <button class="btn btn-warning btn-sm" onclick="updateShowtimes('${tempMovieId}', '${tempCinemaId}')">
                    <i class="bi bi-save"></i> Save
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

    document.getElementById("manageShowtimesModalContainer").innerHTML = modalHtml;
    $('#showtimeSetupModal').modal('show');
}

function updateShowtimes(movieId, cinemaId) {
    const selectedShowtimes = [0, 1, 2, 3, 4].map(index =>
        document.getElementById(`showtime-${movieId}-${index}`).checked ? 1 : 0
    );

    const payload = {
        CinemaId: cinemaId,
        MovieId: movieId,
        Showtimes: selectedShowtimes
    };

    console.log("Updating Showtimes with payload:", payload); // 🛠 Debug log

    fetch('/Manager/api/ShowtimeApi/UpdateShowtimes', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(payload)
    })
        .then(response => {
            if (!response.ok) {
                return response.json().then(error => {
                    throw new Error(error.message || "Failed to update showtimes.");
                });
            }
            return response.json();
        })
        .then(data => {
            Swal.fire({
                title: "Success",
                text: "Showtimes updated successfully!",
                icon: "success",
                confirmButtonColor: "#28a745",
                timer: 3000,
                showConfirmButton: false,
                toast: true,
                position: "top-end"
            });
        })
        .catch(error => {
            console.error("Error:", error);
            Swal.fire({
                title: "Error",
                text: "An error occurred while updating showtimes.",
                icon: "error",
                confirmButtonColor: "#dc3545"
            });
        });
}

