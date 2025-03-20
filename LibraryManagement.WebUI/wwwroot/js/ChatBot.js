var hubConnection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:7254/ai-hub")
    .build();

hubConnection.start()
    .then(() =>
        document.getElementById("connectionId").innerHTML = `(${hubConnection.connectionId})`)
    .catch(err =>
        console.error(err.toString()));

hubConnection.on("ReceiveMessage", responseMessage => {
    const aiMessageEl = document.createElement('div');
    aiMessageEl.className = 'flex items-start'; // Adjust styling as needed
    aiMessageEl.innerHTML = `
        <img src="/img/logo.png" alt="Bot" class="w-10 h-10 rounded-full mr-3">
        <div class="bg-gray-100 p-3 rounded-lg max-w-xl">
            <p class="text-gray-800">${responseMessage}</p>
        </div>
    `;
    chatBox.appendChild(aiMessageEl);
    chatBox.scrollTop = chatBox.scrollHeight;
});


const input = document.getElementById("chat-input");
const chatBox = document.getElementById("chat-box");

function sendPrompt() {
    if (input.value.trim() === "") return;

    const userMessageEl = document.createElement('div');
    userMessageEl.className = 'flex items-end justify-end';
    userMessageEl.innerHTML = `
    <div class="bg-blue-600 p-3 rounded-lg max-w-xl text-white">
        <p>${input.value}</p>
    </div>
    <img src="@((userProfilePicture != null) ? userProfilePicture : "~/img/profilepictureSmall.jpeg")" alt="User" class="w-10 h-10 rounded-full ml-3">
`;
    chatBox.appendChild(userMessageEl);
    chatBox.scrollTop = chatBox.scrollHeight;

    chatBox.scrollTop = chatBox.scrollHeight;

    fetch("https://localhost:7254/ChatBot/ChatBot", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({ prompt: input.value, connectionId: hubConnection.connectionId })
    })
        .then(response => response.json())
        .then(data => console.log("Success : ", data))
        .catch(error => console.error("Error", error));
    input.value = "";
}

document.getElementById("send-btn").addEventListener("click", sendPrompt);

// Also trigger sendPrompt on "Enter" key press
document.getElementById("chat-input").addEventListener("keypress", function (event) {
    if (event.key === "Enter") {
        event.preventDefault(); // Prevents accidental form submission
        sendPrompt();
    }
});
