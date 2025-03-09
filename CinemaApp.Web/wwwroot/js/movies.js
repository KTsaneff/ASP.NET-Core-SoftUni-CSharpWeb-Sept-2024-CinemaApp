document.addEventListener("DOMContentLoaded", function () {
    console.log("✅ DOM fully loaded and parsed.");

    // Get the modal element
    const modalElement = document.getElementById("movieDetailsModal");
    if (!modalElement) {
        console.error("❌ Error: Modal element #movieDetailsModal not found!");
        return;
    }

    const movieDetailsModal = new bootstrap.Modal(modalElement);
    const detailsContainer = document.getElementById("movieDetailsContent");

    // Attach event listeners to all 'View Details' buttons
    const viewDetailsButtons = document.querySelectorAll(".view-details-btn");
    if (viewDetailsButtons.length === 0) {
        console.warn("⚠️ Warning: No '.view-details-btn' elements found on the page.");
    }

    viewDetailsButtons.forEach(button => {
        button.addEventListener("click", function (event) {
            event.preventDefault();

            let movieId = this.getAttribute("data-movie-id");
            console.log(`🎬 Fetching details for movie ID: ${movieId}`);

            fetch(`/Movie/DetailsPartial/${movieId}`)
                .then(response => {
                    if (!response.ok) {
                        throw new Error(`❌ Failed to load movie details! Status: ${response.status}`);
                    }
                    return response.text();
                })
                .then(data => {
                    console.log("📥 Movie details received:", data);

                    detailsContainer.innerHTML = data;

                    // Get movie title from the modal content
                    let movieTitleElement = detailsContainer.querySelector("h3");
                    let movieTitle = movieTitleElement ? movieTitleElement.textContent : "Movie Details";
                    document.getElementById("movieDetailsLabel").textContent = movieTitle;

                    // Ensure modal content is displayed properly
                    detailsContainer.style.display = "block";

                    // Show the modal
                    movieDetailsModal.show();

                    // Attach dynamic event listeners after content is loaded
                    attachDynamicEventListeners();
                })
                .catch(error => {
                    console.error("❌ Error loading movie details:", error);
                    detailsContainer.innerHTML = `<p class="text-danger">Failed to load movie details. Please try again later.</p>`;
                });
        });
    });

    function attachDynamicEventListeners() {
        setTimeout(() => {
            console.log("🔄 Attaching dynamic event listeners...");

            const buyTicketBtn = document.querySelector("#buy-ticket-btn");
            if (buyTicketBtn) {
                buyTicketBtn.addEventListener("click", function () {
                    alert("🎟 Ticket buying functionality will be implemented soon!");
                });
                console.log("✅ 'Buy Ticket' button found and event attached.");
            } else {
                console.warn("⚠️ 'Buy Ticket' button not found.");
            }

            const addToWatchlistBtn = document.querySelector("#add-to-watchlist-btn");
            if (addToWatchlistBtn) {
                addToWatchlistBtn.addEventListener("click", function () {
                    alert("📌 Adding to watchlist functionality will be implemented soon!");
                });
                console.log("✅ 'Add to Watchlist' button found and event attached.");
            } else {
                console.warn("⚠️ 'Add to Watchlist' button not found.");
            }
        }, 200); // Wait for content to load before attaching listeners
    }
});
