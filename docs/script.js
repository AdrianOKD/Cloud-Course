console.log("Script loaded successfully");

const registerPerson = document.getElementById("registerPerson");
registerPerson.addEventListener("submit", async (event) => {
    console.log("Form submitted!");
    event.preventDefault();
    console.log("Form submitted!");
    const data = new FormData(registerPerson)
    const name = data.get("name");
    const email = data.get("email");
    if (email == null) {
        console.log("its null")
    }
    console.log(`${name} , ${email}`);
    fetch('http://localhost:7071/api/HttpCloud', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ name: name, email: email })
    })
});