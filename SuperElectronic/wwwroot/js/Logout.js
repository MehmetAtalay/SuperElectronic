$(function () {

    $('.logout').click(function () {
        if (confirm('Are you sure to logout')) {
            return true;
        }

        return false;
    });
});
$('.logout').ready(function () {
    // Log a message to indicate the script is running
    console.log("signout_confirmation_message");

    // Retrieve all sign-out links with the data-confirm attribute
    const signOutLinks = document.querySelectorAll('a[data-confirm]');
    console.log(signOutLinks);

    // Add a click event listener to each sign-out link
    signOutLinks.forEach(link => {
        link.addEventListener('click', function (event) {
            // Retrieve the confirmation message from the data-confirm attribute
            const confirmationMessage = link.getAttribute('data-confirm');

            // Display the confirmation dialog
            if (!confirm(confirmationMessage)) {
                // If the user cancels, prevent the default logout action
                event.preventDefault();
            }
        });
    });
});