console.log("Script loaded successfully");


const registerPerson = document.getElementById("registerPerson");
registerPerson.addEventListener("submit", async (event) => {
    console.log("Form submitted!");
    event.preventDefault();
    const form = document.getElementById('registerPerson')
    const data = new FormData(registerPerson)
    const name = data.get("name");
    const email = data.get("email");

    fetch('https://mycloudfunctions.azurewebsites.net/api/httpcloud', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ name: name, email: email })
    }).then(response => {

        if (response.ok) {
            console.log("Registered")
            form.reset();
            location.reload()
        }
        else {
            console.log("Not Registered")
        }
    });

});
