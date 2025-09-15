console.log("Script loaded successfully");


const registerPerson = document.getElementById("registerPerson");
registerPerson.addEventListener("submit", async (event) => {
    console.log("Form submitted!");
    event.preventDefault();
    const form = document.getElementById('registerPerson')
    const data = new FormData(registerPerson)
    const name = data.get("name");
    const email = data.get("email");

    await fetch('https://mycloudfunctions.azurewebsites.net/api/httpcloud', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ name: name, email: email })
    }).then(async response => {

        if (response.ok) {
            console.log("Registered")
            form.reset();
            alert("Registered!");
        }
        else {
            const errorMessage = await response.text();
            console.log("Not Registered", errorMessage)
            alert("Not registered!", errorMessage);
        }
    });

});
