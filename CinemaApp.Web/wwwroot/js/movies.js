document.addEventListener("DOMContentLoaded", function () {
    const movieDetailsModal = new bootstrap.Modal(document.getElementById("movieDetailsModal"));

    document.querySelectorAll(".view-details-btn").forEach(button => {
        button.addEventListener("click", function (event) {
            event.preventDefault();

            let movieId = this.getAttribute("data-movie-id");
            let detailsContainer = document.getElementById("movieDetailsContent");

            fetch(`/Movie/DetailsPartial/${movieId}`)
                .then(response => response.text())
                .then(data => {
                    document.getElementById("movieDetailsContent").innerHTML = data;
                    document.getElementById("movieDetailsLabel").textContent = document.querySelector(`[data-movie-id='${movieId}']`).parentElement.querySelector(".card-title").textContent;
                    movieDetailsModal.show();
                });
        });
    });

    // Attach event listeners inside the modal dynamically after loading content
    document.getElementById("movieDetailsModal").addEventListener("shown.bs.modal", function () {
        document.querySelector(".buy-ticket-btn")?.addEventListener("click", function () {
            alert("Ticket buying functionality will be implemented soon!");
        });

        document.querySelector(".add-to-watchlist-btn")?.addEventListener("click", function () {
            alert("Adding to watchlist functionality will be implemented soon!");
        });
    });
});
