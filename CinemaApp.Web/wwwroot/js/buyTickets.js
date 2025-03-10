$(document).ready(function () {
    $(".buy-ticket-btn").on("click", function () {
        const cinemaId = $(this).attr("data-cinema-id");
        const cinemaName = $(this).attr("data-cinema-name");
        const movieId = $(this).attr("data-movie-id");
        const movieName = $(this).attr("data-movie-name");

        console.log("Cinema ID:", cinemaId);
        console.log("Cinema Name:", cinemaName);
        console.log("Movie ID:", movieId);
        console.log("Movie Name:", movieName);

        if (!cinemaId || !movieId) {
            Swal.fire("Error!", "Missing Cinema ID or Movie ID.", "error");
            return;
        }

        $("#cinemaId").val(cinemaId);
        $("#movieId").val(movieId);
        $("#cinemaNamePlaceholder").text(cinemaName);

        $("#buyTicketModalLabel").html(`Buy Ticket - ${cinemaName} <br><small class="text-muted">${movieName}</small>`);

        $("#buyTicketModal").modal("show");
    });

    $("#buyTicketButton").on("click", function () {
        const requestData = {
            cinemaId: $("#cinemaId").val().trim(),
            movieId: $("#movieId").val().trim(),
            quantity: parseInt($("#quantity").val(), 10)
        };

        console.log("Submitting Request:", requestData); // ✅ Added for Debugging

        if (!requestData.quantity || requestData.quantity < 1) {
            $("#errorMessage").text("Please enter a valid ticket quantity.").removeClass("d-none");
            return;
        }

        $.ajax({
            url: "/api/TicketApi/BuyTicket",
            method: "POST",
            contentType: "application/json",
            data: JSON.stringify(requestData),
            success: function (response) {
                console.log("Success Response:", response); // ✅ Log the success response
                Swal.fire("Success!", "Your ticket has been purchased successfully!", "success");
                $("#buyTicketModal").modal("hide");
            },
            error: function (xhr) {
                let errorMessage = "An error occurred while purchasing tickets.";
                console.error("Raw Response:", xhr.responseText); // Log the raw response

                try {
                    if (xhr.responseJSON) {
                        errorMessage = xhr.responseJSON.title || xhr.responseJSON.message || errorMessage;
                    } else if (xhr.responseText) {
                        errorMessage = xhr.responseText; // Use response as-is if it's plain text
                    }
                } catch (e) {
                    console.error("Error processing response:", e);
                }

                Swal.fire("Error!", errorMessage, "error");
            }

        });
    });
});
